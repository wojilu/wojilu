using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using wojilu.Data;
using wojilu.OpenSample.Domain;
using wojilu.Open;

namespace wojilu.OpenSample {

    public class SecurityHelper {

        private static readonly ILog logger = LogManager.GetLogger( typeof( SecurityHelper ) );

        public static Boolean IsLogin() {
            // 统一通过 OpenService 检查是否登录
            return new OpenService().UserIsLogin( HttpContext.Current );
        }

        public static Boolean Login( String name, String pwd ) {

            SampleUser user = SampleUser.GetByNameAndPwd( name, pwd );
            if (user == null) return false;

            // 统一通过 OpenService 登录
            new OpenService().UserLogin( user.Name, HttpContext.Current );

            return true;
        }

        public static string GetWelcomeInfo() {

            // 统一通过 OpenService 获取用户名
            String userName = new OpenService().UserName( HttpContext.Current );

            if (userName == null) {
                return "用户不存在";
            }
            else {
                return string.Format( "欢迎你，{0}！", userName );
            }
        }

        public static void Logout() {
            // 统一通过 OpenService 注销
            new OpenService().UserLogout( HttpContext.Current );
        }

        /// <summary>
        /// 注册：1)本站注册 2)调用wojilu的OpenService注册
        /// </summary>
        /// <param name="user"></param>
        public static void Register( SampleUser user ) {

            DbContext.beginTransactionAll();

            try {

                // 1) 你自己的网站用户注册
                user.insert();

                // 2) 调用 OpenService 进行 wojilu 注册
                new OpenService().UserRegister( user.Name, user.Pwd, user.Email, null, "home,blog,photo,microblog,friend,visitor,forumpost,about,feedback,share" );

                DbContext.commitAll();
            }
            catch (Exception ex) {
                logger.Info( "" + ex.Message );
                logger.Info( "" + ex.StackTrace );
                DbContext.rollbackAll();
            }

        }




    }

}

