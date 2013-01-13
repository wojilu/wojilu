/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Text;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Web.Mvc.Utils;
using wojilu.Web.Mvc.Routes;
using wojilu.Common.AppBase.Interface;
using wojilu.Common.Comments;
using wojilu.Members.Users.Domain;
using wojilu.Members.Users.Interface;
using wojilu.Members.Users.Service;
using wojilu.Members.Sites.Domain;
using wojilu.Web.Controller.Security;

namespace wojilu.Web.Controller.Common {

    public class CommentController<T> : ControllerBase where T : ObjectBase<T>, IComment {

        public ICommentService<T> commentService { get; set; }
        public IBlacklistService blacklistService { get; set; }

        protected virtual String getTargetLink( T post ) {
            IAppData root = (IAppData)ndb.findById( post.GetTargetType(), post.RootId );
            return alink.ToAppData( root );
        }


        public CommentController() {
            commentService = new CommentService<T>();
            blacklistService = new BlacklistService();
        }


        public void ListAndForm() {

            if (config.Instance.Site.CloseComment) {
                set( "commentList", "" );
                return;
            }

            IAppData post = ctx.GetItem( "commentTarget" ) as IAppData;

            DataPage<T> list = commentService.GetPageByTarget( post.Id, 50 );

            ctx.SetItem( "commentList", list );

            String contentLength = string.Format( lang( "contentLength" ), config.Instance.Site.CommentLength );
            set( "contentLength", contentLength );

            load( "commentList", commentList );
            IBlock pageblock = getBlock( "page" );
            if (list.PageCount > 1) {
                pageblock.Set( "page", list.PageBar );
                pageblock.Next();
            }

            set( "CommentActionUrl", ctx.GetItem( "createAction" ) );
            int parentId = getParentId( list, post.Id );
            set( "parentId", parentId );

            IBlock loginForm = getBlock( "loginForm" );
            IBlock guestForm = getBlock( "guestForm" );

            if (ctx.viewer.IsLogin) {

                loginForm.Set( "user.Name", ctx.viewer.obj.Name );
                loginForm.Set( "user.ThumbFace", ctx.viewer.obj.PicSmall );
                loginForm.Next();
            }
            else {
                guestForm.Set( "Captcha", Html.Captcha );
                guestForm.Next();
            }

        }

        private int getParentId( DataPage<T> list, int rootId ) {

            List<T> comments = list.Results;

            if (comments.Count == 0) return 0;

            T lastComment = comments[comments.Count - 1];

            return lastComment.Id;
        }

        public void commentList() {

            String controllerString = getControllerString();

            Boolean canAdmin = hasAdminPermission();
            IBlock block = getBlock( "comments" );
            DataPage<T> list = ctx.GetItem( "commentList" ) as DataPage<T>;
            foreach (IComment comment in list.Results) {
                bindCommentOne( block, comment, controllerString, canAdmin );
                block.Next();
            }
        }

        private static readonly ILog logger = LogManager.GetLogger( "CommentController" );

        private void bindCommentOne( IBlock block, IComment c, String controllerString, Boolean canAdmin ) {
            String userFace = "<img src='" + sys.Path.AvatarGuest + "' style='width:48px;'/></a>";
            String userName = c.Author;
            if (c.Member != null && c.Member.Id > 0) {
                userFace = string.Format( "<a href='{0}'><img src='{1}' style='width:48px;'/></a>", toUser( c.Member ), c.Member.PicSmall );
                userName = string.Format( "<a href='{0}'>{1}</a>", toUser( c.Member ), c.Member.Name );
            }
            block.Set( "c.UserName", userName );
            block.Set( "c.UserFace", userFace );
            block.Set( "c.Created", c.Created );
            block.Set( "c.Content", getContent( c ) );
            block.Set( "c.Id", c.Id );

            logger.Info( "controllerString=" + controllerString );

            //String lnk = Link.To( ctx.owner.obj, controllerString, "SaveReply", c.Id );
            String lnk = getActionUrl( "SaveReply", c.Id );
            logger.Info( "lnk=" + lnk );

            block.Set( "c.ReplyLink", lnk );

            if (canAdmin) {
                IBlock adminBlock = block.GetBlock( "admin" );
                //String deleteLink = Link.To( ctx.owner.obj, controllerString, "Delete", c.Id );
                String deleteLink = getActionUrl( "Delete", c.Id );
                adminBlock.Set( "c.DeleteLink", deleteLink );
                adminBlock.Next();
            }

        }

