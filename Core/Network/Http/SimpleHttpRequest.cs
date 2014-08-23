﻿using Puppet.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace Puppet.Core.Network.Http
{
    public class SimpleHttpRequest : IHttpRequest
    {
        IServerMode server;
        HttpMethod _method;
        Action<IHttpRequest, float> _onProgress;
        Action<IHttpRequest, IHttpResponse> _onResponse;
        string _path = string.Empty;
        Dictionary<string, object> PostData;
        SimpleHttpResponse simpleResponse;

        public SimpleHttpRequest(string path)
        {
            this._path = path;
            simpleResponse = new SimpleHttpResponse();
        }

        public SimpleHttpRequest(string path, params object [] postData) : this(path)
        {
            if(postData != null && postData.Length > 0)
            {
                if (postData.Length % 2 == 0)
                {
                    _method = HttpMethod.Post;
                    PostData = new Dictionary<string, object>();
                    for (int i = 0; i < postData.Length; i += 2)
                        PostData.Add(postData[i].ToString(), postData[i + 1]);
                }
                else
                {
                    Logger.LogError("SimpleHttpRequest with params invalid");
                    simpleResponse.State = HttpStatusCode.BadRequest;
                }
            }
        }

        public void Start(IServerMode server)
        {
            this.server = server;
            Utility.StartCoroutine(StartWebRequest());
        }

        IEnumerator StartWebRequest()
        {
            string url = server.GetPath(_path);
            Logger.Log("Url: " + url);
            WebRequest request = HttpWebRequest.Create(url);

            if (Method == HttpMethod.Post)
            {
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                string postData = string.Empty;
                foreach (string key in PostData.Keys)
                    postData += string.Format("&{0}={1}", key, PostData[key].ToString());
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);

                try
                {
                    using (var writer = new StreamWriter(request.GetRequestStream()))
                    {
                        writer.Write(postData);
                        writer.Close();
                    }
                }
                catch (Exception e)
                {
                    simpleResponse.Error = e.Message;
                    simpleResponse.State = HttpStatusCode.ServiceUnavailable;
                    if (onResponse != null)
                        onResponse(this, simpleResponse);
                    yield break;
                }
            }

            if (onProgress != null)
                onProgress(this, 0f);

            IAsyncResult asyncResult = request.BeginGetResponse(new AsyncCallback(FinishWebRequest), request);

            long timeOut = Convert.ToInt64(this.TimeOut * 1000f);
            ThreadPool.RegisterWaitForSingleObject(asyncResult.AsyncWaitHandle, 
                new WaitOrTimerCallback(ScanTimeoutCallback),
                request, timeOut, true
            );

            while (!asyncResult.IsCompleted) 
                yield return null; 

            if (onProgress != null)
                onProgress(this, 1f);
            if (onResponse != null)
                onResponse(this, simpleResponse);
        }

        void FinishWebRequest(IAsyncResult result)
        {
            try
            {
                WebRequest requestState = result.AsyncState as WebRequest;
                HttpWebResponse response = requestState.EndGetResponse(result) as HttpWebResponse;
                simpleResponse.State = response.StatusCode;

                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                simpleResponse.Data = reader.ReadToEnd();

                Logger.Log("responseFromServer: " + simpleResponse.Data);
                simpleResponse.State = HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                simpleResponse.Error = e.Message;
                simpleResponse.State = HttpStatusCode.BadRequest;
            }
        }

        private void ScanTimeoutCallback(object state, bool timedOut)
        {
            if (timedOut)
            {
                WebRequest requestState = (WebRequest)state;
                if (requestState != null)
                    requestState.Abort();

                simpleResponse.Error = "Request Time Out";
                simpleResponse.State = HttpStatusCode.RequestTimeout;
            }
        }


        public float TimeOut
        {
            get
            {
                return 60f;
            }
            set { }
        }

        public HttpMethod Method
        {
            get { return _method; }
            set { _method = value; }
        }

        public Action<IHttpRequest, float> onProgress
        {
            get { return _onProgress; }
            set { _onProgress = value; }
        }

        public Action<IHttpRequest, IHttpResponse> onResponse
        {
            get { return _onResponse; }
            set { _onResponse = value; }
        }
    }

    internal class SimpleHttpResponse : IHttpResponse
    {
        public HttpStatusCode State { get; set; }
        public string Error { get; set; }
        public string Data { get; set; }
    }
}
