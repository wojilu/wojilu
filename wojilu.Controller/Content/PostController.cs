/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;

using wojilu.DI;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Apps.Content.Domain;
using wojilu.Apps.Content.Interface;
using wojilu.Apps.Content.Service;
using wojilu.Web.Controller.Content.Section;
using wojilu.Web.Context;
using wojilu.Web.Controller.Common;
using wojilu.Web.Controller.Content.Utils;
using wojilu.Web.Controller.Content.Caching;
using wojilu.Common.AppBase;
using wojilu.Members.Sites.Domain;
using wojilu.Common.Tags;
using wojilu.ORM;
using wojilu.Common.AppBase.Interface;

namespace wojilu.Web.Controller.Content {

    [App( typeof( ContentApp ) )]
    public class PostController : ControllerBase {

        public IContentPostService postService { get; set; }
        public IContentSectionService sectionService { get; set; }

        public PostController() {
            LayoutControllerType = typeof( Section.LayoutController );

            postService = new ContentPostService();
            sectionService = new ContentSectionService();
        }

        [CacheAction( typeof( ContentLayoutCache ) )]
        public override void Layout() {
        }

        public void Recent() {

            DataPage<ContentPost> list = postService.GetByApp( ctx.app.Id, 50 );
            bindPosts( list );

            Page.Title = ctx.app.Name + "最新文章";
        }

        private void bindPosts( DataPage<ContentPost> posts ) {
            IBlock block = getBlock( "list" );
            foreach (ContentPost post in posts.Results) {

                if (post.PageSection == null) continue;
                if (post.PageSection.SectionType == typeof( TextController ).FullName) continue;

                BinderUtils.bindListItem( block, post, ctx );
                block.Next();
            }
            set( "page", posts.PageBar );
        }
        

        public void Show( int id ) {

            ContentPost post = postService.GetById( id, ctx.owner.Id );

            if (post == null) {
                echo( lang( "exDataNotFound" ) );
                return;
            }
            else if (post.PageSection == null) {
                echo( lang( "exDataNotFound" ) + ":PageSection is null" );
                return;
            }

            // redirect
            if (strUtil.HasText( post.RedirectUrl )) {
                redirectUrl( post.RedirectUrl );
                return;
            }

            //----------------------------------------------------------------------------------------------------

            // 0) page meta
            bindMetaInfo( post );

            // 1) location
            String location = string.Format( "<a href='{0}'>{1}</a>", Link.To( new ContentController().Index ),
    ((AppContext)ctx.app).Menu.Name );
            location = location + string.Format( " &gt; <a href='{0}'>{1}</a> &gt; {2}", to( new SectionController().Show, post.PageSection.Id ), post.PageSection.Title, alang( "postDetail" ) );
            set( "location", location );

            // 2) detail
            set( "detailContent", loadHtml( post.PageSection.SectionType, "Show", post.Id ) );

            // 3) comment
            loadComment( post );

            // 4) related posts
            loadRelatedPosts( post );

            // 5) prev/next
            bindPrevNext( post );

            // 6) tag
            String tag = post.Tag.List.Count > 0 ? post.Tag.HtmlString : "";
            set( "post.Tag", tag );

            // 7) digg
            set( "lnkDiggUp", to( DiggUp, post.Id ) );
            set( "lnkDiggDown", to( DiggDown, post.Id ) );

            // 8) link
            String postUrl = getFullUrl( alink.ToAppData( post ) );
            set( "post.Url", postUrl );
            bind( "post", post );

        }

        private String getFullUrl( String url ) {
            if (url == null) return "";
            if (url.StartsWith( "http" )) return url;
            return strUtil.Join( ctx.url.SiteAndAppPath, url );
        }

        private void bindMetaInfo( ContentPost post ) {

            WebUtils.pageTitle( this, post.Title, ctx.app.Name );

            if (strUtil.HasText( post.MetaKeywords ))
                this.Page.Keywords = post.MetaKeywords;
            else
                this.Page.Keywords = post.Tag.TextString;

            if (strUtil.HasText( post.MetaDescription ))
                this.Page.Description = post.MetaDescription;
            else
                this.Page.Description = post.Summary;
        }


        [HttpPost, DbTransaction]
        public void DiggUp( int id ) {

            if (ctx.viewer.IsLogin == false) {
                echoText( "必须登录才能操作，请先登录" );
                return;
            }

            ContentPost post = postService.GetById( id, ctx.owner.Id );

            if (post == null) {
                echoText( lang( "exDataNotFound" ) );
                return;
            }

            ContentDigg digg = ContentDigg.find( "UserId=" + ctx.viewer.Id + " and PostId=" + post.Id ).first();
            if (digg != null) {
                echoText( "你已经操作，请勿重复" );
                return;
            }

            ContentDigg d = new ContentDigg();
            d.UserId = ctx.viewer.Id;
            d.PostId = post.Id;
            d.TypeId = 0;
            d.Ip = ctx.Ip;
            d.insert();

            post.DiggUp++;
            post.update( "DiggUp" );

            echoAjaxOk();

        }

        [HttpPost, DbTransaction]
        public void DiggDown( int id ) {

            if (ctx.viewer.IsLogin == false) {
                echoText( "必须登录才能操作，请先登录" );
                return;
            }

            ContentPost post = postService.GetById( id, ctx.owner.Id );

            if (post == null) {
                echoText( lang( "exDataNotFound" ) );
                return;
            }

            ContentDigg digg = ContentDigg.find( "UserId=" + ctx.viewer.Id + " and PostId=" + post.Id ).first();
            if (digg != null) {
                echoText( "你已经操作，请勿重复" );
                return;
            }

            ContentDigg d = new ContentDigg();
            d.UserId = ctx.viewer.Id;
            d.PostId = post.Id;
            d.TypeId = 1;
            d.Ip = ctx.Ip;
            d.insert();

            post.DiggDown++;
            post.update( "DiggDown" );

            echoAjaxOk();

        }

        private void bindPrevNext( ContentPost post ) {

            ContentPost prev = postService.GetPrevPost( post );
            ContentPost next = postService.GetNextPost( post );

            String lnkPrev = prev == null ? "(没了)" : string.Format( "<a href=\"{0}\">{1}</a>", alink.ToAppData( prev ), prev.Title );
            String lnkNext = next == null ? "(没了)" : string.Format( "<a href=\"{0}\">{1}</a>", alink.ToAppData( next ), next.Title );

            set( "prevPost", lnkPrev );
            set( "nextPost", lnkNext );
        }

        private void loadRelatedPosts( ContentPost post ) {

            List<DataTagShip> list = postService.GetRelatedDatas( post );
            IBlock block = getBlock( "related" );

            foreach (DataTagShip dt in list) {

                EntityInfo ei = Entity.GetInfo( dt.TypeFullName );
                if (ei == null) continue;

                IAppData obj = ndb.findById( ei.Type, dt.DataId ) as IAppData;
                if (obj == null) continue;

                block.Set( "p.Title", obj.Title );
                block.Set( "p.Link", alink.ToAppData( obj ) );
                block.Set( "p.Created", obj.Created );

                block.Next();

            }


        }

        private void loadComment( ContentPost post ) {

            ContentApp app = ctx.app.obj as ContentApp;

            if (post.CommentCondition == CommentCondition.Close || app.GetSettingsObj().AllowComment == 0) {
                set( "commentSection", "" );
                return;
            }

            ctx.SetItem( "createAction", to( new ContentCommentController().Create, post.Id ) );
            ctx.SetItem( "commentTarget", post );
            load( "commentSection", new ContentCommentController().ListAndForm );
        }



    }

}
