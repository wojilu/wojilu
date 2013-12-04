/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Web.Utils;
using wojilu.Common.AppInstall;
using wojilu.Common.AppBase;

using wojilu.Members.Users.Domain;
using wojilu.Members.Users.Service;
using wojilu.Members.Users.Interface;
using wojilu.Common.MemberApp.Interface;
using wojilu.Web.Context;

using wojilu.OAuth;

using wojilu.Common;
using wojilu.Members.Sites.Domain;
using wojilu.Common.Money.Interface;
using wojilu.Common.Money.Service;

namespace wojilu.Web.Controller.Users.Admin {


    public partial class UserProfileController : ControllerBase {

        public virtual IUserService userService { get; set; }
        public virtual IMemberAppService userAppService { get; set; }
        public virtual IAppInstallerService appinfoService { get; set; }
        public virtual IUserTagService userTagService { get; set; }

        public virtual ICurrencyService currencyService { get; set; }
        public virtual IUserConnectService connectService { get; set; }

        public UserProfileController() {
            userService = new UserService();
            userAppService = new UserAppService();
            appinfoService = new AppInstallerService();

            userTagService = new UserTagService();
            currencyService = new CurrencyService();
            connectService = new UserConnectService();
        }

        public override void Layout() {

            set( "viewer.ProfileUrl", to( Profile ) );
            set( "viewer.InterestUrl", to( Interest ) );
            set( "viewer.ContactLink", to( Contact ) );
            set( "viewer.TagUrl", to( Tag ) );
            set( "viewer.AccountBind", to( BindAccount ) );
            set( "viewer.FaceUrl", to( Face ) );
            set( "viewer.PwdUrl", to( Pwd ) );

            set( "viewer.LogoutUrl", to( new wojilu.Web.Controller.MainController().Logout ) );

            Boolean isUserPrivacyClose = Component.IsClose( typeof( UserPrivacy ) );
            if (isUserPrivacyClose) {
                set( "viewer.PrivacyLink", "#" );
                set( "privacyLinkStyle", "display:none;" );
            }
            else {
                set( "viewer.PrivacyLink", to( Privacy ) );
                set( "privacyLinkStyle", "" );
            }
        }

        public virtual void Index() {
        }

        public virtual void BindAccount() {


            List<AuthConnectConfig> list = AuthConnectConfig.GetAll();

            IBlock block = getBlock( "list" );

            foreach (AuthConnectConfig x in list) {

                if (x.IsStop == 1) continue;

                block.Set( "connect.Logo", x.LogoM );
                block.Set( "connect.Name", x.Name );

                IBlock yBlock = block.GetBlock( "bind" );
                IBlock xBlock = block.GetBlock( "unbind" );

                String q = "?connectType=" + x.TypeFullName;

                UserConnect c = connectService.GetConnectInfo( ctx.viewer.Id, x.TypeFullName );
                if (c != null) {
                    yBlock.Set( "connect.Uid", c.Uid );
                    yBlock.Set( "connect.Name", c.Name );
                    yBlock.Set( "connect.UnBindLink", to( new ConnectController().UnBind ) + q );
                    yBlock.Set( "connect.SyncLink", to( new ConnectController().Sync ) + q );
                    yBlock.Set( "connect.CheckSync", c.NoSync == 0 ? "checked=\"checked\"" : "" );
                    yBlock.Next();
                }
                else {
                    xBlock.Set( "connect.BindLink", to( new ConnectController().Bind ) + q );
                    xBlock.Next();
                }

                block.Next();
            }

        }

        //-----------------------------------------------------------------------------------

        public virtual void Face() {

            target( FaceSave );
            User user = ctx.owner.obj as User;
            bindFace( user );
        }

        [HttpPost, DbTransaction]
        public virtual void FaceSave() {

            User user = ctx.owner.obj as User;

            Result result = AvatarUploader.Save( ctx.GetFileSingle(), user.Id );
            if (result.HasErrors) {
                errors.Join( result );
                run( Face );
                return;
            }

            if (user.Pic != UserFactory.Guest.Pic) {
                AvatarUploader.Delete( user.Pic );
            }

            userService.UpdateAvatar( user, result.Info.ToString() );

            if (ctx.utils.isFrame()) {
                echoToParent( lang( "uploadThanks" ) );
            }
            else {
                echoRedirectPart( lang( "uploadThanks" ), to( Face ), 0 );
            }
        }


        //-----------------------------------------------------------------------------------

        public virtual void Interest() {
            target( InterestSave );
            User user = ctx.owner.obj as User;
            bindInterest( user );
        }

