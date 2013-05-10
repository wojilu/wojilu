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
            HtmlMaker.GetRecent().ProcessAll( post.AppId );
            logger.Info( "[AfterPostAdd] make html recent done. postId=" + postId );

        }

        public virtual void AfterPostDelete( int postId ) {

            ContentPost post = ContentPost.findById( postId );

            // 3）更新列表页
            HtmlMaker.GetList().Process( post );
            logger.Info( "[AfterPostDelete] make html list done. postId=" + postId );

            // 4) 最近列表页处理
            HtmlMaker.GetRecent().ProcessAll( post.AppId );
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
            HtmlMaker.GetRecent().ProcessAll( post.AppId );
            logger.Info( "[AfterPostUpdate] make html recent done. postId=" + postId );

            // 5) 侧边栏
            HtmlMaker.GetSidebar().Process( post.AppId );
            logger.Info( "[AfterPostUpdate] make html sidebar done. postId=" + postId );

        }

        public virtual void AfterImport( String ids ) {

            if (strUtil.IsNullOrEmpty( ids )) return;
            int[] arrIds = cvt.ToIntArray( ids );
            if (arrIds.Length == 0) return;

            List<int> appIds = new List<int>();
            List<int> sectionIds = new List<int>();
            List<ContentPost> posts = new List<ContentPost>();

            // 1) 逐一生成详细页
            foreach (int id in arrIds) {

                ContentPost x = ContentPost.findById( id );
                if (x == null) continue;

                posts.Add( x );

                if (isAutoMakeHtml( x.AppId ) == false) continue;

                if (appIds.Contains( x.AppId ) == false) {
                    appIds.Add( x.AppId );
                }

                if (sectionIds.Contains( x.SectionId ) == false) {
                    sectionIds.Add( x.SectionId );
                }

                HtmlMaker.GetDetail().Process( x );
                logger.Info( "[AfterImport] make html detail done. postId=" + x );
            }

            // 2) 列表页
            foreach (int x in sectionIds) {
                HtmlMaker.GetList().ProcessSection( x );
                logger.Info( "[AfterImport] make html list done. sectionId=" + x );
            }

            foreach (int appId in appIds) {

                // 3) 生成首页
                HtmlMaker.GetHome().Process( appId );
                logger.Info( "[AfterImport] make html home done. appId=" + appId );

                // 4) 生成侧边栏
                HtmlMaker.GetSidebar().Process( appId );
                logger.Info( "[AfterImport] make html sidebar done. appId=" + appId );

                // 5) 最近列表页处理
                HtmlMaker.GetRecent().ProcessAll( appId );
                logger.Info( "[AfterImport] make html recent done. appId=" + appId );
            }

        }

        private bool isAutoMakeHtml( int appId ) {

            ContentApp app = ContentApp.findById( appId );
            if (app == null) return false;

            ContentSetting setting = app.GetSettingsObj();
            return setting.IsAutoHtml == 1;
        }


    }
}
