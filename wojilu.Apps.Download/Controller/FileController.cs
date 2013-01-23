using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Apps.Download.Domain;
using wojilu.Web.Controller.Common;
using wojilu.Web.Mvc.Attr;

namespace wojilu.Web.Controller.Download {

    [App( typeof( DownloadApp ) )]
    public class FileController : ControllerBase {

        public void Show( int id ) {

            FileItem f = FileItem.findById( id );

            if (f == null) {
                echo( lang( "exDataNotFound" ) );
                return;
            }

            ctx.Page.Title = f.Title;


            bind( "f", f );
            bindTopList( f.CategoryId );

            set( "location", Location.GetFile( ctx, f ) );
            set( "categories", Location.GetSubCategories( ctx, FileCategory.GetById( f.CategoryId ) ) );

            String url = to( Download, id ) + "?url=";

            String link1 = "";
            String link2 = "";
            String link3 = "";

            if (strUtil.HasText( f.Url )) link1 = string.Format( "<a href=\"{0}\">下载网址1</a>", url + 1 );
            if (strUtil.HasText( f.Url2 )) link2 = string.Format( "<a href=\"{0}\">下载网址2</a>", url + 2 );
            if (strUtil.HasText( f.Url3 )) link3 = string.Format( "<a href=\"{0}\">下载网址3</a>", url + 3 );


            set( "downloadLink1", link1 );
            set( "downloadLink2", link2 );
            set( "downloadLink3", link3 );

            String previewPic = "";
            if (f.HasPreviewPic()) previewPic = string.Format( "<br /><img src=\"{0}\" />", f.PreviewPicMedium );
            set( "previewPic", previewPic );


            bindComment( f );

            FileItem.AddHits( f );

        }

        private void bindTopList( int categoryId ) {
            List<FileItem> currentTops = FileItem.GetTops( categoryId );
            List<FileItem> allTops = FileItem.GetTops();
            bindList( "ctops", "data", currentTops, bindLink );
            bindList( "atops", "data", allTops, bindLink );
        }

        private void bindLink( IBlock block, int id ) {
            block.Set( "data.Link", to( Show, id ) );
        }

        private void bindComment( FileItem post ) {
            set( "commentUrl", getCommentUrl( post ) );
        }

        private string getCommentUrl( FileItem post ) {

            return t2( new wojilu.Web.Controller.Open.CommentController().List )
                + "?url=" + alink.ToAppData( post, ctx )
                + "&dataType=" + typeof( FileItem ).FullName
                + "&dataTitle=" + post.Title
                + "&dataUserId=" + post.Creator.Id
                + "&dataId=" + post.Id;
        }


        public void Download( int id ) {
            FileItem f = FileItem.findById( id );
            FileItem.AddDownloads( f );

            int urlno = ctx.GetInt( "url" );
            if (urlno == 1 && strUtil.HasText( f.Url )) {
                redirectUrl( f.Url );
            }
            else if (urlno == 2 && strUtil.HasText( f.Url2 )) {
                redirectUrl( f.Url2 );
            }
            else if (urlno == 3 && strUtil.HasText( f.Url3 )) {
                redirectUrl( f.Url3 );
            }

            echoRedirect( "网址不存在", to( Show, id ) );

        }


    }


}
