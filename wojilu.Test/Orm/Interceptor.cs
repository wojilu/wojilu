using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wojilu.ORM;

namespace wojilu.Test.Orm {

    public class MyTestInterceptor1 : IInterceptor {


        public void BeforInsert( IEntity entity ) {
            Console.WriteLine( "━━━━━━━━━━━━━BeforInsert1━━━━━━━━━━━━━" );
        }

        public void AfterInsert( IEntity entity ) {
            Console.WriteLine( "━━━━━━━━━━━━━AfterInsert1━━━━━━━━━━━━━" );
        }

        public void BeforUpdate( IEntity entity ) {
        }

        public void AfterUpdate( IEntity entity ) {
        }

        public void BeforDelete( IEntity entity ) {
        }

        public void AfterDelete( IEntity entity ) {
        }

        public void BeforUpdateBatch( Type t, string action, string condition ) {
        }

        public void AfterUpdateBatch( Type t, string action, string condition ) {
        }

        public void BeforDeleteBatch( Type t, string condition ) {
        }

        public void AfterDeleteBatch( Type t, string condition ) {
        }
    }

    public class MyTestInterceptor2 : IInterceptor {


        public void BeforInsert( IEntity entity ) {
            Console.WriteLine( "━━━━━━━━━━━━━BeforInsert2━━━━━━━━━━━━━" );
        }

        public void AfterInsert( IEntity entity ) {
            Console.WriteLine( "━━━━━━━━━━━━━AfterInsert2━━━━━━━━━━━━━" );
        }

        public void BeforUpdate( IEntity entity ) {
        }

        public void AfterUpdate( IEntity entity ) {
        }

        public void BeforDelete( IEntity entity ) {
        }

        public void AfterDelete( IEntity entity ) {
        }

        public void BeforUpdateBatch( Type t, string action, string condition ) {
        }

        public void AfterUpdateBatch( Type t, string action, string condition ) {
        }

        public void BeforDeleteBatch( Type t, string condition ) {
        }

        public void AfterDeleteBatch( Type t, string condition ) {
        }
    }
}
