/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Security;

using wojilu.Common.AppBase;
using wojilu.Members.Users.Interface;
using wojilu.Members.Users.Domain;
using wojilu.Members.Users.Service;
using wojilu.Web.Mvc;

namespace wojilu.Web.Context.Initor {


    public class ViewerInit : IContextInit {

        public ILoginService loginService { get; set; }
        public IUserService userService { get; set; }

        public ViewerInit() {
            loginService = new LoginService();
            userService = new UserService();
        }

        public virtual void Init( MvcContext ctx ) {

            if (ctx.utils.isEnd()) return;

            CurrentRequest.setItem( "_user_factory", new UserFactory() );

            User user = this.getViewer( ctx );

            if (user.Id == UserFactory.Guest.Id && ctx.web.UserIsLogin) {
                signOut( ctx );
                return;
            }
            else if (user.Status == MemberStatus.Deleted || user.Status == MemberStatus.Approving) {
                signOut( ctx );
                return;
            }

            if (ctx.web.UserIsLogin) loginService.UpdateLastLogin( user, ctx.Ip );

            ViewerContext context = new ViewerContext( ctx );
            context.Id = user.Id;
            context.obj = user;
            context.IsLogin = ctx.web.UserIsLogin;
            ctx.utils.setViewerContext( context );


        }



        private void signOut( MvcContext ctx ) {

            // 注销并跳转到首页
            FormsAuthentication.SignOut();
            ctx.utils.clearResource();
            ctx.web.Redirect( ctx.url.SiteAndAppPath );
        }

        private User getViewer( MvcContext ctx ) {

            if (!ctx.web.UserIsLogin) {
                return UserFactory.Guest;
            }
            else {
                return this.getViewerById( ctx.web.UserId() );
            }
        }

        private User getViewerById( int id ) {

            if (id <= 0) return UserFactory.Guest;

            User member = userService.GetById( id );
            if (member == null) return UserFactory.Guest;

            return member;
        }


    }

}
