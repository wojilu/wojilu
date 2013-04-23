using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Config;
using wojilu.Web.Controller.Security;
using wojilu.Members.Interface;
using wojilu.Members.Sites.Domain;
using wojilu.Members.Sites.Service;
using wojilu.Members.Users.Domain;

namespace wojilu.Web.Controller.Admin.Members {

    public class UserSettingController : ControllerBase {

        public IAdminLogService<SiteLog> logService { get; set; }

        public UserSettingController() {
            logService = new SiteLogService();
        }

        public void Index() {


            target( UserSave );

            set( "needLoginChecked", config.Instance.Site.NeedLogin ? "checked=\"checked\"" : "" );
            set( "userNeedApproveChecked", config.Instance.Site.UserNeedApprove ? "checked=\"checked\"" : "" );

            set( "needAlertActivation", config.Instance.Site.AlertActivation ? "checked=\"checked\"" : "" );
            set( "needAlertUserPic", config.Instance.Site.AlertUserPic ? "checked=\"checked\"" : "" );

            set( "site.UserSendConfirmEmailInterval", config.Instance.Site.UserSendConfirmEmailInterval );
            set( "confirmEmailEditLink", to( new Admin.Sys.ViewsFileController().Edit ) + "?file=Common/emailConfirmMsg.html" );

            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add( "开放注册", RegisterType.Open.ToString() );
            dic.Add( "关闭注册", RegisterType.Close.ToString() );
            dic.Add( "只有受邀请用户才可以注册", RegisterType.CloseUnlessInvite.ToString() );
            radioList( "registerType", dic, config.Instance.Site.RegisterType.ToString() );

            Dictionary<string, string> dicLogin = new Dictionary<string, string>();
            dicLogin.Add( "注册成功即可登录", LoginType.Open.ToString() );
            dicLogin.Add( "必须激活之后才可以登录", LoginType.ActivationEmail.ToString() );
            radioList( "loginType", dicLogin, config.Instance.Site.LoginType.ToString() );

            Dictionary<string, string> dicNav = new Dictionary<string, string>();
            dicNav.Add( "显示", TopNavDisplay.Show.ToString() );
            dicNav.Add( "隐藏", TopNavDisplay.Hide.ToString() );
            dicNav.Add( "在关闭注册之后隐藏", TopNavDisplay.NoRegHide.ToString() );
            radioList( "topNavDisplay", dicNav, config.Instance.Site.TopNavDisplay.ToString() );

            //--------------------------------------

            bind( "site", config.Instance.Site );

            set( "SystemMsgContent", config.Instance.Site.SystemMsgContent );

            set( "site.ReservedUserNameStr", config.Instance.Site.GetValue( "ReservedUserName" ) );
            set( "site.ReservedUserUrlStr", config.Instance.Site.GetValue( "ReservedUserUrl" ) );
            set( "site.ReservedKeyString", config.Instance.Site.GetValue( "ReservedKey" ) );
        }


