using System;
using System.Collections.Generic;
using System.Text;

namespace wojilu.weibo.Core.QQWeibo
{
    public class ht : QWeiboApiBase
    {
        /// <summary>话题类  构造函数
        ///  <see cref="ht"/> class.
        /// </summary>
        /// <param name="okey">The okey.</param>
        /// <param name="format">The format.</param>
        /// <remarks></remarks>
        public ht(OauthKey okey, string format) : base(okey, format) { }

        /// <summary>根据话题名称查询话题ID
        /// 
        /// </summary>
        /// <param name="httexts">The httexts.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string ids(string httexts)
        {
            List<Parameter> paras = new List<Parameter>();

            paras.Add(new Parameter("format",format));
            paras.Add(new Parameter("httexts",httexts));
            
            return base.SyncRequest(TypeOption.TXWB_HT_IDS,paras,null);
        }

        /// <summary>根据话题ID获取话题相关微博
        /// 
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string info(string ids)
        {
            List<Parameter> paras = new List<Parameter>();

            paras.Add(new Parameter("format",format));
            paras.Add(new Parameter("ids",ids));
            
            return base.SyncRequest(TypeOption.TXWB_HT_INFO,paras,null);
        }
    }
}
