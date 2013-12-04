using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Members.Users.Domain;
using wojilu.Members.Users.Interface;
using wojilu.Web.Utils;

namespace wojilu.Members.Users.Service {

    public class UserErrorPicService : IUserErrorPicService {

        public virtual int GetStatus( User user ) {

            UserErrorPic x = UserErrorPic.find( "UserId=" + user.Id + " order by Id desc" ).first();
            if (x == null) return UserErrorPic.StatusFirstUpload;
            if (strUtil.HasText( x.ReviewMsg )) {
                if (x.IsPass == 0) {
                    return UserErrorPic.StatusWaitingUpload;
                }
                else {
                    return UserErrorPic.StatusOk;
                }
            }
            return UserErrorPic.StatusWaitingApprove;
        }

        public virtual int CheckErrorCount( User user ) {

            return UserErrorPic.count( "UserId=" + user.Id + " and ReviewMsg<>'' and IsPass=0" );

        }

        public virtual String GetLastReviewMsg( User user ) {

            UserErrorPic x = UserErrorPic.find( "UserId=" + user.Id + " and ReviewMsg<>'' order by Id desc" ).first();
            if (x == null) return null;
            return x.ReviewMsg;
        }

        public virtual UserErrorPic GetLastLog( User user ) {

            return UserErrorPic.find( "UserId=" + user.Id + " and ReviewMsg<>'' order by Id desc" ).first();
        }

        /// <summary>
        /// 用户重新上传，增加日志，等待审核
        /// </summary>
        /// <param name="user"></param>
        /// <param name="ip"></param>
        public virtual void AddLog( User user, string ip ) {

            UserErrorPic log = new UserErrorPic();
            log.UserId = user.Id;
            log.Ip = ip;
            log.insert();
        }

        public virtual void AddLogAndPass( User user, string ip ) {
            user.IsPicError = 0;
            user.update();

            UserErrorPic log = new UserErrorPic();
            log.UserId = user.Id;
            log.Ip = ip;
            log.IsPass = 1;
            log.ReviewMsg = "auto pass";
            log.insert();
        }

        /// <summary>
        /// 管理员审核没有通过，将日志增加到数据库
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="reviewMsg"></param>
        /// <param name="isNextApprove">下次上传是否前置审核</param>
        public virtual void ApproveError( String ids, String reviewMsg, int isNextApprove, int isDelete ) {

            List<User> userList = getUserList( ids );

            foreach (User user in userList) {

                user.IsPicError = 1;
                user.update();

                UserErrorPic log = new UserErrorPic();
                log.UserId = user.Id;
                log.Ip = "";
                log.ReviewMsg = reviewMsg;
                log.IsNextAutoPass = isNextApprove;
                log.insert();

                if (isDelete == 1 ) {
                    deleteUserPic( user );
                }

            }

        }

        private void deleteUserPic( User user ) {

            if (user.Pic != UserFactory.Guest.Pic) {
                AvatarUploader.Delete( user.Pic );
            }

            user.Pic = "";
            user.update();
        }

        /// <summary>
        /// 管理员审核通过
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="reviewMsg"></param>
        public virtual void ApproveOk( String ids, String reviewMsg ) {
            List<User> userList = getUserList( ids );

            foreach (User user in userList) {

                user.IsPicError = 0;
                user.update();

                UserErrorPic x = UserErrorPic.find( "UserId=" + user.Id + " order by Id desc" ).first();
                x.ReviewMsg = reviewMsg;
                x.IsPass = 1;
                x.update();
            }
        }

        private List<User> getUserList( string ids ) {
            if (!cvt.IsIdListValid( ids )) return new List<User>();
            return User.find( "Id in (" + ids + ")" ).list();
        }

        /// <summary>
        /// 在管理员尚未审核之前，用户重新上传
        /// </summary>
        /// <param name="user"></param>
        /// <param name="ip"></param>
        public virtual void UpdateLastUpload( User user, string ip ) {
            UserErrorPic x = UserErrorPic.find( "UserId=" + user.Id + " order by Id desc" ).first();
            x.Created = DateTime.Now;
            x.Ip = ip;
            x.update();
        }


    }
}