        [HttpPost, DbTransaction]
        public virtual void InterestSave() {
            User user = ctx.owner.obj as User;
            SaveInterest( user );
            db.update( user.Profile );
            echoRedirect( lang( "opok" ) );
        }

        //-----------------------------------------------------------------------------------


        public virtual void Tag() {
            target( SaveTag );

            List<UserTagShip> us = userTagService.GetPage( ctx.owner.Id );
            IBlock block = getBlock( "tags" );
            foreach (UserTagShip u in us) {
                block.Set( "tag.Id", u.Tag.Id );
                block.Set( "tag.Name", u.Tag.Name );
                block.Set( "tag.DeleteLink", to( DeleteTag, u.Id ) );
                block.Set( "tag.Link", Link.To( Site.Instance, new MainController().Tag, u.Tag.Id ) );
                block.Next();
            }

        }


        public virtual void SaveTag() {

            String tagList = strUtil.SubString( ctx.Post( "tagList" ), 30 );
            if (strUtil.IsNullOrEmpty( tagList )) {
                echoError( "请填写内容" );
                return;
            }

            userTagService.SaveTags( tagList, ctx.viewer.Id, (User)ctx.owner.obj );
            echoRedirect( lang( "opok" ), Tag );
        }

        public virtual void DeleteTag( long id ) {
            UserTagShip u = userTagService.GetById( id );
            if (u != null) {
                userTagService.DeleteUserTag( u );

                echoAjaxOk();
            }
            else {
                echoText( "标签不存在" );
            }
        }



        public virtual void Profile() {
            target( ProfileSave );
            User user = ctx.owner.obj as User;
            bindProfile( user );
        }

        [HttpPost, DbTransaction]
        public virtual void ProfileSave() {
            User user = ctx.owner.obj as User;
            SaveProfile( user, ctx );
            db.update( user );
            db.update( user.Profile );
            echoRedirect( lang( "opok" ) );
        }

        public virtual void Contact() {
            target( ContactSave );
            User user = ctx.owner.obj as User;
            bindContact( user );

            IBlock yblock = getBlock( "ymail" );
            IBlock xblock = getBlock( "xmail" );
            if (strUtil.HasText( user.Email )) {

                yblock.Set( "m.Email", user.Email );
                yblock.Set( "lnkEditEmail", to( Email ) );
                yblock.Next();

            }
            else {

                xblock.Set( "lnkAddEmail", to( AddEmail ) );
                xblock.Next();
            }

        }

        public virtual void ContactSave() {
            User user = ctx.owner.obj as User;
            saveContact( user );
            db.update( user );
            db.update( user.Profile );
            echoRedirect( lang( "opok" ) );
        }

        //-----------------------------------------------------------------------------------

        public virtual void AddEmail() {
            target( CreateEmail );
        }

        [HttpPost]
        public virtual void CreateEmail() {

            User user = ctx.owner.obj as User;
            if (strUtil.HasText( user.Email )) {
                echoError( "已有email，无法创建" );
                return;
            }

            String email = strUtil.CutString( ctx.Post( "Email" ), 30 );

            if (strUtil.IsNullOrEmpty( email )) {
                errors.Add( lang( "exEmail" ) );
            }
            else if (RegPattern.IsMatch( email, RegPattern.Email ) == false) {
                errors.Add( lang( "exUserMail" ) );
            }
            else if (userService.IsEmailExist( email )) {
                errors.Add( lang( "exEmailFound" ) );
            }

            if (ctx.HasErrors) { echoError(); return; }

            Result result = userService.CreateEmail( user, email );
            echoResult( result );
        }

        public virtual void Email() {
            target( SaveEmail );
            User user = ctx.owner.obj as User;
            set( "userEmail", user.Email );
        }

        [HttpPost]
        public virtual void SaveEmail() {

            String pwd = ctx.Post( "Pwd" );
            String email = strUtil.CutString( ctx.Post( "Email" ), 30 );

            User user = ctx.owner.obj as User;

            if (strUtil.IsNullOrEmpty( pwd )) {
                errors.Add( "请填写密码" );
            }
            else if (strUtil.IsNullOrEmpty( email )) {
                errors.Add( lang( "exEmail" ) );
            }
            else if (RegPattern.IsMatch( email, RegPattern.Email ) == false) {
                errors.Add( lang( "exUserMail" ) );
            }
            else if (userService.IsPwdCorrect( user, pwd ) == false) {
                errors.Add( lang( "exPwdError" ) );
            }
            else if (userService.IsEmailExist( email )) {
                errors.Add( lang( "exEmailFound" ) );
            }

            if (ctx.HasErrors) { echoError(); return; }

            userService.UpdateEmailAndResetConfirmStatus( user, email );

            if (config.Instance.Site.EnableEmail) {
                echoToParent( lang( "opok" ), to( new Common.ActivationController().SendEmailButton ) );
            }
            else {
                echoToParentPart( lang( "opok" ) );
            }
        }

