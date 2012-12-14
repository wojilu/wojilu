using System;
using System.Collections.Generic;
using System.Text;

using wojilu.Common;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Apps.Content.Domain;
using wojilu.Apps.Content.Interface;
using wojilu.Apps.Content.Service;
using wojilu.Apps.Content.Enum;

namespace wojilu.Web.Controller.Content {

    [App( typeof( ContentApp ) )]
    public class SidebarController : ControllerBase {

        public IContentPostService postService { get; set; }

        public SidebarController() {
            postService = new ContentPostService();
        }

        public void Index() {

            ContentApp app = ctx.app.obj as ContentApp;
            ContentSetting s = app.GetSettingsObj();

            List<ContentPost> posts = postService.GetRankPost( ctx.app.Id, s.RankPosts, PostCategory.Post );
            bindPosts( posts, "list" );

            List<ContentPost> imgs = postService.GetRankPost( ctx.app.Id, s.RankPics, PostCategory.Img );
            bindImgs( imgs, "img" );

            List<ContentPost> videos = postService.GetRankPost( ctx.app.Id, s.RankVideos, PostCategory.Video );
            bindVideos( videos, "video" );

        }


        private void bindPosts( List<ContentPost> posts, String blockName ) {
            IBlock panel = getBlock( blockName + "Panel" );
            if (posts.Count == 0) return;
            IBlock block = panel.GetBlock( blockName );
            foreach (ContentPost post in posts) {

                if (post.PageSection == null) continue;

                block.Set( "post.TitleFull", post.Title );
                block.Set( "post.Title", strUtil.SubString( post.Title, 18 ) );

                block.Set( "post.Link", alink.ToAppData( post, ctx ) );
                block.Next();
            }
            panel.Next();
        }

        private void bindImgs( List<ContentPost> posts, String blockName ) {
            IBlock panel = getBlock( blockName + "Panel" );
            if (posts.Count == 0) return;

            IBlock block = panel.GetBlock( blockName );
            foreach (ContentPost post in posts) {
                if (post.PageSection == null) continue;

                block.Set( "post.Img", post.GetImgThumb() );
                String lnk = alink.ToAppData( post, ctx );

                block.Set( "post.TitleFull", post.Title );
                block.Set( "post.Title", strUtil.SubString( post.Title, 18 ) );

                block.Set( "post.Link", lnk );
                block.Next();
            }
            panel.Next();
        }

        private void bindVideos( List<ContentPost> posts, String blockName ) {
            IBlock panel = getBlock( blockName + "Panel" );
            if (posts.Count == 0) return;

            IBlock block = panel.GetBlock( blockName );
            foreach (ContentPost post in posts) {

                if (post.PageSection == null) continue;

                block.Set( "post.TitleFull", post.Title );
                block.Set( "post.Title", strUtil.SubString( post.Title, 18 ) );

                block.Set( "post.Img", post.GetImgThumb() );

                block.Set( "post.Link", alink.ToAppData( post, ctx ) );
                block.Next();
            }
            panel.Next();
        }

    }

}
