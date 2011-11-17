/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Web;
using System.Text;
using System.Collections;
using System.Collections.Generic;

using wojilu.Config;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Web.Utils;
using wojilu.Members.Sites.Interface;
using wojilu.Members.Sites.Service;
using wojilu.Members.Users.Domain;
using wojilu.Web.Controller.Security;
using wojilu.Members.Interface;
using wojilu.Members.Sites.Domain;
using wojilu.Web.Controller.Admin.Sys;
using wojilu.Web.Handler;
using wojilu.Common.AppInstall;
using wojilu.Common;

namespace wojilu.Web.Controller.Admin {

    public class SiteConfigController : ControllerBase {

        public IAdminLogService<SiteLog> logService { get; set; }
        public IAppInstallerService appService { get; set; }

        public SiteConfigController() {
            logService = new SiteLogService();
            appService = new AppInstallerService();
        }

        public override void Layout() {

            set( "lnkBase", to( Base ) );
            set( "lnkLogo", to( Logo ) );
            set( "lnkUser", to( User ) );
            set( "lnkEmail", to( Email ) );
            set( "lnkFilter", to( Filter ) );
            set( "lnkBanIp", to( BanIp ) );
            set( "lnkClose", to( Close ) );
            set( "lnkJob", to( new JobController().List ) );

            set( "lnkApp", to( App ) );
            set( "lnkDrawing", to( Drawing ) );

            set( "lnkFiles", to( new SystemFileController().Index ) );

            set( "lnkConfig", to( new ConfigFileController().Index ) );
            set( "lnkCacheData", to( new CacheFileController().Index ) );

            set( "lnkRestart", to( new SiteController().BeginRestart ) );
        }

        private void log( String msg ) {
            logService.Add( (User)ctx.viewer.obj, msg, "", typeof( SiteSetting ).FullName, ctx.Ip );
        }

        public void Drawing() {

            target( DrawingSave );

            bind( "site", config.Instance.Site );

            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add( "纯数字", ValidationType.Digit.ToString() );
            dict.Add( "纯字母", ValidationType.Letter.ToString() );
            dict.Add( "数字加字母", ValidationType.DigitAndLetter.ToString() );
            dict.Add( "中文", ValidationType.Chinese.ToString() );
            radioList( "validationType", dict, config.Instance.Site.ValidationType.ToString() );

            String chk2 = Html.CheckBox( "LoginNeedImgValidation", lang( "enable" ), "1", config.Instance.Site.LoginNeedImgValidation );
            set( "site.CheckLoginNeedImgValidation", chk2 );

            String chk3 = Html.CheckBox( "RegisterNeedImgValidateion", lang( "enable" ), "1", config.Instance.Site.RegisterNeedImgValidateion );
            set( "site.CheckRegisterNeedImgValidateion", chk3 );
        }

        public void DrawingSave() {


            int validationType = ctx.PostInt( "validationType" );
            int ValidationLength = ctx.PostInt( "ValidationLength" );
            int ValidationChineseLength = ctx.PostInt( "ValidationChineseLength" );

            config.Instance.Site.ValidationType = validationType; config.Instance.Site.Update( "ValidationType", validationType );
            config.Instance.Site.ValidationLength = ValidationLength; config.Instance.Site.Update( "ValidationLength", ValidationLength );
            config.Instance.Site.ValidationChineseLength = ValidationChineseLength; config.Instance.Site.Update( "ValidationChineseLength", ValidationChineseLength );

            Boolean RegisterNeedImgValidateion = ctx.PostInt( "RegisterNeedImgValidateion" ) == 1 ? true : false;
            Boolean LoginNeedImgValidation = ctx.PostInt( "LoginNeedImgValidation" ) == 1 ? true : false;

            config.Instance.Site.RegisterNeedImgValidateion = RegisterNeedImgValidateion; config.Instance.Site.Update( "RegisterNeedImgValidateion", RegisterNeedImgValidateion );
            config.Instance.Site.LoginNeedImgValidation = LoginNeedImgValidation; config.Instance.Site.Update( "LoginNeedImgValidation", LoginNeedImgValidation );

            echoRedirect( lang( "opok" ) );


        }

