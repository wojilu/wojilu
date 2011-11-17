/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;

using wojilu.Web.Mvc;
using wojilu.Web.Controller.Common;
using wojilu.Common.Pages.Service;
using wojilu.Common.Pages.Domain;
using wojilu.Common.AppBase;
using wojilu.Common.Pages.Interface;
using wojilu.Members.Users.Interface;
using wojilu.Members.Users.Service;
using wojilu.Web.Mvc.Attr;
using wojilu.Members.Users.Domain;
using wojilu.Common.Msg.Interface;
using wojilu.Common.Msg.Service;
using wojilu.Common.Msg.Enum;

namespace wojilu.Web.Controller.Common {

    public class PageController : ControllerBase {

        public IPageService pageService { get; set; }
        public IUserService userService { get; set; }
        public INotificationService nfService { get; set; }

        public PageController() {
            pageService = new PageService();
            userService = new UserService();
            nfService = new NotificationService();
        }

        public void VersionList( int id ) {

            Page data = pageService.GetPostById( id, ctx.owner.obj );
            if (data == null) {
                echo( lang( "exDataNotFound" ) );
                return;
            }

            bindSidebar( data );

            set( "pageTitle", data.Title );
            set( "editCount", data.EditCount );

            DataPage<PageHistory> list = pageService.GetHistoryPage( id, ctx.owner.obj, 20 );
            bindVersionList( list );

            set( "pageLink", to( Show, id ) );

        }

        private void bindVersionList( DataPage<PageHistory> list ) {
            IBlock block = getBlock( "list" );
            foreach (PageHistory p in list.Results) {
                block.Set( "p.EditorName", p.EditUser.Name );
                block.Set( "p.EditorUrl", Link.ToMember( p.EditUser ) );
                block.Set( "p.EditReason", p.EditReason );
                block.Set( "p.Updated", p.Updated );
                block.Set( "p.VersionUrl", to( VersionShow, p.Id ) );
                block.Next();
            }

            set( "page", list.PageBar );
        }

        public void VersionShow( int id ) {

            PageHistory ph = pageService.GetHistory( id );
            if (ph == null) {
                echo( lang( "exDataNotFound" ) );
                return;
            }

            Page page = pageService.GetPostById( ph.PageId, ctx.owner.obj );
            if (page == null) {
                echo( lang( "exDataNotFound" ) );
                return;
            }

            set( "postTitle", page.Title );
            set( "editorUrl", Link.ToMember( ph.EditUser ) );
            set( "pageLink", to( Show, page.Id ) );

            bind( "post", ph );
            bindSidebar( page );

        }

        public void Show( int id ) {

            Page data = pageService.GetPostById( id, ctx.owner.obj );
            if (data == null) {
                echo( lang( "exDataNotFound" ) );
                return;
            }

            WebUtils.pageTitle( this, data.Title );

            bindWikiStats( id, data );


            bind( "post", data );

            bindSidebar( data );

            pageService.AddHits( data );

            if (data.IsAllowReply == 1) {
                ctx.SetItem( "createAction", to( new PageCommentController().Create, id ) );
                ctx.SetItem( "commentTarget", data );
                load( "commentSection", new PageCommentController().ListAndForm );
            }
            else {
                set( "commentSection", "" );
            }

        }

        private void bindWikiStats( int id, Page data ) {
            IBlock wiki = getBlock( "stats" );
            if (data.Category.IsShowWiki == 1) {
                wiki.Bind( "post", data );
                wiki.Set( "versionLink", to( VersionList, id ) );
                wiki.Set( "creatorLink", Link.ToMember( data.Creator ) );
                String updated = data.Updated.Year <= 1 ? "" : cvt.ToDayString( data.Updated );
                wiki.Set( "postUpdated", updated );

                String cmd = hasPermission( data.Category ) ? string.Format( "<img src=\"{1}\" /> <a href=\"{0}\">编辑</a>", to( Edit, data.Id ), strUtil.Join( sys.Path.Img, "edit.gif" ) ) : "";
                wiki.Set( "editCmd", cmd );

                wiki.Next();
            }
        }


        private Boolean hasPermission( PageCategory category ) {

            if (ctx.viewer.IsLogin == false) return false;
            if (ctx.viewer.IsAdministrator()) return true;
            if (category.OpenStatus == OpenStatus.Close) return false;
            if (category.OpenStatus == OpenStatus.Open) return true;
            if (userIsEditor( ctx.viewer.Id, category.EditorIds )) return true;
            return false;
        }

        private bool userIsEditor( int userId, string editorIds ) {
            int[] arrIds = cvt.ToIntArray( editorIds );
            foreach (int id in arrIds) {
                if (id == userId) return true;
            }
            return false;
        }

        public void Add( int id ) {
            PageCategory category = pageService.GetCategoryById( id, ctx.owner.obj );
            if (category == null) {
                echo( lang( "exDataNotFound" ) );
                return;
            }

            if (hasPermission( category ) == false) {
                echo( lang( "exNoPermission" ) );
                return;
            }

            target( Create, id );
            editorFull( "content", "", "500px" );
        }

        [HttpPost, DbTransaction]
        public void Create( int id ) {

            PageCategory category = pageService.GetCategoryById( id, ctx.owner.obj );
            if (category == null) {
                echo( lang( "exDataNotFound" ) );
                return;
            }

            if (hasPermission( category ) == false) {
                echo( lang( "exNoPermission" ) );
                return;
            }

            Page data = validate( new Page() );
            if (ctx.HasErrors) { run( Add, id ); return; }

            data.Category = category;
            data.IsAllowReply = 1;

            populateOwner( data );

            pageService.Insert( data );

            // 发通知
            String msg = data.Creator.Name + " 创建了页面 <a href=\"" + to( Show, data.Id ) + "\">" + data.Title + "</a>";
            nfService.send( data.OwnerId, data.OwnerType, msg, NotificationType.Normal );


            if (ctx.HasErrors) { run( Add, id ); return; }

            echoRedirect( lang( "opok" ), to( Show, data.Id ) );

        }

