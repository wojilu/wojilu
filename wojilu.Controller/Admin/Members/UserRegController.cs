using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;

namespace wojilu.Web.Controller.Admin.Members {

    public class UserRegController : ControllerBase {

        private static readonly ILog logger = LogManager.GetLogger( typeof( UserRegController ) );

        public void Index() {

            target( SaveSimple );

            if (MvcConfig.Instance.CheckDomainMap()) {
                set( "userUrlPrefix", SystemInfo.HostNoSubdomain );
            }
            else {
                set( "userUrlPrefix", strUtil.TrimStart( strUtil.Append( ctx.url.SiteAndAppPath, "/" ), "http://" ) );
                set( "urlExt", MvcConfig.Instance.UrlExt );

            }
        }

        public void SaveSimple() {

            Boolean chkInstallApp = ctx.PostIsCheck( "chkInstallApp" ) == 1 ? true : false;

            List<UserVo> users = getUserInfoList();
            if (users.Count == 0) {
                echoError( "请填写用户信息" );
                return;
            }

            logger.Info( "user count=" + users.Count );

            try {
                int okCount = 0;
                foreach (UserVo u in users) {

                    Result result;
                    if (chkInstallApp) {
                        result = registerUserAndInstallApp( u );
                    }
                    else {
                        result = registerUser( u ); // 逐个导入用户(导入的过程就是注册的过程)
                    }

                    if (result.IsValid) {
                        logger.Info( "register user=" + u.Name );
                        okCount += 1;
                    }
                    else {
                        logger.Error( "register user error=" + result.ErrorsText );
                        errors.Join( result );
                    }
                }

                if (okCount > 0) {
                    logger.Info( "register done" );
                    echoRedirectPart( "注册成功" );                    
                }
                else {
                    echoError();
                }
            }
            catch (Exception ex) {
                logger.Info( "" + ex.Message );
                logger.Info( "" + ex.StackTrace );

                echoError( "对不起，注册出错，请查看日志" );
            }

        }

        private Result registerUserAndInstallApp( UserVo user ) {

            // 调用 OpenService 进行 wojilu 注册
            String apps = config.Instance.Site.UserInitApp;
            return new wojilu.Open.OpenService().UserRegister( user.Name, user.Pwd, user.Email, user.FriendlyUrl, apps );
        }

        private Result registerUser( UserVo user ) {
            return new wojilu.Open.OpenService().UserRegister( user.Name, user.Pwd, user.Email, user.FriendlyUrl, null );
        }


        private List<UserVo> getUserInfoList() {

            String txtUsers = ctx.Post( "txtUsers" );

            if (strUtil.IsNullOrEmpty( txtUsers )) return new List<UserVo>();

            List<UserVo> users = new List<UserVo>();

            String[] arrLines = txtUsers.Trim().Split( new char[] { '\n', '\r' } );

            foreach (String line in arrLines) {

                if (strUtil.IsNullOrEmpty( line )) continue;

                String[] arrItems = line.Split( '/' );
                if ( isValidLine( arrItems )==false) continue;

                UserVo user = new UserVo();
                user.Name = arrItems[0];
                user.Pwd = arrItems[1];
                user.Email = arrItems[2];

                if (arrItems.Length == 4) {
                    user.FriendlyUrl = arrItems[3];
                }

                if (hasError( user )) continue;

                users.Add( user );
            }

            return users;
        }

        private bool isValidLine( string[] arrItems ) {
            if (arrItems.Length == 3) return true;
            if (arrItems.Length == 4) return true;
            return false;
        }

        private bool hasError( UserVo user ) {
            return string.IsNullOrEmpty( user.Name ) ||
                string.IsNullOrEmpty( user.Pwd ) ||
                string.IsNullOrEmpty( user.Email );
        }

    }

}
