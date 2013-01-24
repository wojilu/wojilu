using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Apps.Download.Domain;
using wojilu.Common.AppBase.Interface;
using wojilu.Common.AppBase;

namespace wojilu.Web.Controller.Download.Admin {

    [App( typeof( DownloadApp ) )]
    public class SubCategoryController : ControllerBase {

        public void List() {
            set( "addLink", to( Add, 0 ) );

            List<FileCategory> cats = FileCategory.GetRootList();
            IBlock block = getBlock( "cat" );
            foreach (FileCategory cat in cats) {

                block.Set( "cat.ThumbIcon", cat.ThumbIcon );

                block.Set( "cat.Name", cat.Name );
                block.Set( "cat.Link", to( new CategoryController().Edit, cat.Id ) );
                block.Set( "cat.AddSubLink", to( Add, cat.Id ) );
                block.Set( "cat.SortLink", to( ListSub, cat.Id ) );

                bindSubCats( block, cat );

                block.Next();

            }
        }

        private void bindSubCats( IBlock block, FileCategory cat ) {
            IBlock subBlock = block.GetBlock( "subcat" );
            List<FileCategory> subcats = FileCategory.GetByParentId( cat.Id );
            foreach (FileCategory subcat in subcats) {

                subBlock.Set( "subcat.ThumbIcon", subcat.ThumbIcon );
                subBlock.Set( "subcat.Name", subcat.Name );
                subBlock.Set( "subcat.Link", to( Edit, subcat.Id ) );
                subBlock.Next();
            }
        }


        public void ListSub( int id ) {
            FileCategory cat = FileCategory.GetById( id );
            set( "cat.Name", cat.Name );
            set( "addLink", to( Add, id ) );
            set( "sortAction", to( SaveSort, id ) );

            IBlock block = getBlock( "list" );
            List<FileCategory> subcats = FileCategory.GetByParentId( cat.Id );
            foreach (FileCategory subcat in subcats) {

                block.Set( "data.ThumbIcon", subcat.ThumbIcon );
                block.Set( "data.Id", subcat.Id );
                block.Set( "data.Name", subcat.Name );
                block.Set( "data.LinkEdit", to( Edit, subcat.Id ) );
                block.Set( "data.LinkDelete", to( Delete, subcat.Id ) );
                block.Next();

            }
        }

        //------------------------------------------------------------------------------------------------------

        public void Files() {

            List<FileCategory> cats = FileCategory.GetRootList();
            IBlock block = getBlock( "cat" );
            foreach (FileCategory cat in cats) {

                block.Set( "cat.ThumbIcon", cat.ThumbIcon );
                block.Set( "cat.Name", cat.Name );
                block.Set( "cat.Link", to( new FileController().Category, cat.Id ) );

                bindSubCatsFiles( block, cat );

                block.Next();
            }
        }

        private void bindSubCatsFiles( IBlock block, FileCategory cat ) {
            IBlock subBlock = block.GetBlock( "subcat" );
            List<FileCategory> subcats = FileCategory.GetByParentId( cat.Id );
            foreach (FileCategory subcat in subcats) {

                subBlock.Set( "subcat.ThumbIcon", subcat.ThumbIcon );
                subBlock.Set( "subcat.Name", subcat.Name );
                subBlock.Set( "subcat.DataCount", subcat.DataCount );
                subBlock.Set( "subcat.Link", to( new FileController().Category, subcat.Id ) );
                subBlock.Next();
            }
        }


        [HttpPost]
        public virtual void SaveSort( int parentId ) {

            int id = ctx.PostInt( "id" );
            String cmd = ctx.Post( "cmd" );

            FileCategory acategory = FileCategory.GetById( id );

            List<FileCategory> list = FileCategory.GetByParentId( parentId );

            if (cmd == "up") {

                new SortUtil<FileCategory>( acategory, list ).MoveUp();
                echoRedirect( "ok" );
            }
            else if (cmd == "down") {

                new SortUtil<FileCategory>( acategory, list ).MoveDown();
                echoRedirect( "ok" );
            }
            else {
                echoError( lang( "exUnknowCmd" ) );
            }

        }

        //------------------------------------------------------------------------------------------------------


        public void Add( int id ) {
            target( Create );

            List<FileCategory> cats = FileCategory.GetRootList();
            dropList( "fileCategory.ParentId", cats, "Name=Id", id );
        }

        [HttpPost]
        public void Create() {

            FileCategory cat = ctx.PostValue<FileCategory>();

            if (strUtil.IsNullOrEmpty( cat.Name )) {
                echoError( "请填写名称" );
                return;
            }


            cat.IsThumbView = ctx.PostIsCheck( "fileCategory.IsThumbView" );

            cat.insert();
            echoToParentPart( lang( "opok" ) );
        }


        public void Edit( int id ) {

            FileCategory cat = FileCategory.GetById( id );
            bind( cat );

            List<FileCategory> cats = FileCategory.GetRootList();
            dropList( "fileCategory.ParentId", cats, "Name=Id", cat.ParentId );
            target( Update, id );

            String chkstr = "";
            if (cat.IsThumbView == 1) chkstr = "checked=\"checked\"";
            set( "checked", chkstr );
        }

        [HttpPost]
        public void Update( int id ) {
            FileCategory c = FileCategory.GetById( id );

            FileCategory cat = ctx.PostValue( c ) as FileCategory;

            if (strUtil.IsNullOrEmpty( cat.Name )) {
                echoError( "请填写名称" );
                return;
            }

            cat.IsThumbView = ctx.PostIsCheck( "fileCategory.IsThumbView" );

            cat.update();
            echoToParentPart( lang( "opok" ) );
        }

        [HttpDelete]
        public void Delete( int id ) {
            FileCategory f = FileCategory.GetById( id );
            if (f != null) {
                f.delete();
                redirect( List );
            }
        }

    }

}
