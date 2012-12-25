using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using wojilu.Apps.Content.Domain;
using wojilu.Web.Context;
using wojilu.Web.Mvc;
using wojilu.Apps.Content.Interface;
using wojilu.Apps.Content.Service;

namespace wojilu.Web.Controller.Content.Caching {

    public class RecentMaker : HtmlMakerBase {

        public IContentPostService postService { get; set; }

        public RecentMaker( MvcContext ctx )
            : base( ctx ) {
            postService = new ContentPostService();
        }

        /// <summary>
        /// 生成某app的最近文章列表的所有页面
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public int ProcessAll( int appId, int recordCount ) {

            CheckDir();

            String cpLink = _ctx.link.To( new PostController().Recent );
            String caLink = _ctx.link.To( new PostController().RecentArchive );

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

            String cpLink = _ctx.link.To( new PostController().Recent );
            String caLink = _ctx.link.To( new PostController().RecentArchive );

            int pageSize = ContentSetting.ListRecentPageSize;

            int recentCount = postService.CountByApp( appId );

            return makeHtmlLoopCache( recentCount, appId, cpLink, caLink, pageSize );
        }

        protected override string GetDir() {
            return PathHelper.Map( "/html/recent/" );
        }



    }

}
