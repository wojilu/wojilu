/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

using wojilu.Web.Mvc;
using wojilu.Web.Context;

using wojilu.Apps.Content.Domain;
using wojilu.Apps.Content.Interface;
using wojilu.Apps.Content.Service;

namespace wojilu.Web.Controller.Content.Caching.Actions {

    public class PostAddObserver : ActionObserver {

        private static readonly ILog logger = LogManager.GetLogger( typeof( PostAddObserver ) );

        public IContentPostService postService { get; set; }

        public PostAddObserver() {
            postService = new ContentPostService();
        }


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
            HtmlMaker.GetHome().Process( ctx.app.Id );
            logger.Info( "PostAddObserver make app home" );


            // 2）新创建的文章，需要通过 ctx 传递
            ContentPost post = HtmlHelper.GetPostFromContext( ctx );
            if (post != null) {
                DetailMaker mk = HtmlMaker.GetDetail();
                mk.Process( post );
                ContentPost prev = postService.GetPrevPost( post );
                if (prev != null) {
                    mk.Process( prev );
                }
            }

            // 3) 其他生成工作放到队列中
            JobManager.PostAdd( post );
        }


    }

}
