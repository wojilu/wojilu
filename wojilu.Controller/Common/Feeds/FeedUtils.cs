/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using wojilu.Common.Feeds.Domain;

namespace wojilu.Web.Controller.Common.Feeds {

    public class FeedUtils {

        /// <summary>
        /// 只是转换，不合并。方便后台逐条删除
        /// </summary>
        /// <param name="onedayFeeds"></param>
        /// <returns></returns>
        public static List<FeedView> convertView( List<Feed> onedayFeeds ) {
            List<FeedView> results = new List<FeedView>();

            foreach (Feed feed in onedayFeeds) {

                results.Add( getView( feed ) );

            }

            return results;
        }

        /// <summary>
        /// 合并 feed
        /// </summary>
        /// <param name="onedayFeeds"></param>
        /// <returns></returns>
        public static List<FeedView> mergeFeed( List<Feed> onedayFeeds ) {

            List<FeedView> results = new List<FeedView>();

            foreach (Feed feed in onedayFeeds) {

                Boolean isMerged = mergeFeedOne( getView( feed ), results );
                if (isMerged == false) results.Add( getView( feed ) );

            }

            return results;
        }

        private static FeedView getView( Feed f ) {
            FeedView v = new FeedView();
            v.Id = f.Id;
            v.TitleData = f.TitleData;
            v.TitleTemplate = f.TitleTemplate;
            v.BodyData = f.BodyData;
            v.BodyTemplate = f.BodyTemplate;
            v.BodyGeneral = f.BodyGeneral;

            v.Creator = f.Creator;
            v.DataId = f.DataId;
            v.DataType = f.DataType;

            v.Replies = f.Replies;
            v.Created = f.Created;

            v.Ip = f.Ip;

            return v;
        }

        private static Boolean mergeFeedOne( FeedView feed, List<FeedView> results ) {

            foreach (FeedView target in results) {

                Boolean isMerged = Merger.Merge( feed, target );
                if (isMerged) return true;
            }

            return false;
        }

    }
}
