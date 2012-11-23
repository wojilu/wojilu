using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Context;

namespace wojilu.Web.Mvc {

    public class PageCache : IPageCache {

        protected List<Type> list = new List<Type>();

        /// <summary>
        /// 是否缓存。页面可能运行在不同环境下(比如不同的owner)，本方法决定哪些环境需要缓存，哪些不需要缓存。
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public virtual Boolean IsCache( MvcContext ctx ) {
            return true;
        }

        /// <summary>
        /// 获取所有相关的布局页面缓存。主要供框架调用
        /// </summary>
        /// <returns></returns>
        public virtual List<Type> GetRelatedActions() {

            if (list.Count == 0) {
                this.ObserveActionCaches();
            }
            return this.list;
        }

        /// <summary>
        /// 请在此方法中使用 observe 监控其他局部页面的 ActionCache
        /// </summary>
        public virtual void ObserveActionCaches() {
        }

        /// <summary>
        /// 一个页面由若干局部页面组成，本方法监控这些局部页面的cache
        /// </summary>
        /// <param name="t">被监控的 ActionCache 的 Type</param>
        protected void observe( Type t ) {
            if (list.Contains( t ) == false) list.Add( t );
        }

        /// <summary>
        /// 一旦其他局部页面的缓存发生变化，则 UpdateCache 方法被执行。本方法让缓存及时更新或失效
        /// </summary>
        /// <param name="ctx"></param>
        public virtual void UpdateCache( MvcContext ctx ) {
        }

        /// <summary>
        /// 网页内容被添加进缓存之后的后续动作
        /// </summary>
        /// <param name="ctx"></param>
        public virtual void AfterCachePage( MvcContext ctx ) {
        }

    }

}
