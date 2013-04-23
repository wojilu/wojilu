/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;

using wojilu.Members.Users.Service;
using wojilu.Members.Users.Domain;
using wojilu.Members.Users.Interface;

using wojilu.Common.Feeds.Service;
using wojilu.Common.Feeds.Interface;
using wojilu.Common.AppBase;
using wojilu.Common.Tags;
using wojilu.Common.Upload;

using wojilu.Apps.Blog.Domain;
using wojilu.Apps.Blog.Service;
using wojilu.Apps.Blog.Interface;
using wojilu.Web.Controller.Users;
using wojilu.Web.Controller.Admin;

namespace wojilu.Web.Controller.Blog.Admin {

    [App( typeof( BlogApp ) )]
    public partial class PostController : ControllerBase {

        public IBlogService blogService { get; set; }
        public IBlogCategoryService categoryService { get; set; }
        public IBlogPostService postService { get; set; }

        public IFeedService feedService { get; set; }
        public IFriendService friendService { get; set; }
        public UserFileService fileService { get; set; }

        public PostController() {

            blogService = new BlogService();
            postService = new BlogPostService();
            categoryService = new BlogCategoryService();
            feedService = new FeedService();
            friendService = new FriendService();
            fileService = new UserFileService();
        }


        public void Add() {

            List<BlogCategory> categories = categoryService.GetByApp( ctx.app.Id );

            Boolean showCategoryBox = categories.Count == 0;
            set( "showCategoryBox", showCategoryBox.ToString().ToLower() );

            target( Create );
            bindAdd( categories );
            bindUploadInfo();
        }

        private void bindUploadInfo() {
            //附件
            set( "uploadLink", to( new UserUploadController().SaveUserFile ) ); // 接受上传的网址
            set( "authJson", ctx.web.GetAuthJson() );
            set( "jsPath", sys.Path.DiskJs );
        }

        [HttpPost, DbTransaction]
        public void Create() {

            BlogPost data = new BlogPost();
            BlogCategory category = new BlogCategory();
            category.Id = ctx.PostInt( "CategoryId" );
            data.Category = category;
            data.Title = ctx.Post( "Title" );
            data.Abstract = ctx.Post( "Abstract" );
            data.Content = ctx.PostHtml( "Content" );

            if (category.Id <= 0) errors.Add( lang( "exUnCategoryTip" ) );
            if (strUtil.IsNullOrEmpty( data.Content )) errors.Add( lang( "exContent" ) );

            if (ctx.HasErrors) {
                echoError();
                return;
            }

            if (ctx.PostIsCheck( "saveContentPic" ) == 1) {
                data.Content = wojilu.Net.PageLoader.ProcessPic( data.Content, null );
            }


            if (strUtil.IsNullOrEmpty( data.Title )) data.Title = getDefaultTitle();

            data.AccessStatus = (int)AccessStatusUtil.GetPostValue( ctx.PostInt( "AccessStatus" ) );
            data.CommentCondition = cvt.ToInt( ctx.Post( "IsCloseComment" ) );
            data.SaveStatus = 0;

            String tagStr = strUtil.SubString( ctx.Post( "TagList" ), 200 );
            data.Tags = TagService.ResetRawTagString( tagStr );
            populatePost( data );

            // 附件处理
            int[] ids = cvt.ToIntArray( ctx.Post( "attachmentIds" ) );

            Result result = postService.Insert( data, ids );
            if (result.IsValid) {

                echoRedirectPart( lang( "opok" ), to( new MyListController().Index ) );
            }
            else {
                echoError( result );
            }

        }



        //-------------------------------- edit&save -------------------------------------------

        public void Edit( int id ) {

            target( Update, id );

            BlogPost data = postService.GetById( id, ctx.owner.Id );
            if (data == null) {
                echoRedirect( lang( "exDataNotFound" ) );
                return;
            }

            List<BlogCategory> categories = categoryService.GetByApp( ctx.app.Id );
            bindEdit( data, categories );

            bindAttachmentPanel( data );
        }

