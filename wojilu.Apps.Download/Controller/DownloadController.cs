using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Apps.Download.Domain;
using wojilu.Web.Controller.Common;

namespace wojilu.Web.Controller.Download {

    [App( typeof( DownloadApp ) )]
    public class DownloadController : ControllerBase {

        public void Index() {

            ctx.Page.Title = "资源下载";

            load( "sidebar", sidebar );

            String key = strUtil.SqlClean( ctx.Get( "q" ), 10 );
            set( "key", key );

            DataPage<FileItem> list = FileItem.GetPage( ctx.app.Id, key );
            ctx.SetItem( "list", list );
            load( "list", new CategoryController().List );
        }

        [NonVisit]
        public void sidebar() {

            List<FileCategory> cats = FileCategory.GetRootList();
            bindCats( cats );

        }

        private void bindCats( List<FileCategory> cats ) {
            IBlock block = getBlock( "cat" );
            foreach (FileCategory cat in cats) {

                block.Set( "cat.Name", cat.Name );
                block.Set( "cat.Link", to( new CategoryController().Show, cat.Id ) );

                bindSubCats( block, cat );

                block.Next();
            }
        }

        private void bindSubCats( IBlock block, FileCategory cat ) {
            IBlock subBlock = block.GetBlock( "subcat" );
            List<FileCategory> subcats = FileCategory.GetByParentId( cat.Id );
            foreach (FileCategory subcat in subcats) {

                subBlock.Set( "subcat.Name", subcat.Name );
                subBlock.Set( "subcat.Link", to( new CategoryController().Show, subcat.Id ) );
                subBlock.Next();

            }
        }

    }

}