        //---------------------------------------------------------------------------------------------

        public void App() {

            target( AppSave );

            Dictionary<string, string> dicApp = new Dictionary<string, string>();
            dicApp.Add( "主页", "home" );
            dicApp.Add( "博客", "blog" );
            dicApp.Add( "相册", "photo" );
            dicApp.Add( "微博", "microblog" );
            dicApp.Add( "分享", "share" );
            dicApp.Add( "好友", "friend" );
            dicApp.Add( "访客", "visitor" );
            dicApp.Add( "论坛帖子", "forumpost" );
            dicApp.Add( "关于我", "about" );
            dicApp.Add( "留言", "feedback" );
            checkboxList( "initApp", dicApp, config.Instance.Site.UserInitApp );

            // app部署管理：按照分类归类，分类进行排序
            List<AppInstaller> list = cdb.findAll<AppInstaller>();
            bindList( "list", "app", list, bindAppAdminLink );

            // 基础组件
            List<Component> clist = cdb.findAll<Component>();
            IBlock cblock = getBlock( "clist" );
            foreach (Component c in clist) {
                cblock.Set( "c.Name", c.Name );
                cblock.Set( "c.StatusName", c.StatusName );
                cblock.Set( "c.AdminLink", to( EditComponent, c.Id ) );
                cblock.Next();
            }

        }


        public void AppSave() {
            String initApp = ctx.Post( "initApp" );
            config.Instance.Site.UserInitApp = initApp; config.Instance.Site.Update( "UserInitApp", initApp );
            echoRedirect( lang( "opok" ) );
        }

        private void bindAppAdminLink( IBlock block, String lbl, object obj ) {
            AppInstaller ai = obj as AppInstaller;
            block.Set( "app.StatusAdminName", "修改" );
            block.Set( "app.StatusAdminLink", to( EditStatus, ai.Id ) );
        }

        public void EditStatus( int id ) {

            target( UpdateStatus, id );

            AppInstaller installer = cdb.findById<AppInstaller>( id );
            if (installer == null) throw new NullReferenceException();

            set( "installer.Name", installer.Name );
            set( "installer.Description", installer.Description );


            List<AppCategory> cats = new List<AppCategory>();
            if (installer.CatId == AppCategory.General) {
                cats = AppCategory.GetAllWithoutGeneral();
            }
            else {
                cats.Add( AppCategory.GetByCatId( installer.CatId ) );
            }
            checkboxList( "appCheckboxList", cats, "Name=TypeFullName", installer.StatusValue );

            radioList( "closeMode", AppCloseMode.GetAllMode(), "Name=Id", installer.CloseMode );
        }

        [HttpPost]
        public void UpdateStatus( int id ) {

            AppInstaller installer = cdb.findById<AppInstaller>( id );
            if (installer == null) throw new NullReferenceException();

            String name = ctx.Post( "Name" );
            String description = strUtil.CutString( ctx.Post( "Description" ), 150 );

            if (strUtil.IsNullOrEmpty( name )) {
                errors.Add( "请填写名称" );
                run( EditStatus, id );
                return;
            }

            if (strUtil.IsNullOrEmpty( description )) {
                errors.Add( "请填写简介" );
                run( EditStatus, id );
                return;
            }

            int closeMode = ctx.PostInt( "closeMode" );

            installer.Name = name;
            installer.Description = description;
            installer.CloseMode = closeMode;

            installer.update();


            String val = ctx.Post( "appCheckboxList" );

            appService.UpdateStatus( installer, val );

            echoToParentPart( lang( "opok" ), to( App ), 0 );
        }


        public void EditComponent( int id ) {
            target( SaveComponent, id );
            Component c = cdb.findById<Component>( id );
            set( "c.Name", c.Name );
            radioList( "status", ComponentStatus.GetAllStatus(), "Name=Id", c.Status );
        }

