using System;
using System.Collections.Generic;
using System.Text;

namespace wojilu.weibo.Core.QQWeibo
{
    public delegate void AsyncRequestCallback(int key, string content);

    
   
    //回调信息
    class CallbackInfo
    {
        public int key = 0;
        public AsyncRequestCallback callback = null;
    }

    //微博请求
    public class QWeiboRequest
    {
        private Dictionary<AsyncHttp, CallbackInfo> asyncRquestMap = new Dictionary<AsyncHttp, CallbackInfo>();
        private int key = 0;

        //同步http请求
        public string SyncRequest(string url, string httpMethod, OauthKey key, List<Parameter> listParam, List<Parameter> listFile)
        {
            Oauth oauth = new Oauth();

            string queryString = null;
            string oauthUrl = oauth.GetOauthUrl(url, httpMethod, key.consumerKey, key.consumerSecret,
                key.tokenKey, key.tokenSecret, key.verify, key.callbackUrl, listParam, out queryString);

            SyncHttp http = new SyncHttp();
            if (httpMethod == "GET")
            {
                return http.HttpGet(oauthUrl, queryString);
            }
            else if ((listFile == null) || (listFile.Count == 0))
            {
                return http.HttpPost(oauthUrl, queryString);
            }
            else
            {
                return http.HttpPostWithFile(oauthUrl, queryString, listFile);
            }
        }

        //异步http请求
        public bool AsyncRequest(string url, string httpMethod, OauthKey key, List<Parameter> listParam, List<Parameter> listFile,
            AsyncRequestCallback callback, out int callbkey)
        {
            Oauth oauth = new Oauth();

            string queryString = null;
            string oauthUrl = oauth.GetOauthUrl(url, httpMethod, key.consumerKey, key.consumerSecret,
                key.tokenKey, key.tokenSecret, key.verify, key.callbackUrl, listParam, out queryString);

            AsyncHttp http = new AsyncHttp();

            callbkey = GetKey();
            CallbackInfo callbackInfo = new CallbackInfo();
            callbackInfo.key = callbkey;
            callbackInfo.callback = callback;

            asyncRquestMap.Add(http, callbackInfo);

            bool bResult = false;

            if (httpMethod == "GET")
            {
                bResult = http.HttpGet(oauthUrl, queryString, new AsyncHttpCallback(HttpCallback));
            }
            else if ((listFile == null) || (listFile.Count == 0))
            {
                bResult = http.HttpPost(oauthUrl, queryString, new AsyncHttpCallback(HttpCallback));
            }
            else
            {
                bResult = http.HttpPostWithFile(oauthUrl, queryString, listFile, new AsyncHttpCallback(HttpCallback));
            }

            if (!bResult)
            {
                asyncRquestMap.Remove(http);
            }
            return bResult;
        }

        //回调
        protected void HttpCallback(AsyncHttp http, string content)
        {
            CallbackInfo info;
            if(!asyncRquestMap.TryGetValue(http, out info))
            {
                return;
            }

            if ((info != null) && (info.callback != null))
            {
                info.callback(info.key, content);
            }
            asyncRquestMap.Remove(http);
        }

        private int GetKey()
        {
            return ++key;
        }
    }
}
