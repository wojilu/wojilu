/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using wojilu.Web.Context;
using wojilu.Web.Mvc;

using wojilu.Apps.Content.Interface;
using wojilu.Apps.Content.Service;
using wojilu.Apps.Content.Domain;
using wojilu.Members.Sites.Domain;

namespace wojilu.Web.Controller.Content.Htmls {

    public class RecentMaker : HtmlMaker {

        public IContentPostService postService { get; set; }

        public RecentMaker( ){
            postService = new ContentPostService();
        }

        /// <summary>
        /// 生成某app的最近文章列表的所有页面
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public int ProcessAll( int appId, int recordCount ) {

            CheckDir();

            String cpLink = Link.To( Site.Instance, new PostController().Recent, appId );
            String caLink = Link.To( Site.Instance, new PostController().RecentArchive, appId );


            int pageSize = ContentSetting.ListRecentPageSize;

            return makeHtmlLoopAll( recordCount, appId, cpLink, caLink, pageSize );
        }

        /// <summary>
        /// 生成某app的最近文章列表的缓存页，不包括存档页。
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        public int ProcessCache( int appId ) {

            CheckDir();

            String cpLink = Link.To( Site.Instance, new PostController().Recent, appId );
            String caLink = Link.To( Site.Instance, new PostController().RecentArchive, appId );

            int pageSize = ContentSetting.ListRecentPageSize;

            int recentCount = postService.CountByApp( appId );

            return makeHtmlLoopCache( recentCount, appId, cpLink, caLink, pageSize );
        }

        protected override string GetDir() {
            return PathHelper.Map( "/html/recent/" );
        }



    }

}
