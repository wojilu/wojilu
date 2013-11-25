/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using wojilu.Members.Users.Service;
using wojilu.Common.Feeds.Domain;
using System.Collections.Generic;
using wojilu.Members.Users.Domain;
using wojilu.Members.Users.Interface;

namespace wojilu.Common.Feeds.Interface {

    public interface IFeedService {

        IFollowerService followerService { get; set; }
        IFriendService friendService { get; set; }

        Feed GetById(long id);
        IEntity GetData(long id);

        DataPage<Feed> GetAll( String dataType, int pageSize );
        DataPage<Feed> GetAll(long userId, string dataType, int pageSize);

        DataPage<Feed> GetByUser(long userId, string dataType);
        DataPage<Feed> GetUserSelf(long userId, string dataType);

        DataPage<Feed> GetByUser(long userId, string dataType, int pageSize);
        DataPage<Feed> GetUserSelf(long userId, string dataType, int pageSize);



        String GetHtmlValue( String template, String templateData, String actorInfo );

        TemplateBundle getRegisteredTemplateBundleByID(long id);
        int GetTemplateBundleCount();

        List<Feed> GetUserFeeds(int count, long userId);

        bool IsUserIdValid(long userId, long ownerId);

        void publishUserAction( Feed data );
        void publishUserAction( IFeed data );
        void publishUserAction(User creator, string dataType, long templateBundleId, string templateData, string bodyGeneral, string ip);

        TemplateBundle registerTemplateBundle( List<OneLineStoryTemplate> oneLineStoryTemplates, List<ShortStoryTemplate> shortStoryTemplates, List<ActionLink> actionLinks );

        void ClearFeeds();

        void SetCommentCount( IEntity target );

        void DeleteOne(long feedId);
    }

}
