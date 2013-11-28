using System;
using System.Collections.Generic;
using System.Text;

namespace wojilu.ORM {

    public class OrmException : Exception {

        public OrmException()
            : base() {
        }

        public OrmException( String message )
            : base( message ) {
        }

        public OrmException( String message, Exception ex )
            : base( message, ex ) {
        }
    }
}
