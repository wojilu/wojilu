/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.ORM;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;

using wojilu.Common.AppBase.Interface;
using wojilu.Common.Tags;

using wojilu.Web.Controller.Content.Caching;
using wojilu.Web.Controller.Content.Section;
using wojilu.Web.Controller.Content.Utils;

using wojilu.Apps.Content.Domain;
using wojilu.Apps.Content.Interface;
using wojilu.Apps.Content.Service;
using System.Text;
using wojilu.Web.Utils;
using wojilu.Common.AppBase;
using wojilu.Web.Controller.Content.Htmls;

namespace wojilu.Web.Controller.Content {

    [App( typeof( ContentApp ) )]
    public class PostController : ControllerBase {

        public IContentPostService postService { get; set; }
        public IContentSectionService sectionService { get; set; }
        public ContentPostSectionService psService { get; set; }
        public IAttachmentService attachmentService { get; set; }

        public PostController() {
            LayoutControllerType = typeof( Section.LayoutController );

            postService = new ContentPostService();
            sectionService = new ContentSectionService();
            attachmentService = new AttachmentService();
            psService = new ContentPostSectionService();
        }

        [CacheAction( typeof( ContentLayoutCache ) )]
        public override void Layout() {
        }

        public void Recent() {

            DataPage<ContentPost> list = postService.GetByApp( ctx.app.Id, ContentSetting.ListRecentPageSize );
            bindPosts( list );

            String cpLink = clink.toRecent( ctx );
            Boolean isMakeHtml = HtmlHelper.IsMakeHtml( ctx );

            int pageWidth = ContentSetting.ListPageWidth;
            set( "page", list.GetPageBar( cpLink, isMakeHtml ) );
        }


        private void bindPosts( DataPage<ContentPost> posts ) {
            Page.Title = ctx.app.Name + "最新文章";
            IBlock block = getBlock( "list" );
            foreach (ContentPost post in posts.Results) {

                if (post.PageSection == null) continue;
                if (post.PageSection.SectionType == typeof( TextController ).FullName) continue;

                BinderUtils.bindListItem( block, post, ctx );
                block.Next();
            }
        }


        public void Show( int id ) {

            ContentPost post = postService.GetById( id, ctx.owner.Id );

            if (post == null) {
                echo( lang( "exDataNotFound" ) + "=ContentPost" );
                return;
            }

            ContentSection section = psService.GetFirstSectionByPost( post.Id );
            if (section == null) {
                echo( lang( "exDataNotFound" ) + "=ContentSection" );
                return;
            }

            // redirect
            if (strUtil.HasText( post.RedirectUrl )) {
                redirectUrl( post.RedirectUrl );
                return;
            }


            //----------------------------------------------------------------------------------------------------

            // 1) location
            set( "location", getLocation( post, section ) );

            // 2) detail
            set( "detailContent", loadHtml( section.SectionType, "Show", post.Id ) );

            // 3) comment
            set( "commentUrl", getCommentUrl( post ) );

            // 4) related posts
            loadRelatedPosts( post );

            // 5) prev/next
            bindPrevNext( post );

            // 6) other info, tag, src, summary
            String tag = post.Tag.List.Count > 0 ? tag = "tag: " + post.Tag.HtmlString : "";
            set( "post.Tag", (post.Tag.List.Count > 0 ? tag = "tag: " + post.Tag.HtmlString : "") );
            set( "post.Source", getSrc( post ) );
            set( "post.Title", post.GetTitle() );
            set( "post.Replies", getReplies( post ) );
            set( "post.Submitter", getSubmitter( post ) );
            bindSummary( post );

            // 7) digg
            set( "lnkDiggUp", to( DiggUp, post.Id ) );
            set( "lnkDiggDown", to( DiggDown, post.Id ) );

            // 9) 附件
            set( "attachmentList", getAttachmentList( post ) );

            // 10) page meta，最后一个绑定，覆盖各 Section 自己的配置
            bindMetaInfo( post );

            // 11) 统计信息
            set( "lnkStats", to( Stats, id ) );

            bind( "post", post );
        }

        private string getCommentUrl( ContentPost post ) {

            if (post.CommentCondition == CommentCondition.Close) {
                return "#";
            }

            return t2( new wojilu.Web.Controller.Open.CommentController().List )
                + "?url=" + alink.ToAppData( post, ctx )
                + "&dataType=" + typeof( ContentPost ).FullName
                + "&dataTitle=" + post.Title
                + "&dataUserId=" + post.Creator.Id
                + "&dataId=" + post.Id
                + "&appId=" + post.AppId;
        }

        private String getLocation( ContentPost post, ContentSection section ) {
            String location = string.Format( "<a href='{0}'>{1}</a>", alink.ToApp( ctx.app.obj as IApp, ctx ),
    ctx.app.Name );

            location = location + string.Format( " &gt; <a href='{0}'>{1}</a> &gt; {2}", clink.toSection( section.Id, ctx ), section.Title, alang( "postDetail" ) );
            return location;
        }

        private string getSubmitter( ContentPost post ) {
            if (post.Creator != null) {
                return string.Format( "<a href=\"{0}\" target=\"_blank\">{1}</a>", toUser( post.Creator ), post.Creator.Name );
            }
            else {
                return "无";
            }
        }

        private String getReplies( ContentPost post ) {

            String replies = lang( "commentClosed" );

            if (post.CommentCondition != CommentCondition.AllowAll) return replies;

            if (post.Replies == 0) {
                replies = string.Format( "{0}:0", lang( "comment" ) );
            }
            else {
                replies = string.Format( "{0}:{1} <a href=\"#comments\">{2}</a>", lang( "comment" ), post.Replies, lang( "viewByHit" ) );
            }
            return replies;
        }