        private void populateOwner( Page data ) {
            data.OwnerId = ctx.owner.Id;
            data.OwnerType = ctx.owner.obj.GetType().FullName;
            data.OwnerUrl = ctx.owner.obj.Url;
            data.Creator = (User)ctx.viewer.obj;
        }

        public void Edit( int id ) {

            Page data = pageService.GetPostById( id, ctx.owner.obj );
            if (data == null) {
                echo( lang( "exDataNotFound" ) );
                return;
            }

            if (hasPermission( data.Category ) == false) {
                echo( lang( "exNoPermission" ) );
                return;
            }

            if( isInEditing( data) ) {

                User user = userService.GetById( data.UpdatingId );
                if (user != null) {
                    view( "EditWarning" );
                    set( "UpdatingTime", data.UpdatingTime );
                    set( "UpdatingUserName", user.Name );
                    set( "UpdatingUserLink", Link.ToMember( user ) );
                    return;
                }
            }

            set( "title", data.Title );
            editorFull( "content", data.Content, "500px" );
            target( Update, id );

            set( "cancelUrl", to( Cancel, id ) );
            set( "showUrl", to( Show, id ) );

            set( "pingUrl", to( Ping, id ) );
        }

        private bool isInEditing( Page data ) {
            if (data.UpdatingTime.Year <= 1) return false;
            if (data.UpdatingId <= 0) return false;
            if (data.UpdatingId == ctx.viewer.Id) return false; // 自己刚刚编辑过
            if (DateTime.Now.Subtract( data.UpdatingTime ).TotalMinutes >= 1) return false; // 超过1分钟表示无人更新
            return true;
        }

        [HttpPost, DbTransaction]
        public void Cancel( int id ) {
            Page data = pageService.GetPostById( id, ctx.owner.obj );
            if (data == null) {
                echo( lang( "exDataNotFound" ) );
                return;
            }

            data.UpdatingId = 0;
            data.update();

        }

        [HttpPost, DbTransaction]
        public void Ping( int id ) {
            Page data = pageService.GetPostById( id, ctx.owner.obj );
            if (data == null) {
                echo( lang( "exDataNotFound" ) );
                return;
            }

            data.UpdatingTime = DateTime.Now;
            data.UpdatingId = ctx.viewer.Id;
            data.update();
        }

        [HttpPost, DbTransaction]
        public void Update( int id ) {

            Page data = pageService.GetPostById( id, ctx.owner.obj );
            if (data == null) {
                echo( lang( "exDataNotFound" ) );
                return;
            }

            if (hasPermission( data.Category ) == false) {
                echo( lang( "exNoPermission" ) );
                return;
            }

            data = validate( data );
            if (ctx.HasErrors) { run( Edit, id ); return; }

            data.EditUser = ctx.viewer.obj as User;
            data.UpdatingId = 0;

            pageService.Update( data );

            String pageLink = to( Show, data.Id );
            sendNotification( data, pageLink );
            echoRedirect( lang( "opok" ), pageLink );
        }

        // 发通知
        private void sendNotification( Page data, String pageLink ) {

            List<int> editorIds = pageService.GetEditorIds( data.Id );

            foreach (int receiverId in editorIds) {

                if (ctx.viewer.Id == receiverId) continue;

                String msg = ctx.viewer.obj.Name + " 修改了您参与过的页面 <a href=\"" + pageLink + "\">" + data.Title + "</a>";
                nfService.send( receiverId, msg );
            }

        }

        private Page validate( Page data ) {

            data.Title = ctx.Post( "title" );
            data.Content = ctx.PostHtml( "content" );
            data.EditReason = ctx.Post( "editReason" );

            if (strUtil.IsNullOrEmpty( data.Title )) errors.Add( lang( "exTitle" ) );
            if (strUtil.IsNullOrEmpty( data.Content )) errors.Add( lang( "exContent" ) );
            if (strUtil.IsNullOrEmpty( data.EditReason )) errors.Add( "请填写编辑原因" );

            return data;
        }

        private void bindSidebar( Page data ) {

            List<Page> relativeList = pageService.GetPosts( ctx.owner.obj, data.Category.Id );

            IBlock sidebar = getBlock( "sidebar" );
            if (relativeList.Count <= 1) return;

            sidebar.Set( "category.Name", data.Category.Name );

            Tree<Page> tree = new Tree<Page>( relativeList );

            treeBinder binder = new treeBinder();
            binder.link = this.Link;

            sidebar.Set( "tree", tree.RenderList( "mytree", true, binder, data.Id ) );

            String cmd = hasPermission( data.Category ) ? string.Format( "<img src=\"{1}\" /> <a href=\"{0}\">添加页面</a>", to( Add, data.Category.Id ), strUtil.Join( sys.Path.Img, "add.gif" ) ) : "";

            sidebar.Set( "addCmd", cmd );

            sidebar.Next();

        }



        class treeBinder : INodeBinder {

            public Link link { get; set; }

            public String Bind( INode node ) {
                String lnk = link.T2( new PageController().Show, node.Id );
                return "<a href=\"" + lnk + "\">" + node.Name + "</a>";
            }

        }

    }

}
