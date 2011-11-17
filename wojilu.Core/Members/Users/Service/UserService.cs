/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Web.Url;
using wojilu.Web.Context;

using wojilu.Members.Users.Interface;
using wojilu.Members.Users.Domain;
using wojilu.Members.Users.Enum;
using wojilu.Members.Sites.Service;
using wojilu.Members.Sites.Domain;

using wojilu.Common;
using wojilu.Common.Money.Service;
using wojilu.Common.Msg.Interface;
using wojilu.Common.Msg.Service;
using wojilu.Common.AppBase;
using wojilu.Common.Menus.Interface;
using wojilu.Common.MemberApp.Interface;
using wojilu.Common.Money.Interface;
using wojilu.Common.Money.Domain;

namespace wojilu.Members.Users.Service {

    public class UserService : IUserService {

        public UserService() {
            currencyService = new CurrencyService();
            roleService = new SiteRoleService();
            userIncomeService = new UserIncomeService();
            //msgService = new MessageService();

            hashTool = new HashTool();
        }

        public virtual ICurrencyService currencyService { get; set; }
        public virtual ISiteRoleService roleService { get; set; }
        public virtual IUserIncomeService userIncomeService { get; set; }
        public virtual IHashTool hashTool { get; set; }
        //public IMessageService msgService { get; set; } 

        //----------------------------------------------------------------------

        // 请勿删除本方法.ContentController.bindAutoData用到
        public User GetCurrent() {
            return null;
        }

        public virtual int GetUserCount() {
            return User.count();
        }

        public virtual User IsNamePwdCorrect( String name, String pwd ) {

            if (strUtil.IsNullOrEmpty( name )) return null;
            if (strUtil.IsNullOrEmpty( pwd )) return null;

            User user = GetByName( name );
            if (user == null) return null;

            String hashedPwd = HashPwd( pwd, user.PwdSalt );
            return user.Pwd == hashedPwd ? user : null;
        }

        public virtual User IsNameEmailPwdCorrect( String nameOrEmail, String pwd ) {

            if (strUtil.IsNullOrEmpty( nameOrEmail )) return null;
            if (strUtil.IsNullOrEmpty( pwd )) return null;

            User user;
            if (nameOrEmail.IndexOf( "@" ) > 0)
                user = GetByMail( nameOrEmail );
            else
                user = GetByName( nameOrEmail );
            if (user == null) return null;

            String hashedPwd = HashPwd( pwd, user.PwdSalt );
            return user.Pwd == hashedPwd ? user : null;
        }


        public virtual String HashPwd( String pwd, String salt ) {

            if (config.Instance.Site.Md5Is16) {
                return hashTool.Get( pwd.Trim(), HashType.MD5_16 );
            }

            return hashTool.GetBySalt( pwd.Trim(), salt, HashType.SHA384 );
        }

        public virtual Boolean IsPwdCorrect( User user, String pwd ) {
            String hashedPwd = HashPwd( pwd, user.PwdSalt );
            return user.Pwd == hashedPwd;
        }

        public virtual void UpdatePwd( User user, String pwd ) {
            user.Pwd = HashPwd( pwd, user.PwdSalt );
            db.update( user, "Pwd" );
        }

        public virtual String GetLastUserName() {
            List<User> users = GetNewList( 1 );
            return users.Count > 0 ? users[0].Name : "";
        }

        public virtual List<IBinderValue> GetNewListWithAvatar( int count ) {
            if (count <= 0) count = 10;
            List<User> list = getNewListWithAvatar( count );

            List<IBinderValue> results = new List<IBinderValue>();
            foreach (User user in list) {
                IBinderValue vo = new ItemValue();
                vo.Title = user.Name;
                vo.PicUrl = user.PicSmall;
                vo.Link = Link.ToUser( user.Url );
                results.Add( vo );
            }

            return results;
        }

        public virtual DataPage<User> GetAll( int pageSize ) {
            return db.findPage<User>( "", pageSize );
        }

        public virtual DataPage<User> GetAllValid( int pageSize ) {
            return db.findPage<User>( "Status>=0", pageSize );
        }
        //----------------------------------------------------------------------


        public virtual void DeletePostCount( int creatorId ) {
            if (creatorId <= 0) return;
            User user = GetById( creatorId );
            if (user == null) return;
            user.PostCount--;
            db.update( user, "PostCount" );
        }

        //----------------------------------------------------------------------

