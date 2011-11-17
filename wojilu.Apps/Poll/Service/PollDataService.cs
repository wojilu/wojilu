/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Apps.Poll.Domain;
using wojilu.Common.Polls.Service;
using wojilu.Members.Users.Service;
using wojilu.Common.Feeds.Domain;

namespace wojilu.Apps.Poll.Service {

    public class PollDataService : PollBaseService<PollData, PollDataResult> {



        public DataPage<PollData> GetPageByApp( int appId ) {
            return db.findPage<PollData>( "AppId=" + appId );
        }


        public DataPage<PollData> GetFriendsPage( int userId ) {

            FriendService fs = new FriendService();
            String fids = fs.FindFriendsIds( userId );

            if (strUtil.IsNullOrEmpty( fids )) return DataPage<PollData>.GetEmpty();

            return db.findPage<PollData>( "CreatorId in (" + fids + ")" );
        }


        public List<PollData> GetHots( int appId, int count ) {
            return db.find<PollData>( "AppId="+appId+" order by VoteCount desc, Replies desc, Hits desc" ).list( count );
        }

    }

}
