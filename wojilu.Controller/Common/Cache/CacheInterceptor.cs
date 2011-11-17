using System;
using System.Collections.Generic;
using System.Text;
using wojilu.ORM;
using wojilu.Apps.Blog.Domain;
using wojilu.Web.Mvc;
using wojilu.Web.Controller.Common.Cache.Updaters;

namespace wojilu.Web.Controller.Common {

    public interface IUpdater{
        void Insert( IEntity entity );
        void Update( IEntity entity );
        void Delete( IEntity entity );
    }

    public class CacheUpdater {

        private static Dictionary<String, IUpdater> dic = getMaps();

        private static Dictionary<String, IUpdater> getMaps() {

            Dictionary<String, IUpdater> maps = new Dictionary<String, IUpdater>();

            maps.Add( typeof( BlogPost ).FullName, new BlogPostCacher() );

            return maps;
        }

        public static Dictionary<String, IUpdater> GetMap() {
            return dic;
        }

        public static void AddUpdater( String typeName, IUpdater obj ) {
            dic.Add( typeName, obj );
        }

        public static IUpdater Get( IEntity entity ) {

            IUpdater obj;
            dic.TryGetValue( entity.GetType().FullName, out obj );
            return obj;
        }
    }

    public class CacheInterceptor : IInterceptor {

        
        public void AfterInsert( IEntity entity ) {

            IUpdater obj = CacheUpdater.Get( entity );
            if (obj != null) obj.Insert( entity );
        }

        public void AfterUpdate( IEntity entity ) {
            IUpdater obj = CacheUpdater.Get( entity );
            if (obj != null) obj.Update( entity );
        }

        public void AfterDelete( IEntity entity ) {
            IUpdater obj = CacheUpdater.Get( entity );
            if (obj != null) obj.Delete( entity );
        }

        //----------------------------------------------------------

        public void BeforInsert( IEntity entity ) {
        }

        public void BeforUpdate( IEntity entity ) {
        }

        public void BeforDelete( IEntity entity ) {
        }



    }

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

}