        public void SaveComponent( int id ) {
            Component c = cdb.findById<Component>( id );
            c.Status = ctx.PostInt( "status" );
            c.update();
            echoToParentPart( lang( "opok" ), to( App ), 0 );
        }



        //---------------------------------------------------------------------------------------------

        public void Logo() {

            target( LogoSave );

            String logo;
            if (strUtil.HasText( config.Instance.Site.SiteLogo )) {
                logo = "<img src=\"" + config.Instance.Site.SiteLogoFull + "\"/>";
                logo += "<div><a href=\"" + to( DeleteLogo ) + "\" class=\"cmd deleteCmd\">× " + lang( "textReplaceLogo" ) + "</a></div>";
            }
            else {
                logo = "<div class=\"nologo\">" + lang( "notUploadLogo" ) + "</div>";
            }

            set( "currentLogo", logo );

        }

        [HttpPost, DbTransaction]
        public void LogoSave() {

            Result result = Uploader.SaveSiteLogo( ctx.GetFileSingle() );

            if (result.IsValid) {

                String logoThumb = result.Info.ToString();
                String logo = wojilu.Drawing.Img.GetOriginalPath( logoThumb );


                config.Instance.Site.SiteLogo = logo;
                config.Instance.Site.Update( "SiteLogo", logo );
                log( SiteLogString.UpdateLogo() );
                redirect( Logo );
            }
            else {
                errors.Join( result );
                run( Logo );
            }

        }

        [HttpDelete, DbTransaction]
        public void DeleteLogo() {

            if (strUtil.HasText( config.Instance.Site.SiteLogo )) {

                wojilu.Drawing.Img.DeleteImgAndThumb( strUtil.Join( sys.Path.DiskPhoto, config.Instance.Site.SiteLogo ) );
                config.Instance.Site.SiteLogo = "";
                config.Instance.Site.Update( "SiteLogo", "" );
                log( SiteLogString.DeleteLogo() );

                redirect( Logo );

            }
            else
                echoRedirect( lang( "notUploadLogo" ) );
        }

        //--------------------------------------------------------------------------------------------------------------

        public void Base() {

            target( BaseSave );

            bind( "site", config.Instance.Site );

            set( "site.SpiderString", config.Instance.Site.GetValue( "Spider" ) );
            set( "site.UploadFileTypesString", config.Instance.Site.GetValue( "UploadFileTypes" ) );
            set( "site.UploadPicTypesString", config.Instance.Site.GetValue( "UploadPicTypes" ) );

            set( "closeCommentChecked", config.Instance.Site.CloseComment ? "checked=\"checked\"" : "" );

            set( "statsChecked", config.Instance.Site.StatsEnabled ? "checked=\"checked\"" : "" );
            set( "statsJs", strUtil.EncodeTextarea( config.Instance.Site.StatsJs ) );
        }