        public virtual User Register( User user, MvcContext ctx ) {

            if (isNameReserved( user.Name )) {
                ctx.errors.Add( lang.get( "exNameFound" ) );
                return null;
            }

            if (isUrlReserved( user.Url )) {
                ctx.errors.Add( lang.get( "exUrlFound" ) );
                return null;
            }


            if (validateUnique( user, ctx ) == false) return null;

            setProfileAndTemplateAndHashPasswork( user );
            user.RoleId = SiteRole.NormalMember.Id;
            Result result = db.insert( user );
            if (result.HasErrors) {
                db.delete( user.Profile );
                ctx.errors.Join( result );
                return null;
            }

            if (Component.IsEnableUserSpace() == false) {
                user.Url = "user" + user.Id;
                db.update( user, "Url" );
            }

            sendMsg( user );

            userIncomeService.InitUserIncome( user );

            if (isFirstUser()) {
                user.RoleId = SiteRole.Administrator.Id;
                db.update( user, "RoleId" );
            }

            return user;
        }

        private Boolean isFirstUser() {
            return db.count<User>() == 1;
        }

        public virtual Boolean IsNameReservedOrExist( String inputName ) {
            Boolean isReserved = isNameReserved( inputName );
            if (isReserved) return true;
            return IsExist( inputName ) != null;
        }

        public virtual Boolean IsUrlReservedOrExist( String inputUrl ) {
            Boolean isReserved = isUrlReserved( inputUrl );
            if (isReserved) return true;
            return IsExistUrl( inputUrl ) != null;
        }


        private Boolean isNameReserved( String inputName ) {

            if (config.Instance.Site.IsReservedKeyContains( inputName )) return true;

            foreach (String name in config.Instance.Site.ReservedUserName) {
                if (strUtil.EqualsIgnoreCase( inputName, name )) {
                    return true;
                }
            }

            return false;
        }

        private Boolean isUrlReserved( String inputUrl ) {

            if (config.Instance.Site.IsReservedKeyContains( inputUrl )) return true;

            foreach (String name in config.Instance.Site.ReservedUserUrl) {
                if (strUtil.EqualsIgnoreCase( inputUrl, name )) {
                    return true;
                }
            }

            return false;
        }

        private Boolean validateUnique( User user, MvcContext ctx ) {
            if (IsExist( user.Name ) != null) {
                ctx.errors.Add( lang.get( "exNameFound" ) );
                return false;
            }
            if (strUtil.HasText( user.Url ) && IsExistUrl( user.Url ) != null) {
                ctx.errors.Add( lang.get( "exUrlFound" ) );
                return false;
            }
            return true;
        }

        protected virtual void sendMsg( User member ) {

            String title = config.Instance.Site.SystemMsgTitle;
            String body = config.Instance.Site.SystemMsgContent;

            IMessageService service = new MessageService();
            service.SiteSend( title, body, member );
        }

        private void setProfileAndTemplateAndHashPasswork( User user ) {
            MemberProfile profile = new MemberProfile();
            db.insert( profile );
            user.ProfileId = profile.Id;
            user.TemplateId = config.Instance.Site.UserTemplateId;
            user.GroupId = 3;

            user.PwdSalt = hashTool.GetSalt( 4 );
            user.Pwd = HashPwd( user.Pwd, user.PwdSalt );

        }


        public virtual List<User> GetNewLoginList( int count ) {
            if (count <= 0) count = 10;
            return db.find<User>( "order by LastLoginTime desc, Id desc" ).list( count );
        }


        public virtual List<User> GetRankedToMakeFriends( int count, List<int> ids ) {

            List<User> results = new List<User>();
            List<User> list = db.find<User>( "order by Credit desc, Hits desc, Id desc" ).list( count );
            foreach (User user in list) {
                if (ids.Contains( user.Id )) continue;
                results.Add( user );
            }
            return results;
        }


        public virtual List<User> GetRanked( int count ) {
            return db.find<User>( "order by Credit desc, Hits desc, Id desc" ).list( count );
        }

        public virtual List<User> GetRanked( String sortBy, int count ) {

            Dictionary<string, string> ranks = new Dictionary<string, string>();
            ranks.Add( "credit", "Credit" );
            ranks.Add( "hits", "Hits" );
            ranks.Add( "blogs", "blogs" );
            ranks.Add( "blogComments", "blogComments" );
            ranks.Add( "photos", "photos" );
            ranks.Add( "posts", "PostCount" );

            String sortField = ranks.ContainsKey( sortBy ) ? ranks[sortBy] : "Credit";

            return db.find<User>( "order by " + sortField + " desc, Id desc" ).list( count );
        }


