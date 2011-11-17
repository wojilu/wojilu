/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Text;
using System.Collections.Generic;

using wojilu.Apps.Photo.Domain;
using wojilu.Web.Mvc;

namespace wojilu.Web.Controller.Photo.Vo {

    public class PostVo {

        public String Title { get; set; }
        public String TitleFull { get; set; }

        public String Url { get; set; }
        public String ImgThumbUrl { get; set; }
        public String ViewsAndReplies { get; set; }
        public String ImgMediumUrl { get; set; }
        public String ImgUrl { get; set; }

        public static List<PostVo> Fill( List<PhotoPost> photoList ) {

            List<PostVo> results = new List<PostVo>();
            foreach (PhotoPost post in photoList) {

                PostVo vo = new PostVo();

                vo.Title = strUtil.CutString( post.Title, 15 );
                vo.TitleFull = post.Title;
                vo.Url= alink.ToAppData( post ) ;
                vo.ImgThumbUrl = post.ImgThumbUrl;
                vo.ImgMediumUrl = post.ImgMediumUrl;
                vo.ImgUrl = post.ImgUrl;

                String viewsAndReplies = getViewsAndReplies( post );
                vo.ViewsAndReplies = viewsAndReplies;

                results.Add( vo );

            }
            return results;
        }

        private static String getViewsAndReplies( PhotoPost post ) {
            String result = "";
            if (post.Hits > 0) result += lang.get( "view" ) + ":" + post.Hits;
            if (post.Replies > 0) result += " " + lang.get( "re" ) + ":" + post.Replies;
            return result;
        }

    }

}
