using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc.Interface;
using wojilu.Web.Context;
using wojilu.Members.Users.Interface;
using wojilu.Members.Users.Service;
using wojilu.Web.Mvc;
using wojilu.Members.Users.Domain;
using wojilu.Members.Interface;
using wojilu.Common.Menus.Interface;
using wojilu.Web.Mvc.Routes;
using wojilu.Common.AppBase;
using System.Web.Security;
using wojilu.Web.Url;
using System.Collections;
using wojilu.Common.Onlines;

namespace wojilu.Web.Controller.Common {

    public class ContextInit : IMvcFilter {

        public ILoginService loginService { get; set; }
        public IUserService userService { get; set; }

        public ContextInit() {
            loginService = new LoginService();
            userService = new UserService();
        }

        public void Process( wojilu.Web.Mvc.MvcEventPublisher publisher ) {

            publisher.Begin_InitContext += new EventHandler<wojilu.Web.Mvc.MvcEventArgs>( publisher_Begin_InitContext );
        }

        void publisher_Begin_InitContext( object sender, wojilu.Web.Mvc.MvcEventArgs e ) {

            MvcContext ctx = e.ctx;

            this.InitViewer( ctx );

            this.InitOwner( ctx );       // 初始化当前被访问对象(site或group或user)
            this.InitController( ctx );  // 初始化控制器

            OnlineManager.Refresh( ctx ); // 刷新当前在线用户
            this.InitApp( ctx );                 // 初始化当前app

            ctx.utils.skipCurrentProcessor( true );

        }



        //-------------------------------- viewer ----------------------------------

        public void InitViewer( MvcContext ctx ) {

            ctx.setCacheCondition( new CacheCondition() );


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

            ViewerContext context = new ViewerContext();
            context.Id = user.Id;
            context.obj = user;
            context.IsLogin = ctx.web.UserIsLogin;
            ctx.utils.setViewerContext( context );

            // 编辑器
            if (context.IsLogin) {
                Link lnk = new Link( ctx );
                ctx.SetItem( "editorUploadUrl", lnk.To( user, "Users/Admin/UserUpload", "UploadForm", -1, -1 ) );
                ctx.SetItem( "editorMyPicsUrl", lnk.To( user, "Users/Admin/UserUpload", "MyPics", -1, -1 ) );
            }
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

        //-------------------------------- owner ----------------------------------


        public void InitOwner( MvcContext ctx ) {


            if (strUtil.IsNullOrEmpty( ctx.route.ownerType )) return;

            IMember owner = getHelper( ctx ).getOwnerByUrl( ctx );

            if (owner.Status == MemberStatus.Deleted || owner.Status == MemberStatus.Approving) {
                throw ctx.ex( HttpStatus.NotFound_404, "owner not found" );
            }

            OwnerContext context = new OwnerContext();
            context.Id = owner.Id;
            context.obj = owner;

            ctx.utils.setOwnerContext( context );

            this.updateRoute_ByOwnerMenus( ctx, owner );
        }

        private void updateRoute_ByOwnerMenus( MvcContext ctx, IMember owner ) {


            List<IMenu> list = getHelper( ctx ).GetMenus( ctx );

            String cleanUrlWithoutOwner = ctx.route.getCleanUrlWithoutOwner( ctx );
            if (cleanUrlWithoutOwner == string.Empty || strUtil.EqualsIgnoreCase( cleanUrlWithoutOwner, "default" )) {

                //if ((cleanUrlWithoutOwner == string.Empty) || (string.Compare( cleanUrlWithoutOwner, "default", true ) == 0)) {
                updateRoute_Menu( ctx, list, "default" );
                ctx.utils.setIsHome( true );


            }
            else {
                updateRoute_Menu( ctx, list, cleanUrlWithoutOwner );
            }
        }

        private void updateRoute_Menu( MvcContext ctx, List<IMenu> list, String cleanUrlWithoutOwner ) {
            foreach (IMenu menu in list) {
                if (cleanUrlWithoutOwner.Equals( menu.Url )) { // 如果有好网址相同

                    // 获取实际的网址
                    String fullUrl = UrlConverter.getMenuFullPath( ctx, menu );
                    Route.setRoutePath( fullUrl );

                    Route newRoute = RouteTool.Recognize( fullUrl, ctx.web.PathApplication );
                    refreshRouteAndOwner( ctx, newRoute );

                    break;
                }
            }
        }

        private void refreshRouteAndOwner( MvcContext ctx, Route newRoute ) {


            if (newRoute.owner != ctx.route.owner || newRoute.ownerType != ctx.route.ownerType) {
                ctx.utils.setRoute( newRoute );
                InitOwner( ctx ); // 当前Owner已经变换，所以需要重新更新owner
            }
            else {
                ctx.utils.setRoute( newRoute );

            }
        }

        //-------------------------------- controller ----------------------------------

        public void InitController( MvcContext ctx ) {

            ControllerBase controller = ControllerFactory.InitController( ctx );

            if (controller == null) {
                String typeName = ctx.route.getControllerNameWithoutRootNamespace();
                String msg = lang.get( "exControllerNotExist" ) + ": " + typeName;

                throw ctx.ex( HttpStatus.NotFound_404, msg );
            }

            ctx.utils.setController( controller );
        }

        public void InitApp( wojilu.Web.Context.MvcContext ctx ) {
            AppInit.InitApp( ctx );
            if (ctx.app.obj != null) {
                getHelper( ctx ).IsAppRunning( ctx );
            }
        }

        //-------------------------------------------------------------------------------------------------------------

        private IInitHelper getHelper( MvcContext ctx ) {
            return InitHelper.GetHelper( ctx );
        }


    }
}
