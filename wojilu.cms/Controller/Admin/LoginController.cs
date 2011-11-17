using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web;
using wojilu.Web.Mvc;
using wojilu.cms.Domain;

namespace wojilu.cms.Controller.Admin {



    public class LoginController : ControllerBase {

        public LoginController() {
            HideLayout( typeof( LayoutController ) );
            HidePermission( typeof( SecurityController ) );
        }

        public void Logout() {

            ctx.web.UserLogout(); // 使用cookie方式的注销

            redirect( new MainController().Index );
        }

        public void Login() {
            target( CheckLogin );
            set( "returnUrl", ctx.Get( "returnUrl" ) );

            set( "vcode", Html.Captcha );
        }

        public void CheckLogin() {

            Html.Captcha.CheckError( ctx );
            if (ctx.HasErrors) {
                echoError();
                return;
            }

            string name = ctx.Post( "name" );
            string pwd = ctx.Post( "pwd" );

            validateInput( name, pwd ); // 验证用户输入是否完整

            if (ctx.HasErrors) {
                run( Login );
                return;
            }

            User user = getUser( name, pwd );
            if (user == null) { // 验证用户名和密码是否正确
                errors.Add( "用户名或密码错误" );
                run( Login );
                return;
            }

            ctx.web.UserLogin( user.Id, user.Name, wojilu.Common.LoginTime.OneMonth );

            string returnUrl = strUtil.HasText( ctx.Post( "returnUrl" ) ) ? ctx.Post( "returnUrl" ) : to( new ArticleController().Index );
            redirectUrl( returnUrl ); // 跳转到文章管理页面
        }

        private void validateInput( string name, string pwd ) {
            if (strUtil.IsNullOrEmpty( name )) errors.Add( "请填写用户名" );
            if (strUtil.IsNullOrEmpty( pwd )) errors.Add( "请填写密码" );
        }

        private User getUser( string name, string pwd ) {
            User user = User.find( "Name=:n and Pwd=:p" )
                .set( "n", name )
                .set( "p", pwd )
                .first();
            return user;
        }





    }
}
