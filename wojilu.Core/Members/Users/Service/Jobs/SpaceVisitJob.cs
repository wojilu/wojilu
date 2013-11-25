/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

namespace wojilu.Members.Users.Service {

    public class SpaceVisitJob {

        public static void Visit( long visitorId, long targetId ) {

            if (visitorId <= 0) return;
            if (visitorId == targetId) return;

            VisitItem cachedItem = findFromCache( visitorId, targetId );

            if (cachedItem != null) {
                cachedItem.VisitTime = DateTime.Now;
                cachedItem.IsUpdated = false;
            }
            else {
                cachedItem = new VisitItem();
                cachedItem.Name = getName( visitorId, targetId );
                cachedItem.VisitorId = visitorId;
                cachedItem.TargetId = targetId;
                cachedItem.insertByIndex( "Name", cachedItem.Name );
            }

        }

        private static VisitItem findFromCache( long visitorId, long targetId ) {
            List<VisitItem> results = cdb.findByName<VisitItem>( getName( visitorId, targetId ) );
            return results.Count == 0 ? null : results[0];
        }

        private static String getName( long visitorId, long targetId ) {
            return visitorId + "_" + targetId;
        }

    }
}
