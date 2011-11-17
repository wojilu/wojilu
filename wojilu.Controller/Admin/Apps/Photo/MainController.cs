/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Apps.Photo.Domain;
using wojilu.Apps.Photo.Service;
using wojilu.Common.AppBase;
using wojilu.Apps.Photo.Interface;
using wojilu.Members.Sites.Service;
using wojilu.Members.Sites.Interface;
using wojilu.Web.Controller.Security;
using wojilu.Members.Interface;
using wojilu.Members.Sites.Domain;

namespace wojilu.Web.Controller.Admin.Apps.Photo {

    [App( typeof( PhotoApp ) )]
    public partial class MainController : ControllerBase {

        public ISysPhotoService photoService { get; set; }
        public IPhotoPostService postService { get; set; }
        public IPickedService pickedService { get; set; }
        public IPhotoSysCategoryService categoryService { get; set; }
        public IAdminLogService<SiteLog> logService { get; set; }

        public MainController() {
            photoService = new SysPhotoService();
            postService = new PhotoPostService();
            pickedService = new PickedService();
            categoryService = new PhotoSysCategoryService();
            logService = new SiteLogService();
        }

        private static readonly int pageSize = 36;

        public void Index( int id ) {

            DataPage<PhotoPost> list = photoService.GetSysPostPage( id, pageSize );
            bindList( "list", "photo", list.Results, bindLink );
            set( "page", list.PageBar );

            //base.AdminSectionShow();

            setCategoryDropList();

            target( Admin );
        }

        public void Picked() {
            target( Admin );
            DataPage<PhotoPost> list = pickedService.GetAll();
            bindList( "list", "photo", list.Results, bindLink );
            set( "page", list.PageBar );
        }

        public void Trash() {
            target( Admin );
            DataPage<PhotoPost> list = photoService.GetSysPostTrashPage( pageSize );
            bindList( "list", "photo", list.Results, bindLink );
            set( "page", list.PageBar );
        }


        [HttpPost, DbTransaction]
        public void Admin() {

            String ids = ctx.Post( "choice" );
            String cmd = ctx.Post( "action" );
            int categoryId = ctx.PostInt( "categoryId" );

            if (strUtil.IsNullOrEmpty( cmd ) || cvt.IsIdListValid( ids ) == false) {
                actionContent( lang( "exCmd" ) );
                return;
            }

            String condition = string.Format( "Id in ({0}) ", ids );

            if ("pick".Equals( cmd )) {
                pickedService.PickPost( ids );
                log( SiteLogString.PickPhotoPost(), ids );

                echoAjaxOk();
            }
            else if ("unpick".Equals( cmd )) {
                pickedService.UnPickPost( ids );
                log( SiteLogString.UnPickPhotoPost(), ids );

                echoAjaxOk();
            }
            else if ("delete".Equals( cmd )) {
                PhotoPost.updateBatch( "set SaveStatus=" + SaveStatus.SysDelete, condition );
                log( SiteLogString.DeleteSysPhotoPost(), ids );
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
            else if ("category".Equals( cmd )) {
                if (categoryId <= 0) {
                    actionContent( lang( "exCategoryNotFound" ) );
                    return;
                }
                PhotoPost.updateBatch( "set SysCategoryId=" + categoryId, condition );
                log( SiteLogString.MovePhotoPost(), ids );

                echoAjaxOk();
            }
            else
                actionContent( lang( "exCmd" ) );
        }


        //[HttpDelete, DbTransaction]
        //public void Delete( int id ) {
        //    PhotoPost post = postService.GetById( id );
        //    if (post == null) {
        //        echoTo( lang( "exDataNotFound" ) );
        //        return;
        //    }
        //    photoService.SystemDelete( post );
        //    log( SiteLogString.DeleteSysPhotoPost(), post );

        //    redirect( Index, -1 );
        //}

        //[HttpPut, DbTransaction]
        //public void UnDelete( int id ) {

        //    PhotoPost post = postService.GetById_Admin( id );
        //    if (post == null) {
        //        echoRedirect( lang( "exDataNotFound" ) );
        //        return;
        //    }
        //    photoService.SystemUnDelete( post );
        //    log( SiteLogString.UnDeleteSysPhotoPost(), post );

        //    redirect( Trash );
        //}

    }

}
