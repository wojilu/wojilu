//using System;
//using System.Collections.Generic;
//using System.Text;
//using wojilu.Caching;
//using wojilu.Web.Mvc.Utils;
//using wojilu.Web.Context;
//using wojilu.Web;
//using wojilu.Apps.Blog.Domain;
//using wojilu.Members.Users.Domain;
//using wojilu.Web.Mvc.Routes;

//namespace wojilu.Apps.Blog.Caching {

//    public class BlogCacheManager {

//        public static void AfterPostInsert( BlogPost post ) {
//            updatePost( post );
//        }


//        public static void AfterPostUpdate( BlogPost post ) {
//            updatePost( post );
//        }

//        public static void AfterPostDelete( BlogPost post ) {
//            updatePost( post );
//        }

//        private static void updatePost( BlogPost post ) {
//            User owner = User.findById( post.OwnerId );
//            int appId = post.AppId;

//            LayoutCacher.Update( owner, appId );
//            HomeCacher.Update( owner, appId );
//            MainCacher.Update( appId );
//        }

//        //--------------------------------------------------------------------------

//        public static void AfterCategoryInsert( BlogCategory category ) {

//            User owner = User.findById( category.OwnerId );
//            int appId = category.AppId;

//            LayoutCacher.Update( owner, appId );

//        }

//        public static void AfterCategoryUpdate( BlogCategory category ) {
//        }

//        public static void AfterCategoryDelete( BlogCategory category ) {
//        }

//        //--------------------------------------------------------------------------

//        public void AfterCommentInsert( BlogPostComment comment ) {

//            BlogPost post = BlogPost.findById( comment.RootId );
//            User owner = User.findById( post.OwnerId );
//            int appId = post.AppId;

//            LayoutCacher.Update( owner, appId );


//        }

//        public void AfterCommentDelete( BlogPostComment comment ) {
//        }

//        //--------------------------------------------------------------------------



//        public void AfterBlogrollInsert( Blogroll br ) {


//            User owner = User.findById( br.OwnerId );
//            int appId = br.AppId;

//            LayoutCacher.Update( owner, appId );

//        }

//        public void AfterBlogrollUpdate() {
//        }

//        public void AfterBlogrollDelete() {
//        }


//    }


// 已过期：
// 拦截器，每当数据保存的时候，自动设置缓存过期
// DependencyList = dic.Add( typeof(LayoutController), List<String> ); ——每个布局页缓存依赖的更新操作
// 将DependencyList的一对多关系重排，转换成 orm action 和 LayoutController 之间的一对多关系 cacheUpdateList
// cacheUpdateList = dic.Add( "wojilu.BlogApp.BlogCategory.insert", typeof(LayoutController) );
// 并且这个 cacheUpdateList 是单例的。
// 得到cacheUpdateList之后，在Interceptor中，每次更新，检查当前的orm action所对应的所有layout，然后更新它的最后更新时间

// 除了 orm action，还有 CacheObject 以及其他持久对象的操作

//public class DependencyList {

//    private static Dictionary<Type, List<String>> pageToAction = new Dictionary<Type, List<String>>();
//    private static Dictionary<String, List<Type>> actionToPage = new Dictionary<String, List<Type>>();

//    public static void LoadData() {

//        List<Type> layoutControllers = new List<Type>();

//        foreach (Type t in layoutControllers) {

//            ControllerBase controller = rft.GetInstance( t ) as ControllerBase;

//            List<String> list = rft.CallMethod( controller, "getDependency" ) as List<String>;

//            pageToAction.Add( t, list );

//        }

//    }

//    public static void ResetData() {

//        foreach (KeyValuePair<Type, List<String>> kv in pageToAction) {

//            foreach (String action in kv.Value) {



//            }


//        }

//    }

//}

//}
