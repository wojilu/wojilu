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
    public class DraftController : ControllerBase {


        public IBlogService blogService { get; set; }
        public IBlogCategoryService categoryService { get; set; }
        public IBlogPostService postService { get; set; }

        public IFeedService feedService { get; set; }
        public IFriendService friendService { get; set; }

        public DraftController() {

            blogService = new BlogService();
            postService = new BlogPostService();
            categoryService = new BlogCategoryService();
            feedService = new FeedService();
            friendService = new FriendService();
        }


        public void Draft() {
            target( Admin );
            DataPage<BlogPost> blogpostList = postService.GetDraft( ctx.app.Id, 25 );
            bindDraftList( blogpostList );
        }


        public void EditDraft( int id ) {

            target( PublishDraft, id );

            BlogPost data = postService.GetById( id, ctx.owner.Id );
            if (data == null) {
                echoRedirect( lang( "exDataNotFound" ) );
                return;
            }

            bindDraftEdit( data );
        }


        [HttpPost, DbTransaction]
        public void PublishDraft( int id ) {

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

            post.AccessStatus = cvt.ToInt( ctx.Post( "AccessStatus" ) );
            post.CommentCondition = cvt.ToInt( ctx.Post( "IsCloseComment" ) );
            if (post.SaveStatus == SaveStatus.Draft) post.Created = DateTime.Now;

            post.Ip = ctx.Ip;

            String tagStr = strUtil.SubString( ctx.Post( "TagList" ), 200 );
            post.Tags = TagService.ResetRawTagString( tagStr );

            Result result = postService.PublishDraft( post );
            if (result.IsValid) {
                echoRedirectPart( lang( "opok" ), to( new MyListController().My ) );
            }
            else {
                echoRedirect( result.ErrorsHtml );
            }

        }


        [HttpPost, DbTransaction]
        public void Admin() {

            if (adminList()) {
                echoAjaxOk();
            }
            else {
                echoText( lang( "exUnknowCmd" ) );
            }
        }


        private Boolean adminList() {

            String ids = ctx.PostIdList( "choice" );
            String cmd = ctx.Post( "action" );
            int categoryId = ctx.PostInt( "categoryId" );

            if (strUtil.IsNullOrEmpty( cmd )) return false;

            int appId = ctx.app.Id;

            if (cmd.Equals( "deletetrue" )) {
                postService.DeleteTrue( ids, appId );
                return true;
            }

            return false;
        }


        private void bindDraftList( DataPage<BlogPost> blogpostList ) {
            IBlock block = getBlock( "list" );
            foreach (BlogPost post in blogpostList.Results) {
                block.Set( "post.CategoryName", post.Category.Name );
                block.Set( "post.CategoryUrl", to( new MyListController().ListByCategory, post.Category.Id ) );
                block.Set( "post.Id", post.Id );
                block.Set( "post.Title", post.Title );
                block.Set( "post.Url", alink.ToAppData( post ) );
                block.Set( "post.Created", post.Created );
                block.Set( "post.EditUrl", to( EditDraft, post.Id ) );
                block.Next();
            }

            set( "page", blogpostList.PageBar );
        }


        private void bindDraftEdit( BlogPost data ) {
            List<BlogCategory> categories = categoryService.GetByApp( ctx.app.Id );

            dropList( "CategoryId", categories, "Name=Id", data.Category.Id );

            set( "data.Id", data.Id );
            set( "data.Abstract", data.Abstract );
            set( "data.TagList", data.Tag.TextString );
            set( "data.Title", data.Title );

            set( "Content", data.Content );

            set( "data.AccessStatus", AccessStatusUtil.GetRadioList( data.AccessStatus ) );
            set( "data.IsCloseComment", Html.CheckBox( "IsCloseComment", lang( "closeComment" ), "1", cvt.ToBool( data.CommentCondition ) ) );

            set( "draftActionUrl", to( SaveDraft ) );
        }


        [HttpPost, DbTransaction]
        public void SaveDraft() {

            Result result;
            String content = ctx.PostHtml( "Content" );
            if (strUtil.IsNullOrEmpty( content )) {
                echoError( lang( "exContent" ) );
                return;
            }

            if (checkIsDraftNew()) {
                result = postService.InsertDraft( getDraftPost( new BlogPost() ) );
            }
            else {
                result = postService.UpdateDraft( getDraftPost( postService.GetDraft( ctx.PostInt( "draftId" ) ) ) );
            }

            if (result.IsValid) {
                echoRedirect( ((IEntity)result.Info).Id.ToString() );
            }
            else {
                echoError( result );
            }
        }


        private Boolean checkIsDraftNew() {
            return ctx.PostInt( "draftId" ) <= 0;
        }

        private BlogPost getDraftPost( BlogPost data ) {

            String title = ctx.Post( "Title" );
            String postabstract = ctx.Post( "Abstract" );
            String body = ctx.PostHtml( "Content" );
            String tags = strUtil.SubString( ctx.Post( "TagList" ), 200 );

            int categoryId = ctx.PostInt( "CategoryId" );
            int accessStatus = ctx.PostInt( "AccessStatus" );
            int isCloseComment = ctx.PostInt( "IsCloseComment" );

            BlogCategory category = new BlogCategory();
            category.Id = categoryId;
            data.Category = category;

            data.Title = title;
            if (strUtil.IsNullOrEmpty( data.Title )) data.Title = getDefaultTitle();

            data.Abstract = postabstract;
            data.Content = body;
            data.AccessStatus = accessStatus;
            data.CommentCondition = isCloseComment;
            data.SaveStatus = SaveStatus.Draft;
            data.Tags = TagService.ResetRawTagString( tags );

            populatePost( data );
            return data;
        }

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


    }
}
