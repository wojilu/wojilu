/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using wojilu.Members.Users.Domain;
using wojilu.Common.AppBase.Interface;
using wojilu.Members.Interface;
using wojilu.Members.Users.Interface;

namespace wojilu.Common.Visitors {

    /// <summary>
    /// 脚印功能，不同于 Users.Service 下的 VisitorService
    /// </summary>
    public class VisitorService {

        private IDataVisitor visitor;

        private Type thisType() { return visitor.GetType(); }

        public void setVisitor( object v ) {
            this.visitor = v as IDataVisitor;
        }

        public IDataVisitor Visit( int visitorId, IAppData target ) {

            if (target.Creator.Id == visitorId) return null;
            if (hasVisit( visitorId, target.Id )) return null;

            visitor.setVisitor( new User( visitorId ) );
            visitor.setTarget( target );

            db.insert( (IEntity)visitor );

            return visitor;
        }

        private Boolean hasVisit( int visitorId, int targetId ) {
            return ndb.count( thisType(), "VisitorId=" + visitorId + " and TargetId=" + targetId ) > 0;
        }

        public List<IUser> GetRecent( int targetId, int count ) {
            IList visitorList = ndb.find( thisType(), "TargetId=" + targetId ).list( count );
            return populateUser( visitorList );
        }

        private List<IUser> populateUser( IList visitorList ) {
            List<IUser> results = new List<IUser>();
            foreach (IDataVisitor visitor in visitorList) {
                results.Add( visitor.getVisitor() );
            }
            return results;
        }

    }

}
