using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Common.Onlines;

namespace wojilu.Web.Context.Initor {

    public class OnlineUserInit : IContextInit {

        public void Init( MvcContext ctx ) {

            if (ctx.utils.isEnd()) return;
            OnlineManager.Refresh( ctx );

        }

    }

}
