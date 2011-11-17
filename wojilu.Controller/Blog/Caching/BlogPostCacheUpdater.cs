using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Apps.Blog.Domain;
using wojilu.Members.Users.Domain;
using wojilu.Web.Controller.Common.Caching;

namespace wojilu.Web.Controller.Blog.Caching {

    public class BlogPostCacheUpdater : IUpdater {

        public void Insert( IEntity entity ) {
            updatePost( (BlogPost)entity );
        }

        public void Update( IEntity entity ) {
            updatePost( (BlogPost)entity );
        }

        public void Delete( IEntity entity ) {
            updatePost( (BlogPost)entity );
        }


        private static void updatePost( BlogPost post ) {
            User owner = User.findById( post.OwnerId );
            int appId = post.AppId;

            LayoutViewCacher.Update( owner, appId );
            HomeViewCacher.Update( owner, appId );
            MainViewCacher.Update( appId );
        }

    }

}
