/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Text;
using System.Collections;

using wojilu.Web.Mvc;

using wojilu.Apps.Photo.Service;
using wojilu.Members.Users.Domain;
using wojilu.Members.Sites.Domain;
using wojilu.Common.MemberApp;
using wojilu.Members.Users.Service;
using wojilu.Common.MemberApp.Interface;
using wojilu.Common.AppBase.Interface;

namespace wojilu.Web.Controller.Photo.Admin {

    public class LayoutController : ControllerBase {

        //public IUserAppService userAppService { get; set; }
        //public IUserAppService siteAppService { get; set; }
        //public IUserAppService memberAppService { get; set; }

        public LayoutController() {
        }

        public IMemberAppService getUserAppService() {
            //if (ctx.owner.obj is Site) {
            //    return this.siteAppService;
            //}
            //if (ctx.owner.obj is User) {
            //    return this.userAppService;
            //}
            //return this.memberAppService;

            return new UserAppService();
        }

        public override void Layout() {

            set( "app.Name", getUserAppService().GetByApp( (IApp)ctx.app.obj ).Name );
            set( "friendsPhotoLink", to( new PhotoController().Friends, -1 ) );
            set( "myLink", to( new MyController().My ) );
            set( "categoryAdmin", to( new AlbumController().List ) );
            set( "categoryAdd", to( new AlbumController().Add ) );

            //set( "batchUploadLink", to( new PostController().BatchAdd ) );
            set( "uploadLink", to( new PostController().Add ) );

        }




    }

}