        public virtual void Pwd() {
            view( "Pwd" );

            User user = ctx.owner.obj as User;

            target( PwdSave );
            set( "member.Name", user.Name );
        }

        [HttpPost, DbTransaction]
        public virtual void PwdSave() {

            String opwd = ctx.Post( "OldPwd" );

            String pwd1 = ctx.Post( "NewPwd" );
            String pwd2 = ctx.Post( "NewPwd2" );

            if (strUtil.IsNullOrEmpty( pwd1 )) {
                errors.Add( lang( "exPwdNew" ) );
            }
            else if (!pwd1.Equals( pwd2 )) {
                errors.Add( lang( "exPwdNotSame" ) );
            }

            User user = ctx.owner.obj as User;
            if (userService.IsPwdCorrect( user, opwd ) == false) errors.Add( lang( "exPwdError" ) );

            if (ctx.HasErrors) { echoError(); return; }

            userService.UpdatePwd( user, pwd1 );

            echoRedirect( lang( "opok" ) );
        }

        //-----------------------------------------------------------------------------------

        public virtual void Privacy() {

            Boolean isUserPrivacyClose = Component.IsClose( typeof( UserPrivacy ) );
            if (isUserPrivacyClose) {
                echo( "对不起，本功能已经停用" );
                return;
            }

            //---------------------------------------------------------

            User user = ctx.owner.obj as User;
            if (user == null) {
                echoRedirect( lang( "exUser" ) );
                return;
            }

            target( SavePrivacy );

            String lbl = string.Format( lang( "appPrivacyInfo" ), to( new AppController().Index ) );
            set( "appPrivacyInfo", lbl );

            set( "profileEditLink", to( Profile ) );
            set( "appListLink", to( new AppController().Index ) );

            Dictionary<string, int> settings = UserSecurity.GetSettingsAll( user );
            foreach (KeyValuePair<string, int> s in settings) {
                set( s.Key, Html.DropList( UserPrivacy.DropOptions, s.Key, "Name", "Value", s.Value ) );
            }

            // app permission
            IList apps = userAppService.GetByMember( ctx.owner.Id );
            bindAppList( apps );
        }


        public virtual void EditPermission( long id ) {

            Boolean isUserPrivacyClose = Component.IsClose( typeof( UserPrivacy ) );
            if (isUserPrivacyClose) {
                echo( "对不起，本功能已经停用" );
                return;
            }

            //---------------------------------------------------------


            target( SavePermission, id );

            IMemberApp app = userAppService.FindById( id, ctx.owner.Id );
            AppInstaller info = getAppInfo( app.AppInfoId );
            bindAppInfo( info );

            set( "app.FriendlyName", app.Name );
            set( "app.AccessStatus", AccessStatusUtil.GetRadioList( app.AccessStatus ) );

        }

        private void bindAppInfo( AppInstaller info ) {
            set( "app.Name", info.Name );
            set( "app.Description", info.Description );
            set( "app.Id", info.Id );
        }

        public virtual void SavePermission( long id ) {

            Boolean isUserPrivacyClose = Component.IsClose( typeof( UserPrivacy ) );
            if (isUserPrivacyClose) {
                echo( "对不起，本功能已经停用" );
                return;
            }

            //---------------------------------------------------------

            AccessStatus accs = AccessStatusUtil.GetPostValue( ctx.PostInt( "AccessStatus" ) );
            IMemberApp app = userAppService.FindById( id, ctx.owner.Id );
            userAppService.UpdateAccessStatus( app, accs );
            echoToParentPart( lang( "opok" ) );
        }

        private AppInstaller getAppInfo( long appInfoId ) {
            AppInstaller appinfo = appinfoService.GetById( appInfoId );
            if (appinfo == null) {
                throw new Exception( lang( "exAppNotFound" ) );
            }
            return appinfo;
        }


        public virtual void SavePrivacy() {

            Boolean isUserPrivacyClose = Component.IsClose( typeof( UserPrivacy ) );
            if (isUserPrivacyClose) {
                echo( "对不起，本功能已经停用" );
                return;
            }

            //---------------------------------------------------------


            User user = ctx.owner.obj as User;
            if (user == null) {
                echoRedirect( lang( "exUser" ) );
                return;
            }

            Dictionary<string, int> settings = UserSecurity.GetSettingsAll( user );
            Dictionary<string, int> values = new Dictionary<string, int>();
            foreach (KeyValuePair<string, int> s in settings) {

                int val = UserPrivacy.GetDefaultValue();
                if (ctx.PostHas( s.Key )) val = ctx.PostInt( s.Key );

                values.Add( s.Key, val );
            }

            String settingsStr = UserSecurity.Save( values );


            user.Security = settingsStr;
            db.update( user, "Security" );

            echoRedirect( lang( "opok" ) );

        }

