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

        Feed GetById( int id );
        //Feed GetByIdWithComments( int id );
        IEntity GetData( int id );

        DataPage<Feed> GetAll( String dataType, int pageSize );
        DataPage<Feed> GetAll( int userId, String dataType, int pageSize );

        DataPage<Feed> GetByUser( int userId, String dataType );
        DataPage<Feed> GetUserSelf( int userId, String dataType );

        DataPage<Feed> GetByUser( int userId, String dataType, int pageSize );
        DataPage<Feed> GetUserSelf( int userId, String dataType, int pageSize );



        String GetHtmlValue( String template, String templateData, String actorInfo );

        TemplateBundle getRegisteredTemplateBundleByID( int id );
        int GetTemplateBundleCount();

        List<Feed> GetUserFeeds( int count, int userId );

        Boolean IsUserIdValid( int userId, int ownerId );

        void publishUserAction( Feed data );
        void publishUserAction( IFeed data );
        void publishUserAction( User creator, String dataType, int templateBundleId, String templateData, String bodyGeneral, String ip );

        TemplateBundle registerTemplateBundle( List<OneLineStoryTemplate> oneLineStoryTemplates, List<ShortStoryTemplate> shortStoryTemplates, List<ActionLink> actionLinks );

        void ClearFeeds();

        //void Comment( FeedComment comment, String feedLink, String targetType );
        //void CommentAppend( User user, IEntity parent, String content, String ip );


        void SetCommentCount( IEntity target );

        void DeleteOne( int feedId );
    }

}
