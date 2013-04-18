using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using wojilu.Common.Msg.Domain;
using wojilu.Common.Msg.Interface;
using wojilu.DI;
using wojilu.Web;
using wojilu.Web.Utils;
using wojilu.ORM;
using wojilu.Members.Users.Domain;
using wojilu.Members.Interface;

namespace wojilu.Common.Upload {

    public class UserFileService {

        private static readonly ILog logger = LogManager.GetLogger( typeof( UserFileService ) );

        public UserFile GetById( int id ) {
            return UserFile.findById( id );
        }

        public DataPage<UserFile> GetByType( Type t ) {
            return UserFile.findPage( "DataType='" + t.FullName + "' " );
        }

        public DataPage<UserFile> GetFileByType( Type t ) {
            return UserFile.findPage( " IsPic=0 and DataType='" + t.FullName + "' " );
        }

        public DataPage<UserFile> GetPicByType( Type t ) {
            return UserFile.findPage( "IsPic=1 and DataType='" + t.FullName + "' " );
        }

        public List<UserFile> GetByIds( string ids, Type t ) {

            if (strUtil.IsNullOrEmpty( ids )) return new List<UserFile>();

            return UserFile.find( "DataType='" + t.FullName + "' and Id in (" + ids + ")" ).list();
        }

        public List<UserFile> GetPicByIds( string ids, Type t ) {

            if (strUtil.IsNullOrEmpty( ids )) return new List<UserFile>();

            return UserFile.find( "IsPic=1 and DataType='" + t.FullName + "' and Id in (" + ids + ")" ).list();
        }

        public List<UserFile> GetByData( Object obj ) {

            IEntity entity = obj as IEntity;

            return UserFile.find( "DataId=" + entity.Id + " and DataType='" + obj.GetType().FullName + "' order by Id asc" ).list();
        }

        public Result SaveFile( HttpFile postedFile, String ip, User creator, IMember owner ) {

            Result result = Uploader.SaveFileOrImage( postedFile );
            if (result.HasErrors) return result;

            UserFile att = new UserFile();
            att.Name = System.IO.Path.GetFileName( postedFile.FileName );
            att.PathRelative = result.Info.ToString();
            att.ContentType = postedFile.ContentType;
            att.Ext = System.IO.Path.GetExtension( att.Name );
            att.FileSize = postedFile.ContentLength;
            att.IsPic = Uploader.IsImage( postedFile ) ? 1 : 0;

            att.Ip = ip;

            if (creator != null) {
                att.Creator = creator;
                att.CreatorUrl = creator.Url;
            }

            if (owner != null) {
                att.OwnerId = owner.Id;
                att.OwnerType = owner.GetType().FullName;
                att.OwnerUrl = owner.Url;
            }

            try {
                this.Insert( att );
            }
            catch (Exception ex) {
                result.Add( ex.Message );
                return result;
            }

            result.Info = att;

            return result;
        }

        public Result Insert( UserFile attachment ) {
            return attachment.insert();
        }

        public Result Delete( int id ) {

            Result result = new Result();

            UserFile attachment = GetById( id );
            if (attachment == null) {
                result.Add( lang.get( "exDataNotFound" ) );
                return result;
            }

            attachment.delete();
            countDataCount( attachment );

            String filePath = strUtil.Join( sys.Path.DiskPhoto, attachment.PathRelative );
            String absPath = PathHelper.Map( filePath );


            if (file.Exists( absPath )) {

                try {
                    file.Delete( absPath );
                    result.Info = attachment;
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

        private void countDataCount( UserFile uFile ) {

            if (uFile.DataId <= 0 || strUtil.IsNullOrEmpty( uFile.DataType )) return;

            int fileCount = UserFile.find( "DataId=" + uFile.DataId + " and DataType=:dtype" )
                .set( "dtype", uFile.DataType )
                .count();

            EntityInfo ei = Entity.GetInfo( uFile.DataType );
            IEntity post = ndb.findById( ei.Type, uFile.DataId );
            if (post != null) {

                String propertyName = "AttachmentCount";
                if (ei.GetProperty( propertyName ) != null) {
                    post.set( propertyName, fileCount );
                    db.update( post );
                }
                else {
                    logger.Warn( "property 'AttachmentCount' not exist: "+ ei.Type.FullName );
                }
            }
        }

        public void UpdateDataInfo( UserFile uFile ) {
            db.update( uFile );
            countDataCount( uFile );
        }


    }

}
