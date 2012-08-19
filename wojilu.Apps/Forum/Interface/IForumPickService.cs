using System;
using System.Collections.Generic;
using wojilu.Apps.Forum.Domain;
namespace wojilu.Apps.Forum.Interface {

    public interface IForumPickService {

        List<MergedPost> GetAll( List<ForumTopic> allTopics, int appId );
        ForumPick GetEditTopic( int appId, int topicId );
        ForumPick GetPinPostByIndex( int appId, int index );

        void AddPinPost( int appId, int index, string title, string link, string summary );
        void AddPinTopic( ForumTopic topic, int index );

        void DeletePinPost( int appId, int index );
        void DeleteTopic( int appId, int topicId );
        void RestoreTopic( int appId, int topicId );

        void EditPinPost( ForumPick x, int index, string title, string link, string summary );
        void EditTopic( int appId, int topicId, string title, string link, string summary );


        DataPage<ForumPick> GetDeleteList( int p );

        String GetIndexIds( int appId );

        String GetIndexIds( int appId, int index );
    }

}
