using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Apps.Blog.Domain;
using wojilu.Members.Users.Domain;
using wojilu.Web.Controller.Common.Caching;
using wojilu.ORM;

namespace wojilu.Web.Controller.Blog.Caching {

    // TODO1 用户批量删除；后台批量删除
    // TODO2 博客的layout还依赖于UserMenu/SpaceVisit等信息
    public class BlogPostInterceptor : IEntityInterceptor {

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

            BlogLayoutViewCacher.Update( owner, appId );
            BlogHomeViewCacher.Update( owner, appId );
            BlogMainViewCacher.Update( appId );
        }

        public void UpdateBatch( Type t, string action, string condition ) { }
        public void DeleteBatch( Type t, string condition ) { }

    }


    public class BlogCategoryInterceptor : IEntityInterceptor {

        public void Insert( IEntity entity ) {
            updatePost( (BlogCategory)entity );
        }

        public void Update( IEntity entity ) {
            updatePost( (BlogCategory)entity );
        }

        public void Delete( IEntity entity ) {
            updatePost( (BlogCategory)entity );
        }


        private static void updatePost( BlogCategory post ) {
            User owner = User.findById( post.OwnerId );
            int appId = post.AppId;

            BlogLayoutViewCacher.Update( owner, appId );
        }

        public void UpdateBatch( Type t, string action, string condition ) { }
        public void DeleteBatch( Type t, string condition ) { }

    }


    public class BlogPostCommentInterceptor : IEntityInterceptor {

        public void Insert( IEntity entity ) {
            updatePost( (BlogPostComment)entity );
        }

        public void Update( IEntity entity ) {
            updatePost( (BlogPostComment)entity );
        }

        public void Delete( IEntity entity ) {
            updatePost( (BlogPostComment)entity );
        }


        private static void updatePost( BlogPostComment comment ) {

            BlogPost post = BlogPost.findById( comment.RootId );

            User owner = User.findById( post.OwnerId );
            int appId = post.AppId;

            BlogLayoutViewCacher.Update( owner, appId );
        }

        public void UpdateBatch( Type t, string action, string condition ) { }
        public void DeleteBatch( Type t, string condition ) { }

    }

}
