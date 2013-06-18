/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Mvc;
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
using wojilu.Common.Money.Interface;
using wojilu.Common.Money.Domain;

namespace wojilu.Members.Users.Service {

    public class UserService : IUserService {

        public UserService() {
            currencyService = new CurrencyService();
            roleService = new SiteRoleService();
            userIncomeService = new UserIncomeService();

            hashTool = new HashTool();
        }

        public virtual ICurrencyService currencyService { get; set; }
        public virtual ISiteRoleService roleService { get; set; }
        public virtual IUserIncomeService userIncomeService { get; set; }
        public virtual IHashTool hashTool { get; set; }

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

            if (strUtil.IsNullOrEmpty( user.Pwd )) {
                return strUtil.IsNullOrEmpty( pwd );
            }

            String hashedPwd = HashPwd( pwd, user.PwdSalt );
            return user.Pwd == hashedPwd;
        }

        public virtual void UpdatePwd( User user, String pwd ) {
            user.Pwd = HashPwd( pwd, user.PwdSalt );
            db.update( user, "Pwd" );
        }

        public virtual Result CreateEmail( User user, String email ) {
            user.Email = email;
            return db.update( user );
        }

        public virtual void UpdateEmail( User user, String email ) {
            user.Email = email;
            db.update( user, "Email" );
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

        private Result validateUser( User user ) {

            Result result = new Result();

            if (strUtil.IsNullOrEmpty( user.Name )) {
                result.Add( lang.get( "exUserName" ) );
                return result;
            }

            if (strUtil.IsNullOrEmpty( user.Url )) {
                result.Add( lang.get( "exUrl" ) );
                return result;
            }

            user.Name = user.Name.Trim().TrimEnd( '/' );
            user.Url = user.Url.Trim().TrimEnd( '/' );

            if (user.Url.IndexOf( "http:" ) >= 0) {
                result.Add( lang.get( "exUserUrlHttpError" ) );
            }
            else {
                user.Url = strUtil.SubString( user.Url, config.Instance.Site.UserNameLengthMax );
                user.Url = user.Url.ToLower();
            }

            if (strUtil.IsUrlItem( user.Url ) == false) {
                result.Add( lang.get( "exUserUrlError" ) );
            }

            if (result.HasErrors) {
                return result;
            }


            if (isNameReserved( user.Name )) {
                result.Add( lang.get( "exNameFound" ) );
                return result;
            }

            if (isUrlReserved( user.Url )) {
                result.Add( lang.get( "exUrlFound" ) );
                return result;
            }

            if (IsExist( user.Name ) != null) {
                result.Add( lang.get( "exNameFound" ) );
                return result;
            }

            if (strUtil.HasText( user.Url ) && IsExistUrl( user.Url ) != null) {
                result.Add( lang.get( "exUrlFound" ) );
                return result;
            }

            return result;
        }

        public virtual Result RegisterNoPwd( User user ) {

            Result result = validateUser( user );
            if (result.HasErrors) return result;

            setProfileAndTemplate( user );
            user.RoleId = SiteRole.NormalMember.Id;
            result = db.insert( user );

            if (result.HasErrors) {
                db.delete( user.Profile );
                return result;
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

            result.Info = user;

            return result;
        }

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

        private void setProfileAndTemplate( User user ) {
            MemberProfile profile = new MemberProfile();
            db.insert( profile );
            user.ProfileId = profile.Id;
            user.TemplateId = config.Instance.Site.UserTemplateId;
            user.GroupId = 3;
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
            List<User> list = db.find<User>( "status>=0 order by Credit desc, Hits desc, Id desc" ).list( count );
            foreach (User user in list) {
                if (ids.Contains( user.Id )) continue;
                results.Add( user );
            }
            return results;
        }


        public virtual List<User> GetRanked( int count ) {
            return db.find<User>( "status>=0 order by Credit desc, Hits desc, Id desc" ).list( count );
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
            if (strUtil.IsNullOrEmpty( name )) return new List<User>();
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

            Boolean isFirst = (user.HasUploadPic() == false);

            user.Pic = newPic;
            db.update( user, "Pic" );

            if (isFirst) {
                addIncomeAndSendMsg( user );
            }

        }

        private void addIncomeAndSendMsg( User user ) {

            int actionId = UserAction.User_UpdateAvatar.Id;
            KeyIncomeRule rule = currencyService.GetKeyIncomeRulesByAction( actionId );

            int creditValue = rule.Income;
            String creditName = rule.CurrencyName;

            String msgTitle = "感谢您上传头像";
            userIncomeService.AddIncome( user, actionId, msgTitle );

            String msgBody = string.Format( "{0}：<br/>您好！<br/>感谢您上传头像，您因此获得{1}奖励，共{2}分。<br/>欢迎继续参与，谢谢。<br/>------------------------------------------<br/>这是系统自动发信，请勿回复。", user.Name, creditName, creditValue );


            MessageService msgService = new MessageService();

            msgService.SiteSend( msgTitle, msgBody, user );
        }

        //-------------------------------------------------------------

        public virtual User GetById( int id ) {
            return User.findById( id );
        }

        public virtual User GetByName( String name ) {
            if (strUtil.IsNullOrEmpty( name )) return null;
            return User.find( "Name=:name" ).set( "name", name ).first();
        }

        public virtual User GetByUrl( String friendUrl ) {
            if (strUtil.IsNullOrEmpty( friendUrl )) return null;
            return User.find( "Url=:furl" ).set( "furl", friendUrl ).first();
        }

        public virtual User GetByMail( String email ) {
            if (strUtil.IsNullOrEmpty( email )) return null;
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
            if (strUtil.IsNullOrEmpty( name )) return null;
            return User.find( "Name=:name" ).set( "name", name ).first();
        }

        public virtual User IsExistUrl( String url ) {
            if (strUtil.IsNullOrEmpty( url )) return null;
            return User.find( "Url=:url" ).set( "url", url ).first();
        }

        public virtual Boolean IsEmailExist( String email ) {
            if (strUtil.IsNullOrEmpty( email )) throw new ArgumentNullException( "email" );
            User user = User.find( "Email=:email and IsEmailConfirmed=" + EmailConfirm.Confirmed ).set( "email", email ).first();
            return user != null;
        }

        public virtual Boolean IsEmailExist( int userId, String email ) {
            if (strUtil.IsNullOrEmpty( email )) throw new ArgumentNullException( "email" );
            User user = User.find( "Email=:email and IsEmailConfirmed=" + EmailConfirm.Confirmed + " and Id<>" + userId ).set( "email", email ).first();
            return user != null;
        }

        //-------------------------------------------------------------


        private List<User> getNewListWithAvatar( int count ) {
            return User.find( "Pic is not null and Pic<>'" + SysPath.AvatarConstString + "'  order by Id desc" ).list( count );
        }


    }
}

