using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Members.Users.Domain;
using wojilu.Drawing;
using wojilu.Web.Utils;
using System.Threading;
using wojilu.Apps.Photo.Domain;

namespace wojilu.Web.Controller.Admin.Upgrade {

    public class ThumbParam {
        public int StartId;
        public int EndId;
    }

    /// <summary>
    /// 如果重新配置了图片的缩略图信息，可以使用本功能对历史图片重新生成处理
    /// </summary>
    public class PhotoController : ControllerBase {

        private static readonly ILog logger = LogManager.GetLogger( typeof( PhotoController ) );

        public void Index() {

            set( "lnkFaceSave", to( MakeFaceThumb ) );
            set( "lnkPhotoSave", to( MakePhotoThumb ) );

            set( "processLink", to( Process ) );
        }

        public void MakeFaceThumb() {

            lastId = 0;
            msgList = new List<string>();

            ThumbParam obj = new ThumbParam();
            obj.StartId = ctx.PostInt( "startId" );
            obj.EndId = ctx.PostInt( "endId" );

            try {
                new Thread( makeUserFace ).Start( obj );
                echoAjaxOk();

            }
            catch (Exception ex) {
                logger.Error( ex.Message );
                logger.Error( ex.StackTrace );
                echo( "生成错误，请查看日志" );
            }
        }

        public void MakePhotoThumb() {

            lastId = 0;
            msgList = new List<string>();

            ThumbParam obj = new ThumbParam();
            obj.StartId = ctx.PostInt( "startId" );
            obj.EndId = ctx.PostInt( "endId" );

            try {
                new Thread( makePhoto ).Start( obj );
                echoAjaxOk();

            }
            catch (Exception ex) {
                logger.Error( ex.Message );
                logger.Error( ex.StackTrace );
                echo( "生成错误，请查看日志" );
            }
        }

        public void Process() {

            if (msgList == null) {
                echoText( "nostart" );
                return;
            }

            StringBuilder sb = new StringBuilder();
            for (int i = msgList.Count - 1; i >= 0; i--) {
                sb.Append( msgList[i] + "<br/>" );
            }

            echoText( sb.ToString() );
        }

        private void makeUserFace( Object objParam ) {

            List<User> userList = getUsersById( objParam as ThumbParam );

            foreach (User user in userList) {

                lastId = user.Id;

                // 循环生成所有缩略图
                foreach (KeyValuePair<String, ThumbInfo> kv in ThumbConfig.GetAvatarConfig()) {

                    String srcPath = getFaceSrcPath( user.Pic );
                    if (file.Exists( srcPath ) == false) {
                        log( "pic original not exist=" + srcPath );
                        continue;
                    }

                    log( "begin make " + kv.Key + "=>" + srcPath );
                    AvatarUploader.SaveThumbSingle( srcPath, kv.Key, kv.Value );

                }
            }

            log( "操作结束, last user id=" + lastId );

        }

        private void makePhoto( Object objParam ) {

            List<PhotoPost> photoList = getPhotoById( objParam as ThumbParam );

            foreach (PhotoPost x in photoList) {

                lastId = x.Id;

                // 循环生成所有缩略图
                foreach (KeyValuePair<String, ThumbInfo> kv in ThumbConfig.GetPhotoConfig()) {

                    String srcPath = getPhotoSrcPath( x.DataUrl );
                    if (file.Exists( srcPath ) == false) {
                        log( "pic original not exist=" + srcPath );
                        continue;
                    }

                    log( "begin make " + kv.Key + "=>" + srcPath );
                    try {
                        Uploader.SaveThumbSingle( srcPath, kv.Key, kv.Value );
                    }
                    catch (Exception ex) {
                        log( "error=>" + ex.Message );
                    }

                }
            }

            log( "操作结束, last photo id=" + lastId );

        }

        // 这里一定要使用disk路径
        private static String getPhotoSrcPath( string x ) {
            String srcPath = strUtil.Join( sys.Path.DiskPhoto, x );
            srcPath = wojilu.Drawing.Img.GetOriginalPath( srcPath );
            return PathHelper.Map( srcPath );
        }

        private List<PhotoPost> getPhotoById( ThumbParam obj ) {
            List<PhotoPost> list = PhotoPost.find( "Id>=:sid and Id<=:eid order by Id" )
                .set( "sid", obj.StartId )
                .set( "eid", obj.EndId )
                .list();
            return list;
        }

        private static List<User> getUsersById( ThumbParam obj ) {
            List<User> list = User.find( "Id>=:sid and Id<=:eid order by Id" )
                .set( "sid", obj.StartId )
                .set( "eid", obj.EndId )
                .list();
            return list;
        }

        private static String getFaceSrcPath( string userPic ) {
            // face/2012/5/5/288_19_17_4_s.jpg
            String srcPath = strUtil.Join( sys.Path.DiskUpload, userPic );
            srcPath = wojilu.Drawing.Img.GetOriginalPath( srcPath );
            return PathHelper.Map( srcPath );
        }

        private void log( String msg ) {
            logger.Info( msg );
            msgList.Add( msg );
        }

        private static int lastId = 0;
        private static List<String> msgList;



    }


}
