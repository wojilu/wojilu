using System;
using System.Collections.Generic;
using System.Text;

namespace wojilu.weibo.Core.QQWeibo
{
     public class info : QWeiboApiBase
    {
         /// <summary>数据更新相关 
         /// 构造函数 <see cref="info"/> class.
         /// </summary>
         /// <param name="okey">The okey.</param>
        /// <param name="format">The format.</param>
        /// <remarks></remarks>
        public info(OauthKey okey, string format) : base(okey, format) { }

        /// <summary>查看数据更新条数 只请求更新数，不清除更新数
        /// 
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public string update()
        {
            List<Parameter> paras = new List<Parameter>();

            int op = 0;

            paras.Add(new Parameter("format",format));
            paras.Add(new Parameter("op",op.ToString()));
            
            return base.SyncRequest(TypeOption.TXWB_INFO_UPDATE,paras,null);
        }

         /// <summary>查看数据更新条数  请求更新数，并对更新数清零
         /// 
         /// </summary>
         /// <param name="type">The type.</param>
         /// <returns></returns>
         /// <remarks></remarks>
         public string update(int type)
        {
            List<Parameter> paras = new List<Parameter>();
            int op = 1;

            paras.Add(new Parameter("format",format));
            paras.Add(new Parameter("op",op.ToString()));
            paras.Add(new Parameter("type",type.ToString()));
            
            return base.SyncRequest(TypeOption.TXWB_INFO_UPDATE,paras,null);
        }
    }
}