        private void bindAttachmentPanel( BlogPost post ) {

            //附件
            String dataInfo = string.Format( "?dataId={0}&dataType={1}&viewerId={2}&viewerUrl={3}&ownerId={4}&ownerType={5}&ownerUrl={6}",
                post.Id,
                typeof( BlogPost ).FullName,
                ctx.viewer.Id,
                ctx.viewer.obj.Url,
                ctx.owner.Id,
                ctx.owner.obj.GetType().FullName,
                ctx.owner.obj.Url );

            set( "uploadLink", to( new UserUploadController().SaveUserFile ) ); // 接受上传的网址
            set( "authJson", AdminSecurityUtils.GetAuthCookieJson( ctx ) );
            set( "jsPath", sys.Path.DiskJs );

            set( "dataId", post.Id );
            set( "dataType", typeof( BlogPost ).FullName );

            set( "viewerId", ctx.viewer.Id );
            set( "viewerUrl", ctx.viewer.obj.Url );

            set( "ownerId", ctx.owner.Id );
            set( "ownerType", ctx.owner.obj.GetType().FullName );
            set( "ownerUrl", ctx.owner.obj.Url );


            bindAttachments( post );

        }

        private void bindAttachments( BlogPost post ) {

            List<UserFile> list = fileService.GetByData( post );

            IBlock block = getBlock( "attachments" );
            foreach (UserFile obj in list) {

                if (obj.IsPic == 1) {
                    block.Set( "obj.PicLink", string.Format( "<a href=\"{0}\" target=\"_blank\" class=\"lnkPreview\">查看图片</a>", obj.PicO ) );
                }
                else {
                    block.Set( "obj.PicLink", "" );
                }

                block.Set( "obj.Id", obj.Id );
                block.Set( "obj.FileName", obj.FileName );
                block.Set( "obj.FileSizeKB", obj.FileSizeKB );
                block.Set( "obj.DeleteLink", to( new UserUploadController().DeleteUserFile, obj.Id ) );
                block.Next();
            }

        }

        [HttpPost, DbTransaction]
        public void Update( int id ) {

            BlogPost post = postService.GetById( id, ctx.owner.Id );
            if (post == null) {
                echoRedirect( lang( "exDataNotFound" ) );
                return;
            }

            BlogCategory category = new BlogCategory();

            category.Id = cvt.ToInt( ctx.Post( "CategoryId" ) );
            post.Category = category;
            post.Abstract = ctx.Post( "Abstract" );
            post.Content = ctx.PostHtml( "Content" );
            post.Title = ctx.Post( "Title" );
            if (strUtil.IsNullOrEmpty( post.Title ) || strUtil.IsNullOrEmpty( post.Content )) {
                echoRedirect( lang( "exTitleContent" ) );
                return;
            }

            if (ctx.PostIsCheck( "saveContentPic" ) == 1) {
                post.Content = wojilu.Net.PageLoader.ProcessPic( post.Content, null );
            }

            post.AccessStatus = cvt.ToInt( ctx.Post( "AccessStatus" ) );
            post.CommentCondition = cvt.ToInt( ctx.Post( "IsCloseComment" ) );
            if (post.SaveStatus == SaveStatus.Draft) post.Created = DateTime.Now;

            post.SaveStatus = SaveStatus.Normal;
            post.Ip = ctx.Ip;

            Result result = db.update( post );
            if (result.IsValid) {
                TagService.SaveDataTag( post, ctx.Post( "TagList" ) );
                echoRedirectPart( lang( "opok" ), to( new MyListController().Index ) );
            }
            else {
                echoRedirect( result.ErrorsHtml );
            }
        }


        //------------------------------ helper ---------------------------------------------

        private String getDefaultTitle() {
            return string.Format( "{0} " + lang( "log" ), DateTime.Now.ToShortDateString() );
        }

        private void populatePost( BlogPost data ) {
            data.Ip = ctx.Ip;
            data.OwnerId = ctx.owner.Id;
            data.OwnerUrl = ctx.owner.obj.Url;
            data.OwnerType = ctx.owner.obj.GetType().FullName;
            data.Creator = (User)ctx.viewer.obj;
            data.CreatorUrl = ctx.viewer.obj.Url;
            data.AppId = ctx.app.Id;
        }

        private void bindAdd( List<BlogCategory> categories ) {
            set( "categoryAddUrl", to( new CategoryController().New ) );
            set( "DraftActionUrl", to( new DraftController().SaveDraft ) );
            dropList( "CategoryId", categories, "Name=Id", null );
        }

        private void bindEdit( BlogPost data, List<BlogCategory> categories ) {
            dropList( "CategoryId", categories, "Name=Id", data.Category.Id );

            set( "data.Id", data.Id );
            set( "data.Abstract", data.Abstract );
            set( "data.TagList", data.Tag.TextString );
            set( "data.Title", data.Title );

            set( "Content", data.Content );

            set( "data.AccessStatus", AccessStatusUtil.GetRadioList( data.AccessStatus ) );
            set( "data.IsCloseComment", Html.CheckBox( "IsCloseComment", lang( "closeComment" ), "1", cvt.ToBool( data.CommentCondition ) ) );
        }


    }

}
