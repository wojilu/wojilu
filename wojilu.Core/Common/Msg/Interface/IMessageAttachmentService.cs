using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Common.Msg.Domain;
using wojilu.Web;

namespace wojilu.Common.Msg.Interface {

    public interface IMessageAttachmentService {


        MessageAttachment GetById( int id );
        List<MessageAttachment> GetByMsg( int msgDataId );

        bool IsReceiver( int viewerId, MessageAttachment attachment );
        bool IsSender( int viewerId, MessageAttachment attachment );

        Result SaveFile( HttpFile postedFile );
        void Insert( MessageAttachment attachment );

        Result Delete( int id );


    }

}
