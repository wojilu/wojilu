/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Web.Controller.Security;
using wojilu.Common.AppBase;

using wojilu.Members.Interface;
using wojilu.Members.Sites.Domain;
using wojilu.Members.Sites.Service;

using wojilu.Apps.Photo.Domain;
using wojilu.Apps.Photo.Interface;
using wojilu.Apps.Photo.Service;
using wojilu.Members.Users.Domain;

namespace wojilu.Web.Controller.Admin.Apps.Photo {

    [App( typeof( PhotoApp ) )]
    public class PostAdminController : ControllerBase {

        public ISysPhotoService photoService { get; set; }
        public IPhotoPostService postService { get; set; }
        public IPickedService pickedService { get; set; }
        public IPhotoSysCategoryService categoryService { get; set; }
        public IAdminLogService<SiteLog> logService { get; set; }

        public PostAdminController() {
            photoService = new SysPhotoService();
            postService = new PhotoPostService();
            pickedService = new PickedService();
            categoryService = new PhotoSysCategoryService();
            logService = new SiteLogService();
        }

        public void Picked() {
            target( Admin );
            DataPage<PhotoPost> list = pickedService.GetAll();
            bindList( "list", "photo", list.Results, bindLink );
            set( "page", list.PageBar );
        }

        public void Trash() {
            target( Admin );
            DataPage<PhotoPost> list = photoService.GetSysPostTrashPage( MainController.pageSize );
            bindList( "list", "photo", list.Results, bindLink );
            set( "page", list.PageBar );
        }


        private void bindLink( IBlock tpl, String lbl, object obj ) {
            PhotoPost data = obj as PhotoPost;

            String cssClass = getStatus( data );
            tpl.Set( "photo.PickedClass", cssClass );

            tpl.Set( "photo.LinkShow", alink.ToAppData( data ) );
        }

        private String getStatus( PhotoPost post ) {

            if (pickedService.IsPicked( post )) return "picked";
            return "";
        }



        [HttpPost, DbTransaction]
        public void Admin() {

            String ids = ctx.Post( "choice" );
            String cmd = ctx.Post( "action" );
            int categoryId = ctx.PostInt( "categoryId" );

            if (strUtil.IsNullOrEmpty( cmd ) || cvt.IsIdListValid( ids ) == false) {
                content( lang( "exCmd" ) );
                return;
            }

            String condition = string.Format( "Id in ({0}) ", ids );


            if ("unpick".Equals( cmd )) {
                pickedService.UnPickPost( ids );
                log( SiteLogString.UnPickPhotoPost(), ids );

                echoAjaxOk();
            }

            else if ("deleteTrue".Equals( cmd )) {
                photoService.SystemDeleteListTrue( ids );
                log( SiteLogString.DeleteSysPhotoPostTrue(), ids );
                echoAjaxOk();
            }
            else if ("unDelete".Equals( cmd )) {
                photoService.SystemUnDeleteList( ids );
                log( SiteLogString.UnDeleteSysPhotoPost(), ids );
                echoAjaxOk();
            }

            else
                content( lang( "exCmd" ) );
        }


        private void log( String msg, String ids ) {
            String dataInfo = "{Ids:[" + ids + "']";
            logService.Add( (User)ctx.viewer.obj, msg, dataInfo, typeof( PhotoPost ).FullName, ctx.Ip );
        }

        private void log( String msg, PhotoPost post ) {
            String dataInfo = "{Id:" + post.Id + ", Title:'" + post.Title + "'}";
            logService.Add( (User)ctx.viewer.obj, msg, dataInfo, typeof( PhotoPost ).FullName, ctx.Ip );
        }
    }
}
