using System;
using System.Collections;
using System.Collections.Generic;
using wojilu.Apps.Blog.Domain;
using wojilu.Common.Picks;

namespace wojilu.Apps.Blog.Interface {

    public interface IBlogPickService {

        List<MergedData> GetAll( IList allTopics );
        BlogPick GetEditTopic( int topicId );
        BlogPick GetPinPostByIndex( int index );

        void AddPinPost( int index, string title, string link, string summary );
        void AddPinTopic( BlogPost topic, int index );

        void DeletePinPost( int index );
        void DeleteTopic( int topicId );
        void RestoreTopic( int topicId );

        void EditPinPost( BlogPick x, int index, string title, string link, string summary );
        void EditTopic( int topicId, string title, string link, string summary );


        DataPage<BlogPick> GetDeleteList();

        String GetIndexIds();

        String GetIndexIds( int index );
    }

}
