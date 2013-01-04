using System;
using System.Collections.Generic;
using System.Text;

namespace wojilu.weibo.Core.QQWeibo
{
     public class friends : QWeiboApiBase
    {
        /// <summary>构造函数 
        /// <see cref="friends"/> class.
        /// </summary>
        /// <param name="okey">The okey.</param>
        /// <param name="format">The format.</param>
        /// <remarks></remarks>
         public friends(OauthKey okey, string format) : base(okey, format) { }

        /// <summary>我的听众列表.
        /// 
        /// </summary>
        /// <param name="reqnum">The reqnum.</param>
        /// <param name="startindex">The startindex.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string fanslist(int reqnum, int startindex)
        {
            List<Parameter> paras = new List<Parameter>();

            paras.Add(new Parameter("format",format));
            paras.Add(new Parameter("reqnum",reqnum.ToString()));
            paras.Add(new Parameter("startindex",startindex.ToString()));
            
            return base.SyncRequest(TypeOption.TXWB_FRIENDS_FANSLIST,paras,null);
        }


        /// <summary>我收听的人列表.
        /// 
        /// </summary>
        /// <param name="reqnum">The reqnum.</param>
        /// <param name="startindex">The startindex.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string idollist(int reqnum, int startindex)
        {
            List<Parameter> paras = new List<Parameter>();

            paras.Add(new Parameter("format",format));
            paras.Add(new Parameter("reqnum",reqnum.ToString()));
            paras.Add(new Parameter("startindex",startindex.ToString()));
            
            return base.SyncRequest(TypeOption.TXWB_FRIENDS_IDOLLIST,paras,null);
        }

          /// <summary>黑名单列表.
          /// 
          /// </summary>
          /// <param name="reqnum">The reqnum.</param>
          /// <param name="startindex">The startindex.</param>
          /// <returns></returns>
          /// <remarks></remarks>
          public string blacklist(int reqnum, int startindex)
        {
            List<Parameter> paras = new List<Parameter>();

            paras.Add(new Parameter("format",format));
            paras.Add(new Parameter("reqnum",reqnum.ToString()));
            paras.Add(new Parameter("startindex",startindex.ToString()));
            
            return base.SyncRequest(TypeOption.TXWB_FRIENDS_BLACKLIST,paras,null);
        }


          /// <summary>
          /// 特别收听列表.
          /// </summary>
          /// <param name="reqnum">The reqnum.</param>
          /// <param name="startindex">The startindex.</param>
          /// <returns></returns>
          /// <remarks></remarks>
          public string speciallist(int reqnum, int startindex)
        {
            List<Parameter> paras = new List<Parameter>();

            paras.Add(new Parameter("format",format));
            paras.Add(new Parameter("reqnum",reqnum.ToString()));
            paras.Add(new Parameter("startindex",startindex.ToString()));
            
            return base.SyncRequest(TypeOption.TXWB_FRIENDS_SPECIALLIST,paras,null);
        }

          /// <summary> 收听某个用户
          /// </summary>
          /// <param name="name">The name.</param>
          /// <param name="clientip">The clientip.</param>
          /// <returns></returns>
          /// <remarks></remarks>
          public string add(string name, string clientip)
        {
            List<Parameter> paras = new List<Parameter>();

            paras.Add(new Parameter("format",format));
            paras.Add(new Parameter("name",name));
            paras.Add(new Parameter("clientip",clientip));
            
            return base.SyncRequest(TypeOption.TXWB_FRIENDS_ADD,paras,null);
        }

         /// <summary>取消收听某个用户.
         /// 
         /// </summary>
         /// <param name="name">The name.</param>
         /// <returns></returns>
         /// <remarks></remarks>
         public string del(string name)
        {
            List<Parameter> paras = new List<Parameter>();

            paras.Add(new Parameter("format",format));
            paras.Add(new Parameter("name",name));
                       
            return base.SyncRequest(TypeOption.TXWB_FRIENDS_DEL,paras,null);
        }

         /// <summary>特别收听某个用户.
         /// 
         /// </summary>
         /// <param name="name">The name.</param>
         /// <returns></returns>
         /// <remarks></remarks>
         public string addspecial(string name)
        {
            List<Parameter> paras = new List<Parameter>();

            paras.Add(new Parameter("format",format));
            paras.Add(new Parameter("name",name));
                       
            return base.SyncRequest(TypeOption.TXWB_FRIENDS_ADDSPECIAL,paras,null);
        }


