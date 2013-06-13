/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;

using wojilu.Web.Mvc;
using wojilu.Members.Users.Domain;
using wojilu.Apps.Photo.Domain;
using wojilu.Apps.Photo.Service;
using wojilu.Apps.Blog.Service;
using wojilu.Apps.Photo.Interface;
using wojilu.Apps.Blog.Interface;

namespace wojilu.Web.Controller.Users {

    public class ForbiddenController :ControllerBase {

        public IPhotoPostService photoPostService { get; set; }
        public IBlogPostService blogPostService { get; set; }

        public ForbiddenController() {
            photoPostService = new PhotoPostService();
            blogPostService = new BlogPostService();
        }


        public void User() {


            HideLayout( typeof( wojilu.Web.Controller.LayoutController ) );

            User owner = ctx.owner.obj as User;

            set( "user.Name", owner.Name );
            set( "user.Face", owner.PicM );

            int friendCount = owner.FriendCount;
            int blogCount = blogPostService.GetCountByUser( owner.Id );
            int photoCount = photoPostService.GetCountByUser( owner.Id );

            set( "user.FriendCount", friendCount );
            set( "user.BlogCount", blogCount );
            set( "user.PhotoCount", photoCount );

            String friendCmd = "";
            if (ctx.viewer.IsFriend( owner.Id )==false) {
                String lnk = to( new FriendController().AddFriend , owner.Id );
                String msg = lang( "addAsFriend" );
                String imgAdd = strUtil.Join( sys.Path.Img, "add.gif" );
                friendCmd = "<span class=\"frmBox cmd\" href=\"" + lnk + "\" title=\"" + msg + "\"><img src=\"" + imgAdd + "\"/> " + msg + "</span>";
            }

            set( "friendCmd", friendCmd );
        }


    }

}
