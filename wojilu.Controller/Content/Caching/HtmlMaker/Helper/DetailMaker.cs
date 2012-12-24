using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Context;
using wojilu.Apps.Content.Domain;
using wojilu.Web.Mvc;
using System.IO;

namespace wojilu.Web.Controller.Content.Caching {

    public class DetailMaker : HtmlMakerBase {

        public DetailMaker( MvcContext ctx )
            : base( ctx ) {
        }

        private ContentPost _post;

        protected override string GetDir() {
            DateTime n = _post.Created;
            return PathHelper.Map( string.Format( "/html/{0}/{1}/{2}/", n.Year, n.Month, n.Day ) );
        }

        public void Process( ContentPost post ) {
            _post = post;

            base.CheckDir();

            List<String> pagedUrls = new List<String>(); // 翻页的链接
            String addr = strUtil.Join( siteUrl, alink.ToAppData( post ) );
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
                String addrPaged = strUtil.Join( siteUrl, url );
                String htmlPaged = makeHtml( addrPaged );
                file.Write( getPath( post, PageHelper.GetPageNoByUrl( url ) ), htmlPaged );
            }
        }

        public void Delete( ContentPost post ) {

            _post = post;

            String filePath = getPath( post );
            if (file.Exists( filePath )) file.Delete( filePath );
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
