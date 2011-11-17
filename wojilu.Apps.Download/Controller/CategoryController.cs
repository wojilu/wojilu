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

            WebUtils.pageTitle( this, c.Name );


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

        //private void bindSubCategories( FileCategory c ) {
        //    IBlock subblock = getBlock( "sub" );

        //    int rootId = c.Id;
        //    if (c.ParentId > 0) rootId = c.ParentId;

        //    IBlock block = subblock.GetBlock( "subcats" );
        //    List<FileCategory> subs = FileCategory.GetByParentId( rootId );
        //    foreach (FileCategory sub in subs) {
        //        block.Set( "cat.Name", sub.Name );
        //        block.Set( "cat.Link", to( Show, sub.Id ) );
        //        block.Next();
        //    }
        //    subblock.Next();

        //}

        private void bindLink( IBlock block, int id ) {
            block.Set( "f.ShowLink", to( new FileController().Show, id ) );
        }

    }
}
