using System;
using System.Collections.Generic;
using System.Text;

namespace wojilu.weibo.Core.QQWeibo
{
    //时间线相关接口
    public class statuses : QWeiboApiBase
    {
        /// <summary> 时间线相关
        /// 构造函数<see cref="statuses"/> class.
        /// </summary>
        /// <param name="okey">The okey.</param>
        /// <param name="format">The format.</param>
        /// <remarks></remarks>
        public statuses(OauthKey okey, string format) : base(okey, format) { }        


        /// <summary> 主时间线
        /// 
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="pageflag">The pageflag.</param>
        /// <param name="pagetime">The pagetime.</param>
        /// <param name="reqnum">The reqnum.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string home_timeline(int pageflag, int pagetime, int reqnum)
        {
            List<Parameter> paras = new List<Parameter>();

            paras.Add(new Parameter("format",format));
            paras.Add(new Parameter("pageflag",pageflag.ToString()));
            paras.Add(new Parameter("pagetime",pagetime.ToString()));
            paras.Add(new Parameter("reqnum",reqnum.ToString()));


            return base.SyncRequest(TypeOption.TXWB_STATUSES_HOME_TL,paras,null);
        }

        /// <summary> 广播大厅时间线
        /// 
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="pos">The pos.</param>
        /// <param name="reqnum">The reqnum.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string public_timeline(int pos, int reqnum)
        {
            List<Parameter> paras = new List<Parameter>();

            paras.Add(new Parameter("format",format));
            paras.Add(new Parameter("pos",pos.ToString()));
            paras.Add(new Parameter("reqnum",reqnum.ToString()));
            
            return base.SyncRequest(TypeOption.TXWB_STATUSES_PUBLIC_TL,paras,null);
        }

        /// <summary>其他用户发表时间线
        /// 
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="pageflag">The pageflag.</param>
        /// <param name="pagetime">The pagetime.</param>
        /// <param name="reqnum">The reqnum.</param>
        /// <param name="lastid">The lastid.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string user_timeline(int pageflag, int pagetime, int reqnum, 
                        int lastid, string name)
        {
            List<Parameter> paras = new List<Parameter>();

            
            paras.Add(new Parameter("format",format));
            paras.Add(new Parameter("pageflag",pageflag.ToString()));
            paras.Add(new Parameter("pagetime",pagetime.ToString()));
            paras.Add(new Parameter("reqnum",reqnum.ToString()));
            paras.Add(new Parameter("lastid",lastid.ToString()));
            paras.Add(new Parameter("name",name));


            return base.SyncRequest(TypeOption.TXWB_STATUSES_USER_TL,paras,null);
        }

        /// <summary>用户提及时间线
        /// 
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="pageflag">The pageflag.</param>
        /// <param name="pagetime">The pagetime.</param>
        /// <param name="reqnum">The reqnum.</param>
        /// <param name="lastid">The lastid.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string mentions_timeline(int pageflag,int pagetime, int reqnum,int lastid)
        {
            List<Parameter> paras = new List<Parameter>();

            paras.Add(new Parameter("format",format));
            paras.Add(new Parameter("pageflag",pageflag.ToString()));
            paras.Add(new Parameter("pagetime",pagetime.ToString()));
            paras.Add(new Parameter("reqnum",reqnum.ToString()));
            paras.Add(new Parameter("lastid",lastid.ToString()));


            return base.SyncRequest(TypeOption.TXWB_STATUSES_MENTIONS_TL,paras,null);
        }

        /// <summary>话题时间线
        /// 
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="httext">The httext.</param>
        /// <param name="pageflag">The pageflag.</param>
        /// <param name="pageinfo">The pageinfo.</param>
        /// <param name="reqnum">The reqnum.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string ht_timeline(string httext ,int pageflag, int pageinfo, int reqnum)
        {
            List<Parameter> paras = new List<Parameter>();

            paras.Add(new Parameter("format",format));
            paras.Add(new Parameter("httext",httext));
            paras.Add(new Parameter("pageflag",pageflag.ToString()));
            paras.Add(new Parameter("pageinfo",pageinfo.ToString()));
            paras.Add(new Parameter("reqnum",reqnum.ToString()));


            return base.SyncRequest(TypeOption.TXWB_STATUSES_HT_TL,paras,null);
        }

