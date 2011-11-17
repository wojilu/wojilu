/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Text;

using wojilu.Web.Jobs;
using wojilu.Members.Users.Domain;
using wojilu.Data;

namespace wojilu.Members.Users.Service {


    public class SpaceVisitJobItem : IWebJobItem {

        public void Execute() {
            IList list = new VisitItem().findAll();
            foreach (VisitItem item in list) {

                //if (item.IsUpdated) continue;

                updateVisit( item );
                item.delete();

                //item.IsUpdated = true;
            }
        }

        private void updateVisit( VisitItem item ) {

            db.deleteBatch<SpaceVisitor>( "VisitorId=" + item.VisitorId + " and TargetId=" + item.TargetId );

            SpaceVisitor visitor = new SpaceVisitor();
            visitor.Visitor = new User( item.VisitorId );
            visitor.Target = new User( item.TargetId );
            db.insert( visitor );

        }

        public void End() {
            DbContext.closeConnectionAll();
        }

    }

}
