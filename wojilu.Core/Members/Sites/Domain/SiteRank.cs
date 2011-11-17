/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Text;
using wojilu.Data;
using wojilu.ORM;
using wojilu.Common.Security;

namespace wojilu.Members.Sites.Domain {

    [Serializable]
    public class SiteRank : CacheObject, IRole {

        public int Credit { get; set; }
        public String StarPath { get; set; }
        public int StarCount { get; set; }

        [NotSave, NotSerialize]
        public IRole Role {
            get { return this; }
            set { }
        }

        [NotSave, NotSerialize]
        public String StarHtml {
            get { return getStarHtml(); }
        }

        private String _starHtml = null;
        private String getStarHtml() {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < StarCount; i++) {
                builder.AppendFormat( "<img src='{0}'/> ", strUtil.Join( sys.Path.ImgStar, StarPath ) );
            }
            _starHtml = builder.ToString();
            return _starHtml;
        }

        public static SiteRank GetById( int id ) {
            SiteRank siteRank = new SiteRank().findById( id ) as SiteRank;
            if (siteRank == null) return GetNullRank();
            return siteRank;
        }

        public static SiteRank GetNullRank() {
            SiteRank siteRank = new SiteRank();
            siteRank.Id = -1;
            return siteRank;
        }

    }

}
