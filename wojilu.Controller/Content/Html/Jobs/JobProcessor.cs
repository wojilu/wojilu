/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Apps.Content.Domain;

namespace wojilu.Web.Controller.Content.Htmls {

    public class JobProcessor {

        private static readonly ILog logger = LogManager.GetLogger( typeof( JobProcessor ) );

        public virtual void AfterPostAdd( int postId ) {

            ContentPost post = ContentPost.findById( postId );

            // 3）相关列表页也要更新
            HtmlMaker.GetList().Process( post );
            logger.Info( "[AfterPostAdd] make html list done. postId=" + postId );

            // 4) 最近列表页处理
            HtmlMaker.GetRecent().ProcessCache( post.AppId );
            logger.Info( "[AfterPostAdd] make html recent done. postId=" + postId );

        }

        public virtual void AfterPostDelete( int postId ) {

            ContentPost post = ContentPost.findById( postId );

            // 3）更新列表页
            HtmlMaker.GetList().Process( post );
            logger.Info( "[AfterPostDelete] make html list done. postId=" + postId );

            // 4) 最近列表页处理
            HtmlMaker.GetRecent().ProcessCache( post.AppId );
            logger.Info( "[AfterPostDelete] make html recent done. postId=" + postId );

            // 5) 侧边栏
            HtmlMaker.GetSidebar().Process( post.AppId );
            logger.Info( "[AfterPostDelete] make html sidebar done. postId=" + postId );

        }

        public virtual void AfterPostUpdate( int postId ) {

            ContentPost post = ContentPost.findById( postId );

            // 3) 列表页处理
            HtmlMaker.GetList().Process( post );
            logger.Info( "[AfterPostUpdate] make html list done. postId=" + postId );

            // 4) 最近列表页处理
            HtmlMaker.GetRecent().ProcessCache( post.AppId );
            logger.Info( "[AfterPostUpdate] make html recent done. postId=" + postId );

            // 5) 侧边栏
            HtmlMaker.GetSidebar().Process( post.AppId );
            logger.Info( "[AfterPostUpdate] make html sidebar done. postId=" + postId );

        }


    }
}
