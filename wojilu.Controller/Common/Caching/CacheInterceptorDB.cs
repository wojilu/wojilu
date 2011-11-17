using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Apps.Blog.Domain;
using wojilu.Web.Controller.Blog.Caching;

namespace wojilu.Web.Controller.Common.Caching {

    public class CacheInterceptorDB {

        private static Dictionary<String, ICacheHelper> dic = getMaps();

        private static Dictionary<String, ICacheHelper> getMaps() {

            Dictionary<String, ICacheHelper> maps = new Dictionary<String, ICacheHelper>();

            maps.Add( typeof( BlogPost ).FullName, new BlogPostInterceptor() );
            maps.Add( typeof( BlogCategory ).FullName, new BlogCategoryInterceptor() );

            return maps;
        }

        public static Dictionary<String, ICacheHelper> GetMap() {
            return dic;
        }

        public static void AddUpdater( String typeName, ICacheHelper obj ) {
            dic.Add( typeName, obj );
        }

        public static ICacheHelper Get( IEntity entity ) {

            ICacheHelper obj;
            dic.TryGetValue( entity.GetType().FullName, out obj );
            return obj;
        }

        public static ICacheHelper Get( Type t ) {

            ICacheHelper obj;
            dic.TryGetValue( t.FullName, out obj );
            return obj;
        }

    }
}
