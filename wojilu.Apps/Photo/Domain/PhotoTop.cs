/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

using wojilu.ORM;

namespace wojilu.Apps.Photo.Domain {


    [Serializable]
    public class PhotoTop : ObjectBase<PhotoTop> {

        public int PostId { get; set; }
        public String Style { get; set; }
        public int SystemCategoryId { get; set; }

        public String Title { get; set; }
        public String Url { get; set; }
        [Column( Length = 50 )]
        public String Author { get; set; }

        public String AuthorFace { get; set; }
        public String AuthorUrl { get; set; }

        public String ImgUrl { get; set; }
        public String Abstract { get; set; }

        public int IsGlobal { get; set; }
        [TinyInt]
        public int IsHeadline { get; set; }
        [TinyInt]
        public int IsImgPhoto { get; set; }

        public DateTime LastUpdateTime { get; set; }


        public DateTime CreateTime { get; set; }


        public static void DeleteHeadline( String choice ) {
            throw new NotImplementedException();
        }

        //public static void SaveHeadline( String selectId ) {
        //    PhotoTop top = new PhotoTop();
        //    IList list = new PhotoPost().find( "Id in (" + selectId + ")" )
        //        .select( "Id,ModuleId,SysCategoryId,PhotoAlbum.Id,Member.Name,Member.Url,Title,DataUrl" )
        //        .list();

        //    foreach (PhotoPost post in list) {

        //        Boolean nopost = false;
        //        PhotoTop pt = top.find( "PostId=" + post.Id ).first() as PhotoTop;
        //        if (pt == null) {
        //            nopost = true;
        //        }
        //        if (nopost) {
        //            pt = new PhotoTop();
        //        }

        //        pt.Author = post.Creator.Name;
        //        pt.AuthorUrl = post.Creator.Url;
        //        pt.PostId = post.Id;
        //        pt.Title = post.Title;
        //        pt.ImgUrl = post.DataUrl;
        //        pt.IsGlobal = 0;
        //        pt.IsHeadline = 1;
        //        if (nopost) {
        //            pt.CreateTime = DateTime.Now;
        //            pt.insert();
        //        }
        //        else {
        //            pt.LastUpdateTime = DateTime.Now;
        //            pt.update();
        //        }
        //    }
        //}

        //public static void SaveTop( String selectId, Boolean isGlobal, Boolean isCategory ) {
        //    PhotoTop top = new PhotoTop();
        //    IList list = new PhotoPost().find( "Id in (" + selectId + ")" )
        //        .select( "Id,ModuleId,SysCategoryId,PhotoAlbum.Id,Member.Name,Member.Url,Title,DataUrl" )
        //        .list();

        //    foreach (PhotoPost post in list) {
        //        Boolean nopost = false;
        //        PhotoTop pt = top.find( "PostId=" + post.Id ).first() as PhotoTop;
        //        if (pt == null) {
        //            nopost = true;
        //        }
        //        if (nopost) {
        //            pt = new PhotoTop();
        //        }
        //        pt.Author = post.Creator.Name;
        //        pt.AuthorUrl = post.Creator.Url;
        //        pt.PostId = post.Id;
        //        pt.Title = post.Title;
        //        pt.ImgUrl = post.DataUrl;
        //        if (isGlobal) {
        //            pt.IsGlobal = 1;
        //        }
        //        if (isCategory) {
        //            pt.SystemCategoryId = post.SysCategoryId;
        //        }
        //        if (nopost) {
        //            pt.CreateTime = DateTime.Now;
        //            pt.insert();
        //        }
        //        else {
        //            pt.LastUpdateTime = DateTime.Now;
        //            pt.update();
        //        }
        //    }
        //}

        public static void SaveTopPic( String choice, Boolean isHeadline, Boolean isCategory ) {
            throw new NotImplementedException();
        }

        //public static void UpdateStyle( String selectId, String style ) {
        //    db.updateBatch<PhotoTop>( "set Style='" + strUtil.SqlClean( style, 2000 ) + "'", "Id in (" + selectId + ")" );
        //}

        public static Boolean UpdateTitle( int p, String p_2 ) {
            throw new NotImplementedException();
        }


        public String AbstractIcon {
            get {
                if (strUtil.HasText( this.Abstract )) {
                    return string.Format( "<img src=\"{0}\" align=\"absmiddle\" />", strUtil.Join( sys.Path.Root, "/images/icon/abstract.gif" ) );
                }
                return "";
            }
        }


        public String ImgIcon {
            get {
                if (this.IsImgPhoto == 1) {
                    return string.Format( "<a href=\"{0}\"><img src=\"{1}\" align=\"absmiddle\" /></a>", this.ImgUrl, strUtil.Join( sys.Path.Root, "/images/icon/img.gif" ) );
                }
                return "";
            }
        }

    }
}

