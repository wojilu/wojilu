using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;

namespace wojilu.Web.Controller.Demo {

    public class FrmLinkController : ControllerBase {

        public void Index() {
            set( "lnkRight", to( MainRight ) );
        }

        public void MainRight() {
            set( "lnkBottom", to( MainBottom ) );
        }

        public void MainBottom() {
            set( "lnkBottomBox", to( MainBottomBox ) );
        }

        public void MainBottomBox() {
            set( "lnkRefreshParent", to( RefreshParent ) );
        }

        public void RefreshParent() {
            echoToParentPart( "刷新父页面局部" );
        }


    }

}
