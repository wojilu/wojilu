using System;
using System.Collections.Generic;
using wojilu.Common.Msg.Interface;

namespace wojilu.Common.Comments {

    public interface IOpenCommentService {

        INotificationService nfService { get; set; }

        OpenComment GetById( int id );

        Result Create( OpenComment c );

        void Delete( OpenComment c );
        void DeleteAll( string url, int dataId, string dataType );
        void DeleteBatch( string ids );

        List<OpenComment> GetByApp( Type type, int appId, int listCount );

        DataPage<OpenComment> GetByDataAsc( string dataType, int dataId );
        DataPage<OpenComment> GetByDataDesc( string dataType, int dataId );
        DataPage<OpenComment> GetByDataDesc( string dataType, int dataId, int pageSize );

        DataPage<OpenComment> GetByUrlAsc( string url );
        DataPage<OpenComment> GetByUrlDesc( string url );

        DataPage<OpenComment> GetPageAll( string condition );

        List<OpenComment> GetMore( int parentId, int startId, int replyPageSize, string sort );

        int GetReplies( int dataId, string dataType, string url );
        int GetRepliesByData( int dataId, string dataType );
        int GetRepliesByUrl( string url );

        Result Import( OpenComment c );


    }

}
