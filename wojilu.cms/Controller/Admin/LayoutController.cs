using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.cms.Domain;

namespace wojilu.cms.Controller.Admin {

    public class LayoutController : ControllerBase {

        private static readonly ILog logger = LogManager.GetLogger( typeof( LayoutController ) );

        public LayoutController() {
            HideLayout( typeof( wojilu.cms.Controller.LayoutController ) );
        }

        public override void Layout() {

            logger.Info( "开始加载布局文件" );

            int userId = ctx.web.UserId(); // 获取 cookie 中存储的加密过的 userId
            User user = User.findById( userId ); // 根据此 userId 检索出 user

            if (user == null) { // 某些用户长时间不登录，可能不存在了。比如被管理员删除了
                ctx.web.UserLogout();
                return;
            }

            set( "user.Name", user.Name );
            set( "logoutLink", to( new LoginController().Logout ) ); // 设置注销的网址

            set( "listLink", to( new ArticleController().Index  ) );
            set( "addLink", to( new ArticleController().Add ) );
            set( "listShowLink", to( new ArticleController().List ) );

            set( "addCategory", to( new CategoryController().Add ) );
            set( "categoryList", to( new CategoryController().Index ) );

            set( "loopListLink", to( new ArticleController().LoopList ) );

            set( "userList", to( new UserController().Index ) );
            set( "userAdd", to( new UserController().Add ) );

            set( "configLink", to( new ConfigController().Index ) );
            set( "footerLink", to( new FooterController().Index ) );

            set( "uploadLink", to( new FileController().Index ) );

        }


    }
}
