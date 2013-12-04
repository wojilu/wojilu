using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using wojilu.Common.Msg.Domain;
using wojilu.Common.Msg.Interface;
using wojilu.Web;
using wojilu.Web.Utils;

namespace wojilu.Common.Msg.Service {

    public class MessageAttachmentService : IMessageAttachmentService {

        private static readonly ILog logger = LogManager.GetLogger( typeof( MessageAttachmentService ) );

        public virtual bool IsReceiver(long viewerId, MessageAttachment attachment) {

            List<Message> msgList = Message.find( "MessageData.Id=" + attachment.MessageData.Id ).list();
            foreach (Message msg in msgList) {
                if (msg.Receiver.Id == viewerId) return true;
            }
            return false;
        }

        public virtual bool IsSender(long viewerId, MessageAttachment attachment) {
            if (attachment.MessageData == null) return false;
            MessageData x = MessageData.findById( attachment.MessageData.Id );
            if (attachment.MessageData.Sender == null) return false;
            return attachment.MessageData.Sender.Id == viewerId;
        }

        public virtual MessageAttachment GetById(long id) {
            return MessageAttachment.findById( id );
        }

        public virtual List<MessageAttachment> GetByMsg(long msgDataId) {
            return MessageAttachment.find( "MessageData.Id=" + msgDataId + " order by Id asc" ).list();
        }

        public virtual Result SaveFile( HttpFile postedFile ) {

            Dictionary<String, String> dic = new Dictionary<String, String>();

            Result result = Uploader.SaveFile( postedFile );
            if (result.HasErrors) return result;

            MessageAttachment att = new MessageAttachment();
            att.Name = System.IO.Path.GetFileName( postedFile.FileName );
            att.Url = result.Info.ToString();
            att.Type = System.IO.Path.GetExtension( postedFile.FileName );
            att.FileSize = postedFile.ContentLength;

            this.Insert( att );

            result.Info = att;

            return result;
        }

        public virtual void Insert( MessageAttachment attachment ) {
            attachment.insert();
        }

        public virtual Result Delete(long id) {

            Result result = new Result();

            MessageAttachment attachment = GetById( id );
            if (attachment == null) {
                result.Add( lang.get( "exDataNotFound" ) );
                return result;
            }

            attachment.delete();

            String filePath = strUtil.Join( sys.Path.DiskPhoto, attachment.Url );
            String absPath = PathHelper.Map( filePath );


            if (file.Exists( absPath )) {

                try {
                    file.Delete( absPath );
                }
                catch (IOException ex) {
                    logger.Error( ex.ToString() );
                    result.Add( ex.ToString() );
                }

            }
            else {
                result.Add( "文件不存在:" + absPath );
            }

            return result;

        }
    }

}
