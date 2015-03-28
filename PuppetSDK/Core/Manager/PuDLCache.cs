using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Puppet.Core;
using Puppet.Core.Network.Http;
using System.IO;
using Puppet.Core.Model;
using System.Runtime.Serialization;
using Puppet.Utils;
using System.Net;

namespace Puppet.Core.Manager
{
    public class PuDLCache : BaseSingleton<PuDLCache>
    {
        PuDownloadData _downloadData;
        protected override void Init() {}

        internal void Start()
        {
            if (CacheHandler.Instance.HasKey("CacheDownloadFile"))
                _downloadData = CacheHandler.Instance.GetObject("CacheDownloadFile") as PuDownloadData;
            else
                _downloadData = new PuDownloadData();

            if (_downloadData.md5Dict == null)
                _downloadData.md5Dict = new Dictionary<string, string>();

            Logger.Log("Cache Download File Info: " + _downloadData.ToJson());
        }

        internal void SaveCache()
        {
            CacheHandler.Instance.SetObject("CacheDownloadFile", _downloadData);
        }

        public void HttpRequestCache(string path, Action<bool, string, byte[]> callback, Action<float> progress = null)
        {
            string md5Hash = StringUtil.GetMd5Hash(path);
            if(ContainsCache(md5Hash, callback, progress))
                return;

            System.Net.WebClient web = new System.Net.WebClient();
            if (progress != null)
            {
                web.DownloadProgressChanged += (sender, e) =>
                {
                    progress(e.ProgressPercentage / 100f);
                };
            }
            if (callback != null)
            {
                web.DownloadDataCompleted += (sender, e) =>
                {
                    bool status = !e.Cancelled && e.Error == null;
                    ThreadHandler.QueueOnMainThread(() => callback(status, status ? string.Empty : e.Error.Message, e.Result));

                    if (status && PuMain.Setting.Platform != EPlatform.WebPlayer)
                        File.WriteAllBytes(Path.Combine(PuMain.Setting.PathCache, md5Hash), e.Result);

                    string value = string.Format("{0}|{1}", path, DateTime.Now.ToString("dd/MM/yyyy-hh:mm:ss"));
                    if (_downloadData.md5Dict.ContainsKey(md5Hash))
                        _downloadData.md5Dict[md5Hash] = value;
                    else
                        _downloadData.md5Dict.Add(md5Hash, value);
                };
            }
            web.DownloadDataAsync(new Uri(path));

            //SimpleHttpRequest request = new SimpleHttpRequest(path);
            //request.isFullUrl = true;
            //request.onProgress = progress;
            //request.onResponse = (rRequest, rResponse) =>
            //{
            //    if(rResponse.State == System.Net.HttpStatusCode.OK && PuMain.Setting.Platform != EPlatform.WebPlayer)
            //    {
            //        File.WriteAllBytes(Path.Combine(PuMain.Setting.PathCache, md5Hash + "_"), rResponse.Bytes);
            //    }

            //    //if (callback != null)
            //    //    callback(rResponse.State == System.Net.HttpStatusCode.OK, rResponse.Error, rResponse.Bytes);
            //};
            //PuMain.WWWHandler.Request(request);
        }

        public bool ContainsCache(string md5Hash, Action<bool, string, byte[]> callback, Action<float> progress = null)
        {
            if (PuMain.Setting.Platform == EPlatform.WebPlayer)
                return false;

            string filePath = Path.Combine(PuMain.Setting.PathCache, md5Hash);
            if(_downloadData.md5Dict.ContainsKey(md5Hash) && File.Exists(filePath))
            {
                if(callback != null)
                    callback(true, string.Empty, File.ReadAllBytes(filePath));
                if (progress != null)
                    progress(1f);

                return true;
            }

            return false;
        }

        [Serializable()]
        public class PuDownloadData : DataModel
        {
            public Dictionary<string, string> md5Dict { get; set; }

            public PuDownloadData() : base() { }

            public PuDownloadData(SerializationInfo info, StreamingContext ctxt) : base(info, ctxt) { }
        }
    }
}
