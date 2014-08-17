using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Puppet.Core.Network.Http
{
    public class WWWRequest : IHttpRequest
    {
        Action<IHttpRequest, float> _onProgress;
        Action<IHttpRequest, IHttpResponse> _onResponse;
        IServerMode serverMode;
        WWWResponse dataResponse;
        MonoBehaviour gameObject;

        float timeOut;
        HttpMethod method = HttpMethod.Get;

        float originTimeOut;

        public string Path;
        public Hashtable Header;
        public Dictionary<string, object> PostData;
        public int RetryCount;
        public float WaitForProgress = 0.01f;
        public bool EnableTimeOut;

        public WWWRequest(MonoBehaviour gameObject, string _path, float _timeOut, int _retryCount)
        {
            this.gameObject = gameObject;
            this.Path = _path;
            this.TimeOut = _timeOut;
            this.originTimeOut = _timeOut;
            this.RetryCount = _retryCount;
            this.EnableTimeOut = timeOut > 0;
        }

        public void Start(IServerMode server)
        {
            dataResponse = new WWWResponse();
            this.serverMode = server;
            gameObject.StartCoroutine(DownloadData());
        }

        IEnumerator DownloadData()
        {
            string url = serverMode + Path;
            WWW www;
            if (Method == HttpMethod.Get)
                www = new WWW(url);
            else
            {
                WWWForm form = new WWWForm();
                www = new WWW(url, form);
            }

            float lastProgress = -1;
            while (www != null && www.progress < 1f && string.IsNullOrEmpty(www.error) && !IsTimeOut)
            {
                if (lastProgress < www.progress)
                {
                    //Only dispatch event onDownloading when have a changing progress
                    lastProgress = www.progress;
                    if (_onProgress != null)
                        _onProgress(this, www.progress);
                }

                yield return new WaitForSeconds(WaitForProgress);
                TimeOut -= WaitForProgress;
            }

            if (IsTimeOut)
            {
                //DOWNLOAD TIMEOUT!!!
                dataResponse.State = StateResponse.Request_Timeout;
            }
            else
            {
                if (string.IsNullOrEmpty(www.error))
                {
                    //Just yield www when successful
                    yield return www;
                    //Set RetryCount to less than or equal 0 for not continue to download more 
                    RetryCount = -1;
                    dataResponse.State = StateResponse.Success;
                }
                else
                    dataResponse.State = StateResponse.Network_Fail;
            }
            Dispose(www);
            yield return null;
        }

        public HttpMethod Method
        {
            get { return method; } set { method = value; }
        }

        public float TimeOut
        {
            get { return timeOut; } set { timeOut = value; }
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


        void Dispose(WWW www)
        {
            if (--RetryCount > 0)
            {
                //If you still need to download again
                ResetDownload();
                gameObject.StartCoroutine(DownloadData());
            }
            else
            {
                dataResponse.www = www;
                dataResponse.Error = www.error;
                dataResponse.Data = www.text;

                //When complete download, Whether success or failure
                if (_onResponse != null)
                    _onResponse(this, dataResponse);
            }
            www.Dispose();
            www = null;
        }

        void ResetDownload()
        {
            TimeOut = originTimeOut;
        }

        bool IsTimeOut
        {
            get { return EnableTimeOut && TimeOut <= 0; }
        }
    }
}
