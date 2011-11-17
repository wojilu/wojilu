/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using wojilu.ORM;

namespace wojilu.Apps.Photo.Domain {


    [Serializable]
    public class TopAlbum : ObjectBase<TopAlbum> {

        public int AlbumId { get; set; }
        public int AlbumModuleId { get; set; }

        public String AlbumUrl {
            get {
                PhotoAlbum cat = new PhotoAlbum();
                cat.Id = this.AlbumId;
                cat.OwnerUrl = this.AuthorUrl;
                //return Link.ToCategory(cat);
                // TODO AlbumUrl
                return "";
            }
        }

        [Column( Length = 50 )]
        public String Author { get; set; }
        public String AuthorFace { get; set; }
        public String AuthorUrl { get; set; }


        public String Title { get; set; }
        public String Style { get; set; }
        public String Description { get; set; }

        public int PhotoCount { get; set; }

        public String ImgUrl1 { get; set; }
        public String ImgUrl2 { get; set; }
        public String ImgUrl3 { get; set; }

        public DateTime CreateTime { get; set; }
        public DateTime LastUpdateTime { get; set; }




    }
}

