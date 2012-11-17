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
using wojilu.Members.Users.Domain;

namespace wojilu.Web.Controller.Admin.Apps.Photo {

    public partial class MainController : ControllerBase {


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

        private void setCategoryDropList() {
            List<PhotoSysCategory> categories = categoryService.GetAll();
            List<PhotoSysCategory> list = addSelectInfo( categories );
            dropList( "adminDropCategoryList", list, "Name=Id", null );
        }

        private static readonly int zeroCatId = 99999999;

        private List<PhotoSysCategory> addSelectInfo( List<PhotoSysCategory> categories ) {
            PhotoSysCategory category = new PhotoSysCategory();
            category.Id = -1;
            category.Name = lang( "setCategory" );

            PhotoSysCategory nullCat = new PhotoSysCategory();
            nullCat.Id = zeroCatId;
            nullCat.Name = "--无分类--";

            List<PhotoSysCategory> list = new List<PhotoSysCategory>();
            list.Add( category );
            list.Add( nullCat );
            foreach (PhotoSysCategory cat in categories) {
                list.Add( cat );
            }
            return list;
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
