/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;

using wojilu.Members.Users.Service;
using wojilu.Members.Users.Domain;
using wojilu.Members.Users.Interface;

using wojilu.Common.Feeds.Service;
using wojilu.Common.Feeds.Interface;
using wojilu.Common.AppBase;
using wojilu.Common.Tags;

using wojilu.Apps.Blog.Domain;
using wojilu.Apps.Blog.Service;
using wojilu.Apps.Blog.Interface;

namespace wojilu.Web.Controller.Blog.Admin {

    [App( typeof( BlogApp ) )]
    public class MyListController : ControllerBase {

        public IBlogService blogService { get; set; }
        public IBlogCategoryService categoryService { get; set; }
        public IBlogPostService postService { get; set; }

        public IFeedService feedService { get; set; }
        public IFriendService friendService { get; set; }

        public MyListController() {
            blogService = new BlogService();
            postService = new BlogPostService();
            categoryService = new BlogCategoryService();
            feedService = new FeedService();
            friendService = new FriendService();
        }

        public override void Layout() {
            List<BlogCategory> categories = categoryService.GetByApp( ctx.app.Id );
            bindList( "categories", "c", categories, bindLink );
        }

        public void Index() {
            set( "myLinkUrl", to( My ) );
        }

        public void My() {
            getListString( -1 );
        }

        public void ListByCategory( int id ) {
            getListString( id );
        }

        private void getListString( int categoryId ) {
            view( "List" );
            target( Admin );

            set( "addLink", to( new PostController().Add ) );
            set( "appType", typeof( BlogApp ).FullName );

            String lnkBook = Link.To( ctx.owner.obj, "Blog/Admin/Chm", "Select", -1 );
            set( "makeBookLink", lnkBook );

            DataPage<BlogPost> blogpostList = postService.GetPageByCategory( ctx.app.Id, categoryId, 25 );

            bindCategoryName( categoryId );


            setCategoryDropList();
            setBlock( blogpostList );
        }



        private void bindCategoryName( int categoryId ) {
            String categoryName = "";
            if (categoryId > 0) {
                BlogCategory category = categoryService.GetById( categoryId, ctx.owner.obj.Id );
                if (category != null) categoryName = "：" + category.Name;
            }
            set( "category.Title", categoryName );
        }


        private void setBlock( DataPage<BlogPost> blogpostList ) {
            IBlock block = getBlock( "list" );

            foreach (BlogPost post in blogpostList.Results) {

                block.Set( "post.Id", post.Id );
                block.Set( "post.CategoryName", post.Category.Name );
                block.Set( "post.CategoryUrl", to( ListByCategory, post.Category.Id ) );

                String strStatus = getStatusStr( post );
                block.Set( "post.Status", strStatus );
                block.Set( "post.Title", post.Title );
                block.Set( "post.Hits", post.Hits );
                block.Set( "post.ReplyCount", post.Replies );
                block.Set( "post.Created", post.Created );
                block.Set( "post.Url", alink.ToAppData( post ) );
                block.Set( "post.EditUrl", to( new PostController().Edit , post.Id ) );
                block.Next();
            }
            set( "page", blogpostList.PageBar );

        }

        private String getStatusStr( BlogPost post ) {
            String strStatus = string.Empty;
            if (post.IsTop == 1) strStatus = "<span class=\"lblTop\">[" + lang( "sticky" ) + "]</span>";
            if (post.IsPick == 1) strStatus = strStatus + "<span class=\"lblTop\">[" + lang( "picked" ) + "]</span>";
            if (post.AttachmentCount > 0) {
                strStatus = strStatus + string.Format( "<span><img src=\"{0}\"/></span>", strUtil.Join( sys.Path.Img, "attachment.gif" ) );
            }
            return strStatus;
        }

        private void setCategoryDropList() {
            int appId = ctx.app.Id;
            List<BlogCategory> categories = categoryService.GetByApp( ctx.app.Id );
            BlogCategory category = new BlogCategory();
            category.Id = -1;
            category.Name = lang( "moveCategory" );
            List<BlogCategory> list = new List<BlogCategory>();
            list.Add( category );
            foreach (BlogCategory cat in categories) {
                list.Add( cat );
            }
            String dropHtml = Html.DropList( list, "categoryList", "Name", "Id", null ).Replace( "select name", "select id=\"adminDropCategoryList\" name" );
            set( "blog.CategoryList", dropHtml );
        }




        [HttpPost, DbTransaction]
        public void Admin() {

            String ids = ctx.PostIdList( "choice" );
            String cmd = ctx.Post( "action" );
            int categoryId = ctx.PostInt( "categoryId" );

            if (strUtil.IsNullOrEmpty( cmd )) {
                echoErrorCmd();
                return;
            }

            int appId = ctx.app.Id;

            Boolean cmdValid = true;

            if (cmd.Equals( "top" )) {
                postService.SetTop( ids, appId );
            }
            else if (cmd.Equals( "untop" )) {
                postService.SetUntop( ids, appId );
            }
            else if (cmd.Equals( "pick" )) {
                postService.SetPick( ids, appId );
            }
            else if (cmd.Equals( "unpick" )) {
                postService.SetUnpick( ids, appId );
            }
            else if (cmd.Equals( "category" )) {
                postService.ChangeCategory( categoryId, ids, appId );
            }
            else if (cmd.Equals( "delete" )) {
                postService.Delete( ids, appId );
            }
            else if (cmd.Equals( "deletetrue" )) {
                postService.DeleteTrue( ids, appId );
            }
            else {
                cmdValid = false;
            }

            // echo
            if (cmdValid == false) {
                echoErrorCmd();
            }
            else {
                logAdmin( cmd, ids );
                echoAjaxOk();
            }
        }

        private void logAdmin( string cmd, string ids ) {
        }

        private void echoErrorCmd() {
            echoText( lang( "exUnknowCmd" ) );
        }

        private void bindLink( IBlock tpl, String lbl, object obj ) {
            BlogCategory category = obj as BlogCategory;
            tpl.Set( "c.LinkPostAdmin", to( new MyListController().ListByCategory, category.Id ) );
        }


    }

}
