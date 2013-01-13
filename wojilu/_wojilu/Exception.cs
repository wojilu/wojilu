using System;
using System.Collections.Generic;
using System.Text;

namespace wojilu {

    public class AssemblyNotFoundException : Exception {

        public AssemblyNotFoundException()
            : base() {
        }

        public AssemblyNotFoundException( string message )
            : base( message ) {
        }

    }

    public class TypeNotFoundException : Exception {

        public TypeNotFoundException()
            : base() {
        }

        public TypeNotFoundException( string message )
            : base( message ) {
        }

    }

    public class MethodNotFoundException : Exception {

        public MethodNotFoundException()
            : base() {
        }

        public MethodNotFoundException( string message )
            : base( message ) {
        }

    }

    public class MethodNotVirtualException : Exception {

        public MethodNotVirtualException()
            : base() {
        }

        public MethodNotVirtualException( string message )
            : base( message ) {
        }

    }


}
