using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Context;

namespace wojilu.Web.Mvc.Processors {

    internal class UrlRewriteProcessor : ProcessorBase {

        public override void Process( ProcessContext context ) {

            MvcContext ctx = context.ctx;
            MvcEventPublisher.Instance.BeginUrlRewrite( ctx );
            if (ctx.utils.isSkipCurrentProcessor()) return;

            foreach (KeyValuePair<String, String> kv in LinkMap.GetShortUrlMap()) {

                if (strUtil.IsNullOrEmpty( kv.Value )) continue;

                String sPath = strUtil.Join( "", kv.Key );
                sPath = strUtil.Append( sPath, MvcConfig.Instance.UrlExt );

                if (ctx.url.PathAndQueryWithouApp.Equals( sPath )) {

                    if (kv.Value.StartsWith( "http:" )) {
                        ctx.web.Redirect( kv.Value );
                        ctx.utils.end();
                    }
                    else {
                        String newUrl = strUtil.Join( ctx.url.SiteAndAppPath, kv.Value );
                        ctx.setUrl( newUrl );
                    }

                }

            }


        }

    }
}
