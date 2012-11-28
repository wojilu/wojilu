using System;
using System.Data;
using System.Configuration;
using System.Web;
using wojilu.Members.Users.Domain;
using wojilu.Web;

namespace wojilu.Open {

    /// <summary>
    /// 供其他 .net 项目引用的开放service
    /// </summary>
    public class OpenService {

        /// <summary>
        /// 用户注册(不开启空间，使用默认的友好网址)
        /// </summary>
        /// <param name="name">用户名</param>
        /// <param name="pwd">密码</param>
        /// <param name="email">电子邮件</param>
        public Result UserRegister( String name, String pwd, String email ) {

            if (strUtil.IsNullOrEmpty( name ) || strUtil.IsNullOrEmpty( pwd )) {
                throw new ArgumentNullException( "请设置用户名和密码" );
            }

            User user = new User();
            user.Name = name;
            user.Pwd = pwd;
            user.Email = email;

            Result result = new Result();
            UserService userService = new UserService();
            ISiteConfig sconfig = getSiteConfig( false );
            userService.Register( user, result, sconfig );
            result.Info = user;
            return result;
        }

        /// <summary>
        /// 用户注册(根据参数决定是否开启空间，是否安装应用程序)
        /// </summary>
        /// <param name="name">用户名</param>
        /// <param name="pwd">密码</param>
        /// <param name="email">电子邮件</param>
        /// <param name="friendlyUrl">用户的友好网址，可以为空</param>
        /// <param name="apps">默认开启的应用程序(从 home,blog,photo,microblog,friend,visitor,forumpost,about,feedback,share 中选择)。如果为空，则不安装程序</param>
        public Result UserRegister( String name, String pwd, String email, String friendlyUrl, String apps ) {

            if (strUtil.IsNullOrEmpty( name ) || strUtil.IsNullOrEmpty( pwd )) {
                throw new ArgumentNullException( "请设置用户名和密码" );
            }

            User user = new User();
            user.Name = name;
            user.Pwd = pwd;
            user.Email = email;

            user.Url = friendlyUrl == null ? "" : friendlyUrl.ToLower();

            Result result = new Result();
            UserService userService = new UserService();

            if (strUtil.HasText( apps )) {

                ISiteConfig sconfig = getSiteConfig( true );
                userService.Register( user, result, sconfig );
                new AppService().InstallAppAndMenu( user, apps );
            }
            else {
                ISiteConfig sconfig = getSiteConfig( false );
                userService.Register( user, result, sconfig );
            }


            result.Info = user;
            return result;

        }

        private ISiteConfig getSiteConfig( Boolean isEnableUserSpace ) {

            WojiluSiteConfig s = new WojiluSiteConfig();
            s.AdministratorId = 1;
            s.NormalRoleId = 2;
            s.UserSkinId = 1;

            s.ReservedKey = new string[] { };
            s.ReservedUserName = new string[] { };
            s.ReservedUserUrl = new string[] { };

            s.MsgTitle = "";
            s.MsgBody = "";

            s.IsSendMsg = false; // 不发送欢迎短信
            s.IsEnableUserSpace = isEnableUserSpace; // 是否开启用户空间

            s.Md5Is16 = false;

            return s;
        }

        //--------------------登录、注销相关方法------------------------------------------------

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="context"></param>
        public void UserLogin( String userName, HttpContext context ) {

            User user = getUserByName( userName );
            if (user == null) throw new NullReferenceException( "试图登录的用户不存在，用户名=" + user.Name );

            WebContext webContext = new WebContext( context );
            // 注意：传入的Id和wojilu的ID是不同步的
            webContext.UserLogin( user.Id, user.Name, wojilu.Common.LoginTime.OneMonth );
        }

        /// <summary>
        /// 当前用户是否登录？
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Boolean UserIsLogin( HttpContext context ) {
            WebContext webContext = new WebContext( context );
            return webContext.UserIsLogin;
        }

        /// <summary>
        /// 获取当前用户的name，如果用户未登录，则返回null
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public String UserName( HttpContext context ) {
            WebContext webContext = new WebContext( context );
            int userId = webContext.UserId();

            User user = User.findById( userId );
            if (user == null) {
                this.UserLogout( context );
                return null;
            }

            return user.Name;
        }

        /// <summary>
        /// 用户注销
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="context"></param>
        public void UserLogout( HttpContext context ) {
            WebContext webContext = new WebContext( context );
            webContext.UserLogout();
        }

        //--------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// 用户删除
        /// </summary>
        /// <param name="userName"></param>
        public void UserDelete( String userName ) {

            User user = getUserByName( userName );
            if (user == null) throw new NullReferenceException( "试图删除的用户不存在，用户名=" + user.Name );

            user.delete();
        }

        /// <summary>
        /// 资料同步修改
        /// </summary>
        /// <param name="user"></param>
        public void UserUpdate( User user ) {

            if (user == null) throw new ArgumentNullException();

            User dbUser = getUserByName( user.Name );
            if (dbUser == null) throw new NullReferenceException( "试图修改的用户不存在，用户名=" + user.Name );

            user.Id = dbUser.Id;
            user.update();
        }

        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="newPwd"></param>
        public void UserChangePwd( String userName, String newPwd ) {

            User user = getUserByName( userName );
            if (user == null) throw new NullReferenceException( "试图修改密码用户不存在，用户名=" + userName );


            wojilu.Members.Users.Service.UserService us = new wojilu.Members.Users.Service.UserService();
            us.UpdatePwd( user, newPwd );
        }

        private User getUserByName( String name ) {
            if (strUtil.IsNullOrEmpty( name )) return null;
            return User.find( "Name=:name" ).set( "name", name ).first();
        }


    }

}
