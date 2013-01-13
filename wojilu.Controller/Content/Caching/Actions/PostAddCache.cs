using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using System.IO;
using wojilu.Web.Context;
using wojilu.Apps.Content.Domain;
using wojilu.Web.Controller.Content.Section;

namespace wojilu.Web.Controller.Content.Caching.Actions {

    public class PostAddCache : ActionCache {


        public override string GetCacheKey( Context.MvcContext ctx, string actionName ) {
            return null;
        }

        public override void ObserveActions() {

            Admin.Common.PostController post = new wojilu.Web.Controller.Content.Admin.Common.PostController();
            observe( post.Create );
            observe( post.SaveAdmin );
            observe( post.Restore );
            observe( post.Update );
            observe( post.UpdateTitleStyle );

            Admin.Section.TalkController talk = new wojilu.Web.Controller.Content.Admin.Section.TalkController();
            observe( talk.Create );
            observe( talk.Update );

            Admin.Section.TextController txt = new wojilu.Web.Controller.Content.Admin.Section.TextController();
            observe( txt.Create );
            observe( txt.Update );

            Admin.Section.VideoController video = new wojilu.Web.Controller.Content.Admin.Section.VideoController();
            observe( video.Create );
            observe( video.Update );

            Admin.Section.ImgController img = new wojilu.Web.Controller.Content.Admin.Section.ImgController();
            observe( img.CreateListInfo );
            observe( img.CreateImgList );
            observe( img.SetLogo );
            observe( img.UpdateListInfo );
            observe( img.DeleteImg );

            Admin.Common.PollController poll = new wojilu.Web.Controller.Content.Admin.Common.PollController();
            observe( poll.Create );
            observe( poll.Delete );
        }

        public override void UpdateCache( Context.MvcContext ctx ) {

            HtmlHelper.MakeDetailHtml( ctx );


            new HtmlListMaker().MakeHtml( ctx );


            // 频道首页生成在 ContentIndexCache 中监控

        }


    }

}
