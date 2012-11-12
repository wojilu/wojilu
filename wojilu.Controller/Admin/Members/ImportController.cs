using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using System.IO;
using wojilu.Open;
using wojilu.Web.Utils;
using wojilu.Members.Users.Domain;

namespace wojilu.Web.Controller.Admin.Members {

    public class ImportUser {
        public String Name { get; set; }
        public String FriendlyUrl { get; set; }
    }

    public class ImportController : ControllerBase {

        private static readonly ILog logger = LogManager.GetLogger( typeof( ImportController ) );

        public void Index() {
            target( SaveImport );
        }

        public void SaveImport() {

            String userNamePath = ctx.Post( "userNamePath" );
            String userPicDir = ctx.Post( "userPicDir" );

            if (strUtil.IsNullOrEmpty( userNamePath )) errors.Add( "请填写用户名文件的地址" );

            if (ctx.HasErrors) {
                echoError();
                return;
            }

            List<String> picList = getUserPicList( userPicDir );

            // 读取用户名和furl
            List<ImportUser> users = getUserNameList( userNamePath );
            foreach (ImportUser u in users) {

                //if (picList.Count == 0) {
                //    logger.Info( "图片用完" );
                //    break;
                //}

                importUserSingle( u, picList );
                logger.Info( "import user done=" + u.Name );
            }

            echoAjaxOk();
        }

        private void importUserSingle( ImportUser u, List<string> picList ) {

            OpenService os = new OpenService();

            // 
            String pwd = getRandomPwd();
            String email = pwd + "@xxx.com";
            String apps = " home,blog,photo,microblog,friend,visitor,forumpost,about";

            // 注册
            Result result;
            if (strUtil.HasText( u.FriendlyUrl )) {
                result = os.UserRegister( u.Name, pwd, email, u.FriendlyUrl, apps );
            }
            else {
                String uFriendlyUrl = getFriendlyUrl();
                result = os.UserRegister( u.Name, pwd, email, uFriendlyUrl, apps );
            }

            if (result.HasErrors) return;
            User user = result.Info as User;

            if (picList.Count > 0) {

                String randomPic = picList[0];

                // 上传头像
                Result saved = AvatarUploader.Save( randomPic, user.Id );

                // 更新头像信息
                user.Pic = saved.Info.ToString();
                user.update();

                picList.RemoveAt( 0 );

            }
        }

        private string getFriendlyUrl() {

            int userId = rd.Next( 10000000, 99999990 );
            String furl = "u" + userId;

            User u = User.find( "Url=:url" ).set( "url", furl ).first();
            if (u == null) return furl;

            return getFriendlyUrl();
        }


        private static Random rd = new Random();

        private string getRandomPwd() {
            return rd.Next( 100000, 999999 ).ToString();
        }

        private List<string> getUserPicList( string userPicDir ) {

            if (strUtil.IsNullOrEmpty( userPicDir )) return new List<string>();

            String[] pics = Directory.GetFiles( PathHelper.Map( userPicDir ) );
            return new List<string>( pics );
        }

        private List<ImportUser> getUserNameList( string userNamePath ) {

            String[] lines = file.ReadAllLines( PathHelper.Map( userNamePath ) );

            List<ImportUser> list = new List<ImportUser>();
            foreach (String rUser in lines) {

                if (strUtil.IsNullOrEmpty( rUser )) continue;

                ImportUser user = new ImportUser();

                if (rUser.IndexOf( ',' ) <= 0 && rUser.IndexOf( '，' ) <= 0) {
                    user.Name = rUser.Trim();


                }
                else {
                    String[] arr = rUser.Split( new char[] { ',', '，' } );
                    if (arr.Length != 2) continue;

                    user.Name = arr[0].Trim();
                    user.FriendlyUrl = arr[1].Trim();
                }

                if (user.Name.Length > config.Instance.Site.UserNameLengthMax) continue;
                if (strUtil.IsAbcNumberAndChineseLetter( user.Name ) == false) continue;

                if (strUtil.HasText( user.FriendlyUrl )) {

                    if (user.FriendlyUrl.Length > config.Instance.Site.UserNameLengthMax) continue;
                    if (strUtil.IsUrlItem( user.FriendlyUrl ) == false) continue;
                }

                list.Add( user );

            }

            return list;
        }

    }

}
