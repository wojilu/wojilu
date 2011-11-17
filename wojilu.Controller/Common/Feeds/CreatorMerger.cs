/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using wojilu.Members.Users.Domain;

namespace wojilu.Web.Controller.Common.Feeds {

    public class CreatorMerger : Merger {

        public CreatorMerger( FeedView feed, FeedView target )
            : base( feed, target ) {
        }

        public override Boolean Merge() {
            addCreator( target.CreatorList, feed.Creator );
            target.IsMerged = true;
            return true;
        }

        private void addCreator( List<User> list, User user ) {
            Boolean isExist = false;
            foreach (User creator in list) {
                if (creator.Id == user.Id) {
                    isExist = true;
                    break;
                }
            }

            if (isExist == false) list.Add( user );
        }

    }

}
