using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Controller.Blog.Caching;
using wojilu.Web.Controller.Content.Caching;
using wojilu.Web.Controller.Forum.Caching;

namespace wojilu.Web.Controller.Common.Caching {

    public class CacheFilterDB {

        private CacheFilterDB() {
        }

        public static readonly CacheFilterDB Instance = getDb();

        public List<ICacheFilter> Renews { get; set; }

        private static CacheFilterDB getDb() {

            List<ICacheFilter> dbs = new List<ICacheFilter>();

            dbs.Add( new SiteCacheFilter() );

            dbs.Add( new BlogIndexFilter() );
            dbs.Add( new BlogLayoutFilter() );
            dbs.Add( new BlogMainFilter() );

            dbs.Add( new ContentIndexFilter() );
            dbs.Add( new ContentLayoutFilter() );

            dbs.Add( new ForumIndexFilter() );
            dbs.Add( new ForumBoardFilter() );
            dbs.Add( new ForumRecentFilter() );


            CacheFilterDB db = new CacheFilterDB();
            db.Renews = dbs;

            return db;

        }

    }

}