        //-----------------------------------------------------------------------------------



        internal void SaveInterest( User m ) {
            m.Profile.Music = ctx.Post( "Music" );
            m.Profile.Movie = ctx.Post( "Movie" );
            m.Profile.Book = ctx.Post( "Book" );
            m.Profile.Sport = ctx.Post( "Sport" );
            m.Profile.Eat = ctx.Post( "Eat" );
            m.Profile.OtherHobby = ctx.Post( "OtherHobby" );
        }

        internal static void SaveProfile( User m, MvcContext ctx ) {

            m.RealName = ctx.Post( "Name" );
            m.Title = strUtil.SubString( ctx.Post( "Title" ), 20 );

            m.Gender = ctx.PostInt( "Gender" );
            m.Blood = ctx.PostInt( "Blood" );
            m.Degree = ctx.PostInt( "Degree" );
            m.Relationship = ctx.PostInt( "Relationship" );
            m.Zodiac = ctx.PostInt( "Zodiac" );

            m.BirthYear = ctx.PostInt( "Year" );
            m.BirthMonth = ctx.PostInt( "Month" );
            m.BirthDay = ctx.PostInt( "Day" );

            m.ProvinceId1 = ctx.PostInt( "ProvinceId1" );
            m.ProvinceId2 = ctx.PostInt( "ProvinceId2" );
            m.City1 = ctx.Post( "City1" );
            m.City2 = ctx.Post( "City2" );

            m.Profile.Purpose = ctx.Post( "Purpose" );
            m.Profile.ContactCondition = ctx.PostInt( "ContactCondition" );

            if (config.Instance.Site.ShowSexyInfoInProfile) {
                m.Profile.Sexuality = ctx.PostInt( "Sexuality" );
                m.Profile.Smoking = ctx.PostInt( "Smoking" );
                m.Profile.Sleeping = ctx.PostInt( "Sleeping" );
                m.Profile.Body = ctx.PostInt( "Body" );
                m.Profile.Hair = ctx.PostInt( "Hair" );
                m.Profile.Height = ctx.PostInt( "Height" );
                m.Profile.Weight = ctx.PostInt( "Weight" );
                m.Profile.OtherInfo = ctx.Post( "OtherInfo" );
            }

            if (ctx.viewer != null && ctx.viewer.IsAdministrator()) {

                m.Profile.Description = ctx.PostHtmlAll( "Description" );
                m.Signature = ctx.PostHtmlAll( "Signature" );
            }
            else {

                saveDescriptionAndSignature( m, ctx );
            }
        }

        private static void saveDescriptionAndSignature( User m, MvcContext ctx ) {

            Dictionary<String, String> tags = new Dictionary<String, String>();
            tags.Add( "a", "href,target" );
            tags.Add( "br", "" );
            tags.Add( "strong", "" );

            String desc = ctx.PostHtml( "Description", tags );

            if (strUtil.CountString( desc.ToLower(), "<br" ) <= 3) { // 超过3次换行无效，不保存

                if (desc.Length >= config.Instance.Site.UserDescriptionMin) {
                    if (desc.Length > config.Instance.Site.UserDescriptionMax) {
                        m.Profile.Description = strUtil.ParseHtml( desc, config.Instance.Site.UserDescriptionMax );
                    }
                    else {
                        m.Profile.Description = desc;
                    }
                }

            }

            String sign = ctx.PostHtml( "Signature", tags );

            if (strUtil.CountString( sign.ToLower(), "<br" ) <= 3) { // 超过3次换行无效，不保存


                if (sign.Length >= config.Instance.Site.UserSignatureMin) {
                    if (sign.Length > config.Instance.Site.UserSignatureMax) {
                        m.Signature = strUtil.ParseHtml( sign, config.Instance.Site.UserSignatureMax );
                    }
                    else {
                        m.Signature = sign;
                    }
                }

            }

        }



        private void saveContact( User m ) {


            m.Profile.EmailNotify = ctx.PostInt( "EmailNotify" );

            m.QQ = ctx.Post( "QQ" );
            m.MSN = ctx.Post( "MSN" );

            m.Profile.Tel = ctx.Post( "Tel" );
            m.Profile.Address = ctx.Post( "Address" );
            m.Profile.IM = ctx.Post( "IM" );
            m.Profile.WebSite = ctx.Post( "WebSite" );

        }



    }
}

