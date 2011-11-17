using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Apps.Blog.Domain;
using wojilu.Members.Users.Domain;
using wojilu.Apps.Blog.Caching;

namespace wojilu.Web.Controller.Common.Cache.Updaters {

    public class BlogPostCacher : IUpdater {

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

            LayoutCacher.Update( owner, appId );
            HomeCacher.Update( owner, appId );
            MainCacher.Update( appId );
        }



    }

}
