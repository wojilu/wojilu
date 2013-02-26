/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

namespace wojilu.Web.Controller.Common.Feeds {


    public class BodyMerger : Merger {

        public BodyMerger( FeedView feed, FeedView target )
            : base( feed, target ) {
        }

        public override Boolean Merge() {


            mergeBodyData(  );
            mergeNumberInTitle();

            target.IsMerged = true;

            return true;
        }

        private void mergeBodyData( ) {

            JsonObject dic = Json.ParseJson( feed.BodyData );
            JsonObject dicTarget = Json.ParseJson( target.BodyData );

            String mergeKey = getMergeKey( dic, dicTarget );

            if( strUtil.HasText( mergeKey) )
                dicTarget[mergeKey] = dicTarget[mergeKey] + " " + dic[mergeKey];

            target.BodyData = Json.ToString( dicTarget );
        }

        private void mergeNumberInTitle() {

            JsonObject dic = Json.ParseJson( feed.TitleData );
            JsonObject dicTarget = Json.ParseJson( target.TitleData );

            foreach (KeyValuePair<string, object> kv in dic) {

                if (kv.Value == null) continue;

                if (cvt.IsInt( kv.Value.ToString() )) {


                    int valNumber = cvt.ToInt( kv.Value );
                    int val = cvt.ToInt( dicTarget[ kv.Key ] );

                    dicTarget[kv.Key] = val + valNumber;

                    target.TitleData = Json.ToString( dicTarget );

                }

            }

        }

    }

}
