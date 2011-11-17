using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Apps.Blog.Domain;
using wojilu.Web.Controller.Common;
using wojilu.Web.Mvc;

namespace wojilu.Web.Controller.Blog {

    /// <summary>
    /// 修改(CRUD)数据之后，对缓存的处理
    /// 移除缓存：1）删除缓存项 2）标记缓存最后更新时间
    /// </summary>
    public class BlogCache : CacheRemoveBase {


        public BlogCache( ControllerBase controller ) {
            base.controller = controller;
            base.owner = controller.ctx.owner.obj;
        }

        // 搜索页不缓存

        public void AddPost( BlogPost post ) {

            // 全站首页缓存移除
            removeCacheSingle( controller.t2( new Blog.MainController().Index ) );

            // 全站首页列表标记过期
            String recentKey = "blog_site_recent";
            cacheHelper.SetTimestamp( recentKey, DateTime.Now );

            // 用户博客首页
            String homeKey = controller.Link.To( post.Creator, new BlogController().Index );
            removeCacheSingle( homeKey );

            // 检查是否生成新的列表页

            // 移除所在分类列表页
        }

        private void removeListPage( BlogPost post ) {

            // 

        }

        // TODO 不同的数据类型，有不同的列表项
        public void UpdatePost( BlogPost post ) {
            // 全站首页缓存移除
            // 全站首页列表所在页面移除
            // 博客列表页缓存移除

            // 移除所在列表页
            removeListPage( post );

            // 详细页缓存移除
        }

        public void DeletePost( BlogPost post ) {
            // 全站首页缓存移除(如果在首页)
            // 全站首页列表页标记过期
            // 用户博客列表页标记过期
            // 分类列表页标记过期
            // 详细页缓存移除
        }


    }

}
