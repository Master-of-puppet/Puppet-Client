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
        public bool isFullUrl;

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
            StartWebRequest();
        }

        void StartWebRequest()
        {
            string url = isFullUrl ? _path : server.GetPath(_path);
            WebRequest request = HttpWebRequest.Create(url);
            
            string printLog = url + "?";
            if (Method == HttpMethod.Post)
            {
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                ((HttpWebRequest)request).UserAgent = "http://www.dungnv.info with Puppet";
                string postData = string.Empty;

                int i = 0;
                foreach (string key in PostData.Keys)
                {
                    string format = "&{0}={1}";
                    if (i == 0 && _path.EndsWith("?"))
                        format = "{0}={1}";
                    postData += string.Format(format, key, PostData[key].ToString());
                    i++;
                }
                printLog += postData;

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
                    Logger.LogError("SimpleHttp Request Exception: ", printLog);
                    simpleResponse.Error = e.Message;
                    simpleResponse.State = HttpStatusCode.ServiceUnavailable;

                    DispatchEventResponse();
                    return;
                }
            }
            Logger.Log("SimpleHttp Request Url: ", printLog, ELogColor.LIME);

            if (onProgress != null)
                onProgress(this, 0f);
            IAsyncResult asyncResult = request.BeginGetResponse(new AsyncCallback(FinishWebRequest), request);
            long timeOut = Convert.ToInt64(this.TimeOut * 1000f);
            ThreadPool.RegisterWaitForSingleObject(asyncResult.AsyncWaitHandle, 
                new WaitOrTimerCallback(ScanTimeoutCallback),
                request, timeOut, true
            );
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

                //using (BinaryReader br = new BinaryReader(dataStream))
                //{
                //    simpleResponse.Bytes = br.ReadBytes(Convert.ToInt32(dataStream.Length));
                //    br.Close();
                //}

                simpleResponse.State = HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                simpleResponse.Error = e.Message;
                simpleResponse.State = HttpStatusCode.BadRequest;
            }

            if (onProgress != null)
                onProgress(this, 1f);

            DispatchEventResponse();
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

        void DispatchEventResponse()
        {
            if (string.IsNullOrEmpty(simpleResponse.Error))
                Logger.Log("SimpleHttp Response: ", simpleResponse.Data
                    + "\nFromRequest: " + (isFullUrl ? _path : server.GetPath(_path))
                    , ELogColor.LIME);
            else
                Logger.LogError("SimpleHttp Error: {0}", simpleResponse.Error
                    + "\nFromRequest: " + (isFullUrl ? _path : server.GetPath(_path))
                    , ELogColor.LIME);

            PuMain.Setting.Threading.QueueOnMainThread(() =>
            {
                if (onResponse != null)
                    onResponse(this, simpleResponse);
            });
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
        public byte[] Bytes { get; set; }
    }
}
