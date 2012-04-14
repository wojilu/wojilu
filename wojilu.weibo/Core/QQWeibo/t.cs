using System;
using System.Collections.Generic;
using System.Text;

namespace wojilu.weibo.Core.QQWeibo
{
    /// <summary>微博相关
    /// 
    /// </summary>
    /// <remarks></remarks>
    public  class t : QWeiboApiBase
    {
        /// <summary>构造函数
        ///  <see cref="t"/> class.
        /// </summary>
        /// <param name="okey">The okey.</param>
        /// <param name="format">The format.</param>
        /// <remarks></remarks>
        public t(OauthKey okey, string format) : base(okey, format) { }


   
        /// <summary>获取一条微博数据
        /// 
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string show(int id)
        {
            List<Parameter> paras = new List<Parameter>();

            paras.Add(new Parameter("format",format));
            paras.Add(new Parameter("id",id.ToString()));
            
            return base.SyncRequest(TypeOption.TXWB_T_SHOW,paras,null);
        }

        /// <summary>发表一条微博
        /// 
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="clientip">The clientip.</param>
        /// <param name="jing">The jing.</param>
        /// <param name="wei">The wei.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string add(string content, string clientip, string jing, string wei)
        {
            List<Parameter> paras = new List<Parameter>();

            paras.Add(new Parameter("format",format));
            paras.Add(new Parameter("content",content));
            paras.Add(new Parameter("clientip",clientip));
            paras.Add(new Parameter("jing",jing));
            paras.Add(new Parameter("wei",wei));

            return base.SyncRequest(TypeOption.TXWB_T_ADD,paras,null);
        }


        /// <summary>删除一条微博
        /// 
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string del(string id)
        {
            List<Parameter> paras = new List<Parameter>();

            paras.Add(new Parameter("format",format));
            paras.Add(new Parameter("id",id));
            
            return base.SyncRequest(TypeOption.TXWB_T_DEL,paras,null);
        }


        /// <summary>转播一条微博
        /// 
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="clientip">The clientip.</param>
        /// <param name="jing">The jing.</param>
        /// <param name="wei">The wei.</param>
        /// <param name="reid">The reid.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string re_add(string content, string clientip, string jing, string wei,string reid)
        {
            List<Parameter> paras = new List<Parameter>();

            paras.Add(new Parameter("format",format));
            paras.Add(new Parameter("content",content));
            paras.Add(new Parameter("clientip",clientip));
            paras.Add(new Parameter("jing",jing));
            paras.Add(new Parameter("wei",wei));
            paras.Add(new Parameter("reid",reid));

            return base.SyncRequest(TypeOption.TXWB_T_RE_ADD,paras,null);
        }

        /// <summary>回复一条微博
        /// 
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="clientip">The clientip.</param>
        /// <param name="jing">The jing.</param>
        /// <param name="wei">The wei.</param>
        /// <param name="reid">The reid.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string reply(string content, string clientip, string jing, string wei, string reid)
        {
            List<Parameter> paras = new List<Parameter>();

            paras.Add(new Parameter("format",format));
            paras.Add(new Parameter("content",content));
            paras.Add(new Parameter("clientip",clientip));
            paras.Add(new Parameter("jing",jing));
            paras.Add(new Parameter("wei",wei));
            paras.Add(new Parameter("reid",reid));

            return base.SyncRequest(TypeOption.TXWB_T_REPLY,paras,null);
        }

        /// <summary>发表一条带图片的微博
        /// 
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="clientip">The clientip.</param>
        /// <param name="jing">The jing.</param>
        /// <param name="wei">The wei.</param>
        /// <param name="pic">The pic.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string add_pic(string content, string clientip, string jing, 
                        string wei,string pic)
        {
            List<Parameter> paras = new List<Parameter>();
            List<Parameter> files = new List<Parameter>();

            paras.Add(new Parameter("format",format));
            paras.Add(new Parameter("content",content));
            paras.Add(new Parameter("clientip",clientip));
            paras.Add(new Parameter("jing",jing));
            paras.Add(new Parameter("wei",wei));

            files.Add(new Parameter("pic",pic));


            return base.SyncRequest(TypeOption.TXWB_T_ADD_PIC,paras,files);
        }

