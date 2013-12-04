using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Apps.Download.Domain;
using wojilu.Members.Users.Domain;
using wojilu.Web.Utils;
using System.Web;
using wojilu.Web.Controller.Admin;

namespace wojilu.Web.Controller.Download.Admin {

    [App( typeof( DownloadApp ) )]
    public class FileController : ControllerBase {

        public virtual void List() {

            set( "addLink", to( Add ) );
            set( "lnkCateShow", to( new Admin.SubCategoryController().Files ) );

            DataPage<FileItem> pages = FileItem.GetPage( ctx.app.Id );
            bindList( "list", "data", pages.Results, bindLink );
            set( "page", pages.PageBar );
        }

        public virtual void Category( long id ) {

            FileCategory cat = FileCategory.GetById( id );
            if (cat.IsThumbView == 1) {
                view( "ThumbList" );
            }
            else {
                view( "List" );
            }
            set( "addLink", to( Add ) );
            set( "lnkCateShow", to( new Admin.SubCategoryController().Files ) );

            DataPage<FileItem> pages = FileItem.GetPage( ctx.app.Id, id );
            bindList( "list", "data", pages.Results, bindLink );
            set( "page", pages.PageBar );
        }

        private void bindLink( IBlock block, long id ) {
            block.Set( "data.LinkEdit", to( Edit, id ) );
            block.Set( "data.LinkDelete", to( Delete, id ) );
            block.Set( "data.PreviewPicLink", to( new UploadController().PreviewPic, id ) );
        }

        public virtual void Add() {

            target( Create );

            dropList( "categoryId", getRootList(), "Name=Id", "" );
            dropList( "fileItem.LicenseTypeId", LicenseType.GetAll(), "Name=Id", "" );
            dropList( "fileItem.Lang", FileLang.GetAll(), "Name=Name", "" );
            set( "subCategoriesJson", FileCategory.GetSubCatsJson() );
            checkboxList( "fileItem.PlatformIds", Platform.GetAll(), "Name=Id", "" );

            set( "authInfo", AdminSecurityUtils.GetAuthCookieJson( ctx ) );
            set( "uploadLink", to( SaveUpload ) );
            set( "jsPath", sys.Path.DiskJs );

        }

        private List<FileCategory> getRootList() {
            List<FileCategory> list = FileCategory.GetRootList();
            FileCategory f = new FileCategory {
                Id = 0, Name = lang( "plsSelect" )
            };
            list.Insert( 0, f );
            return list;
        }


        [HttpPost]
        public virtual void SaveUpload() {


            Result result = Uploader.SaveFile( ctx.GetFileSingle() );

            if (result.HasErrors) {
                echoError( result );
                return;
            }

            String fileName = result.Info.ToString();
            String fileUrl = strUtil.Join( sys.Path.Photo, fileName ); // 获取文件完整路径
            Dictionary<String, String> dic = new Dictionary<String, String>();
            dic.Add( "IsValid", "true" );
            dic.Add( "FileUrl", fileUrl );

            echoJson( dic );
        }

        [HttpPost, DbTransaction]
        public virtual void Create() {

            FileItem fi = ctx.PostValue<FileItem>();
            if (ctx.HasErrors) {
                echoError();
                return;
            }

            if (fi.Rank > 5) fi.Rank = 0;

            fi.OwnerId = ctx.owner.Id;
            fi.OwnerType = ctx.owner.obj.GetType().FullName;
            fi.OwnerUrl = ctx.owner.obj.Url;

            fi.AppId = ctx.app.Id;
            fi.Creator = (User)ctx.viewer.obj;
            fi.CreatorUrl = ctx.viewer.obj.Url;

            fi.Ip = ctx.Ip;

            FileItem.CreateFile( fi );

            redirect( List );
        }

        public virtual void Edit( long id ) {

            target( Update, id );

            FileItem f = FileItem.findById( id );
            bind( f );

            dropList( "categoryId", getRootList(), "Name=Id", f.ParentCategoryId );
            dropList( "fileItem.LicenseTypeId", LicenseType.GetAll(), "Name=Id", f.LicenseTypeId );
            dropList( "fileItem.Lang", FileLang.GetAll(), "Name=Name", f.Lang );

            set( "subCategoriesJson", FileCategory.GetSubCatsJson() );
            checkboxList( "fileItem.PlatformIds", Platform.GetAll(), "Name=Id", f.PlatformIds );

            set( "fileItem.Description", f.Description );

            set( "authInfo", AdminSecurityUtils.GetAuthCookieJson( ctx ) );
            set( "uploadLink", to( SaveUpload ) );
            set( "jsPath", sys.Path.DiskJs );

        }

        [HttpPost, DbTransaction]
        public virtual void Update( long id ) {

            FileItem f = FileItem.findById( id );
            f = ctx.PostValue( f ) as FileItem;
            if (f.Rank > 5) f.Rank = 0;
            f.update();

            redirect( List );
        }

        [HttpDelete, DbTransaction]
        public virtual void Delete( long id ) {
            FileItem f = FileItem.findById( id );
            FileItem.DeleteFile( f );
            redirect( List );
        }

    }

}
