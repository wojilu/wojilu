using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Apps.Content.Domain;
using wojilu.Web.Context;
using System.IO;
using wojilu.Web.Mvc;

namespace wojilu.Web.Controller.Content.Caching {

    public class RecentMaker : HtmlMakerBase {

        public RecentMaker( MvcContext ctx )
            : base( ctx ) {
        }

        /// <summary>
        /// 生成某app的最近文章列表
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public int Process( int appId, int recordCount ) {

            CheckDir();

            String cpLink = _ctx.link.To( new PostController().Recent );
            String caLink = _ctx.link.To( new PostController().RecentArchive );

            int pageSize = PostController.pageSize;

            return makeHtmlLoop( recordCount, appId, cpLink, caLink, pageSize );
        }

        protected override string GetDir() {
            return PathHelper.Map( "/html/recent/" );
        }



    }

}
