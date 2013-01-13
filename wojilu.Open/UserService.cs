using System;
using System.Data;
using System.Configuration;
using System.Web;
using wojilu.Members.Users.Domain;
using wojilu.Members.Sites.Domain;
using wojilu.Common.Msg.Service;
using wojilu.Common.Msg.Interface;
using wojilu.Common.Money.Service;
using wojilu.Common.Money.Interface;

namespace wojilu.Open {

    public interface ISiteConfig {

        Boolean IsEnableUserSpace { get; set; }
        int AdministratorId { get; set; }
        int NormalRoleId { get; set; }

        String[] ReservedKey { get; set; }
        String[] ReservedUserName { get; set; }
        String[] ReservedUserUrl { get; set; }

        String MsgTitle { get; set; }
        String MsgBody { get; set; }
        Boolean IsSendMsg { get; set; }

        int UserSkinId { get; set; }
        Boolean Md5Is16 { get; set; }


    }

    public class WojiluSiteConfig : ISiteConfig {
        #region ISiteConfig 成员

        public bool IsEnableUserSpace { get; set; }
        public int AdministratorId { get; set; }
        public int NormalRoleId { get; set; }

        public string[] ReservedKey { get; set; }
        public string[] ReservedUserName { get; set; }
        public string[] ReservedUserUrl { get; set; }

        public string MsgTitle { get; set; }
        public string MsgBody { get; set; }
        public bool IsSendMsg { get; set; }

        public int UserSkinId { get; set; }
        public bool Md5Is16 { get; set; }

        #endregion


    }

    public class UserService {


        public UserService() {
            hashTool = new HashTool();
            userIncomeService = new UserIncomeService();
        }

        public virtual IHashTool hashTool { get; set; }
        public virtual IUserIncomeService userIncomeService { get; set; }

        public virtual User Register( User user, Result errors, ISiteConfig sconfig ) {

            if (isNameReserved( user.Name, sconfig )) {
                errors.Add( lang.get( "exNameFound" ) );
                return null;
            }

            if (isUrlReserved( user.Url, sconfig )) {
                errors.Add( lang.get( "exUrlFound" ) );
                return null;
            }


            if (validateUnique( user, errors ) == false) return null;

            setProfileAndTemplateAndHashPasswork( user, sconfig );
            user.RoleId = sconfig.NormalRoleId;
            Result result = db.insert( user );
            if (result.HasErrors) {
                db.delete( user.Profile );
                errors.Join( result );
                return null;
            }

            if (strUtil.IsNullOrEmpty( user.Url )) {
                user.Url = "user" + user.Id;
                db.update( user, "Url" );
            }

            sendMsg( user, sconfig );

            userIncomeService.InitUserIncome( user );

            if (isFirstUser()) {
                user.RoleId = sconfig.AdministratorId;
                db.update( user, "RoleId" );
            }

            return user;
        }

        private Boolean isFirstUser() {
            return db.count<User>() == 1;
        }


        private Boolean isNameReserved( String inputName, ISiteConfig sconfig ) {

            if (IsReservedKeyContains( inputName, sconfig )) return true;

            foreach (String name in sconfig.ReservedUserName) {
                if (strUtil.EqualsIgnoreCase( inputName, name )) {
                    return true;
                }
            }

            return false;
        }

        private Boolean isUrlReserved( String inputUrl, ISiteConfig sconfig ) {

            if (IsReservedKeyContains( inputUrl, sconfig )) return true;

            foreach (String name in sconfig.ReservedUserUrl) {
                if (strUtil.EqualsIgnoreCase( inputUrl, name )) {
                    return true;
                }
            }

            return false;
        }

        public Boolean IsReservedKeyContains( String inputName, ISiteConfig sconfig ) {

            foreach (String key in sconfig.ReservedKey) {
                if (strUtil.EqualsIgnoreCase( inputName, key ) || strUtil.EqualsIgnoreCase( inputName, key + "s" )) {
                    return true;
                }
            }

            return false;
        }

        private Boolean validateUnique( User user, Result errors ) {
            if (IsExist( user.Name ) != null) {
                errors.Add( lang.get( "exNameFound" ) );
                return false;
            }
            if (strUtil.HasText( user.Url ) && IsExistUrl( user.Url ) != null) {
                errors.Add( lang.get( "exUrlFound" ) );
                return false;
            }
            return true;
        }

        public virtual User IsExist( String name ) {
            return User.find( "Name=:name" ).set( "name", name ).first();
        }

        public virtual User IsExistUrl( String url ) {
            return User.find( "Url=:url" ).set( "url", url ).first();
        }

        protected virtual void sendMsg( User member, ISiteConfig sconfig ) {

            if (sconfig.IsSendMsg == false) return;

            String title = sconfig.MsgTitle;
            String body = sconfig.MsgBody;

            IMessageService service = new MessageService();
            service.SiteSend( title, body, member );
        }

        private void setProfileAndTemplateAndHashPasswork( User user, ISiteConfig sconfig ) {
            MemberProfile profile = new MemberProfile();
            db.insert( profile );
            user.ProfileId = profile.Id;
            user.TemplateId = sconfig.UserSkinId;
            user.GroupId = 3;

            user.PwdSalt = hashTool.GetSalt( 4 );
            user.Pwd = HashPwd( user.Pwd, user.PwdSalt, sconfig );

        }

        public virtual String HashPwd( String pwd, String salt, ISiteConfig sconfig ) {

            if (sconfig.Md5Is16) {
                return hashTool.Get( pwd.Trim(), HashType.MD5_16 );
            }

            return hashTool.GetBySalt( pwd.Trim(), salt, HashType.SHA384 );
        }


    }
}
