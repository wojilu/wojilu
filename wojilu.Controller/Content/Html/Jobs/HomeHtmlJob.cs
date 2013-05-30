/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Jobs;
using wojilu.Data;
using wojilu.Apps.Content.Domain;
using wojilu.Members.Sites.Domain;

namespace wojilu.Web.Controller.Content.Htmls {

    /// <summary>
    /// 定时生成app首页的静态html。在聚合第三方数据的时候，可以解决无法自动更新的问题。
    /// </summary>
    public class HomeHtmlJob : IWebJobItem {

        private static readonly ILog logger = LogManager.GetLogger( typeof( HomeHtmlJob ) );

        public void Execute() {

            logger.Info( "begin execute" );
            List<ContentApp> apps = ContentApp.find( "OwnerType=:otype" ).set( "otype", typeof( Site ).FullName ).list();

            foreach (ContentApp app in apps) {

                ContentSetting setting = app.GetSettingsObj();
                if (setting.IsAutoHtml == 0) {
                    logger.Info( "skip home, appId=" + app.Id );
                    continue;
                }

                HtmlMaker.GetHome().Process( app.Id );
                logger.Info( "make home, appId=" + app.Id );
            }
        }

        public void End() {
            DbContext.closeConnectionAll();
            logger.Info( "end execute" );
        }
    }

}
