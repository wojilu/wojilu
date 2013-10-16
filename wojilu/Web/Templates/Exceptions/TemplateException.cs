using System;
using System.Collections.Generic;
using System.Text;

namespace wojilu.Web.Templates {

    public class TemplateException : Exception {

        public TemplateException()
            : base() {
        }

        public TemplateException( String message )
            : base( message ) {
        }

        public TemplateException( String message, Exception ex )
            : base( message, ex ) {
        }
    }

    public class TemplateCompileException : Exception {

        public TemplateCompileException()
            : base() {
        }

        public TemplateCompileException( String message )
            : base( message ) {
        }

        public TemplateCompileException( String message, Exception ex )
            : base( message, ex ) {
        }
    }

    public class TemplateRunException : Exception {

        public TemplateRunException()
            : base() {
        }

        public TemplateRunException( String message )
            : base( message ) {
        }

        public TemplateRunException( String message, Exception ex )
            : base( message, ex ) {
        }
    }



}
