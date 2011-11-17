using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Members.Users.Domain;
using wojilu.Members.Interface;
using wojilu.Drawing;
using wojilu.Web;
using wojilu.Web.Utils;

namespace wojilu.Common.Upload {

    public class TempUploadFileService {

        private static readonly ILog logger = LogManager.GetLogger( typeof( TempUploadFileService ) );

        public virtual Result Upload( HttpFile postedFile ) {
            return Upload( postedFile, null, null );
        }

        public virtual Result Upload( HttpFile postedFile, IMember user, IMember owner ) {

            Result result = Uploader.SaveImg( postedFile );

            if (result.HasErrors) {
                return result;
            }

            String picUrl = strUtil.Join( sys.Path.Photo, result.Info.ToString() );
            String picThumbUrl = Img.GetThumbPath( picUrl );

            String filePath = result.Info.ToString();

            TempUploadFile uploadFile = new TempUploadFile();
            uploadFile.FileSize = postedFile.ContentLength;
            uploadFile.Type = postedFile.ContentType;
            uploadFile.Name = filePath;

            return Upload( uploadFile, (User)user, owner );
        }


        private Result Upload( TempUploadFile tfile, User user, IMember owner ) {

            if (owner != null) {
                tfile.OwnerId = owner.Id;
                tfile.OwnerType = owner.GetType().FullName;
                tfile.OwnerUrl = owner.Url;
            }

            if (user != null) {
                tfile.Creator = user;
                tfile.CreatorUrl = user.Url;
            }

            tfile.Guid = Guid.NewGuid().ToString();

            return db.insert( tfile );
        }


        public virtual Result DeleteTempFile( int id ) {

            TempUploadFile tfile = TempUploadFile.findById( id );
            if (tfile == null) {
                return new Result( "file not found" );
            }

            try {

                tfile.delete();

                Img.DeleteImgAndThumb( tfile.FileUrl );

                return new Result();
            }

            catch (Exception ex) {

                logger.Error( ex.Message );
                logger.Error( ex.StackTrace );

                return new Result( ex.Message );
            }

        }



    }

}
