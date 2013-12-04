/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using wojilu.Web.Mvc;

using wojilu.Web.Context;
using wojilu.Apps.Content.Domain;
using wojilu.Apps.Content.Interface;
using wojilu.Apps.Content.Service;
using wojilu.Members.Sites.Domain;

namespace wojilu.Web.Controller.Content.Htmls {

    public class PostDeleteObserver : ActionObserver {

        private static readonly ILog logger = LogManager.GetLogger( typeof( PostDeleteObserver ) );

        public virtual IContentPostService postService { get; set; }

        public PostDeleteObserver() {
            postService = new ContentPostService();
        }

        public override void ObserveActions() {

            Admin.Common.PostController post = new wojilu.Web.Controller.Content.Admin.Common.PostController();
            observe( post.Delete );
            observe( post.DeleteSys );

            Admin.Section.TalkController talk = new wojilu.Web.Controller.Content.Admin.Section.TalkController();
            observe( talk.Delete );

            Admin.Section.TextController txt = new wojilu.Web.Controller.Content.Admin.Section.TextController();
            observe( txt.Delete );

            Admin.Section.VideoController video = new wojilu.Web.Controller.Content.Admin.Section.VideoController();
            observe( video.Delete );

            Admin.Section.ImgController img = new wojilu.Web.Controller.Content.Admin.Section.ImgController();
            observe( img.Delete );

            Admin.Common.PollController poll = new wojilu.Web.Controller.Content.Admin.Common.PollController();
            observe( poll.Delete );
        }

        private ContentPost _contentPost;


        public override bool BeforeAction( MvcContext ctx ) {

            ContentPost post = postService.GetById( ctx.route.id, ctx.owner.Id );
            _contentPost = post;

            return base.BeforeAction( ctx );
        }

        public override void AfterAction( Context.MvcContext ctx ) {

            if (!HtmlHelper.CanHtml( ctx )) return;

            // 1）文章删除之后，app首页更新
            HtmlMaker.GetHome().Process( ctx.app.Id );

            // 2）删除文章详细页
            HtmlMaker.GetDetail().Delete( _contentPost );

            // 3) 其他生成工作放到队列中
            JobManager.PostDelete( _contentPost );


        }


    }

}
