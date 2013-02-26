/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

namespace wojilu.Web.Controller.Common.Feeds {

    public class TitleMerger : Merger {

        public TitleMerger( FeedView feed, FeedView target )
            : base( feed, target ) {
        }

        public override Boolean Merge() {

            JsonObject dic = Json.ParseJson( feed.TitleData );
            JsonObject dicTarget = Json.ParseJson( target.TitleData );

            if (dic.Count != dicTarget.Count) return false;

            String mergeKey = getMergeKey( dic, dicTarget );
            if (feed.Creator.Id == target.Creator.Id && mergeKey != null) {

                dicTarget[mergeKey] = dicTarget[mergeKey] + mergeSeperator + dic[mergeKey];
                target.TitleData = Json.ToString( dicTarget );
                target.IsMerged = true;


                return true;
            }

            return false;
        }

    }

}
