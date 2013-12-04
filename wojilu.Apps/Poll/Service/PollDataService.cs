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



        public virtual DataPage<PollData> GetPageByApp(long appId) {
            return db.findPage<PollData>( "AppId=" + appId );
        }


        public virtual DataPage<PollData> GetFriendsPage(long userId) {

            FriendService fs = new FriendService();
            String fids = fs.FindFriendsIds( userId );

            if (strUtil.IsNullOrEmpty( fids )) return DataPage<PollData>.GetEmpty();

            return db.findPage<PollData>( "CreatorId in (" + fids + ")" );
        }


        public virtual List<PollData> GetHots(long appId, int count) {
            return db.find<PollData>( "AppId="+appId+" order by VoteCount desc, Replies desc, Hits desc" ).list( count );
        }

    }

}
