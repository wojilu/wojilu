using System;
using System.Collections.Generic;
using System.Text;

namespace wojilu.Web.Mvc {

    public interface IControllerFactory {
        ControllerBase New();
    }



}
