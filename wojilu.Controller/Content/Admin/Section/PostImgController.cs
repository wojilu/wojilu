using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Common.AppBase.Interface;
using wojilu.Common.AppBase;
using wojilu.Web.Mvc.Attr;
using wojilu.Apps.Content.Domain;
using wojilu.Apps.Content.Enum;
using wojilu.Apps.Content.Interface;
using wojilu.Apps.Content.Service;

namespace wojilu.Web.Controller.Content.Admin.Section {

    [App( typeof( ContentApp ) )]
    public class PostImgController : ControllerBase, IPageAdminSection {

        public IContentPostService postService { get; set; }
        public IContentImgService imgService { get; set; }

        public PostImgController() {
            postService = new ContentPostService();
            imgService = new ContentImgService();
        }

        public void AdminSectionShow( int sectionId ) {

            int postcat = PostCategory.Post;
            int imgcat = PostCategory.Img;
            int imgPostCat = PostCategory.ImgPost;

            bindCmds( sectionId, postcat, imgcat, imgPostCat );

            List<ContentPost> posts = postService.GetTopBySectionAndCategory( sectionId, postcat );
            ContentPost img = imgService.GetTopImg( sectionId, imgPostCat, ctx.app.Id );
            List<ContentPost> imgs = this.imgService.GetByCategory( sectionId, imgcat, ctx.app.Id, 4 );

            bindPosts( posts );
            bindTopImg( img );
            bindImgs( imgs );
        }

        public List<ContentPost> GetSectionPosts( int sectionId ) {

            int postcat = PostCategory.Post;
            int imgcat = PostCategory.Img;
            int imgPostCat = PostCategory.ImgPost;

            List<ContentPost> posts = postService.GetTopBySectionAndCategory( sectionId, postcat );
            ContentPost img = imgService.GetTopImg( sectionId, imgPostCat, ctx.app.Id );
            List<ContentPost> imgs = this.imgService.GetByCategory( sectionId, imgcat, ctx.app.Id, 4 );

            List<ContentPost> list = new List<ContentPost>();
            list.AddRange( posts );
            list.Add( img );
            list.AddRange( imgs );
            return list;
        }

        public String GetEditLink( int postId ) {
            return to( new Common.PostController().Edit, postId );
        }

        public String GetSectionIcon( int sectionId ) {
            return "";
        }

        private void bindCmds( int sectionId, int postcat, int imgcat, int imgPostCat ) {

            set( "postAddUrl", to( new Common.PostController().Add, sectionId ) + "?categoryId=" + postcat );
            set( "postListUrl", to( new ListController().AdminList, sectionId ) + "?categoryId=" + postcat );
            set( "imgAddUrl", to( new Common.PostController().Add, sectionId ) + "?categoryId=" + imgcat );
            set( "imgListUrl", to( new ListController().AdminList, sectionId ) + "?categoryId=" + imgcat );
            set( "imgPostAddUrl", to( new Common.PostController().Add, sectionId ) + "?categoryId=" + imgPostCat );
            set( "imgPostListUrl", to( new ListController().AdminList, sectionId ) + "?categoryId=" + imgPostCat );
        }

        private void bindPosts( List<ContentPost> posts ) {

            IBlock block = getBlock( "list" );
            foreach (ContentPost post in posts) {

                block.Set( "post.TitleCss", post.Style );
                block.Set( "post.TitleFull", post.Title );

                if (strUtil.HasText( post.TitleHome ))
                    block.Set( "post.Title", post.TitleHome );
                else
                    block.Set( "post.Title", post.Title );


                block.Set( "post.Url", to( new Common.PostController().Edit, post.Id ) );
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

                String content = strUtil.HasText( img.Summary ) ? img.Summary : strUtil.ParseHtml( img.Content, 50 );
                imgBlock.Set( "ipost.Content", content );
                imgBlock.Set( "ipost.Width", img.Width );
                imgBlock.Set( "ipost.Height", img.Height );

                imgBlock.Set( "ipost.EditLink", to( new Common.PostController().EditImg, img.Id ) );
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
                imgBlock.Set( "img.Url", to( new Common.PostController().EditImg, img.Id ) );
                imgBlock.Bind( "img", img );
                imgBlock.Next();
            }

        }

        public List<IPageSettingLink> GetSettingLink( int sectionId ) {

            List<IPageSettingLink> links = new List<IPageSettingLink>();

            PageSettingLink lnk = new PageSettingLink();
            lnk.Name = lang( "editSetting" );
            lnk.Url = to( new SectionSettingController().Edit, sectionId );
            links.Add( lnk );

            PageSettingLink lnktmp = new PageSettingLink();
            lnktmp.Name = alang( "editTemplate" );
            lnktmp.Url = to( new TemplateCustomController().Edit, sectionId );
            links.Add( lnktmp );


            return links;
        }

    }

}
