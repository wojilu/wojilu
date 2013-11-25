using System;
using System.Collections.Generic;
using wojilu.Common.Msg.Interface;

namespace wojilu.Common.Comments {

    public interface IOpenCommentService {

        INotificationService nfService { get; set; }

        OpenComment GetById(long id);
        IEntity GetTarget( OpenComment c );

        Result Create( OpenComment c );
        Result CreateNoNotification( OpenComment c );

        void Delete( OpenComment c );
        void DeleteAll(string url, long dataId, string dataType);
        void DeleteBatch( string ids );

        DataPage<OpenComment> GetByMicroblogOwnerId(long ownerId);
        DataPage<OpenComment> GetByDataAndOwnerId(string dataType, long ownerId);

        List<OpenComment> GetByApp(Type type, long appId, int listCount);

        DataPage<OpenComment> GetByDataAsc(string dataType, long dataId);
        DataPage<OpenComment> GetByDataDesc(string dataType, long dataId);
        DataPage<OpenComment> GetByDataDesc(string dataType, long dataId, int pageSize);

        DataPage<OpenComment> GetByUrlAsc( string url );
        DataPage<OpenComment> GetByUrlDesc( string url );

        DataPage<OpenComment> GetPageAll( string condition );

        List<OpenComment> GetMore(long parentId, long startId, int replyPageSize, string sort);

        int GetReplies(long dataId, string dataType, string url);
        int GetRepliesByData(long dataId, string dataType);
        int GetRepliesByUrl( string url );

        Result Import( OpenComment c );



    }

}
