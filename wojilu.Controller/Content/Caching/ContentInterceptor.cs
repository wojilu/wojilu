//using System;
//using System.Collections.Generic;
//using System.Text;
//using wojilu.Web.Controller.Common.Caching;
//using wojilu.Apps.Content.Domain;
//using wojilu.Members.Users.Domain;
//using wojilu.Members.Interface;
//using wojilu.DI;
//using wojilu.Members.Sites.Domain;

//namespace wojilu.Web.Controller.Content.Caching {

//    public class ContentPostInterceptor : IEntityInterceptor {

//        public void Insert( IEntity entity ) {
//            updatePost( (ContentPost)entity );
//        }

//        public void Update( IEntity entity ) {
//            updatePost( (ContentPost)entity );
//        }

//        public void Delete( IEntity entity ) {
//            updatePost( (ContentPost)entity );
//        }

//        public void UpdateBatch( Type t, string action, string condition ) {
//        }

//        public void DeleteBatch( Type t, string condition ) {
//        }

//        private static void updatePost( ContentPost post ) {

//            if (post.OwnerType != typeof( Site ).FullName) return;

//            IMember owner = Site.Instance;
//            int appId = post.AppId;

//            LayoutViewCacher.Update( owner, appId );
//            IndexViewCacher.Update( owner, appId );
//        }

//    }

//    public class ContentSectionInterceptor : IEntityInterceptor {

//        public void Insert( IEntity entity ) {
//            updatePost( (ContentSection)entity );
//        }

//        public void Update( IEntity entity ) {
//            updatePost( (ContentSection)entity );
//        }

//        public void Delete( IEntity entity ) {
//            updatePost( (ContentSection)entity );
//        }

//        public void UpdateBatch( Type t, string action, string condition ) {
//        }

//        public void DeleteBatch( Type t, string condition ) {
//        }

//        private static void updatePost( ContentSection section ) {

//            ContentApp app = ContentApp.findById( section.AppId );

//            IMember owner = ndb.findById( ObjectContext.Instance.TypeList[app.OwnerType], app.OwnerId ) as IMember;
//            int appId = app.Id;

//            LayoutViewCacher.Update( owner, appId );
//            IndexViewCacher.Update( owner, appId );
//        }

//    }

//}
