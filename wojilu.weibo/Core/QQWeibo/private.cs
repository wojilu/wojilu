using System;
using System.Collections.Generic;
using System.Text;

namespace wojilu.weibo.Core.QQWeibo
{
     public class @private : QWeiboApiBase
    {
         /// <summary> 私信相关
         /// 构造函数 <see cref="@private"/> class.
         /// </summary>
         /// <param name="okey">The okey.</param>
        /// <param name="format">The format.</param>
        /// <remarks></remarks>
        public @private(OauthKey okey, string format) : base(okey, format) { }


         /// <summary> 发私信
         /// 
         /// </summary>
         /// <param name="content">The content.</param>
         /// <param name="clientip">The clientip.</param>
         /// <param name="jing">The jing.</param>
         /// <param name="wei">The wei.</param>
         /// <param name="name">The name.</param>
         /// <returns></returns>
         /// <remarks></remarks>
         public string add(string content, string clientip, string jing, 
                        string wei,string name)
        {
            List<Parameter> paras = new List<Parameter>();

            paras.Add(new Parameter("format",format));
            paras.Add(new Parameter("content",content));
            paras.Add(new Parameter("clientip",clientip));
            paras.Add(new Parameter("jing",jing));
            paras.Add(new Parameter("wei",wei));
            paras.Add(new Parameter("name",name));

            return base.SyncRequest(TypeOption.TXWB_PRIVATE_ADD,paras,null);
        }

         /// <summary> 删除私信
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
            
            return base.SyncRequest(TypeOption.TXWB_PRIVATE_DEL,paras,null);
        }

          /// <summary> 收件箱
          /// 
          /// </summary>
          /// <param name="pageflag">The pageflag.</param>
          /// <param name="pagetime">The pagetime.</param>
          /// <param name="reqnum">The reqnum.</param>
          /// <param name="lastid">The lastid.</param>
          /// <returns></returns>
          /// <remarks></remarks>
          public string recv(int pageflag, int pagetime, int reqnum, int lastid)
        {
            List<Parameter> paras = new List<Parameter>();

            paras.Add(new Parameter("format",format));
            paras.Add(new Parameter("pageflag",pageflag.ToString()));
            paras.Add(new Parameter("pagetime",pagetime.ToString()));
            paras.Add(new Parameter("reqnum",reqnum.ToString()));
            paras.Add(new Parameter("lastid",lastid.ToString()));

            return base.SyncRequest(TypeOption.TXWB_PRIVATE_RECV,paras,null);
        }

         /// <summary> 发件箱
         /// 
         /// </summary>
         /// <param name="pageflag">The pageflag.</param>
         /// <param name="pagetime">The pagetime.</param>
         /// <param name="reqnum">The reqnum.</param>
         /// <param name="lastid">The lastid.</param>
         /// <returns></returns>
         /// <remarks></remarks>
         public string send(int pageflag, int pagetime, int reqnum, int lastid)
        {
            List<Parameter> paras = new List<Parameter>();

            paras.Add(new Parameter("format",format));
            paras.Add(new Parameter("pageflag",pageflag.ToString()));
            paras.Add(new Parameter("pagetime",pagetime.ToString()));
            paras.Add(new Parameter("reqnum",reqnum.ToString()));
            paras.Add(new Parameter("lastid",lastid.ToString()));

            return base.SyncRequest(TypeOption.TXWB_PRIVATE_SEND,paras,null);
        }

    }
}
