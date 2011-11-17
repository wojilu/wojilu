/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;

using wojilu.Common.Jobs;
using wojilu.Members.Users.Domain;
using wojilu.Members.Users.Interface;

namespace wojilu.Members.Users.Service {

    /// <summary>
    /// 空间最近访问
    /// </summary>
    public class VisitorService : IVisitorService {

        public virtual void Visit( int visitorId, User target ) {

            // 记录最近访客
            SpaceVisitJob.Visit( visitorId, target.Id );

            // 同时增加空间的点击数
            HitsJob.Add( target );
        }

        public virtual List<User> GetRecent( int count, int targetId ) {
            if (count <= 0) count = 18;
            List<SpaceVisitor> visitorList = db.find<SpaceVisitor>( "TargetId=" + targetId + " and VisitorId>0" ).list( count );
            return populateUser( visitorList );
        }

        public virtual DataPage<User> GetPage( int targetId, int pageSize ) {
            DataPage<SpaceVisitor> visitorList = db.findPage<SpaceVisitor>( "TargetId=" + targetId + " and VisitorId>0", pageSize );
            List<User> users = populateUser( visitorList.Results );

            DataPage<User> userPage = new DataPage<User>( visitorList );
            userPage.Results = users;
            return userPage;
        }


        private List<User> populateUser( List<SpaceVisitor> visitorList ) {
            List<User> results = new List<User>();
            foreach (SpaceVisitor visitor in visitorList) {
                if (visitor.Visitor == null || visitor.Visitor.Id<=0) continue;
                results.Add( visitor.Visitor );
            }
            return results;
        }

    }

}
