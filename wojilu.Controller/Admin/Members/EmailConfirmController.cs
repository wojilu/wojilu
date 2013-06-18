using System;

using wojilu.Net;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Web.Utils;

using wojilu.Members.Users.Domain;
using wojilu.Members.Users.Interface;
using wojilu.Members.Users.Service;
using wojilu.Web.Controller.Admin.Sys;
using wojilu.Web.Controller.Common;

namespace wojilu.Web.Controller.Admin.Members {

    public class EmailConfirmController : ControllerBase {

        private static readonly ILog logger = LogManager.GetLogger( typeof( EmailConfirmController ) );

        public IUserService userService { get; set; }
        public IUserConfirmService confirmService { get; set; }
        public IConfirmEmail confirmEmail { get; set; }

        public EmailConfirmController() {
            userService = new UserService();
            confirmService = new UserConfirmService();
            confirmEmail = new ConfirmEmail();
        }

        public void EditTemplate() {
            redirectDirect( to( new ViewsFileController().Edit ) + "?file=" + confirmEmail.getTemplatePath() + MvcConfig.Instance.ViewExt );            
        }

        [HttpPost, DbTransaction]
        public void SendConfirmMail( ) {

            if (config.Instance.Site.EnableEmail == false) {
                echo( "对不起，邮件服务尚未开启，无法发送邮件" );
                return;
            }

            String ids = ctx.GetIdList( "id" );
            String title = ctx.Post( "Title" );
            if (strUtil.IsNullOrEmpty( title )) title = config.Instance.Site.SiteName + lang( "exAccountConfirm" );

            MailClient mail = MailClient.Init();

            int[] arrId = cvt.ToIntArray( ids );

            foreach (int userId in arrId) {

                User user = userService.GetById( userId );
                if (user == null) continue;

                if (System.Text.RegularExpressions.Regex.IsMatch( user.Email, RegPattern.Email ) == false) {
                    userService.ConfirmEmailIsError( user );
                    logger.Info( lang( "exEmailFormat" ) + ": " + user.Name + "[" + user.Email + "]" );
                    return;
                }

                confirmEmail.SendEmail( user, title, null );

            }

            if( ctx.HasErrors )
                echoError();
            else
                echoRedirectPart( lang( "opok" ) );

        }


    }

}
