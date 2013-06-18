using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Members.Users.Domain;
using wojilu.Web.Mvc.Utils;
using wojilu.Members.Users.Interface;
using wojilu.Members.Users.Service;
using wojilu.Web.Mvc;
using wojilu.Net;
using wojilu.Web.Utils;

namespace wojilu.Web.Controller.Common {

    public interface IConfirmEmail {
        String GetEmailBody( User user );
        Result SendEmail( User user, String title, String msg );
        String getTemplatePath();
    }

    public class ConfirmEmail : IConfirmEmail {

        private static readonly ILog logger = LogManager.GetLogger( typeof( ConfirmEmail ) );

        public IUserConfirmService confirmService { get; set; }
        public IUserService userService { get; set; }

        public ConfirmEmail() {
            confirmService = new UserConfirmService();
            userService = new UserService();
        }

        public Result SendEmail( User user, String title, String msg ) {

            if (strUtil.IsNullOrEmpty( title )) title = config.Instance.Site.SiteName + lang.get( "exAccountConfirm" );
            if (strUtil.IsNullOrEmpty( msg )) msg = GetEmailBody( user );

            if (System.Text.RegularExpressions.Regex.IsMatch( user.Email, RegPattern.Email ) == false) {
                userService.ConfirmEmailIsError( user );
                String errorMail = lang.get( "exEmailFormat" ) + ": " + user.Name + "[" + user.Email + "]";
                logger.Info( errorMail );
                return new Result( errorMail );
            }

            MailClient mail = MailClient.Init();
            Result sentResult = mail.Send( user.Email, title, msg );

            if (sentResult.IsValid) {
                userService.SendConfirmEmail( user );
                logger.Info( lang.get( "sentok" ) + ": " + user.Name + "[" + user.Email + "]" );
            }
            else {
                userService.ConfirmEmailIsError( user );
                logger.Info( lang.get( "exSentError" ) + ": " + user.Name + "[" + user.Email + "]" );
            }

            return sentResult;
        }

        public String getTemplatePath() {
            return "Common/emailConfirmMsg";
        }

        public String GetEmailBody( User user ) {

            String codeLink = logEmailConfirm( user );

            ITemplate template = MvcUtil.LoadTemplate( "Common/emailConfirmMsg" );

            template.Set( "userName", user.Name );
            template.Set( "siteName", config.Instance.Site.SiteName );
            template.Set( "siteLink", config.Instance.Site.SiteUrl );

            template.Set( "codeLink", codeLink );
            template.Set( "created", DateTime.Now.ToShortDateString() );

            return template.ToString();
        }

        private String logEmailConfirm( User user ) {
            String code = Guid.NewGuid().ToString().Replace( "-", "" ).ToLower();
            UserConfirm uc = new UserConfirm();
            uc.User = user;
            uc.Code = code;
            confirmService.AddConfirm( uc );

            String codeLink = strUtil.Join( config.Instance.Site.SiteUrl, "Main/ConfirmEmail" );
            codeLink = codeLink + MvcConfig.Instance.UrlExt;
            codeLink += "?c=" + user.Id + "_" + code;
            return codeLink;
        }

    }
}
