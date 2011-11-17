using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;

namespace wojilu.Web.Controller.Demo {

    public class BoxController : ControllerBase {

        public void NBox() {

            set( "link", to( BoxA ) );

            set( "nPartLink", to( new FrmTest.Main.Data.PostController().Index ) );
        }

        public void BoxA() {
            set( "link", to( BoxB ) );
        }

        public void BoxB() {
            set( "link", to( BoxC ) );
        }

        public void BoxC() {
            set( "link", to( BoxD ) );
        }

        public void BoxD() {
            echoToParentPart( "这是窗口D，我会刷新父窗口C" );
        }


    }

}
