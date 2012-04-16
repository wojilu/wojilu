using System;
using System.Collections.Generic;
using System.Text;

namespace wojilu.weibo.Core.QQWeibo
{
    /// <summary>热度趋势
    /// 
    /// </summary>
    /// <remarks></remarks>
    public class trends : QWeiboApiBase
    {
        /// <summary>
        /// 构造函数<see cref="trends"/> class.
        /// </summary>
        /// <param name="okey">The okey.</param>
        /// <param name="format">The format.</param>
        /// <remarks></remarks>
        public trends(OauthKey okey, string format) : base(okey, format) { }

        /// <summary>话题热榜
        /// 
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="reqnum">The reqnum.</param>
        /// <param name="pos">The pos.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string ht(int type, int reqnum, int pos )
        {
            List<Parameter> paras = new List<Parameter>();
            

            paras.Add(new Parameter("format",format));
            paras.Add(new Parameter("type",type.ToString()));
            paras.Add(new Parameter("reqnum",reqnum.ToString()));
            paras.Add(new Parameter("pos",pos.ToString()));
            
            
            return base.SyncRequest(TypeOption.TXWB_TRENDS_HT,paras,null);
        }

        /// <summary>转播热榜
        /// 
        /// </summary>
        /// <param name="reqnum">The reqnum.</param>
        /// <param name="pos">The pos.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string t(int reqnum, int pos )
        {
            List<Parameter> paras = new List<Parameter>();
            

            paras.Add(new Parameter("format",format));
            
            paras.Add(new Parameter("reqnum",reqnum.ToString()));
            paras.Add(new Parameter("pos",pos.ToString()));
            
            
            return base.SyncRequest(TypeOption.TXWB_TRENDS_T,paras,null);
        }

    }
}