         /// <summary>取消特别收听某个用户.
         /// 
         /// </summary>
         /// <param name="name">The name.</param>
         /// <returns></returns>
         /// <remarks></remarks>
         public string delspecial(string name)
        {
            List<Parameter> paras = new List<Parameter>();

            paras.Add(new Parameter("format",format));
            paras.Add(new Parameter("name",name));
                       
            return base.SyncRequest(TypeOption.TXWB_FRIENDS_DELSPECIAL,paras,null);
        }



         /// <summary>添加某个用户到黑名单.
         /// 
         /// </summary>
         /// <param name="name">The name.</param>
         /// <returns></returns>
         /// <remarks></remarks>
         public string addblacklist(string name)
        {
            List<Parameter> paras = new List<Parameter>();

            paras.Add(new Parameter("format",format));
            paras.Add(new Parameter("name",name));
                       
            return base.SyncRequest(TypeOption.TXWB_FRIENDS_ADDBLACKLIST,paras,null);
        }

         /// <summary>从黑名单中删除某个用户.
         /// 
         /// </summary>
         /// <param name="name">The name.</param>
         /// <returns></returns>
         /// <remarks></remarks>
         public string delblacklist(string name)
        {
            List<Parameter> paras = new List<Parameter>();

            paras.Add(new Parameter("format",format));
            paras.Add(new Parameter("name",name));
                       
            return base.SyncRequest(TypeOption.TXWB_FRIENDS_DELBLACKLIST,paras,null);
        }


          /// <summary>检测是否我的听众或收听的人.
          /// 
          /// </summary>
          /// <param name="names">The names.</param>
          /// <param name="flag">The flag.</param>
          /// <returns></returns>
          /// <remarks></remarks>
          public string check(string names,int flag)
        {
            List<Parameter> paras = new List<Parameter>();

            paras.Add(new Parameter("format",format));
            paras.Add(new Parameter("names",names));
            paras.Add(new Parameter("flag",flag.ToString()));
                       
            return base.SyncRequest(TypeOption.TXWB_FRIENDS_CHECK,paras,null);
        }

          /// <summary>其他帐户听众列表.
          /// 
          /// </summary>
          /// <param name="reqnum">The reqnum.</param>
          /// <param name="startindex">The startindex.</param>
          /// <param name="name">The name.</param>
          /// <returns></returns>
          /// <remarks></remarks>
          public string user_fanslist(int reqnum, int startindex, string name)
        {
            List<Parameter> paras = new List<Parameter>();

            paras.Add(new Parameter("format",format));
            paras.Add(new Parameter("reqnum",reqnum.ToString()));
            paras.Add(new Parameter("startindex",startindex.ToString()));
            paras.Add(new Parameter("name",name));
            
            return base.SyncRequest(TypeOption.TXWB_FRIENDS_USER_FANSLIST,paras,null);
        }

         /// <summary>其他帐户收听的人列表
         /// 
         /// </summary>
         /// <param name="reqnum">The reqnum.</param>
         /// <param name="startindex">The startindex.</param>
         /// <param name="name">The name.</param>
         /// <returns></returns>
         /// <remarks></remarks>
         public string user_idollist(int reqnum, int startindex, string name)
        {
            List<Parameter> paras = new List<Parameter>();

            paras.Add(new Parameter("format",format));
            paras.Add(new Parameter("reqnum",reqnum.ToString()));
            paras.Add(new Parameter("startindex",startindex.ToString()));
            paras.Add(new Parameter("name",name));
            
            return base.SyncRequest(TypeOption.TXWB_FRIENDS_USER_IDOLLIST,paras,null);
        }

         /// <summary>其他帐户特别收听的人列表.
         /// 
         /// </summary>
         /// <param name="reqnum">The reqnum.</param>
         /// <param name="startindex">The startindex.</param>
         /// <param name="name">The name.</param>
         /// <returns></returns>
         /// <remarks></remarks>
         public string user_speciallist(int reqnum, int startindex, string name)
        {
            List<Parameter> paras = new List<Parameter>();

            paras.Add(new Parameter("format",format));
            paras.Add(new Parameter("reqnum",reqnum.ToString()));
            paras.Add(new Parameter("startindex",startindex.ToString()));
            paras.Add(new Parameter("name",name));
            
            return base.SyncRequest(TypeOption.TXWB_FRIENDS_USER_SPECIALLIST,paras,null);
        }



    }
}