        [HttpPost, DbTransaction]
        public void UserSave() {

            String UserPageKeywords = ctx.Post( "UserPageKeywords" );
            String UserPageDescription = ctx.Post( "UserPageDescription" );

            Boolean needLogin = ctx.PostIsCheck( "NeedLogin" ) == 1 ? true : false;
            Boolean userNeedApprove = ctx.PostIsCheck( "UserNeedApprove" ) == 1 ? true : false;

            Boolean alertActivation = ctx.PostIsCheck( "AlertActivation" ) == 1 ? true : false;
            Boolean alertUserPic = ctx.PostIsCheck( "AlertUserPic" ) == 1 ? true : false;

            int confirmEmailInterval = ctx.PostInt( "confirmEmailInterval" );

            int registerType = ctx.PostInt( "registerType" );
            int loginType = ctx.PostInt( "loginType" );
            int topNavDisplay = ctx.PostInt( "topNavDisplay" );

            int UserNameLengthMin = ctx.PostInt( "UserNameLengthMin" );
            int UserNameLengthMax = ctx.PostInt( "UserNameLengthMax" );

            int UserDescriptionMin = ctx.PostInt( "UserDescriptionMin" );
            int UserDescriptionMax = ctx.PostInt( "UserDescriptionMax" );
            int UserSignatureMin = ctx.PostInt( "UserSignatureMin" );
            int UserSignatureMax = ctx.PostInt( "UserSignatureMax" );

            int PublishTimeAfterReg = ctx.PostInt( "PublishTimeAfterReg" );

            String SystemMsgTitle = ctx.Post( "SystemMsgTitle" );
            String SystemMsgContent = ctx.PostHtml( "SystemMsgContent" );

            String ReservedUserNameStr = ctx.Post( "ReservedUserName" );
            String ReservedUserUrlStr = ctx.Post( "ReservedUserUrl" );
            String ReservedKeyString = ctx.Post( "ReservedKeyString" );

            if (UserNameLengthMin <= 0) errors.Add( lang( "exUserNameMinGreater0" ) );
            if (UserNameLengthMax <= 0) errors.Add( lang( "exUserNameMaxGreater0" ) );
            if (strUtil.IsNullOrEmpty( SystemMsgTitle )) errors.Add( lang( "exMsgTitle" ) );
            if (strUtil.IsNullOrEmpty( SystemMsgContent )) errors.Add( lang( "exMsgContent" ) );

            if (UserDescriptionMin < 0) UserDescriptionMin = 0;
            if (UserDescriptionMax < 0) UserDescriptionMax = 0;
            if (UserSignatureMin < 0) UserSignatureMin = 0;
            if (UserSignatureMax < 0) UserSignatureMax = 0;

            if (PublishTimeAfterReg < 0) PublishTimeAfterReg = 0;

            if (ctx.HasErrors) {
                echoError();
            }
            else {

                config.Instance.Site.UserPageKeywords = UserPageKeywords; config.Instance.Site.Update( "UserPageKeywords", UserPageKeywords );
                config.Instance.Site.UserPageDescription = UserPageDescription; config.Instance.Site.Update( "UserPageDescription", UserPageDescription );

                config.Instance.Site.NeedLogin = needLogin; config.Instance.Site.Update( "NeedLogin", needLogin );
                config.Instance.Site.UserNeedApprove = userNeedApprove; config.Instance.Site.Update( "UserNeedApprove", userNeedApprove );

                config.Instance.Site.AlertActivation = alertActivation; config.Instance.Site.Update( "AlertActivation", alertActivation );
                config.Instance.Site.AlertUserPic = alertUserPic; config.Instance.Site.Update( "AlertUserPic", alertUserPic );

                config.Instance.Site.RegisterType = registerType; config.Instance.Site.Update( "RegisterType", registerType );
                config.Instance.Site.LoginType = loginType; config.Instance.Site.Update( "LoginType", loginType );
                config.Instance.Site.TopNavDisplay = topNavDisplay; config.Instance.Site.Update( "TopNavDisplay", topNavDisplay );

                config.Instance.Site.UserSendConfirmEmailInterval = confirmEmailInterval;
                config.Instance.Site.Update( "UserSendConfirmEmailInterval", confirmEmailInterval );

                //---------------------------------------------------------------------
                config.Instance.Site.UserNameLengthMin = UserNameLengthMin;
                config.Instance.Site.Update( "UserNameLengthMin", config.Instance.Site.UserNameLengthMin );

                config.Instance.Site.UserNameLengthMax = UserNameLengthMax;
                config.Instance.Site.Update( "UserNameLengthMax", config.Instance.Site.UserNameLengthMax );

                //---------------------------------------------------------------------
                config.Instance.Site.UserDescriptionMin = UserDescriptionMin;
                config.Instance.Site.Update( "UserDescriptionMin", config.Instance.Site.UserDescriptionMin );

                config.Instance.Site.UserDescriptionMax = UserDescriptionMax;
                config.Instance.Site.Update( "UserDescriptionMax", config.Instance.Site.UserDescriptionMax );

                config.Instance.Site.UserSignatureMin = UserSignatureMin;
                config.Instance.Site.Update( "UserSignatureMin", config.Instance.Site.UserSignatureMin );

                config.Instance.Site.UserSignatureMax = UserSignatureMax;
                config.Instance.Site.Update( "UserSignatureMax", config.Instance.Site.UserSignatureMax );

                //---------------------------------------------------------------------

                config.Instance.Site.PublishTimeAfterReg = PublishTimeAfterReg;
                config.Instance.Site.Update( "PublishTimeAfterReg", config.Instance.Site.PublishTimeAfterReg );

                //---------------------------------------------------------------------

                config.Instance.Site.SystemMsgTitle = SystemMsgTitle; config.Instance.Site.Update( "SystemMsgTitle", SystemMsgTitle );
                config.Instance.Site.SystemMsgContent = SystemMsgContent; config.Instance.Site.Update( "SystemMsgContent", SystemMsgContent );

                config.Instance.Site.ReservedUserName = SiteSetting.GetArrayValueByString( ReservedUserNameStr );
                config.Instance.Site.Update( "ReservedUserName", ReservedUserNameStr );

                config.Instance.Site.ReservedUserUrl = SiteSetting.GetArrayValueByString( ReservedUserUrlStr );
                config.Instance.Site.Update( "ReservedUserUrl", ReservedUserUrlStr );

                config.Instance.Site.ReservedKey = SiteSetting.GetArrayValueByString( ReservedKeyString );
                config.Instance.Site.Update( "ReservedKey", ReservedKeyString );

                log( SiteLogString.EditSiteSettingUser() );

                echoRedirectPart( lang( "opok" ) );
            }
        }

        private void log( String msg ) {
            logService.Add( (User)ctx.viewer.obj, msg, "", typeof( SiteSetting ).FullName, ctx.Ip );
        }


    }

}
