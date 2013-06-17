using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Apps.Photo.Domain;
using wojilu.Web.Context;
using wojilu.Members.Users.Domain;
using wojilu.Web.Controller.Photo.Wf;
using wojilu.Web.Mvc;
using wojilu.Drawing;

namespace wojilu.Web.Controller.Photo {

    public class PhotoBinder {

        public static void BindPhotoList( ControllerBase controller, DataPage<PhotoPost> list, int userId ) {

            IBlock block = controller.getBlock( "list" );

            List<int> likedIds = GetLikedIds( list.Results, userId );

            foreach (PhotoPost x in list.Results) {
                PhotoBinder.BindPostSingle( controller.ctx, block, x, likedIds );
                block.Next();
            }

            controller.set( "page", list.PageBar );
        }

        public static List<int> GetLikedIds( List<PhotoPost> list, int userId ) {

            List<int> ids = new List<int>();

            if (list.Count == 0) return ids;

            String postIds = "";
            foreach (PhotoPost x in list) {
                postIds += x.Id + ",";
            }
            postIds = postIds.TrimEnd( ',' );

            List<PhotoLike> likeList = PhotoLike.find( "UserId=" + userId + " and PostId in (" + postIds + ")" ).list();

            foreach (PhotoLike x in likeList) {
                ids.Add( x.Post.Id );
            }

            return ids;
        }



        public static void BindPostSingleFull( MvcContext ctx, IBlock block, PhotoPost x, List<int> likedIds ) {
            BindPostSingle( ctx, block, x, likedIds );

            block.Set( "x.SrcInfo", getSrcInfo( x ) );
        }

        public static void BindPostSingle( MvcContext ctx, IBlock block, PhotoPost x, List<int> likedIds ) {

            block.Set( "x.Link", PhotoLink.ToPost( x.Id ) );
            block.Set( "x.Title", x.Title );
            block.Set( "x.Description", x.Description );

            block.Set( "x.Pic", x.ImgThumbUrl );
            block.Set( "x.PicS", x.ImgSmallUrl );
            block.Set( "x.PicM", x.ImgMediumUrl );
            block.Set( "x.PicO", x.ImgUrl );

            int width = x.SizeSX == null ? 170 : x.SizeSX.Width;
            int height = x.SizeSX == null ? 170 : x.SizeSX.Height;
            int cfgWidth = getCfgWidth();
            if (width > cfgWidth) {
                height = Convert.ToInt32( (decimal)(cfgWidth * height) / (decimal)width );
                width = cfgWidth;
            }

            block.Set( "x.WidthSx", width );
            block.Set( "x.HeightSx", height );

            block.Set( "x.Pins", x.Pins );
            block.Set( "x.Likes", x.Likes );

            String pinsLikes = "";
            if (x.Pins > 0) pinsLikes += "收集:" + x.Pins;
            if (x.Likes > 0) pinsLikes += " 喜欢:" + x.Likes;
            if (x.Replies > 0) pinsLikes += " 评论:" + x.Replies;
            block.Set( "x.PinsLikes", pinsLikes );

            if (x.PhotoAlbum != null) {
                block.Set( "x.AlbumName", x.PhotoAlbum.Name );
                block.Set( "x.AlbumLink", PhotoLink.ToAlbumOne( x.PhotoAlbum.OwnerUrl, x.PhotoAlbum.Id ) );
            }
            else {
                block.Set( "x.AlbumName", "" );
                block.Set( "x.AlbumLink", "#" );
            }

            block.Set( "x.CreatorName", x.Creator.Name );
            block.Set( "x.CreatorPic", x.Creator.PicSmall );
            block.Set( "x.CreatorLink", PhotoLink.ToUser( x.Creator ) );
            block.Set( "x.Created", cvt.ToTimeString( x.Created ) );

            block.Set( "x.RepinLink", ctx.link.To( new HomeController().Repin, x.Id ) );
            block.Set( "x.LikeLink", ctx.link.To( new HomeController().Like, x.Id ) );
            block.Set( "x.UnLikeLink", ctx.link.To( new HomeController().UnLike, x.Id ) );

            if (likedIds.Contains( x.Id )) {
                block.Set( "x.LikedCss", "wfpost-liked disabled" );
                block.Set( "x.LikeName", "已喜欢" );
            }
            else {
                block.Set( "x.LikedCss", "wfpost-like" );
                block.Set( "x.LikeName", "<i class=\"icon-heart icon-white\"></i> 喜欢" );
            }
        }

        private static int getCfgWidth() {
            ThumbInfo t = ThumbConfig.GetPhoto( "sx" );
            if (t == null) return 170;
            return t.Width;
        }

        private static String getSrcInfo( PhotoPost x ) {

            if (x.RootId == 0 || x.RootId == x.Id) return "用户上传";

            PhotoPost root = PhotoPost.findById( x.RootId );

            User creator = root.Creator;

            if (strUtil.IsNullOrEmpty( x.SrcTool )) return string.Format( "<a href=\"{0}\">原图</a> 由用户 <a href=\"{1}\">{2}</a> 上传", PhotoLink.ToPost( x.RootId ), PhotoLink.ToUser( creator ), creator.Name );

            return string.Format( "<a href=\"{0}\">{1}</a>  通过 {2} 从 <a href=\"{3}\">{4}</a> 收集", PhotoLink.ToUser( creator ), creator.Name, root.SrcTool, root.SrcName, root.SrcUrl );

        }

    }
}
