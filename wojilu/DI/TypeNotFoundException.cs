using System;
using System.Collections.Generic;
using System.Text;

namespace wojilu.DI {

    public class TypeNotFoundException : Exception {

        public TypeNotFoundException()
            : base() {
        }

        public TypeNotFoundException( string message )
            : base( message ) {
        }

    }
}
