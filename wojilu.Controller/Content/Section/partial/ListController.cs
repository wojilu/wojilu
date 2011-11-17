/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;

using wojilu.Apps;
using wojilu.ORM;

using wojilu.Apps.Content.Domain;
using wojilu.Apps.Content.Interface;
using wojilu.Apps.Content.Service;
using wojilu.Common.AppBase;
using wojilu.Common.AppBase.Interface;
using System.Text;
using wojilu.Web.Controller.Content.Utils;
using wojilu.Members.Sites.Domain;
using wojilu.Web.Utils;

namespace wojilu.Web.Controller.Content.Section {


    public partial class ListController : ControllerBase, IPageSection {


        private void bindSectionPosts( IList posts ) {
            IBlock block = getBlock( "list" );
            foreach (ContentPost post in posts) {

                BinderUtils.bindPostSingle( block, post );

                block.Next();
            }
        }


        private void bindPostList( ContentSection section, DataPage<ContentPost> posts, ContentSetting setting ) {
            set( "section.Name", section.Title );
            IBlock block = getBlock( "list" );
            foreach (ContentPost post in posts.Results) {

                BinderUtils.bindListItem( block, post, ctx );

                if (setting.ArticleListMode == ArticleListMode.Summary) {
                    block.Set( "post.Summary", post.GetSummary( setting.SummaryLength ) );
                }

                block.Next();
            }
            set( "page", posts.PageBar );
        }


        private void bindDetail( int id, ContentPost post ) {

            set( "post.Title", post.GetTitle() );
            set( "post.Author", post.Author );
            set( "post.Created", post.Created );

            String tag = post.Tag.List.Count > 0 ? tag = "tag: " + post.Tag.HtmlString : "";
            String src = getSrc( post );
            String replies = getReplies( post );

            set( "post.Tag", tag );
            set( "post.Replies", replies );
            set( "post.Hits", post.Hits );
            set( "post.Source", src );

            if (post.Creator != null) {
                set( "post.Submitter", string.Format( "<a href=\"{0}\" target=\"_blank\">{1}</a>", Link.ToMember( post.Creator ), post.Creator.Name ) );
            }
            else {
                set( "post.Submitter", "ÎÞ" );
            }

            bindSummary( post );

            String content;

            String pageSeparator = getPageSeparator( post.Content );
            if (pageSeparator == null) {
                content = post.Content;
            }
            else {
                content = getPagedContent( post, pageSeparator );
            }

            if (post.HasImg()) {
                content = string.Format( "<div style=\"text-align:center;\"><img src=\"{0}\" />", post.GetImgMedium() ) + "</div>" + content;
            }

            if (post.IsAttachmentLogin == 1 && ctx.viewer.IsLogin == false) {
                content += "<div class=\"downloadWarning\"><div>" + alang( "downloadNeedLogin" ) + "</div></div>";
            }
            else {
                content = addAttachment( post, content );
            }

            set( "post.Content", content );
        }

        private static readonly String staticPageSeparator = "<hr>"; //---page---

        private static String getPageSeparator( String content ) {
            if (content.IndexOf( staticPageSeparator ) > 20) return staticPageSeparator;
            if (content.IndexOf( staticPageSeparator.ToUpper() ) > 20) return staticPageSeparator.ToUpper(); //¾ÀÕýIEµÄbug
            if (content.IndexOf( "<hr />" ) > 20) return "<hr />";
            return null;
        }

        private String getPagedContent( ContentPost post, String pageSeparator ) {
            String content;
            string[] ss = { pageSeparator };
            string[] arrContent = post.Content.Split( ss, StringSplitOptions.None );

            int currentPage = ctx.GetInt( "cp" );
            int pidx = currentPage - 1;
            if (pidx < 0) pidx = 0;

            content = arrContent[pidx];
            content = strUtil.CloseHtml( content );
            content += ObjectPage.GetPageBarByLink( alink.ToAppData( post ), arrContent.Length, pidx + 1 );
            return content;
        }

        private void bindSummary( ContentPost post ) {
            IBlock summaryBlock = getBlock( "summary" );
            if (strUtil.HasText( post.Summary )) {
                summaryBlock.Set( "post.Summary", post.Summary );
                summaryBlock.Next();
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




        private String addAttachment( ContentPost data, String content ) {

            if (data.Attachments <= 0) return content;

            List<ContentAttachment> attachList = attachmentService.GetAttachmentsByPost( data.Id );

            StringBuilder sb = new StringBuilder();
            String created = attachList[0].Created.ToString();
            sb.Append( "<div class=\"hr\"></div><div class=\"attachmentTitleWrap\"><div class=\"attachmentTitle\">" + alang( "attachment" ) + " <span class=\"note\">(" + created + ")</span> " );
            sb.Append( "</div></div><ul class=\"attachmentList\">" );

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
                    sb.AppendFormat( "<img src=\"{1}\" /><a href=\"{0}\" target=\"_blank\">" + alang( "hitDownload" ) + "</a></div>", to( new ContentAttachmentController().Show, attachment.Id ) + "?id=" + attachment.Guid, strUtil.Join( sys.Path.Img, "/s/download.png" ) );
                    sb.Append( "</li>" );
                }
            }
            sb.Append( "</ul>" );

            content = string.Format( "<div>{0}</div><div id=\"attachmentPanel\">{1}</div>", content, sb.ToString() );

            return content;
        }

        private Boolean isImage( ContentAttachment attachment ) {
            return Uploader.IsImage( attachment.Type );
        }


    }
}

