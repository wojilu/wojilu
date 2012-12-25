using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using System.IO;
using wojilu.Web.Context;
using wojilu.Apps.Content.Domain;
using wojilu.Web.Controller.Content.Section;
using wojilu.DI;

namespace wojilu.Web.Controller.Content.Caching.Actions {

    public class PostAddObserver : ActionObserver {

        private static readonly ILog logger = LogManager.GetLogger( typeof( PostAddObserver ) );

        public override void ObserveActions() {

            Admin.Common.PostController post = new wojilu.Web.Controller.Content.Admin.Common.PostController();
            observe( post.Create );

            Admin.Section.TalkController talk = new wojilu.Web.Controller.Content.Admin.Section.TalkController();
            observe( talk.Create );

            Admin.Section.TextController txt = new wojilu.Web.Controller.Content.Admin.Section.TextController();
            observe( txt.Create );

            Admin.Section.VideoController video = new wojilu.Web.Controller.Content.Admin.Section.VideoController();
            observe( video.Create );

            Admin.Section.ImgController img = new wojilu.Web.Controller.Content.Admin.Section.ImgController();
            observe( img.CreateListInfo );

            Admin.Common.PollController poll = new wojilu.Web.Controller.Content.Admin.Common.PollController();
            observe( poll.Create );
        }

        public override void AfterAction( MvcContext ctx ) {

            // 1）文章添加之后，app首页和侧边栏都要更新
            new HomeMaker( ctx ).Process( ctx.app.Id );
            logger.Info( "PostAddObserver make app home" );

            new SidebarMaker( ctx ).Process( ctx.app.Id );

            // 2）新创建的文章，需要通过 ctx 传递
            ContentPost post = HtmlHelper.GetPostFromContext( ctx );
            if (post != null) {
                new DetailMaker( ctx ).Process( post );
            }

            // 3）相关列表页也要更新
            new ListMaker( ctx ).Process( post );

            // 4) 最近列表页处理
            new RecentMaker( ctx ).ProcessCache( ctx.app.Id );
        }


    }

}