        /// <summary>我发表时间线
        /// 
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="pageflag">The pageflag.</param>
        /// <param name="pagetime">The pagetime.</param>
        /// <param name="reqnum">The reqnum.</param>
        /// <param name="lastid">The lastid.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string broadcast_timeline(int pageflag, int pagetime, int reqnum,int lastid)
        {
            List<Parameter> paras = new List<Parameter>();

            paras.Add(new Parameter("format",format));
            paras.Add(new Parameter("pageflag",pageflag.ToString()));
            paras.Add(new Parameter("pagetime",pagetime.ToString()));
            paras.Add(new Parameter("reqnum",reqnum.ToString()));
            paras.Add(new Parameter("lastid",lastid.ToString()));


            return base.SyncRequest(TypeOption.TXWB_STATUSES_BROADCAST_TL,paras,null);
        }

        /// <summary>特别收听的人发表时间线
        /// 
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="pageflag">The pageflag.</param>
        /// <param name="pagetime">The pagetime.</param>
        /// <param name="reqnum">The reqnum.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string special_timeline(int pageflag, int pagetime, int reqnum)
        {
            List<Parameter> paras = new List<Parameter>();

            paras.Add(new Parameter("format",format));
            paras.Add(new Parameter("pageflag",pageflag.ToString()));
            paras.Add(new Parameter("pagetime",pagetime.ToString()));
            paras.Add(new Parameter("reqnum",reqnum.ToString()));


            return base.SyncRequest(TypeOption.TXWB_STATUSES_SPECIAL_TL,paras,null);
        }

