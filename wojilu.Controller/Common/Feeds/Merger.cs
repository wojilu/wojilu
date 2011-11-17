/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using wojilu.Common.Feeds.Domain;

namespace wojilu.Web.Controller.Common.Feeds {

    public abstract class Merger {


        public static Boolean Merge( FeedView feed, FeedView target ) {
            return GetMerger( feed, target ).Merge(  );
        }

        private static Merger GetMerger( FeedView feed, FeedView target ) {

            if ( strUtil.Equals( feed.TitleTemplate, target.TitleTemplate )
                && strUtil.Equals( feed.TitleData, target.TitleData )
                && strUtil.Equals( feed.BodyTemplate, target.BodyTemplate )
                && strUtil.Equals( feed.BodyData, target.BodyData )
                && strUtil.Equals( feed.BodyGeneral, target.BodyGeneral )) {
                return new CreatorMerger( feed, target );
            }

            if (feed.Creator == null || target.Creator == null) return new NoMerger();
            if (feed.Creator.Id != target.Creator.Id) return new NoMerger();
            if (feed.TitleTemplate.Equals( target.TitleTemplate ) == false) return new NoMerger();
            if (feed.BodyTemplate.Equals( target.BodyTemplate ) == false) return new NoMerger();

            if (feed.DataType.Equals( typeof( Share ).FullName )) return new NoMerger();
                 

            if (strUtil.Equals( feed.TitleData, target.TitleData ) == false && strUtil.Equals( feed.BodyData, target.BodyData ) == false) return new NoMerger();

            // 依次合并
            if (strUtil.Equals( feed.TitleData, target.TitleData ) && strUtil.Equals( feed.BodyData, target.BodyData )) return new CommentMerger( feed, target );
            if (strUtil.Equals( feed.TitleData, target.TitleData ) == false) return new TitleMerger( feed, target );
            if (strUtil.Equals( feed.BodyData, target.BodyData ) == false) return new BodyMerger( feed, target );


            return new NoMerger();

        }

        //----------------------------------------------------------------------------------------------------------

        protected static readonly String mergeSeperator = "、";

        public Merger() {
        }

        public Merger( FeedView feed, FeedView target ) {
            this.feed = feed;
            this.target = target;
        }

        public abstract Boolean Merge( );

        protected FeedView feed;
        protected FeedView target;

        protected String getMergeKey( Dictionary<string, object> dic, Dictionary<string, object> dicTarget ) {

            String mergeKey = null;
            foreach (KeyValuePair<string, object> item in dic) {

                if (dicTarget.ContainsKey( item.Key ) == false) return null;

                if (item.Value != dicTarget[item.Key]) {

                    if (mergeKey == null)
                        mergeKey = item.Key;
                    else
                        return null;
                }
            }

            return mergeKey;
        }

    }



}
