using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Common.Msg.Domain;
using wojilu.Web;

namespace wojilu.Common.Msg.Interface {

    public interface IMessageAttachmentService {


        MessageAttachment GetById(long id);
        List<MessageAttachment> GetByMsg(long msgDataId);

        bool IsReceiver(long viewerId, MessageAttachment attachment);
        bool IsSender(long viewerId, MessageAttachment attachment);

        Result SaveFile( HttpFile postedFile );
        void Insert( MessageAttachment attachment );

        Result Delete(long id);


    }

}