        [HttpPost, DbTransaction]
        public void BaseSave() {

            String SiteName = ctx.Post( "SiteName" );
            String SiteUrl = ctx.Post( "SiteUrl" );
            String Webmaster = ctx.Post( "Webmaster" );
            String Email = ctx.Post( "Email" );

            String Keywords = ctx.Post( "Keywords" );
            String PageDefaultTitle = ctx.Post( "PageDefaultTitle" );
            String Description = ctx.Post( "Description" );
            String ExceptionInfo = ctx.PostHtml( "ExceptionInfo" );

            String spiderString = ctx.Post( "SpiderString" );
            String uploadFileTypes = ctx.Post( "UploadFileTypes" );
            String uploadPicTypes = ctx.Post( "UploadPicTypes" );

            int UploadFileMaxMB = ctx.PostInt( "UploadFileMaxMB" );
            int UploadPicMaxMB = ctx.PostInt( "UploadPicMaxMB" );


            Boolean CloseComment = ctx.PostIsCheck( "CloseComment" ) == 1 ? true : false;


            Boolean StatsEnabled = ctx.PostIsCheck( "StatsEnabled" ) == 1 ? true : false;
            String StatsJs = ctx.PostHtmlAll( "StatsJs" );

            if (strUtil.IsNullOrEmpty( SiteName )) errors.Add( lang( "exSiteName" ) );
            if (strUtil.IsNullOrEmpty( SiteUrl )) errors.Add( lang( "exUrl" ) );

            if (ctx.HasErrors)
                echoError();
            else {


                config.Instance.Site.SiteName = SiteName; config.Instance.Site.Update( "SiteName", SiteName );
                config.Instance.Site.SiteUrl = SiteUrl; config.Instance.Site.Update( "SiteUrl", SiteUrl );
                config.Instance.Site.Webmaster = Webmaster; config.Instance.Site.Update( "Webmaster", Webmaster );
                config.Instance.Site.Email = Email; config.Instance.Site.Update( "Email", Email );
                config.Instance.Site.Keywords = Keywords; config.Instance.Site.Update( "Keywords", Keywords );
                config.Instance.Site.PageDefaultTitle = PageDefaultTitle; config.Instance.Site.Update( "PageDefaultTitle", PageDefaultTitle );
                config.Instance.Site.Description = Description; config.Instance.Site.Update( "Description", Description );

                config.Instance.Site.Spider = SiteSetting.GetArrayValueByString( spiderString );
                config.Instance.Site.Update( "Spider", spiderString );


                config.Instance.Site.UploadFileTypes = SiteSetting.GetArrayValueByString( uploadFileTypes );
                config.Instance.Site.Update( "UploadFileTypes", uploadFileTypes );

                config.Instance.Site.UploadPicTypes = SiteSetting.GetArrayValueByString( uploadPicTypes );
                config.Instance.Site.Update( "UploadPicTypes", uploadPicTypes );

                config.Instance.Site.UploadFileMaxMB = UploadFileMaxMB;
                config.Instance.Site.Update( "UploadFileMaxMB", UploadFileMaxMB );

                config.Instance.Site.UploadPicMaxMB = UploadPicMaxMB;
                config.Instance.Site.Update( "UploadPicMaxMB", UploadPicMaxMB );


                config.Instance.Site.CloseComment = CloseComment; config.Instance.Site.Update( "CloseComment", CloseComment );

                config.Instance.Site.StatsJs = StatsJs; config.Instance.Site.UpdateHtml( "StatsJs", StatsJs );
                config.Instance.Site.StatsEnabled = StatsEnabled; config.Instance.Site.Update( "StatsEnabled", StatsEnabled );

                log( SiteLogString.EditSiteSettingBase() );

                echoRedirect( lang( "opok" ) );
            }

        }

        public void Close() {

            target( SaveClose );
            set( "closeChecked", config.Instance.Site.IsClose ? "checked=\"checked\"" : "" );
            editor( "CloseReason", config.Instance.Site.CloseReason, "150px" );

        }

        public void SaveClose() {

            Boolean isClose = ctx.PostIsCheck( "IsClose" ) == 1 ? true : false;
            String CloseReason = ctx.PostHtml( "CloseReason" );

            config.Instance.Site.CloseReason = CloseReason; config.Instance.Site.UpdateHtml( "CloseReason", CloseReason );
            config.Instance.Site.IsClose = isClose; config.Instance.Site.Update( "IsClose", isClose );

            IDictionaryEnumerator e = HttpRuntime.Cache.GetEnumerator();

            while (e.MoveNext()) {
                DictionaryEntry entry = e.Entry;
                HttpRuntime.Cache.Remove( entry.Key.ToString() );
            }


            log( SiteLogString.EditSiteSettingBase() );

            echoRedirect( lang( "opok" ) );

        }

        //--------------------------------------------------------------------------------------------------------------

        public void User() {

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

            editor( "SystemMsgContent", config.Instance.Site.SystemMsgContent, "100px" );

            set( "site.ReservedUserNameStr", config.Instance.Site.GetValue( "ReservedUserName" ) );
            set( "site.ReservedUserUrlStr", config.Instance.Site.GetValue( "ReservedUserUrl" ) );
            set( "site.ReservedKeyString", config.Instance.Site.GetValue( "ReservedKey" ) );


        }

