using System;
using System.Collections.Generic;
using System.Text;

namespace wojilu.weibo.Core.QQWeibo
{
    public class other : QWeiboApiBase
    {
        /// <summary> 其他api 接口
        /// Initializes a new instance of the <see cref="other"/> class.
        /// </summary>
        /// <param name="okey">The okey.</param>
        /// <param name="format">The format.</param>
        /// <remarks></remarks>
        public other(OauthKey okey, string format) : base(okey, format) { }

        /// <summary>可能认识的人
        /// 
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public string kownperson()
        {
            List<Parameter> paras = new List<Parameter>();

            paras.Add(new Parameter("format",format));
            
            return base.SyncRequest(TypeOption.TXWB_OTHER_KNOWNPERSON,paras,null);
        }

        /// <summary>短url变长url
        /// 
        /// </summary>
        /// <param name="url">短url.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string shorturl(string url)
        {
            List<Parameter> paras = new List<Parameter>();

            paras.Add(new Parameter("format",format));
            paras.Add(new Parameter("url",url));
            
            return base.SyncRequest(TypeOption.TXWB_OTHER_SHORTURL,paras,null);
        }

        /// <summary>获取上传视频的key
        /// 
        /// </summary>
        /// <returns>key</returns>
        /// <remarks></remarks>
        public string videokey()
        {
            List<Parameter> paras = new List<Parameter>();

            paras.Add(new Parameter("format",format));
            
            return base.SyncRequest(TypeOption.TXWB_OTHER_VIDEOKEY,paras,null);
        }

    }
}
