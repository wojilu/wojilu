using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Members.Users.Domain;
using wojilu.Drawing;
using wojilu.Web.Utils;
using System.Threading;

namespace wojilu.Web.Controller.Admin.Upgrade {

    public class ThumbParam {
        public int StartId;
        public int EndId;
    }

    /// <summary>
    /// 如果头像没有生成某种类型的缩略图(比如只生成 small, middle, 没有生成 big 类型)，可以使用本功能对历史头像进行修补
    /// </summary>
    public class FaceController : ControllerBase {

        private static readonly ILog logger = LogManager.GetLogger( typeof( FaceController ) );

        public void Index() {

            set( "lnkSave", to( MakeThumb ) );
            set( "processLink", to( Process ) );
        }

        public void MakeThumb() {

            lastId = 0;
            msgList = new List<string>();

            ThumbParam obj = new ThumbParam();
            obj.StartId = ctx.PostInt( "startId" );
            obj.EndId = ctx.PostInt( "endId" );

            try {
                String thumbType = ctx.Post( "thumbType" );
                if (thumbType == "s") {
                    new Thread( makeSmall ).Start( obj );
                }
                else if (thumbType == "m") {
                    new Thread( makeMiddle ).Start( obj );
                }
                else if (thumbType == "b") {
                    new Thread( makeBig ).Start( obj );
                }
                else {
                    echo( "缩略图类型错误：" + thumbType );
                    return;
                }

                echoAjaxOk();

            }
            catch (Exception ex) {
                logger.Error( ex.Message );
                logger.Error( ex.StackTrace );
                echo( "生成错误，请查看日志" );
            }
        }

        public void Process() {

            StringBuilder sb = new StringBuilder();
            for (int i = msgList.Count - 1; i >= 0; i--) {
                sb.Append( msgList[i] + "<br/>" );
            }

            echoText( sb.ToString() );
        }

        private void makeSmall( Object objParam ) {

            List<User> userList = getByStartEndId( objParam as ThumbParam );

            foreach (User user in userList) {

                lastId = user.Id;

                if (picExist( user.Pic, ThumbnailType.Small )) {
                    log( "pic exist=id" + user.Id + ", pic=" + user.Pic + ", small" );
                    continue;
                }

                int x = config.Instance.Site.AvatarThumbWidth;
                int y = config.Instance.Site.AvatarThumbHeight;

                String srcPath = getSrcPath( user.Pic );
                if (file.Exists( srcPath ) == false) {
                    log( "pic original not exist=" + srcPath );
                    continue;
                }

                log( "begin make small=>" + srcPath );
                AvatarUploader.saveAvatarPrivate( x, y, srcPath, ThumbnailType.Small );
            }

            log( "操作结束, last user id=" + lastId );
        }

        private void makeMiddle( Object objParam ) {

            List<User> userList = getByStartEndId( objParam as ThumbParam );
            foreach (User user in userList) {

                lastId = user.Id;

                if (picExist( user.Pic, ThumbnailType.Medium )) {
                    log( "pic exist=id" + user.Id + ", pic=" + user.Pic + ", middle" );
                    continue;
                }

                int x = config.Instance.Site.AvatarThumbWidthMedium;
                int y = config.Instance.Site.AvatarThumbHeightMedium;
                String srcPath = getSrcPath( user.Pic );
                if (file.Exists( srcPath ) == false) {
                    log( "pic original not exist=" + srcPath );
                    continue;
                }
                log( "begin make middle=>" + srcPath );
                AvatarUploader.saveAvatarPrivate( x, y, srcPath, ThumbnailType.Medium );
            }

            log( "操作结束, last user id=" + lastId );
        }

        private void makeBig( Object objParam ) {

            List<User> userList = getByStartEndId( objParam as ThumbParam );

            foreach (User user in userList) {

                lastId = user.Id;

                if (picExist( user.Pic, ThumbnailType.Big )) {
                    log( "pic exist=id" + user.Id + ", pic=" + user.Pic + ", big" );
                    continue;
                }

                int x = config.Instance.Site.AvatarThumbHeightBig;
                int y = config.Instance.Site.AvatarThumbWidthBig;
                String srcPath = getSrcPath( user.Pic );
                if (file.Exists( srcPath ) == false) {
                    log( "pic original not exist=" + srcPath );
                    continue;
                }
                log( "begin make big=>" + srcPath );

                Boolean formatValid = AvatarUploader.saveAvatarPrivate( x, y, srcPath, ThumbnailType.Big );
                if (formatValid == false) {
                    msgList.Add( "■■■■■■ format error=" + srcPath );
                }
            }

            log( "操作结束, last user id=" + lastId );
        }

        private static List<User> getByStartEndId( ThumbParam obj ) {
            List<User> userList = User.find( "Id>=:sid and Id<=:eid order by Id" )
                .set( "sid", obj.StartId )
                .set( "eid", obj.EndId )
                .list();
            return userList;
        }

        private bool picExist( string userPic, ThumbnailType ttype ) {

            String srcPath = getSrcPath( userPic );
            String thumbPath = Img.GetThumbPath( srcPath, ttype );

            return file.Exists( thumbPath );
        }

        private static String getSrcPath( string userPic ) {
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
