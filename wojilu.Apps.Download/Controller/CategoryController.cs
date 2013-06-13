using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Apps.Download.Domain;
using wojilu.Web.Controller.Common;

namespace wojilu.Web.Controller.Download {

    [App( typeof( DownloadApp ) )]
    public class CategoryController : ControllerBase {

        public void Show( int id ) {


            FileCategory c = FileCategory.GetById( id );
            if (c == null) {
                echo( lang( "exDataNotFound" ) );
                return;
            }

            ctx.Page.Title = c.Name;


            load( "sidebar", new DownloadController().sidebar );

            set( "location", Location.GetCategory( ctx, c.Id ) );

            set( "categories", Location.GetSubCategories( ctx, c ) );

            DataPage<FileItem> list = FileItem.GetPage( ctx.app.Id, c.Id );
            ctx.SetItem( "list", list );
            ctx.SetItem( "category", c );
            load( "list", List );
        }

        [NonVisit]
        public void List() {
            FileCategory c = ctx.GetItem( "category" ) as FileCategory;
            if (c != null && c.IsThumbView == 1) view( "ThumbList" );

            DataPage<FileItem> list = ctx.GetItem( "list" ) as DataPage<FileItem>;
            bindList( "list", "f", list.Results, bindLink );
            set( "page", list.PageBar );
        }

        private void bindLink( IBlock block, int id ) {
            block.Set( "f.ShowLink", to( new FileController().Show, id ) );
        }

    }
}
