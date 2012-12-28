/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.IO;

using wojilu.Web.Mvc;
using wojilu.Web.Context;
using wojilu.Apps.Content.Domain;

namespace wojilu.Web.Controller.Content.Htmls {

    public class DetailMaker : HtmlMaker {

        private static readonly ILog logger = LogManager.GetLogger( typeof( DetailMaker ) );

        private ContentPost _post;

        protected override string GetDir() {
            DateTime n = _post.Created;
            return PathHelper.Map( string.Format( "/html/{0}/{1}/{2}/", n.Year, n.Month, n.Day ) );
        }

        public void Process( ContentPost post ) {
            _post = post;

            base.CheckDir();

            List<String> pagedUrls = new List<String>(); // 翻页的链接
            String addr = alink.ToAppData( post );
            String html = makeHtml( addr, pagedUrls );
            file.Write( getPath( post ), html );

            if (pagedUrls.Count > 0) {
                makeDetailPages( post, pagedUrls );
            }
        }

        // 处理需要翻页的详细页
        private void makeDetailPages( ContentPost post, List<String> pagedUrls ) {
            _post = post;
            foreach (String url in pagedUrls) {
                String addrPaged = url;
                String htmlPaged = makeHtml( addrPaged );
                String htmlPath = getPath( post, PageHelper.GetPageNoByUrl( url ) );
                file.Write( htmlPath, htmlPaged );
                logger.Info( "make html done=>" + htmlPath );
            }
        }

        public void Delete( ContentPost post ) {

            _post = post;

            String htmlPath = getPath( post );
            if (file.Exists( htmlPath )) {
                file.Delete( htmlPath );
                logger.Info( "delete html done=>" + htmlPath );
            }
        }

        //------------------------------------------------------------------------------

        private String getPath( ContentPost post ) {
            return Path.Combine( GetDir(), post.Id + ".html" );
        }


        private string getPath( ContentPost post, int page ) {
            return Path.Combine( GetDir(), post.Id + "_" + page + ".html" );
        }


    }
}
