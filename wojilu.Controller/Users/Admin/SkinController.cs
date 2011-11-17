/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.IO;
using System.Web;

using wojilu.Web.Utils;
using wojilu.Common.Skins;

using wojilu.Members.Users.Domain;

using wojilu.Apps.Photo.Domain;
using wojilu.Apps.Photo.Interface;
using wojilu.Apps.Photo.Service;

using wojilu.Web.Controller.Common;
using wojilu.Common;

namespace wojilu.Web.Controller.Users.Admin {

    public class SkinController : SkinBaseController {

        public IPhotoPostService postService { get; set; }

        public SkinController() {
            postService = new PhotoPostService();
        }

        public override ISkin objSkin {
            get { return new SpaceSkin(); }
        }

        public override SkinService skinService {
            get {
                SkinService obj = new SkinService();
                obj.SetSkin( this.objSkin );
                return obj;
            }
        }

        public override void Layout() {
            String customLink = wojilu.Web.Mvc.Link.ToMember( ctx.owner.obj ) + "?skin=custom";
            set( "customLink", customLink );

            Boolean isCustom = skinService.IsUserCustom( ctx.owner.obj );
            String customInfo = isCustom ? lang( "userSkinTip" ) : "";
            set( "customInfo", customInfo );
        }

        public override void CheckPermission() {
            Boolean isSkinClose = Component.IsClose( typeof( SkinApp ) );
            if (isSkinClose) {
                echo( "对不起，本功能已经停用" );
            }
        }

        //------------------------------------------------------------------------------------------------
        public void CustomBg() {
            bindLayout();
        }

        private void bindLayout() {
            set( "bgLink", to( CustomBg ) );
            set( "headerLink", to( CustomHeader ) );
            set( "navLink", to( CustomNav ) );
            set( "mainLink", to( CustomMain ) );
            set( "footerLink", to( CustomFooter ) );

            set( "myPicsLink", to( MyPics ) );
            set( "picUrlLink", to( AddPicUrl ) );
            set( "uploadLink", to( UploadForm ) );
            set( "saveUrl", to( SaveBg ) );

            load( "autoSaveScript", script );
        }

        public void SaveBg() {

            String ele = ctx.Post( "ele" );
            String kvItem = ctx.Post( "kv" );

            skinService.CustomBg( ctx.owner.obj, ele, kvItem );
            echoAjaxOk();
        }

        public void CustomHeader() {
            bindLayout();
        }

        public void CustomMain() {
            bindLayout();
        }

        public void CustomFooter() {
            bindLayout();
        }

        public void CustomNav() {
            bindLayout();
        }

        public void script() {
        }

        public void MyPics() {

            DataPage<PhotoPost> list = postService.GetByUser( ctx.viewer.Id, 8 );
            IBlock block = getBlock( "list" );
            foreach (PhotoPost post in list.Results) {
                block.Set( "img.Thumb", post.ImgThumbUrl );
                block.Set( "img.OriginalUrl", post.ImgUrl );
                block.Next();
            }
            set( "page", list.PageBar );
        }


        public void AddPicUrl() {
        }

        public void UploadForm() {
            target( SavePic );
        }

        public void SavePic() {

            HttpFile postedFile = ctx.GetFileSingle();

            Result result = Uploader.SaveImg( postedFile );
            if (result.HasErrors) {
                ctx.errors.Join( result );
                run( UploadForm );
                return;
            }

            PhotoPost post = new PhotoPost();
            post.OwnerId = ctx.owner.Id;
            post.OwnerType = ctx.owner.obj.GetType().FullName;
            post.OwnerUrl = ctx.owner.obj.Url;

            post.Creator = (User)ctx.viewer.obj;
            post.CreatorUrl = ctx.viewer.obj.Url;
            post.DataUrl = result.Info.ToString();
            post.Title = strUtil.CutString( Path.GetFileNameWithoutExtension( postedFile.FileName ), 20 );
            post.Ip = ctx.Ip;
            post.PhotoAlbum = new PhotoAlbum();

            postService.CreatePostTemp( post );

            set( "picThumbUrl", post.ImgThumbUrl );
            set( "picUrl", post.ImgUrl );
        }


    }

}
