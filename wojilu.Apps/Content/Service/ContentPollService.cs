/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Apps.Content.Domain;
using wojilu.Common.Polls.Service;
using wojilu.Common.AppBase;

namespace wojilu.Apps.Content.Service {

    public class ContentPollService : PollBaseService<ContentPoll, ContentPollResult> {


        public Result CreatePoll( int sectionId, ContentPoll poll ) {

            ContentPost post = new ContentPost();
            post.OwnerId = poll.OwnerId;
            post.OwnerType = poll.OwnerType;
            post.OwnerUrl = poll.OwnerUrl;

            post.Creator = poll.Creator;
            post.CreatorUrl = poll.CreatorUrl;

            post.TypeName = typeof( ContentPoll ).FullName;

            post.AppId = poll.AppId;
            post.PageSection = new ContentSection { Id = sectionId };
            post.Ip = post.Ip;

            post.Title = poll.Title;

            Result result = post.insert();
            if (result.HasErrors) return result;

            // 双向引用1
            poll.TopicId = post.Id;

            poll.insert();

            // 双向引用2
            post.Content = poll.Id.ToString();
            post.update( "Content" );

            return result;
        }

        public ContentPoll GetRecentPoll( int appId, int sectionId ) {

            List<ContentPost> list = ContentPost.find( "AppId=" + appId + " and PageSection.Id=" + sectionId + " and SaveStatus=" + SaveStatus.Normal + " order by Id desc" ).list();

            if (list.Count == 0) return null;

            return db.find<ContentPoll>( "TopicId=" + list[0].Id ).first();


        }

    }
}
