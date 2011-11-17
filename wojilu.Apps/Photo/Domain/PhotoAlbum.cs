/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

using wojilu.ORM;
using wojilu.Common.Categories;

namespace wojilu.Apps.Photo.Domain {


    [Serializable]
    public class PhotoAlbum : CategoryBase {

        public static PhotoAlbum New( int albumId ) {
            PhotoAlbum album = new PhotoAlbum();
            album.Id = albumId;
            return album;
        }

        [TinyInt]
        public int IsDefault { get; set; }

        [Column( Name = "Cover", Length = 15 )]
        public String Pwd { get; set; }

        //public DateTime Update { get; set; }

    }
}

