using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Controller.Common.Caching;
using wojilu.Apps.Forum.Domain;
using wojilu.Members.Sites.Domain;
using wojilu.Members.Interface;

namespace wojilu.Web.Controller.Forum.Caching {

    // 发布主题的时候，实际topic和post都插入了，所以两个interceptor都会运行，也就是说ViewCacher会重复运行……
    public class ForumTopicInterceptor : IEntityInterceptor {

        public void Insert( IEntity entity ) {
            updatePost( (ForumTopic)entity );
        }

        public void Update( IEntity entity ) {
            updatePost( (ForumTopic)entity );
        }

        public void Delete( IEntity entity ) {
            updatePost( (ForumTopic)entity );
        }

        public void UpdateBatch( Type t, string action, string condition ) {
        }

        public void DeleteBatch( Type t, string condition ) {
        }

        private static void updatePost( ForumTopic post ) {

            if (post.OwnerType != typeof( Site ).FullName) return;

            IMember owner = Site.Instance;
            int appId = post.AppId;

            IndexViewCacher.Update( owner, appId );
            BoardViewCacher.Update( owner, appId, post.ForumBoard.Id, 1 );
            RecentViewCacher.Update( owner, appId, "Topic" );

        }

    }


    public class ForumPostInterceptor : IEntityInterceptor {

        public void Insert( IEntity entity ) {
            updatePost( (ForumPost)entity );
        }

        public void Update( IEntity entity ) {
            updatePost( (ForumPost)entity );
        }

        public void Delete( IEntity entity ) {
            updatePost( (ForumPost)entity );
        }

        public void UpdateBatch( Type t, string action, string condition ) {
        }

        public void DeleteBatch( Type t, string condition ) {
        }

        private static void updatePost( ForumPost post ) {

            if (post.OwnerType != typeof( Site ).FullName) return;

            IMember owner = Site.Instance;
            int appId = post.AppId;

            IndexViewCacher.Update( owner, appId );
            BoardViewCacher.Update( owner, appId, post.ForumBoardId, 1 );
            RecentViewCacher.Update( owner, appId, "Post" );
        }

    }



    public class ForumBoardInterceptor : IEntityInterceptor {

        public void Insert( IEntity entity ) {
            updatePost( (ForumBoard)entity );
        }

        public void Update( IEntity entity ) {
            updatePost( (ForumBoard)entity );
        }

        public void Delete( IEntity entity ) {
            updatePost( (ForumBoard)entity );
        }

        public void UpdateBatch( Type t, string action, string condition ) {
        }

        public void DeleteBatch( Type t, string condition ) {
        }

        private static void updatePost( ForumBoard post ) {

            if (post.OwnerType != typeof( Site ).FullName) return;

            IMember owner = Site.Instance;
            int appId = post.AppId;

            //LayoutViewCacher.Update( owner, appId );
            IndexViewCacher.Update( owner, appId );
        }

    }


    public class ForumAppInterceptor : IEntityInterceptor {

        public void Insert( IEntity entity ) {
            updatePost( (ForumApp)entity );
        }

        public void Update( IEntity entity ) {
            updatePost( (ForumApp)entity );
        }

        public void Delete( IEntity entity ) {
            updatePost( (ForumApp)entity );
        }

        public void UpdateBatch( Type t, string action, string condition ) {
        }

        public void DeleteBatch( Type t, string condition ) {
        }

        private static void updatePost( ForumApp app ) {

            if (app.OwnerType != typeof( Site ).FullName) return;

            IMember owner = Site.Instance;

            //LayoutViewCacher.Update( owner, appId );
            IndexViewCacher.Update( owner, app.Id );
        }

    }


    public class ForumLinkInterceptor : IEntityInterceptor {

        public void Insert( IEntity entity ) {
            updatePost( (ForumLink)entity );
        }

        public void Update( IEntity entity ) {
            updatePost( (ForumLink)entity );
        }

        public void Delete( IEntity entity ) {
            updatePost( (ForumLink)entity );
        }

        public void UpdateBatch( Type t, string action, string condition ) {
        }

        public void DeleteBatch( Type t, string condition ) {
        }

        private static void updatePost( ForumLink post ) {

            if (post.OwnerType != typeof( Site ).FullName) return;

            IMember owner = Site.Instance;
            int appId = post.AppId;

            //LayoutViewCacher.Update( owner, appId );
            IndexViewCacher.Update( owner, appId );
        }

    }


}
