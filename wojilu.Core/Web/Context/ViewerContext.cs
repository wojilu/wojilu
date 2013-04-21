/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;

using wojilu.Members.Users.Domain;
using wojilu.Members.Users.Service;
using wojilu.Members.Sites.Domain;
using wojilu.Common.Msg.Service;
using wojilu.Members.Interface;
using wojilu.DI;
using wojilu.ORM;
using wojilu.Common.Security;

namespace wojilu.Web.Context {

    public class ViewerContext : IViewerContext {

        private FriendService friendService { get; set; }
        private FollowerService followService { get; set; }
        private MessageService msgService { get; set; }

        public ViewerContext( MvcContext ctx ) {
            friendService = new FriendService();
            msgService = new MessageService();
            followService = new FollowerService();
            this.ctx = ctx;
        }

        public int Id { get; set; }
        public Boolean IsLogin { get; set; }
        public IList Menus { get; set; }
        public IUser obj { get; set; }

        private MvcContext ctx { get; set; }

        public Result AddFriend( int ownerId, String msg ) {
            return friendService.AddFriend( this.obj.Id, ownerId, msg, ctx.Ip );
        }

        public Boolean IsFriend( int ownerId ) {
            return friendService.IsFriend( this.obj.Id, ownerId );
        }

        public Boolean IsFollowing( int ownerId ) {
            return followService.IsFollowing( this.obj.Id, ownerId );
        }

        public Result SendMsg( String ownerName, String title, String body ) {
            return msgService.SendMsg( (User)this.obj, ownerName, title, body );
        }

        public Boolean IsAdministrator() {
            return SiteRole.IsAdministrator( this.Role );
        }

        public Boolean IsOwnerAdministrator( IMember owner ) {

            IRole role = owner.GetUserRole( (IMember)this.obj );
            if (role == null) return false;

            if (owner is User) {
                return owner.Id == this.Id;
            }
            else {

                IRole adminRole = owner.GetAdminRole();
                if (adminRole == null) return false;
                return role.Id == adminRole.Id;

            }
        }

        private SiteRole _role;

        public SiteRole Role {
            get {
                if (_role == null) {
                    _role = SiteRole.GetById( this.Id, this.user.RoleId );
                }
                return _role;
            }
        }

        private User user {
            get { return this.obj as User; }
        }

        /// <summary>
        /// 检索隐私配置，当前 viewer 对 owner 的某个item是否具有访问权限
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public Boolean HasPrivacyPermission( IMember owner, String item ) {

            // 自己总可以访问
            if (this.Id == owner.Id) return true;

            User target = owner as User;
            if (target == null) return true;

            int val = UserSecurity.GetPrivacy( target, item );

            if (val == UserPrivacy.EveryOne) return true;
            if (val == UserPrivacy.Self) return (this.Id == owner.Id);
            if (val == UserPrivacy.Friend) { return IsFriend( owner.Id ); }
            if (val == UserPrivacy.LoginUser) { return IsLogin; }

            return true;

        }

        //----------------------------------------------------------------------------

        private SiteRank _siteRank;

        public SiteRank Rank {
            get {
                if (_siteRank == null) {
                    _siteRank = SiteRank.GetById( this.user.RankId );
                }
                return _siteRank;
            }
        }

    }
}

