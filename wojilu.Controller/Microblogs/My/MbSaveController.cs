/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.IO;

using wojilu.Web.Mvc;
using wojilu.Web.Utils;
using wojilu.Members.Users.Domain;
using wojilu.Common.Microblogs.Service;
using wojilu.Common.Microblogs.Domain;
using wojilu.Common.Microblogs.Interface;
using wojilu.Apps.Photo.Domain;
using wojilu.Apps.Photo.Interface;
using wojilu.Apps.Photo.Service;
using wojilu.Web.Controller.Users.Admin;
using System.Collections.Generic;
using wojilu.Common;

namespace wojilu.Web.Controller.Microblogs.My {

    public class MbSaveController : ControllerBase {

        public IMicroblogService microblogService { get; set; }
        public IPhotoPostService postService { get; set; }

        public MbSaveController() {
            microblogService = new MicroblogService();
            postService = new PhotoPostService();
        }

        public void Publish() {

            if (Component.IsClose( typeof( MicroblogApp ) )) {
                content( "" );
                return;
            }


            target( new MicroblogSaveController().Create );

            User user = ctx.owner.obj as User;

            set( "my.Name", user.Name );
            set( "my.Face", user.PicSmall );
            set( "my.Link", to( new UserProfileController().Face ) );

            set( "uploadLink", to( UploadForm ) );

            String logStr = "";
            Microblog blog = microblogService.GetFirst( user.Id );
            if (blog != null) {
                String lnkMore = alink.ToUserMicroblog( user );
                logStr = string.Format( "{0} <span class=\"note\">{1}</span> <a href='{2}'>{3}...</a>", blog.Content, cvt.ToTimeString( blog.Created ), lnkMore, lang( "more" ) );
            }
            set( "currentBlog", logStr );

        }

        public void UploadForm() {
            target( SavePicIE );
        }

        public void SavePicIE() {
            HttpFile postedFile = ctx.GetFileSingle();

            Result result = Uploader.SaveImg( postedFile );
            if (result.HasErrors) {
                ctx.errors.Join( result );
                run( UploadForm );
                return;
            }

            PhotoPost post = savePicPrivate( postedFile, result );
            set( "picThumbUrl", post.ImgThumbUrl );
            set( "picUrl", post.DataUrl );
        }

        public void SavePic() {

            HttpFile postedFile = ctx.GetFileSingle();

            Result result = Uploader.SaveImg( postedFile );
            if (result.HasErrors) {
                ctx.errors.Join( result );
                echoError();
                return;
            }

            PhotoPost post = savePicPrivate( postedFile, result );

            Dictionary<String, String> dic = new Dictionary<string, string>();
            dic["picThumbUrl"] = post.ImgThumbUrl;
            dic["picUrl"] = post.DataUrl;

            String json = Json.ToString( dic );

            echoText( json );

        }

        private PhotoPost savePicPrivate( HttpFile postedFile, Result result ) {

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
            return post;
        }


    }
}
