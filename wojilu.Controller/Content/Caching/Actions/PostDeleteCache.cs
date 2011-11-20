using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using System.IO;
using wojilu.Web.Context;
using wojilu.Apps.Content.Domain;

namespace wojilu.Web.Controller.Content.Caching.Actions {

    public class PostDeleteCache : ActionCache {


        public override string GetCacheKey( Context.MvcContext ctx, string actionName ) {
            return null;
        }

        public override void ObserveActions() {

            Admin.PostController post = new wojilu.Web.Controller.Content.Admin.PostController();
            observe( post.Delete );
            observe( post.DeleteTrue );

            //---------------------------------------------------------

            Admin.Section.ListController list = new wojilu.Web.Controller.Content.Admin.Section.ListController();
            observe( list.Delete );

            Admin.Section.TalkController talk = new wojilu.Web.Controller.Content.Admin.Section.TalkController();
            observe( talk.Delete );

            Admin.Section.TextController txt = new wojilu.Web.Controller.Content.Admin.Section.TextController();
            observe( txt.Delete );

            Admin.Section.VideoController video = new wojilu.Web.Controller.Content.Admin.Section.VideoController();
            observe( video.Delete );

            Admin.Section.VideoShowController vshow = new wojilu.Web.Controller.Content.Admin.Section.VideoShowController();
            observe( vshow.Delete );

            Admin.Section.ImgController img = new wojilu.Web.Controller.Content.Admin.Section.ImgController();
            observe( img.Delete );

            Admin.Section.PollController poll = new wojilu.Web.Controller.Content.Admin.Section.PollController();
            observe( poll.Delete );
        }

        public override void UpdateCache( Context.MvcContext ctx ) {

            deleteHtml( ctx );
        }

        // 删除静态 html 页面
        private static void deleteHtml( wojilu.Web.Context.MvcContext ctx ) {

            ContentPost post = ctx.GetItem( "_currentContentPost" ) as ContentPost;
            if (post == null) return;

            String filePath = HtmlHelper.GetPostPath( post );
            if (file.Exists( filePath )) {
                file.Delete( filePath );
            }
        }



    }

}
