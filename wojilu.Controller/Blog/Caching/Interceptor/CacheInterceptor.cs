using System;
using System.Collections.Generic;
using System.Text;
using wojilu.ORM;
using wojilu.Apps.Blog.Domain;
using wojilu.Web.Mvc;
using wojilu.Web.Controller.Blog.Caching;

namespace wojilu.Web.Controller.Common.Caching {

    public class CacheInterceptor : IInterceptor {

        public void AfterInsert( IEntity entity ) {

            IEntityInterceptor obj = EntityInterceptorDB.Get( entity );
            if (obj != null) obj.Insert( entity );
        }

        public void AfterUpdate( IEntity entity ) {
            IEntityInterceptor obj = EntityInterceptorDB.Get( entity );
            if (obj != null) obj.Update( entity );
        }

        public void AfterDelete( IEntity entity ) {
            IEntityInterceptor obj = EntityInterceptorDB.Get( entity );
            if (obj != null) obj.Delete( entity );
        }

        //----------------------------------------------------------

        public void AfterUpdateBatch( Type t, string action, string condition ) {
        }

        public void AfterDeleteBatch( Type t, string condition ) {
        }

        public void BeforInsert( IEntity entity ) {
        }

        public void BeforUpdate( IEntity entity ) {
        }

        public void BeforDelete( IEntity entity ) {
        }

        public void BeforUpdateBatch( Type t, string action, string condition ) {
        }

        public void BeforDeleteBatch( Type t, string condition ) {
        }



    }


}
