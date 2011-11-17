/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Text.RegularExpressions;
using wojilu.ORM;
using wojilu.Common;

namespace wojilu.Apps.Reader.Domain {

    [Serializable]
    public class FeedEntry : ObjectBase<FeedEntry>, IHits, IShareData {

        public FeedEntry() {
            this.Id = -1;
        }

        [CacheCount( "EntryCount" )]
        public FeedSource FeedSource { get; set; }
        public String Title { get; set; }
        public String Link { get; set; }

        [LongText]
        public String Description { get; set; }
        public String Author { get; set; }
        public String Category { get; set; }
        public DateTime PubDate { get; set; }
        public DateTime Created { get; set; }
        public int FavoriteCount { get; set; }
        public int Hits { get; set; }

        [NotSave]
        public String Abstract {
            get {
                String ab = Regex.Replace( this.Description, "<.+?>", "" );
                ab = strUtil.CutString( ab, 200 );
                return ab;
            }
        }

        public IShareInfo GetShareInfo() {
            return new FeedShare( this );
        }

    }


}
