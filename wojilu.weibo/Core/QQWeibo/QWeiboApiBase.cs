using System;
using System.Collections.Generic;
using System.Text;

namespace wojilu.weibo.Core.QQWeibo
{
    public class QWeiboApiBase : QWeiboRequest
    {
        public OauthKey oauthkey;
        public string format;

        public QWeiboApiBase(OauthKey authkey, string format)
        {
            this.oauthkey = authkey;
            this.format = format;
        }

        public string SyncRequest(TypeOption option, List<Parameter> listParam, List<Parameter> listFile)
        {
             return base.SyncRequest(ApiType.GetUrl(option), ApiType.GetHttpMethod(option), 
                            oauthkey,listParam,listFile);
        }

        public bool AsyncRequest(TypeOption option,List<Parameter> listParam, List<Parameter> listFile,
            AsyncRequestCallback callback, out int callbkey)
        {
            return base.AsyncRequest(ApiType.GetUrl(option), ApiType.GetHttpMethod(option), 
                            oauthkey,listParam,listFile,callback,out callbkey);
        }
    }
}