        [HttpPost, DbTransaction]
        public void UserSave() {

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

            String SystemMsgTitle = ctx.Post( "SystemMsgTitle" );
            String SystemMsgContent = ctx.PostHtml( "SystemMsgContent" );


            String ReservedUserNameStr = ctx.Post( "ReservedUserName" );
            String ReservedUserUrlStr = ctx.Post( "ReservedUserUrl" );
            String ReservedKeyString = ctx.Post( "ReservedKeyString" );

            if (UserNameLengthMin <= 0) errors.Add( lang( "exUserNameMinGreater0" ) );
            if (UserNameLengthMax <= 0) errors.Add( lang( "exUserNameMaxGreater0" ) );
            if (strUtil.IsNullOrEmpty( SystemMsgTitle )) errors.Add( lang( "exMsgTitle" ) );
            if (strUtil.IsNullOrEmpty( SystemMsgContent )) errors.Add( lang( "exMsgContent" ) );

            if (ctx.HasErrors)
                echoError();
            else {

                config.Instance.Site.NeedLogin = needLogin; config.Instance.Site.Update( "NeedLogin", needLogin );
                config.Instance.Site.UserNeedApprove = userNeedApprove; config.Instance.Site.Update( "UserNeedApprove", userNeedApprove );

                config.Instance.Site.AlertActivation = alertActivation; config.Instance.Site.Update( "AlertActivation", alertActivation );
                config.Instance.Site.AlertUserPic = alertUserPic; config.Instance.Site.Update( "AlertUserPic", alertUserPic );

                config.Instance.Site.RegisterType = registerType; config.Instance.Site.Update( "RegisterType", registerType );
                config.Instance.Site.LoginType = loginType; config.Instance.Site.Update( "LoginType", loginType );
                config.Instance.Site.TopNavDisplay = topNavDisplay; config.Instance.Site.Update( "TopNavDisplay", topNavDisplay );

                config.Instance.Site.UserSendConfirmEmailInterval = confirmEmailInterval;
                config.Instance.Site.Update( "UserSendConfirmEmailInterval", confirmEmailInterval );

                config.Instance.Site.UserNameLengthMin = UserNameLengthMin; config.Instance.Site.Update( "UserNameLengthMin", UserNameLengthMin );
                config.Instance.Site.UserNameLengthMax = UserNameLengthMax; config.Instance.Site.Update( "UserNameLengthMax", UserNameLengthMax );
                config.Instance.Site.SystemMsgTitle = SystemMsgTitle; config.Instance.Site.Update( "SystemMsgTitle", SystemMsgTitle );
                config.Instance.Site.SystemMsgContent = SystemMsgContent; config.Instance.Site.Update( "SystemMsgContent", SystemMsgContent );

                config.Instance.Site.ReservedUserName = SiteSetting.GetArrayValueByString( ReservedUserNameStr );
                config.Instance.Site.Update( "ReservedUserName", ReservedUserNameStr );

                config.Instance.Site.ReservedUserUrl = SiteSetting.GetArrayValueByString( ReservedUserUrlStr );
                config.Instance.Site.Update( "ReservedUserUrl", ReservedUserUrlStr );

                config.Instance.Site.ReservedKey = SiteSetting.GetArrayValueByString( ReservedKeyString );
                config.Instance.Site.Update( "ReservedKey", ReservedKeyString );

                log( SiteLogString.EditSiteSettingUser() );

                echoRedirect( lang( "opok" ) );
            }
        }

        //--------------------------------------------------------------------------------------------------------------

        public void Email() {
            target( EmailSave );
            set( "EmailEnableLink", to( EmailEnableSave ) );

            bind( "site", config.Instance.Site );

            String chk1 = Html.CheckBox( "SmtpEnableSsl", lang( "enable" ), "1", config.Instance.Site.SmtpEnableSsl );
            set( "site.CheckSmtpEnableSsl", chk1 );

            String chk2 = Html.CheckBox( "EnableEmail", lang( "enable" ), "1", config.Instance.Site.EnableEmail );
            set( "site.EnableEmail", chk2 );

        }

