using System;
using System.Collections.Generic;
using System.Text;

namespace wojilu.weibo.Core.QQWeibo
{
     public class search : QWeiboApiBase 
    {
         /// <summary> 搜索相关 需要 合作伙伴权限
         /// 构造函数 <see cref="search"/> class.
         /// </summary>
         /// <param name="okey">The okey.</param>
        /// <param name="format">The format.</param>
        /// <remarks></remarks>
        public search(OauthKey okey, string format) : base(okey, format) { } 

        /// <summary> 搜索用户
        /// Users the specified keyword.
        /// </summary>
        /// <param name="keyword">The keyword.</param>
        /// <param name="pagesize">The pagesize.</param>
        /// <param name="page">The page.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string user(string keyword, int pagesize, int page)
        {
            List<Parameter> paras = new List<Parameter>();

            paras.Add(new Parameter("format",format));
            paras.Add(new Parameter("keyword",keyword));
            paras.Add(new Parameter("pagesize",pagesize.ToString()));
            paras.Add(new Parameter("page",page.ToString()));
            
            return base.SyncRequest(TypeOption.TXWB_SEARCH_USER,paras,null);
        }

         /// <summary> 搜索微博
         /// 
         /// </summary>
         /// <param name="keyword">The keyword.</param>
         /// <param name="pagesize">The pagesize.</param>
         /// <param name="page">The page.</param>
         /// <returns></returns>
         /// <remarks></remarks>
         public string t(string keyword, int pagesize, int page)
        {
            List<Parameter> paras = new List<Parameter>();

            paras.Add(new Parameter("format",format));
            paras.Add(new Parameter("keyword",keyword));
            paras.Add(new Parameter("pagesize",pagesize.ToString()));
            paras.Add(new Parameter("page",page.ToString()));
            
            return base.SyncRequest(TypeOption.TXWB_SEARCH_T,paras,null);
        }

         /// <summary> 通过标签搜索用户
         /// 
         /// </summary>
         /// <param name="keyword">The keyword.</param>
         /// <param name="pagesize">The pagesize.</param>
         /// <param name="page">The page.</param>
         /// <returns></returns>
         /// <remarks></remarks>
         public string userbytag(string keyword, int pagesize, int page)
        {
            List<Parameter> paras = new List<Parameter>();

            paras.Add(new Parameter("format",format));
            paras.Add(new Parameter("keyword",keyword));
            paras.Add(new Parameter("pagesize",pagesize.ToString()));
            paras.Add(new Parameter("page",page.ToString()));
            
            return base.SyncRequest(TypeOption.TXWB_SEARCH_USERBYTAG,paras,null);
        }
    }
}
