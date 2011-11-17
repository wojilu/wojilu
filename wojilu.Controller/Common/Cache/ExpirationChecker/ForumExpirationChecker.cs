using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;

namespace wojilu.Web.Controller.Common {

    public class ForumExpirationChecker {

        private ICacheHelper cacher;

        public ForumExpirationChecker( ICacheHelper cacher ) {
            this.cacher = cacher;
        }

        public Boolean IsExpried( String key ) {

            if (isForumBoard( key ) == false) return false;

            // 以下四种页面，都要检查依赖对象是否已经更新
            if (key.IndexOf( "/Recent/" ) > 0 && forumExpired( key )) return true;
            if (key.IndexOf( "/Board/" ) > 0 && boardExpired( key )) return true;
            if (key.IndexOf( "/Topic/" ) > 0 && topicExpired( key )) return true;
            if (key.IndexOf( "/Post/" ) > 0 && topicExpired( key )) return true;

            return false;
        }

        private Boolean isForumBoard( String key ) {
            return key.StartsWith( "/Forum" );
        }

        // 访问最新主题和帖子的时候，检查论坛是否更新过
        // 需要检查的页面：/Forum1/Recent/Post
        private Boolean forumExpired( String key ) {
            String[] arrItem = key.TrimStart( '/' ).Split( '/' );
            int appId = cvt.ToInt( strUtil.TrimStart( arrItem[0], "Forum" ) );

            // 主题最后更新时间
            String fkey = "forum_" + appId;
            DateTime ts = this.cacher.GetTimestamp( fkey );
            if (isTimeNull( ts )) return false; // 没有更新过

            // 缓存加入时间
            DateTime created = this.cacher.GetTimestamp( key );
            if (isTimeNull( created )) return true;

            if (ts >= created) return true;

            return false;
        }

        // 访问主题回帖的各个分页的时候，检查主题是否更新过
        // 需要检查的页面：/Forum1/Topic/16.aspx /Forum1/Topic/16/p3.aspx 和 /Forum1/Post/5161
        private Boolean topicExpired( String key ) {


            String[] arrItem = strUtil.TrimEnd( key, MvcConfig.Instance.UrlExt ).TrimStart( '/' ).Split( '/' );
            int topicId = cvt.ToInt( arrItem[2] );

            // 主题最后更新时间
            String fkey = "forumtopic_" + topicId;
            DateTime ts = this.cacher.GetTimestamp( fkey );
            if (isTimeNull( ts )) return false; // 没有更新过

            // 缓存加入时间
            DateTime created = this.cacher.GetTimestamp( key );
            if (isTimeNull( created )) return true;

            if (ts >= created) return true;

            return false;
        }

        // 访问版块分页的时候，检查版块是否更新过
        // 需要检查的页面：/Forum1/Board/2/p3.aspx
        private Boolean boardExpired( string key ) {

            String[] arrItem = strUtil.TrimEnd( key, MvcConfig.Instance.UrlExt ).TrimStart( '/' ).Split( '/' );
            int boardId = cvt.ToInt( arrItem[2] );

            // 版块最后更新时间
            String fkey = "forumboard_" + boardId;
            DateTime ts = this.cacher.GetTimestamp( fkey );
            if (isTimeNull( ts )) return false; // 没有更新过

            // 缓存加入时间
            DateTime created = this.cacher.GetTimestamp( key );
            if (isTimeNull( created )) return true;

            if (ts >= created) return true;

            return false;
        }

        private Boolean isTimeNull( DateTime created ) {
            return created.Year < 1900;
        }
    }

}
