using System;
using System.Collections.Generic;
using System.Text;

namespace wojilu.Web.Context.Initor {

    public interface IContextInit {
        void Init( MvcContext ctx );
    }

}
