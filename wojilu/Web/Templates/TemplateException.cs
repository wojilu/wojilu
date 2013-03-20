using System;
using System.Collections.Generic;
using System.Text;

namespace wojilu.Web.Templates {

    public class TemplateException : Exception {

        public TemplateException()
            : base() {
        }

        public TemplateException( string message )
            : base( message ) {
        }
    }

}
