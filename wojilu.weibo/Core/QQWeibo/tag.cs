using System;
using System.Collections.Generic;
using System.Text;

namespace wojilu.weibo.Core.QQWeibo
{
    /// <summary>标签相关
    /// 
    /// </summary>
    /// <remarks></remarks>
    public class tag :QWeiboApiBase
    {
        /// <summary>
        /// 构造函数 <see cref="tag"/> class.
        /// </summary>
        /// <param name="okey">The okey.</param>
        /// <param name="format">The format.</param>
        /// <remarks></remarks>
        public tag(OauthKey okey, string format) : base(okey, format) { }

        /// <summary> 添加标签
        /// 
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string add(string tag)
        {
            List<Parameter> paras = new List<Parameter>();

            paras.Add(new Parameter("format",format));
            paras.Add(new Parameter("tag",tag));
            
            return base.SyncRequest(TypeOption.TXWB_TAG_ADD,paras,null);
        }

        /// <summary>删除标签
        /// 
        /// </summary>
        /// <param name="tagid">The tagid.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string del(string tagid)
        {
            List<Parameter> paras = new List<Parameter>();

            paras.Add(new Parameter("format",format));
            paras.Add(new Parameter("ids",tagid));
            
            return base.SyncRequest(TypeOption.TXWB_TAG_DEL,paras,null);
        }
    }
}