        [HttpPost, DbTransaction]
        public void EmailEnableSave() {
            Boolean EnableEmail = ctx.PostInt( "EnableEmail" ) == 1 ? true : false;
            config.Instance.Site.EnableEmail = EnableEmail; config.Instance.Site.Update( "EnableEmail", EnableEmail );
            echoRedirect( lang( "opok" ) );
        }

        [HttpPost, DbTransaction]
        public void EmailSave() {

            String SmtpUrl = ctx.Post( "SmtpUrl" );
            String SmtpUser = ctx.Post( "SmtpUser" );
            String SmtpPwd = ctx.Post( "SmtpPwd" );
            Boolean SmtpEnableSsl = ctx.PostInt( "SmtpEnableSsl" ) == 1 ? true : false;

            //if (strUtil.IsNullOrEmpty( SmtpUrl )) errors.Add( lang( "exServer" ) );
            //if (strUtil.IsNullOrEmpty( SmtpUser )) errors.Add( lang( "exUserName" ) );
            //if (strUtil.IsNullOrEmpty( SmtpPwd )) errors.Add( lang( "exPwd" ) );

            if (ctx.HasErrors)
                echoError();
            else {
                config.Instance.Site.SmtpUrl = SmtpUrl; config.Instance.Site.Update( "SmtpUrl", SmtpUrl );
                config.Instance.Site.SmtpUser = SmtpUser; config.Instance.Site.Update( "SmtpUser", SmtpUser );
                config.Instance.Site.SmtpPwd = SmtpPwd; config.Instance.Site.Update( "SmtpPwd", SmtpPwd );
                config.Instance.Site.SmtpEnableSsl = SmtpEnableSsl; config.Instance.Site.Update( "SmtpEnableSsl", SmtpEnableSsl );

                log( SiteLogString.EditSiteSettingEmail() );

                echoRedirect( lang( "opok" ) );
            }

        }

        //--------------------------------------------------------------------------------------------------------------

        public void Filter() {
            target( FilterSave );
            bind( "site", config.Instance.Site );
            set( "site.BadWordsStr", config.Instance.Site.GetValue( "BadWords" ) );
        }

        [HttpPost, DbTransaction]
        public void FilterSave() {

            String BadWordsStr = ctx.Post( "BadWords" );
            String BadWordsReplacement = ctx.Post( "BadWordsReplacement" );

            config.Instance.Site.BadWords = SiteSetting.GetArrayValueByString( BadWordsStr );
            config.Instance.Site.Update( "BadWords", BadWordsStr );

            config.Instance.Site.BadWordsReplacement = BadWordsReplacement;
            config.Instance.Site.Update( "BadWordsReplacement", BadWordsReplacement );
            log( SiteLogString.EditSiteSettingFilter() );

            echoRedirect( lang( "opok" ) );

        }

        //--------------------------------------------------------------------------------------------------------------

        public void BanIp() {

            target( SaveBanIp );

            StringBuilder sb = new StringBuilder();
            foreach (String ip in config.Instance.Site.BannedIp) {
                sb.AppendLine( ip );
            }

            set( "site.BannedIp", sb );
            set( "site.BannedIpInfo", config.Instance.Site.BannedIpInfo );
        }

        public void SaveBanIp() {

            String bannedIp = ctx.Post( "BannedIp" );

            String[] arrItem = bannedIp.Split( '\n' );
            StringBuilder sb = new StringBuilder();
            foreach (String ip in arrItem) {
                if (strUtil.HasText( ip )) {
                    sb.Append( ip );
                    sb.Append( "/" );
                }
            }

            String ips = sb.ToString().TrimEnd( '/' );

            config.Instance.Site.BannedIp = SiteSetting.GetArrayValueByString( ips );
            config.Instance.Site.Update( "BannedIp", ips );

            String bannedIpInfo = ctx.Post( "BannedIpInfo" );
            config.Instance.Site.BannedIpInfo = bannedIpInfo;
            config.Instance.Site.Update( "BannedIpInfo", bannedIpInfo );

            echoRedirect( lang( "opok" ) );

        }

    }

}
