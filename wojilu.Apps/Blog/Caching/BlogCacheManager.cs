using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Caching;
using wojilu.Web.Mvc.Utils;
using wojilu.Web.Context;
using wojilu.Web;
using wojilu.Apps.Blog.Domain;
using wojilu.Members.Users.Domain;
using wojilu.Web.Mvc.Routes;

namespace wojilu.Apps.Blog.Caching {

    public class BlogCacheManager {

        public static void AfterPostInsert( BlogPost post ) {
            updatePost( post );
        }


        public static void AfterPostUpdate( BlogPost post ) {
            updatePost( post );
        }

        public static void AfterPostDelete( BlogPost post ) {
            updatePost( post );
        }

        private static void updatePost( BlogPost post ) {
            User owner = User.findById( post.OwnerId );
            int appId = post.AppId;

            LayoutCacher.Update( owner, appId );
            HomeCacher.Update( owner, appId );
            MainCacher.Update( appId );
        }

        //--------------------------------------------------------------------------

        public static void AfterCategoryInsert( BlogCategory category ) {

            User owner = User.findById( category.OwnerId );
            int appId = category.AppId;

            LayoutCacher.Update( owner, appId );

        }

        public static void AfterCategoryUpdate( BlogCategory category ) {
        }

        public static void AfterCategoryDelete( BlogCategory category ) {
        }

        //--------------------------------------------------------------------------

        public void AfterCommentInsert( BlogPostComment comment ) {

            BlogPost post = BlogPost.findById( comment.RootId );
            User owner = User.findById( post.OwnerId );
            int appId = post.AppId;

            LayoutCacher.Update( owner, appId );


        }

        public void AfterCommentDelete( BlogPostComment comment ) {
        }

        //--------------------------------------------------------------------------



        public void AfterBlogrollInsert( Blogroll br ) {


            User owner = User.findById( br.OwnerId );
            int appId = br.AppId;

            LayoutCacher.Update( owner, appId );

        }

        public void AfterBlogrollUpdate() {
        }

        public void AfterBlogrollDelete() {
        }


    }

}