        // http://zhangsan.mytest.com/Blog1/BlogComment/64/Create.aspx
        private string getActionUrl( string actionName, int parentId ) {

            if (ctx.GetItem( "createAction" ) == null) return "#";

            String lnkCreate = ctx.GetItem( "createAction" ).ToString();

            String[] arrItem = lnkCreate.Split( '/' );
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < arrItem.Length - 2;i++ ) {
                sb.Append( arrItem[i] );
                sb.Append( "/" );
            }

            sb.Append( parentId.ToString() );
            sb.Append( "/" );

            sb.Append( actionName );

            sb.Append( MvcConfig.Instance.UrlExt );

            return sb.ToString();
        }

        private String getContent( IComment c ) {
            if (c.ParentId == 0) return c.Content;
            IComment parent = commentService.GetById( c.ParentId, ctx.app.Id );
            if (parent == null) return c.Content;
            String quote = "<div class='quote'><span class='qSpan'>{0} : {1}</span></div>";
            return string.Format( quote, parent.Author, strUtil.CutString( parent.Content, 50 ) ) + "<div>" + c.Content + "</div>";
        }

        private String getControllerString() {

            if (ctx.GetItem( "createAction" ) == null) return "";

            String lnkCreate = ctx.GetItem( "createAction" ).ToString();
            lnkCreate = strUtil.TrimEnd( lnkCreate, MvcConfig.Instance.UrlExt );

            String controllerString = wojilu.Web.Mvc.Routes.RouteTool.RecognizePath( lnkCreate ).getControllerNameWithoutRootNamespace();
            controllerString = controllerString.Replace( ".", "/" );

            return controllerString;
        }

        //-----------------------------------------------------------------------------------------------------------

        public virtual void Reply( int id ) {
            target( SaveReply, id );
            set( "parentId", id );
        }

        public virtual void SaveReply( int id ) {

            if (config.Instance.Site.CloseComment) return;


            if (blacklistService.IsBlack( ctx.owner.Id, ctx.viewer.Id )) {
                echoError( lang( "backComment" ) );
                return;
            }

            IComment parent = commentService.GetById( id, ctx.app.Id );

            IComment comment = Validate( parent.RootId );
            if (ctx.HasErrors) {
                echoError();
                return;
            }

            comment.ParentId = parent.Id; // 重新设置parentId

            String lnkTarget = getTargetLink( (T)comment );
            commentService.Reply( parent, comment, lnkTarget );

            String url = ctx.web.PathReferrer.ToString();

            echoRedirect( lang( "opok" ), addRefreshInfo( url ) + "#commentFormStart" );
        }

        private static String addRefreshInfo( String rUrl ) {

            String result;

            if (rUrl.IndexOf( "?" ) > 0) {
                if (rUrl.IndexOf( "refresh=true" ) < 0) {
                    result = rUrl + "&refresh=true";
                }
                else {
                    result = rUrl.Replace( "?refresh=true", "" );
                    result = result.Replace( "refresh=true", "" );
                }
            }
            else {
                result = rUrl + "?refresh=true";
            }
            return result;
        }


        [HttpPost, DbTransaction]
        public virtual void Create( int postId ) {

            if (config.Instance.Site.CloseComment) return;


            if (blacklistService.IsBlack( ctx.owner.Id, ctx.viewer.Id )) {
                echoError( lang( "backComment" ) );
                return;
            }

            IComment comment = Validate( postId );

            if (ctx.HasErrors) {
                echoError();
                return;
            }

            String lnkTarget = getTargetLink( (T)comment );

            Result result = commentService.Insert( comment, lnkTarget );
            if (result.HasErrors) {
                echoError( result );
                return;
            }

            ctx.SetItem( "comment", comment );

            String htmlValue = loadHtml( returnHtml );
            echoJsonMsg( htmlValue, true, "formResult" );
        }

        public void returnHtml() {
            IComment c = ctx.GetItem( "comment" ) as IComment;
            IBlock block = getBlock( "comments" );

            String controllerString = getControllerString();

            bindCommentOne( block, c, controllerString, hasAdminPermission() );
            block.Next();
        }