        /// <summary>地区发表时间线
        /// Area_timelines the specified format.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="pageflag">The pageflag.</param>
        /// <param name="pagetime">The pagetime.</param>
        /// <param name="reqnum">The reqnum.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string area_timeline(int pos, int reqnum,
                int country, int province, int city)
        {
            List<Parameter> paras = new List<Parameter>();

            paras.Add(new Parameter("format",format));
            paras.Add(new Parameter("pos",pos.ToString()));
            paras.Add(new Parameter("reqnum",reqnum.ToString()));
            paras.Add(new Parameter("country",country.ToString()));
            paras.Add(new Parameter("province",province.ToString()));
            paras.Add(new Parameter("city",city.ToString()));



            return base.SyncRequest(TypeOption.TXWB_STATUSES_AREA_TL,paras,null);
        }

       
        /// <summary>主页时间线索引
        /// 
        /// </summary>
        /// <param name="pageflag">The pageflag.</param>
        /// <param name="pagetime">The pagetime.</param>
        /// <param name="reqnum">The reqnum.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string home_timeline_ids(int pageflag, int pagetime, int reqnum)
        {
            List<Parameter> paras = new List<Parameter>();

            paras.Add(new Parameter("format",format));
            paras.Add(new Parameter("pageflag",pageflag.ToString()));
            paras.Add(new Parameter("pagetime",pagetime.ToString()));
            paras.Add(new Parameter("reqnum",reqnum.ToString()));


            return base.SyncRequest(TypeOption.TXWB_STATUSES_HOME_TL_IDS,paras,null);
        }

        /// <summary>其他用户发表时间线索引
        /// 
        /// </summary>
        /// <param name="pageflag">The pageflag.</param>
        /// <param name="pagetime">The pagetime.</param>
        /// <param name="reqnum">The reqnum.</param>
        /// <param name="lastid">The lastid.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string user_timeline_ids(int pageflag, int pagetime,
                            int reqnum,int lastid, string name)
        {
            List<Parameter> paras = new List<Parameter>();

            paras.Add(new Parameter("format",format));
            paras.Add(new Parameter("pageflag",pageflag.ToString()));
            paras.Add(new Parameter("pagetime",pagetime.ToString()));
            paras.Add(new Parameter("reqnum",reqnum.ToString()));
            paras.Add(new Parameter("lastid",lastid.ToString()));
            paras.Add(new Parameter("name",name.ToString()));


            return base.SyncRequest(TypeOption.TXWB_STATUSES_USER_TL_IDS,paras,null);
        }

        /// <summary>我发表时间线索引
        /// 
        /// </summary>
        /// <param name="pageflag">The pageflag.</param>
        /// <param name="pagetime">The pagetime.</param>
        /// <param name="reqnum">The reqnum.</param>
        /// <param name="lastid">The lastid.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string broadcast_timeline_ids(int pageflag, int pagetime,
                            int reqnum, int lastid)
        {
            List<Parameter> paras = new List<Parameter>();

            paras.Add(new Parameter("format",format));
            paras.Add(new Parameter("pageflag",pageflag.ToString()));
            paras.Add(new Parameter("pagetime",pagetime.ToString()));
            paras.Add(new Parameter("reqnum",reqnum.ToString()));
            paras.Add(new Parameter("lastid",lastid.ToString()));


            return base.SyncRequest(TypeOption.TXWB_STATUSES_BROADCAST_TL_IDS,paras,null);

        }

        /// <summary>用户提及时间线索引
        /// 
        /// </summary>
        /// <param name="pageflag">The pageflag.</param>
        /// <param name="pagetime">The pagetime.</param>
        /// <param name="reqnum">The reqnum.</param>
        /// <param name="lastid">The lastid.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string mentions_timeline_ids(int pageflag, int pagetime, 
                        int reqnum, int lastid)
        {
            List<Parameter> paras = new List<Parameter>();

            paras.Add(new Parameter("format",format));
            paras.Add(new Parameter("pageflag",pageflag.ToString()));
            paras.Add(new Parameter("pagetime",pagetime.ToString()));
            paras.Add(new Parameter("reqnum",reqnum.ToString()));
            paras.Add(new Parameter("lastid",lastid.ToString()));

            return base.SyncRequest(TypeOption.TXWB_STATUSES_MENTIONS_TL_IDS,paras,null);

        }

        /// <summary>多用户发表时间线
        /// 
        /// </summary>
        /// <param name="pageflag">The pageflag.</param>
        /// <param name="pagetime">The pagetime.</param>
        /// <param name="reqnum">The reqnum.</param>
        /// <param name="lastid">The lastid.</param>
        /// <param name="names">The names.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string users_timeline(int pageflag, int pagetime,
                        int reqnum, int lastid, string names)
        {
            List<Parameter> paras = new List<Parameter>();

            paras.Add(new Parameter("format",format));
            paras.Add(new Parameter("pageflag",pageflag.ToString()));
            paras.Add(new Parameter("pagetime",pagetime.ToString()));
            paras.Add(new Parameter("reqnum",reqnum.ToString()));
            paras.Add(new Parameter("lastid",lastid.ToString()));
            paras.Add(new Parameter("names",names));


            return base.SyncRequest(TypeOption.TXWB_STATUSES_USERS_TL,paras,null);

        }

        /// <summary>多用户发表时间线索引
        /// 
        /// </summary>
        /// <param name="pageflag">The pageflag.</param>
        /// <param name="pagetime">The pagetime.</param>
        /// <param name="reqnum">The reqnum.</param>
        /// <param name="lastid">The lastid.</param>
        /// <param name="names">The names.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string users_timeline_ids(int pageflag, int pagetime,
                        int reqnum, int lastid, string names)
        {
            List<Parameter> paras = new List<Parameter>();

            paras.Add(new Parameter("format",format));
            paras.Add(new Parameter("pageflag",pageflag.ToString()));
            paras.Add(new Parameter("pagetime",pagetime.ToString()));
            paras.Add(new Parameter("reqnum",reqnum.ToString()));
            paras.Add(new Parameter("lastid",lastid.ToString()));
            paras.Add(new Parameter("names",names));


            return base.SyncRequest(TypeOption.TXWB_STATUSES_USERS_TL_IDS ,paras,null);

        }

      

    }
}
