/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Common.Microblogs.Service;
using wojilu.Common.Microblogs.Domain;
using wojilu.Common.Microblogs.Interface;
using wojilu.Common.Feeds.Interface;
using wojilu.Common.Feeds.Domain;
using wojilu.Common.Feeds.Service;
using wojilu.Members.Users.Interface;
using wojilu.Members.Users.Service;
using wojilu.Members.Users.Domain;
using wojilu.Web.Controller.Users.Admin;

namespace wojilu.Web.Controller.Users.Admin {

    public partial class ShareCommentsController : ControllerBase {

        public IShareService shareService { get; set; }
        public IFollowerService followService { get; set; }

        public ShareCommentsController() {
            shareService = new ShareService();
            followService = new FollowerService();
        }

        public void Show( int id ) {
            set( "commentList", loadHtml( commentList, id ) );
            set( "commentForm", loadHtml( commentForm, id ) );
            set( "share.Id", id );
        }

        [NonVisit]
        public void commentList( int id ) {
            Share share = shareService.GetByIdWithComments( id );

            if (share == null) {
                echoText( lang( "error" ) );
                return;
            }

            bindOne( this.utils.getCurrentView(), share );
        }

        [NonVisit]
        public void commentForm( int id ) {
            target( SaveComment );
            set( "c.RootId", id );
            set( "viewer.PicSmall", ctx.viewer.obj.PicSmall );
        }

        private void bindOne( IBlock block, Share share ) {

            block.Set( "share.Id", share.Id );
            //block.Set( "share.Content", share.Content );
            block.Set( "share.Created", share.Created );

            IBlock cblock = block.GetBlock( "comments" );
            bindComments( cblock, share.GetComments() );
        }

        private void bindComments( IBlock cblock, List<ShareComment> clist ) {

            Tree<ShareComment> tree = new Tree<ShareComment>( clist );

            List<Node<ShareComment>> list = tree.FindAllOrdered();

            foreach (Node<ShareComment> nc in list) {

                ShareComment c = nc.getNode();


                cblock.Set( "c.UserPic", c.User.PicSmall );
                cblock.Set( "c.UserLink", toUser( c.User ) );
                cblock.Set( "c.UserName", c.User.Name );

                cblock.Set( "c.Id", c.Id );
                cblock.Set( "c.RootId", c.Root.Id );
                cblock.Set( "c.Content", c.Content );
                cblock.Set( "c.Created", c.Created );

                cblock.Set( "c.Indent", nc.getDepth() * 45 );

                cblock.Next();
            }

        }

        [HttpPost, DbTransaction]
        public void SaveComment() {

            if (ctx.viewer.IsLogin == false) {
                echoRedirect( lang( "exPlsLogin" ) );
                return;
            }

            String content = ctx.Post( "content" );
            if (strUtil.IsNullOrEmpty( content )) {
                echoError( lang( "exContent" ) );
                return;
            }

            int rootId = ctx.PostInt( "rootId" );
            int parentId = ctx.PostInt( "parentId" );

            //content = strUtil.CutString( content, Microblog.ContentLength );

            Share share = shareService.GetById( rootId );

            ShareComment c = new ShareComment();
            c.Root = share;
            c.ParentId = parentId;
            c.User = (User)ctx.viewer.obj;
            c.Ip = ctx.Ip;
            c.Content = content;

            //String shareLink = Link.To( share.Creator, Show, share.Id );
            // 应该是接收者可以查看的，所以网址在接收者后台中

            String rootShareLink = Link.To( share.Creator, new ShareController().Show, share.Id );
            String parentShareLink = null;
            if (parentId > 0) {
                ShareComment pshare = shareService.GetCommentById( parentId );
                parentShareLink = Link.To( pshare.User, new ShareController().Show, share.Id );
            }


            shareService.InsertComment( c, rootShareLink, parentShareLink );

            String str = @"
<table style=""width: 95%; margin:5px 0px 5px 0px;background:#ebf3f7;""> 
    <tr> 
        <td style=""width:38px;""><a href=""{0}""><img src=""{1}"" style=""width:32px;""/></a></td> 
        <td style=""vertical-align:top;""> 
        <div><a href=""{0}"">{2}</a> <span class=""note"">" + lang( "postedAt" )

                                                           + @" {3}</span></div> 
        <div style=""margin-top:5px;"">{4}</div> </td> 
    </tr> 
</table>
";
            String msg = string.Format( str,
                toUser( ctx.viewer.obj ),
                ctx.viewer.obj.PicSmall,
                ctx.viewer.obj.Name,
                cvt.ToTimeString( DateTime.Now ),
                content
                );

            if (parentId == 0)
                echoHtmlTo( "shareComments" + rootId, msg );
            else
                echoHtmlTo( "commentContent" + parentId, msg );

        }


    }
}
