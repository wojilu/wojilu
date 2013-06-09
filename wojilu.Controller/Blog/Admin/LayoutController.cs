/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

using wojilu.Web.Mvc;

using wojilu.Apps.Blog.Interface;
using wojilu.Apps.Blog.Service;
using wojilu.Members.Users.Service;
using wojilu.Common.MemberApp.Interface;
using wojilu.Common.AppBase.Interface;

namespace wojilu.Web.Controller.Blog.Admin {

    public class LayoutController : ControllerBase {

        public IBlogCategoryService categoryService { get; set; }

        public LayoutController() {
            categoryService = new BlogCategoryService();
            base.HideLayout( typeof( Blog.LayoutController ) );
        }

        public IMemberAppService getUserAppService() {
            return new UserAppService();
        }

        public override void Layout() {

            set( "app.Name", getUserAppService().GetByApp( (IApp)ctx.app.obj ).Name );

            set( "friendsBlogLink", to( new BlogController().Friends, -1 ) );
            set( "myBlogLink", to( new MyListController().My ) );
            set( "addBlogLink", to( new PostController().Add ) );
            set( "categoryLink", to( new CategoryController().List ) );
            set( "blogrollLink", to( new BlogrollController().AdminList ) );

            set( "draftLink", to( new DraftController().Draft ) );
            set( "trashLink", to( new TrashController().Trash ) );

            set( "settingLink", to( new SettingController().Index ) );


        }



    }

}