        public virtual DataPage<User> SearchBy( String condition, int pageSize ) {
            if (pageSize <= 0) pageSize = 20;
            return db.findPage<User>( condition, pageSize );
        }

        public virtual List<User> SearchByName( String name ) {
            name = strUtil.SqlClean( name, 20 );
            return db.find<User>( "Name like  '%" + name + "%' " ).list();
        }


        public virtual List<User> GetByIds( String idsStr ) {
            if (strUtil.IsNullOrEmpty( idsStr )) return new List<User>();
            return db.find<User>( "id in (" + idsStr + ") " ).list();
        }

        public virtual List<User> GetUnSendConfirmEmailUsers() {
            return db.find<User>( "IsEmailConfirmed=" + EmailConfirm.UnSendEmail ).list();
        }

        public virtual void SendConfirmEmail( User user ) {
            user.IsEmailConfirmed = EmailConfirm.UnConfirmed;
            db.update( user, "IsEmailConfirmed" );
        }


        public virtual void ConfirmEmailIsError( User user ) {
            user.IsEmailConfirmed = EmailConfirm.EmailError;
            db.update( user, "IsEmailConfirmed" );
        }


        public virtual Boolean IsUserDeleted( User user ) {
            return user.Status == MemberStatus.Deleted;
        }


        public virtual void UpdateAvatar( User user, String newPic ) {

            Boolean isFirst = (user.HasUploadPic()==false);

            user.Pic = newPic;
            db.update( user, "Pic" );

            if (isFirst) {
                addIncomeAndSendMsg( user );
            }

        }

        private void addIncomeAndSendMsg( User user ) {

            int actionId = 17;
            KeyIncomeRule rule = currencyService.GetKeyIncomeRulesByAction( actionId );

            int creditValue = rule.Income;
            String creditName = rule.CurrencyName;

            userIncomeService.AddIncome( user, actionId );

            String msgTitle = "感谢您上传头像";
            String msgBody = string.Format( "{0}：<br/>您好！<br/>感谢您上传头像，您因此获得{1}奖励，共{2}分。<br/>欢迎继续参与，谢谢。<br/>------------------------------------------<br/>这是系统自动发信，请勿回复。", user.Name, creditName, creditValue );


            MessageService msgService = new MessageService();

            msgService.SiteSend( msgTitle, msgBody, user );
        }

        //-------------------------------------------------------------

        public virtual User GetById( int id ) {
            return User.findById( id );
        }

        public virtual User GetByName( String name ) {
            return User.find( "Name=:name" ).set( "name", name ).first();
        }

        public virtual User GetByUrl( String friendUrl ) {
            return User.find( "Url=:furl" ).set( "furl", friendUrl ).first();
        }

        public virtual User GetByMail( String email ) {
            return db.find<User>( "Email=:email" ).set( "email", email ).first();
        }

        public virtual List<User> GetHitsList( int count ) {
            return User.find( "order by Hits desc, Id asc" ).list( count );
        }

        public virtual List<User> GetNewList( int count ) {
            return User.find( "order by Id desc" ).list( count );
        }

        public virtual List<User> GetNewListValid( int count ) {
            return User.find( "status>=0 order by Id desc" ).list( count );
        }

        public virtual List<User> GetPickedList( int count ) {
            return User.find( "Status=" + MemberStatus.Pick ).list( count );
        }

        public virtual void AddPostCount( User user ) {
            user.PostCount++;
            db.update( user, "PostCount" );
        }

        public virtual User IsExist( String name ) {
            return User.find( "Name=:name" ).set( "name", name ).first();
        }

        public virtual User IsExistUrl( String url ) {
            return User.find( "Url=:url" ).set( "url", url ).first();
        }

        public virtual Boolean IsEmailExist( String email ) {
            User user = User.find( "Email=:email and IsEmailConfirmed=" + EmailConfirm.Confirmed ).set( "email", email ).first();
            return user != null;
        }

        public virtual Boolean IsEmailExist( int userId, String email ) {
            User user = User.find( "Email=:email and IsEmailConfirmed=" + EmailConfirm.Confirmed + " and Id<>"+userId ).set( "email", email ).first();
            return user != null;
        }

        //-------------------------------------------------------------


        private List<User> getNewListWithAvatar( int count ) {
            return User.find( "Pic is not null and Pic<>'" + SysPath.AvatarConstString + "'  order by Id desc" ).list( count );
        }


    }
}

