/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

namespace wojilu.Web.Controller.Common.Feeds {

    public class CommentMerger : Merger {

        public CommentMerger( FeedView feed, FeedView target )
            : base( feed, target ) {
        }

        public override Boolean Merge() {

            target.BodyGeneral = feed.BodyGeneral + "<br/>" + target.BodyGeneral;

            target.IsMerged = true;

            return true;
        }

    }

}
