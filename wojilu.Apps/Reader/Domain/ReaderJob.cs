/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

using wojilu.Data;
using wojilu.Web.Jobs;
using wojilu.Apps.Reader.Interface;
using wojilu.Apps.Reader.Service;

namespace wojilu.Apps.Reader.Domain {

    public class ReaderJob : IWebJobItem {

        private static readonly ILog logger = LogManager.GetLogger( typeof( ReaderJob ) );

        public IFeedSourceService feedService { get; set; }

        public ReaderJob() {
            feedService = new FeedSourceService();
        }

        private static Random rd = new Random();
        public void Execute() {

            List<FeedSource> list = feedService.GetUnRefreshedList( getIntervalMinute() );
            DbContext.closeConnectionAll();

            foreach (FeedSource f in list) {
                feedService.DownloadBlogItems( f );
                logger.Info( "refresh feed -- [" + f.Title + "]" + f.FeedLink );
                DbContext.closeConnectionAll(); //一旦下载成功，即关闭数据库连接，防止超时

                int sleepSecond = rd.Next( 10, 60 );
                Thread.Sleep( sleepSecond * 1000 ); // 随机暂停稍许时间，防止被封
            }

        }

        public void End() {
        }

        private int getIntervalMinute() {

            // 一天没有更新的
            int defaultVal = 60 * 24;

            List<WebJob> list = cdb.findBy<WebJob>( "Type", typeof( ReaderJob ).FullName );
            if (list.Count == 0) return defaultVal;

            int result = list[0].Interval / 1000 / 60;
            return result;
        }

    }

}
