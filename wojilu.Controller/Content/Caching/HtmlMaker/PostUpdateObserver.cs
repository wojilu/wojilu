using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Web.Context;
using wojilu.Apps.Content.Interface;
using wojilu.Apps.Content.Domain;
using wojilu.Apps.Content.Service;

namespace wojilu.Web.Controller.Content.Caching.Actions {

    public class PostUpdateObserver : ActionObserver {

        public IContentImgService imgService { get; set; }
        public IContentPostService postService { get; set; }

        public PostUpdateObserver() {
            postService = new ContentPostService();
            imgService = new ContentImgService();
        }


        private static readonly ILog logger = LogManager.GetLogger( typeof( PostUpdateObserver ) );

        public override void ObserveActions() {

            Admin.Common.PostController post = new wojilu.Web.Controller.Content.Admin.Common.PostController();

            observe( post.Restore );
            observe( post.Update );
            observe( post.UpdateTitleStyle );

            Admin.Section.TalkController talk = new wojilu.Web.Controller.Content.Admin.Section.TalkController();
            observe( talk.Update );

            Admin.Section.TextController txt = new wojilu.Web.Controller.Content.Admin.Section.TextController();
            observe( txt.Update );

            Admin.Section.VideoController video = new wojilu.Web.Controller.Content.Admin.Section.VideoController();
            observe( video.Update );

            Admin.Section.ImgController img = new wojilu.Web.Controller.Content.Admin.Section.ImgController();
            observe( img.CreateImgList );
            observe( img.UpdateListInfo );
            observe( img.SetLogo );
            observe( img.DeleteImg );

            Admin.Common.PollController poll = new wojilu.Web.Controller.Content.Admin.Common.PollController();
            observe( poll.Update );
        }

        private ContentPost _contentPost;

        public override bool BeforeAction( MvcContext ctx ) {

            // 被删除的对象，需要在 action 之前处理
            if (ctx.route.action == "DeleteImg") {
                ContentImg img = imgService.GetImgById( ctx.route.id );
                _contentPost = img.Post;
            }

            return true;
        }

        public override void AfterAction( MvcContext ctx ) {

            // 1）文章更新之后，比如标题被修改，那么app首页和侧边栏都要更新
            new HomeMaker( ctx ).Process( ctx.app.Id );
            new SidebarMaker( ctx ).Process( ctx.app.Id );

            // 2）详细页设置
            ContentPost post;
            if (ctx.route.action == "SetLogo") { // 图片设置封面
                ContentImg img = imgService.GetImgById( ctx.route.id );
                post = img.Post;
                new DetailMaker( ctx ).Process( post );
            }
            else if (ctx.route.action == "DeleteImg") { // 删除图片
                post = _contentPost;
                new DetailMaker( ctx ).Process( post );
            }
            else {
                // 其他处理
                post = postService.GetById( ctx.route.id, ctx.owner.Id );
                new DetailMaker( ctx ).Process( post );
            }

            // 3）TODO 列表页处理
            new ListMaker( ctx ).Process( post );

        }

    }

}