        /// <summary>转播数或点评数
        /// 
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <param name="flag">The flag.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string re_count(string ids, int flag)
        {
            List<Parameter> paras = new List<Parameter>();

            paras.Add(new Parameter("format",format));
            paras.Add(new Parameter("ids",ids));
            paras.Add(new Parameter("flag",flag.ToString()));
           

            return base.SyncRequest(TypeOption.TXWB_T_RE_COUNT,paras,null);
        }

        /// <summary>获取单条微博的转发或点评列表
        /// 
        /// </summary>
        /// <param name="flag">The flag.</param>
        /// <param name="rootid">The rootid.</param>
        /// <param name="pageflag">The pageflag.</param>
        /// <param name="reqnum">The reqnum.</param>
        /// <param name="twitterid">The twitterid.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string re_list(int flag, string rootid, int pageflag, 
                                int reqnum, int twitterid)
        {
            List<Parameter> paras = new List<Parameter>();

            paras.Add(new Parameter("format",format));
            paras.Add(new Parameter("flag",flag.ToString()));
            paras.Add(new Parameter("rootid",rootid));
            paras.Add(new Parameter("pageflag",pageflag.ToString()));
            paras.Add(new Parameter("reqnum",reqnum.ToString()));
            paras.Add(new Parameter("twitterid",twitterid.ToString()));
            

            return base.SyncRequest(TypeOption.TXWB_T_RE_LIST,paras,null);
        }

        /// <summary>点评一条微博
        /// 
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="clientip">The clientip.</param>
        /// <param name="jing">The jing.</param>
        /// <param name="wei">The wei.</param>
        /// <param name="reid">The reid.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string comment(string content, string clientip, string jing, string wei,string reid)
        {
            List<Parameter> paras = new List<Parameter>();

            paras.Add(new Parameter("format",format));
            paras.Add(new Parameter("content",content));
            paras.Add(new Parameter("clientip",clientip));
            paras.Add(new Parameter("jing",jing));
            paras.Add(new Parameter("wei",wei));
            paras.Add(new Parameter("reid",reid));

            return base.SyncRequest(TypeOption.TXWB_T_COMMENT,paras,null);
        }

        /// <summary>发表音乐微博
        /// 
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="clientip">The clientip.</param>
        /// <param name="jing">The jing.</param>
        /// <param name="wei">The wei.</param>
        /// <param name="url">The URL.</param>
        /// <param name="title">The title.</param>
        /// <param name="author">The author.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string add_music(string content, string clientip, string jing, 
                    string wei, string url, string title, string author)
        {
            List<Parameter> paras = new List<Parameter>();

            paras.Add(new Parameter("format",format));
            paras.Add(new Parameter("content",content));
            paras.Add(new Parameter("clientip",clientip));
            paras.Add(new Parameter("jing",jing));
            paras.Add(new Parameter("wei",wei));
            paras.Add(new Parameter("url",url));
            paras.Add(new Parameter("title",title));
            paras.Add(new Parameter("author",author));

            return base.SyncRequest(TypeOption.TXWB_T_ADD_MUSIC,paras,null);
        }

        /// <summary>发表视频微博
        /// 
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="clientip">The clientip.</param>
        /// <param name="jing">The jing.</param>
        /// <param name="wei">The wei.</param>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string add_video(string content, string clientip, string jing,
                    string wei,string url)
        {
            List<Parameter> paras = new List<Parameter>();

            paras.Add(new Parameter("format",format));
            paras.Add(new Parameter("content",content));
            paras.Add(new Parameter("clientip",clientip));
            paras.Add(new Parameter("jing",jing));
            paras.Add(new Parameter("wei",wei));
            paras.Add(new Parameter("url",url));

            return base.SyncRequest(TypeOption.TXWB_T_ADD_VIDEO,paras,null);
        }

        /// <summary>获取视频信息
        /// 
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string getvideoinfo(string url)
        {
            List<Parameter> paras = new List<Parameter>();

            paras.Add(new Parameter("format",format));
            paras.Add(new Parameter("url",url));
            

            return base.SyncRequest(TypeOption.TXWB_T_GETVIDEOINFO,paras,null);
        }

        /// <summary>根据微博ID批量获取微博内容（与索引合起来用）
        /// 
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string list(string ids)
        {
            List<Parameter> paras = new List<Parameter>();

            paras.Add(new Parameter("format",format));
            paras.Add(new Parameter("ids",ids));
            

            return base.SyncRequest(TypeOption.TXWB_T_LIST,paras,null);
        }

       


    }
}
