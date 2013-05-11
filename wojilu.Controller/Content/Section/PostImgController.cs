using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Common.AppBase.Interface;
using wojilu.Web.Mvc.Attr;
using wojilu.Apps.Content.Domain;
using wojilu.Apps.Content.Interface;
using wojilu.Apps.Content.Service;
using wojilu.Apps.Content.Enum;
using wojilu.Common.AppBase;

namespace wojilu.Web.Controller.Content.Section {

    [App( typeof( ContentApp ) )]
    public class PostImgController : ControllerBase, IPageSection {

        public IContentPostService postService { get; set; }
        public IContentImgService imgService { get; set; }

        public PostImgController() {
            postService = new ContentPostService();
            imgService = new ContentImgService();
        }


        public void SectionShow( int sectionId ) {

            int postcat = PostCategory.Post;
            int imgcat = PostCategory.Img;
            int imgPostCat = PostCategory.ImgPost;


            List<ContentPost> posts = postService.GetTopBySectionAndCategory( sectionId, postcat );
            ContentPost img = imgService.GetTopImg( sectionId, imgPostCat, ctx.app.Id );
            List<ContentPost> imgs = this.imgService.GetByCategory( sectionId, imgcat, ctx.app.Id, 4 );

            bindPosts( posts );
            bindTopImg( img );
            bindImgs( imgs );

        }


        private void bindPosts( List<ContentPost> posts ) {

            IBlock block = getBlock( "list" );
            foreach (ContentPost post in posts) {

                block.Set( "post.TitleCss", post.Style );
                block.Set( "post.TitleFull", post.Title );
                block.Set( "post.Url", alink.ToAppData( post, ctx ) );


                if (strUtil.HasText( post.TitleHome ))
                    block.Set( "post.Title", post.TitleHome );
                else
                    block.Set( "post.Title", post.Title );


                block.Set( "post.Created", post.Created.ToShortTimeString() );

                block.Bind( "post", post );

                block.Next();
            }
        }

        private void bindTopImg( ContentPost img ) {
            IBlock imgBlock = getBlock( "img" );
            if (img != null) {
                imgBlock.Set( "ipost.ImgUrl", img.GetImgThumb() );
                imgBlock.Set( "ipost.TitleCss", img.Style );
                imgBlock.Set( "ipost.TitleFull", img.Title );

                if (strUtil.HasText( img.TitleHome ))
                    imgBlock.Set( "ipost.Title", img.TitleHome );
                else
                    imgBlock.Set( "ipost.Title", img.Title );

                String content = strUtil.HasText( img.Summary ) ? img.Summary : strUtil.ParseHtml( img.Content, 100 );
                imgBlock.Set( "ipost.Content", content );
                imgBlock.Set( "ipost.Width", img.Width );
                imgBlock.Set( "ipost.Height", img.Height );

                imgBlock.Set( "ipost.Url", alink.ToAppData( img, ctx ) );


                imgBlock.Next();
            }
        }

        private void bindImgs( List<ContentPost> imgs ) {

            IBlock imgBlock = getBlock( "imgs" );
            foreach (ContentPost img in imgs) {
                imgBlock.Set( "img.TitleFull", img.Title );
                imgBlock.Set( "img.TitleCss", img.Style );

                if (strUtil.HasText( img.TitleHome ))
                    imgBlock.Set( "img.Title", img.TitleHome );
                else
                    imgBlock.Set( "img.Title", img.Title );


                imgBlock.Set( "img.Thumb", img.GetImgThumb() );

                imgBlock.Set( "img.Url", alink.ToAppData( img, ctx ) );

                imgBlock.Bind( "img", img );


                imgBlock.Next();
            }

        }

        public void List( int sectionId ) {
            run( new ListController().List, sectionId );
        }

        public void Show( int id ) {
            run( new ListController().Show, id );
        }

    }

}
