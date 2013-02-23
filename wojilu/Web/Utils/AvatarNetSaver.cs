using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using wojilu.Net;
using System.IO;
using wojilu.Drawing;

namespace wojilu.Web.Utils {

    public class AvatarNetSaver : AvatarSaver {

        private static readonly ILog logger = LogManager.GetLogger( typeof( AvatarNetSaver ) );

        private String picUrl;

        public AvatarNetSaver( String _picUrl ) {
            picUrl = _picUrl;
        }

        public override string GetExt() {
            using (WebClient client = new WebClient()) {
                client.Headers.Add( "user-agent", PageLoader.AgentIE6 );
                logger.Info( "get ext, picUrl:" + picUrl );
                client.OpenRead( picUrl );
                String ext = getFileType( client );
                return ext == null ? "jpg" : ext;
            }
        }

        private static String getFileType( WebClient client ) {

            WebHeaderCollection headers = client.ResponseHeaders;
            for (int i = 0; i < headers.Count; i++) {

                String key = headers.GetKey( i );
                String val = headers.Get( i );

                if (key == null) continue;

                //Content-Type : image/jpeg
                if (key.ToLower() == "content-type") {
                    return Img.GetImageExt( val );
                }
            }

            return null;
        }

        public override void Save( string absPath ) {
            using (WebClient client = new WebClient()) {
                client.Headers.Add( "user-agent", PageLoader.AgentIE6 );
                client.DownloadFile( picUrl, absPath );
            }
        }


    }

}
