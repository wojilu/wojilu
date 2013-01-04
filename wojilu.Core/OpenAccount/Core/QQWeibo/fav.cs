using System;
using System.Collections.Generic;
using System.Text;

namespace wojilu.weibo.Core.QQWeibo
{
    public class fav : QWeiboApiBase
    {
        /// <summary> 数据收藏类的 构造函数
        ///  <see cref="fav"/> class.
        /// </summary>
        /// <param name="okey">The okey.</param>
        /// <param name="format">The format.</param>
        /// <remarks></remarks>
        public fav(OauthKey okey, string format) : base(okey, format) { }


        /// <summary>收藏一条微博.
        /// 
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string addt(string id)
        {
            List<Parameter> paras = new List<Parameter>();

            paras.Add(new Parameter("format",format));
            paras.Add(new Parameter("id",id));
            
            return base.SyncRequest(TypeOption.TXWB_FAV_ADDT,paras,null);
        }

        /// <summary>从收藏夹删除一条微博.
        /// 
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string delt(string id)
        {
            List<Parameter> paras = new List<Parameter>();

            paras.Add(new Parameter("format",format));
            paras.Add(new Parameter("id",id));
            
            return base.SyncRequest(TypeOption.TXWB_FAV_DELT,paras,null);
        }

        /// <summary>收藏的微博列表.
        /// 
        /// </summary>
        /// <param name="pageflag">The pageflag.</param>
        /// <param name="nexttime">The nexttime.</param>
        /// <param name="prevtime">The prevtime.</param>
        /// <param name="reqnum">The reqnum.</param>
        /// <param name="lastid">The lastid.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string list_t(int pageflag, int nexttime, int prevtime,
                    int reqnum, int lastid)
        {
            List<Parameter> paras = new List<Parameter>();

            paras.Add(new Parameter("format",format));
            paras.Add(new Parameter("pageflag",pageflag.ToString()));

            paras.Add(new Parameter("nexttime",nexttime.ToString()));
            paras.Add(new Parameter("prevtime",prevtime.ToString()));
            paras.Add(new Parameter("reqnum",reqnum.ToString()));
            paras.Add(new Parameter("lastid",lastid.ToString()));
            
            return base.SyncRequest(TypeOption.TXWB_FAV_LIST_T,paras,null);
        }


        /// <summary>订阅话题.
        /// 
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string addht(string id)
        {
            List<Parameter> paras = new List<Parameter>();

            paras.Add(new Parameter("format",format));
            paras.Add(new Parameter("id",id));
            
            return base.SyncRequest(TypeOption.TXWB_FAV_ADDHT,paras,null);
        }

        /// <summary>删除话题从收藏.
        /// 
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string delht(string id)
        {
            List<Parameter> paras = new List<Parameter>();

            paras.Add(new Parameter("format",format));
            paras.Add(new Parameter("id",id));
            
            return base.SyncRequest(TypeOption.TXWB_FAV_DELHT,paras,null);
        }

        /// <summary>获取已订阅话题列表.
        /// 
        /// </summary>
        /// <param name="pageflag">The pageflag.</param>
        /// <param name="pagetime">The pagetime.</param>
        /// <param name="reqnum">The reqnum.</param>
        /// <param name="lastid">The lastid.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string list_t(int pageflag, int pagetime,
                    int reqnum, int lastid)
        {
            List<Parameter> paras = new List<Parameter>();

            paras.Add(new Parameter("format",format));
            paras.Add(new Parameter("pageflag",pageflag.ToString()));

            paras.Add(new Parameter("pagetime",pagetime.ToString()));
            paras.Add(new Parameter("reqnum",reqnum.ToString()));
            paras.Add(new Parameter("lastid",lastid.ToString()));
            
            return base.SyncRequest(TypeOption.TXWB_FAV_LIST_HT,paras,null);
        }

    }


}
