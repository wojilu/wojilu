using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;

namespace wojilu.Web.Controller.Demo.FrmTest {

    public class LayoutController : ControllerBase {
        public override void Layout() {
        }
    }

}

namespace wojilu.Web.Controller.Demo.FrmTest.Main {

    public class LayoutController : ControllerBase {
        public override void Layout() {
        }
    }

}

namespace wojilu.Web.Controller.Demo.FrmTest.Main.Data {

    public class LayoutController : ControllerBase {
        public override void Layout() {
        }
    }


    public class PostController : ControllerBase {

        public override void Layout() {
        }

        public void Index() {

            set( "redirectPage", to( GoPage ) );
            set( "redirectPageCurrent", to( GoPartCurrent ) );
            set( "redirectPage1", to( GoPartOne ) );
            set( "redirectPage2", to( GoPartTwo ) );
            set( "redirectPage3", to( GoPartThree ) );
            set( "redirectPage4", to( GoPageNoUrl ) );


            set( "boxPage", to( BoxPage ) );
            set( "boxPageCurrent", to( BoxPartCurrent ) );
            set( "boxPage1", to( BoxPartOne ) );
            set( "boxPage2", to( BoxPartTwo ) );
            set( "boxPage3", to( BoxPartThree ) );
            set( "boxPage4", to( BoxPartNoUrl ) );

        }

        public void GoPage() {
            echoRedirect( "跳转成功", to( Index ) );
        }

        public void GoPageNoUrl() {
            echoRedirect( "跳转成功" ); //整页刷新
        }

        public void GoPartCurrent() {
            echoRedirectPart( "跳转成功", to( Index ), 0 );
        }

        public void GoPartOne() {
            echoRedirectPart( "跳转成功", to( Index ), 1 );
        }

        public void GoPartTwo() {
            echoRedirectPart( "跳转成功", to( Index ), 2 );
        }

        public void GoPartThree() {
            echoRedirectPart( "跳转成功", to( Index ), 3 );
        }

        //-------------------------------------------------------------------

        public void BoxPage() {
            echoToParent( "跳转成功", to( Index ) );
            
        }

        public void BoxPartCurrent() {
            echoToParentPart( "跳转成功", to( Index ), 0 );
        }

        public void BoxPartOne() {
            echoToParentPart( "跳转成功", to( Index ), 1 );
        }

        public void BoxPartTwo() {
            echoToParentPart( "跳转成功", to( Index ), 2 );
        }

        public void BoxPartThree() {
            echoToParentPart( "跳转成功", to( Index ), 3 );
        }

        public void BoxPartNoUrl() {
            echoToParent( "跳转成功" );
        }


    }

}
