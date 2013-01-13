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

        public static readonly int pageSize = 40;

        public override void Layout() {

            List<PhotoSysCategory> categories = categoryService.GetAll();
            bindList( "categories", "c", categories, bindCatLink );
        }

        private void bindCatLink( IBlock tpl, int id ) {
            tpl.Set( "c.LinkCategory", to( new MainController().Index, id ) );
        }

        public void Index( int id ) {

            DataPage<PhotoPost> list = photoService.GetSysPostPage( id, pageSize );
            bindList( "list", "photo", list.Results, bindLink );
            set( "page", list.PageBar );

            setCategoryDropList();

            target( Admin );
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

            if ("pick".Equals( cmd )) {
                pickedService.PickPost( ids );
                log( SiteLogString.PickPhotoPost(), ids );

                echoAjaxOk();
            }

            else if ("delete".Equals( cmd )) {
                PhotoPost.updateBatch( "set SaveStatus=" + SaveStatus.SysDelete, condition );
                log( SiteLogString.DeleteSysPhotoPost(), ids );
                echoAjaxOk();
            }

            else if ("category".Equals( cmd )) {
                if (categoryId < 0) {
                    content( lang( "exCategoryNotFound" ) );
                    return;
                }

                if (categoryId == zeroCatId) categoryId = 0;

                PhotoPost.updateBatch( "set SysCategoryId=" + categoryId, condition );
                log( SiteLogString.MovePhotoPost(), ids );

                echoAjaxOk();
            }
            else
                content( lang( "exCmd" ) );
        }


    }

}
