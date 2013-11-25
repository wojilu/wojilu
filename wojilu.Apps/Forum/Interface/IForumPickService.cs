using System;
using System.Collections.Generic;
using wojilu.Apps.Forum.Domain;
using System.Collections;
using wojilu.Common.Picks;

namespace wojilu.Apps.Forum.Interface {

    public interface IForumPickService {

        List<MergedData> GetAll(IList allTopics, long appId);
        ForumPick GetEditTopic(long appId, long topicId);
        ForumPick GetPinPostByIndex(long appId, int index);

        Result AddPinPost(long appId, int index, string title, string link, string summary);
        Result AddPinTopic( ForumTopic topic, int index );

        void DeletePinPost(long appId, int index);
        void DeleteTopic(long appId, long topicId);
        void RestoreTopic(long appId, long topicId);

        void EditPinPost( ForumPick x, int index, string title, string link, string summary );
        void EditTopic(long appId, long topicId, string title, string link, string summary);


        DataPage<ForumPick> GetDeleteList(long p);

        string GetIndexIds(long appId);

        string GetIndexIds(long appId, int index);
    }

}
