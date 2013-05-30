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
using wojilu.Web.Controller.Content.Caching;
using wojilu.Web.Controller.Content.Htmls;

namespace wojilu.Web.Controller.Content.Section {


    public partial class ListController : ControllerBase, IPageSection {


        private void bindSectionPosts( IList posts ) {
            IBlock block = getBlock( "list" );
            foreach (ContentPost post in posts) {

                BinderUtils.bindPostSingle( block, post, ctx );

                block.Next();
            }
        }


        private void bindPostList( ContentSection section, DataPage<ContentPost> posts, ContentSetting setting ) {
            set( "section.Name", section.Title );
            IBlock block = getBlock( "list" );
            foreach (ContentPost post in posts.Results) {

                BinderUtils.bindPostSingle( block, post, ctx );

                if (setting.ArticleListMode == ArticleListMode.Summary) {
                    block.Set( "post.Summary", post.GetSummary( setting.SummaryLength ) );
                }

                block.Next();
            }
        }


        private void bindDetail( int id, ContentPost post ) {

            String content;

            String pageSeparator = getPageSeparator( post.Content );
            if (pageSeparator == null) {
                content = post.Content;
            }
            else {
                content = getPagedContent( post, pageSeparator );
            }

            if (post.HasImg()) {

                String imgUrl = post.GetImgMedium();
                if (content.IndexOf( imgUrl ) < 0) {

                    content = string.Format( "<div class=\"post-detail-content-pic\"><img src=\"{0}\" /></div>", imgUrl )
                        + content;
                }
            }

            set( "post.Content", content );
        }

        private static readonly String staticPageSeparator = "<hr>"; //---page---

        private static String getPageSeparator( String content ) {
            if (content.IndexOf( staticPageSeparator ) > 20) return staticPageSeparator;
            if (content.IndexOf( staticPageSeparator.ToUpper() ) > 20) return staticPageSeparator.ToUpper(); //纠正IE的bug
            if (content.IndexOf( "<hr />" ) > 20) return "<hr />";
            return null;
        }

        private String getPagedContent( ContentPost post, String pageSeparator ) {
            String content;
            string[] ss = { pageSeparator };
            string[] arrContent = post.Content.Split( ss, StringSplitOptions.None );

            int currentPage = ctx.route.page;

            int pidx = currentPage - 1;
            if (pidx < 0) pidx = 0;

            content = arrContent[pidx];
            content = strUtil.CloseHtml( content );

            Boolean isMakeHtml = HtmlHelper.IsMakeHtml( ctx );
            content += PageHelper.GetSimplePageBar( alink.ToAppData( post, ctx ), currentPage, arrContent.Length, isMakeHtml );

            setPagedUrls( post, arrContent.Length );

            return content;
        }

        // 将需要翻页的 url 放入上下文，
        private void setPagedUrls( ContentPost post, int pageCount ) {
            if (pageCount <= 1) return;

            String lnkPost = alink.ToAppData( post );
            List<String> pagedUrls = new List<String>();
            for (int i = 2; i < pageCount + 1; i++) {
                pagedUrls.Add( PageHelper.AppendNo( lnkPost, i ) );
            }

            ctx.SetItem( "_relativeUrls", pagedUrls );
        }



    }
}