        public IComment Validate( int postId ) {

            IComment comment = Entity.New( typeof( T ).FullName ) as IComment;

            String userName;
            if (ctx.viewer.IsLogin) {
                userName = ctx.viewer.obj.Name;
            }
            else {
                userName = ctx.Post( "UserName" );
                if (strUtil.IsNullOrEmpty( userName )) errors.Add( lang( "exRequireAuthor" ) );
                if (strUtil.HasText( userName ) && userName.Length < 2) errors.Add( lang( "exAuthorShort" ) );
            }

            String content = ctx.Post( "Content" );
            if (strUtil.IsNullOrEmpty( content )) errors.Add( lang( "exRequireContent" ) );
            if (strUtil.HasText( content ) && content.Length < 3) errors.Add( lang( "exContentShort" ) );

            if (ctx.viewer.IsLogin == false) Html.Captcha.CheckError( ctx );


            if (errors.HasErrors) {
                return null;
            }

            comment.RootId = postId;
            comment.ParentId = ctx.PostInt( "ParentId" );
            comment.AppId = ctx.app.Id;
            comment.Author = userName;
            comment.Content = content;
            comment.Ip = ctx.Ip;
            comment.Created = DateTime.Now;

            if (ctx.viewer.IsLogin) {
                comment.Member = (User)ctx.viewer.obj;
            }
            return comment;
        }

        //-----------------------------------------------------------------------------------------------------------

        public void AdminList() {

            if (!checkPermission()) return;

            bindList();
        }

        public void List() {
            bindList();

            ctx.Page.Title = "评论列表";
        }

        private void bindList() {

            int appId = ctx.app.Id;

            DataPage<T> page = commentService.GetPage( appId );
            IBlock block = getBlock( "comment" );
            foreach (IComment c in page.Results) {

                if (c.Member == null) {
                    block.Set( "c.Author", c.Author );
                }
                else {
                    String loginUser = string.Format( "<a href='{0}'>{1}</a>", toUser( c.Member ), c.Author );
                    block.Set( "c.Author", (c.Member.Id > 0) ? loginUser : c.Author );
                }
                block.Set( "c.Content", strUtil.ParseHtml( c.Content, 30 ) );
                block.Set( "c.Url", getPostLink( c ) + "#comments" );
                block.Set( "c.Created", c.Created );
                block.Set( "c.DeleteUrl", to( Delete, c.Id ) );
                block.Set( "c.ReplyUrl", getPostLink( c ) + "#reply" );
                block.Next();
            }
            set( "page", page.PageBar );
        }

        private String getPostLink( IComment comment ) {

            IAppData data = Entity.New( comment.GetTargetType().FullName ) as IAppData;
            data.Id = comment.RootId;
            data.OwnerType = ctx.owner.obj.GetType().FullName;
            data.OwnerUrl = ctx.owner.obj.Url;
            data.AppId = comment.AppId;

            return alink.ToAppData( data );
        }

        [HttpDelete, DbTransaction]
        public void Delete( int commentId ) {

            if (!checkPermission()) return;

            T comment = commentService.GetById( commentId, ctx.app.Id );
            if (comment == null) {
                echoRedirect( lang( "exDataNotFound" ) );
                return;
            }

            commentService.Delete( comment );

            //redirect();
            echoAjaxOk();
        }

        private Boolean checkPermission() {
            if (!hasAdminPermission()) {
                echoRedirect( lang( "exNoPermission" ) );
                return false;
            }
            return true;
        }

        private Boolean hasAdminPermission() {

            if (!ctx.viewer.IsLogin) return false;

            if (ctx.viewer.IsAdministrator()) return true;

            if (ctx.owner.obj.GetType() == typeof( User )) return ctx.owner.Id == ctx.viewer.Id;

            if (ctx.owner.obj.GetType() == typeof( Site ) && ctx.app != null && ctx.app.obj != null) {

                return AppAdminRole.CanAppAdmin( ctx.viewer.obj, ctx.app.obj.GetType(), ctx.app.Id );

            }

            return ctx.viewer.IsOwnerAdministrator( ctx.owner.obj );
        }

    }

}