        private void bindSummary( ContentPost post ) {
            IBlock block = getBlock( "summary" );
            if (strUtil.HasText( post.Summary )) {
                block.Set( "post.Summary", post.Summary );
                block.Next();
            }
        }

        private String getSrc( ContentPost post ) {
            String src = null;
            if (strUtil.HasText( post.SourceLink )) {
                if (post.SourceLink.ToLower().StartsWith( "http:" )) {
                    src = lang( "src" ) + string.Format( ": <a href=\"{0}\" target=\"_blank\">{0}</a>", post.SourceLink );
                }
                else {
                    src = lang( "src" ) + ": " + post.SourceLink;
                }
            }
            return src;
        }


        private String getAttachmentList( ContentPost data ) {

            if (data.Attachments <= 0) return "";

            if (data.IsAttachmentLogin == 1 && ctx.viewer.IsLogin == false) {
                return "<div class=\"downloadWarning\"><div>" + alang( "downloadNeedLogin" ) + "</div></div>";
            }

            List<ContentAttachment> attachList = attachmentService.GetAttachmentsByPost( data.Id );

            StringBuilder sb = new StringBuilder();
            String created = attachList[0].Created.ToString();
            sb.Append( "<div class=\"hr\"></div><div class=\"attachmentTitleWrap\"><div class=\"attachmentTitle\">" + alang( "attachment" ) + " <span class=\"note\">(" + created + ")</span> " );
            sb.Append( "</div></div><ul class=\"attachmentList unstyled\">" );

            foreach (ContentAttachment attachment in attachList) {

                string fileName = attachment.GetFileShowName();

                if (isImage( attachment )) {

                    sb.AppendFormat( "<li><div>{0} <span class=\"note\">({1}KB)</span></div>", fileName, attachment.FileSizeKB );
                    sb.AppendFormat( "<div><a href=\"{0}\" target=\"_blank\"><img src=\"{1}\" /></a></div>",
                        attachment.FileUrl, attachment.FileMediuUrl );
                    sb.Append( "</li>" );

                }
                else {


                    sb.AppendFormat( "<li><div>{0} <span class=\"note right10\">({1}KB)</span>", fileName, attachment.FileSizeKB );
                    sb.AppendFormat( "<img src=\"{1}\" /><a href=\"{0}\" target=\"_blank\">" + alang( "hitDownload" ) + "</a></div>", to( new Common.AttachmentController().Show, attachment.Id ) + "?id=" + attachment.Guid, strUtil.Join( sys.Path.Img, "/s/download.png" ) );
                    sb.Append( "</li>" );
                }
            }
            sb.Append( "</ul>" );

            return string.Format( "<div id=\"attachmentPanel\">{0}</div>", sb.ToString() );
        }

        private Boolean isImage( ContentAttachment attachment ) {
            return Uploader.IsImage( attachment.Type );
        }

        private void bindMetaInfo( ContentPost post ) {

            ctx.Page.SetTitle( post.GetTitle(), ctx.app.Name );

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

            String lnkPrev = prev == null ? "(没了)" : string.Format( "<a href=\"{0}\">{1}</a>", alink.ToAppData( prev, ctx ), prev.Title );
            String lnkNext = next == null ? "(没了)" : string.Format( "<a href=\"{0}\">{1}</a>", alink.ToAppData( next, ctx ), next.Title );

            set( "prevPost", lnkPrev );
            set( "nextPost", lnkNext );
        }

        private void loadRelatedPosts( ContentPost post ) {

            List<DataTagShip> list = postService.GetRelatedDatas( post, 21 );
            IBlock block = getBlock( "related" );

            List<IAppData> addList = new List<IAppData>();

            foreach (DataTagShip dt in list) {

                if (dt.DataId == post.Id && dt.TypeFullName.Equals( typeof( ContentPost ).FullName )) continue;

                EntityInfo ei = Entity.GetInfo( dt.TypeFullName );
                if (ei == null) continue;

                IAppData obj = ndb.findById( ei.Type, dt.DataId ) as IAppData;
                if (obj == null) continue;

                if (hasAdded( addList, obj )) continue;

                block.Set( "p.Title", obj.Title );

                String lnkPost = "";
                if (obj is ContentPost) {
                    lnkPost = alink.ToAppData( obj, ctx ); // 暂时只有 ContentPost 支持 Html 静态页面生成
                }
                else {
                    lnkPost = alink.ToAppData( obj );
                }

                block.Set( "p.Link", lnkPost );
                block.Set( "p.Created", obj.Created );

                block.Next();
            }
        }

        private bool hasAdded( List<IAppData> xlist, IAppData obj ) {

            foreach (IAppData x in xlist) {
                if (x.Id == obj.Id && x.GetType() == obj.GetType()) return true;
            }
            return false;
        }

        public void Stats( int id ) {

            Dictionary<String, String> dic = new Dictionary<String, String>();
            dic["hits"] = "0";
            dic["diggUp"] = "0";
            dic["diggDown"] = "0";
            dic["diggUpPercent"] = "";
            dic["diggDownPercent"] = "";

            ContentPost post = postService.GetById( id, ctx.owner.Id );

            if (post == null) {
                echoJson( dic );
                return;
            }

            postService.AddHits( post );


            dic["hits"] = post.Hits.ToString();
            dic["diggUp"] = post.DiggUp.ToString();
            dic["diggDown"] = post.DiggDown.ToString();
            dic["diggUpPercent"] = post.DiggUpPercent;
            dic["diggDownPercent"] = post.DiggDownPercent;

            echoJson( dic );
        }


    }

}
