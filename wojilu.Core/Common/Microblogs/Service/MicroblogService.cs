/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Utils;

using wojilu.Common.AppBase;

using wojilu.Common.Feeds.Domain;
using wojilu.Common.Feeds.Interface;
using wojilu.Common.Feeds.Service;

using wojilu.Common.Microblogs.Domain;
using wojilu.Common.Microblogs.Interface;
using wojilu.Common.Microblogs.Parser;

using wojilu.Common.Msg.Interface;
using wojilu.Common.Msg.Service;

using wojilu.Common.Tags;

using wojilu.Members.Users.Domain;
using wojilu.Members.Users.Interface;
using wojilu.Members.Users.Service;

namespace wojilu.Common.Microblogs.Service {

    public class MicroblogService : IMicroblogService {

        public virtual INotificationService nfService { get; set; }
        public virtual IFriendService friendService { get; set; }
        public virtual IFollowerService followerService { get; set; }

        public MicroblogService() {
            nfService = new NotificationService();
            friendService = new FriendService();
            followerService = new FollowerService();
        }

        private static String showCondition() {
            return " and SaveStatus=" + SaveStatus.Normal;
        }

        public virtual int CountByUser(long userId) {
            return Microblog.count( "UserId=" + userId + showCondition() );
        }

        public virtual List<Microblog> GetByUser( int count, int userId ) {
            if (count <= 0) count = 10;
            return Microblog.find( "User.Id=" + userId + showCondition() ).list( count );
        }

        public virtual List<IBinderValue> GetMyRecent( int count, int userId ) {

            return SysMicroblogService.populatePost( GetByUser( count, userId ) );
        }

        public virtual Microblog GetById(long id) {
            return Microblog.findById( id );
        }

        public virtual Microblog GetFirst(long userId) {
            return Microblog.find( "User.Id=" + userId + showCondition() ).first();
        }

        public virtual List<Microblog> GetCurrent(int count, long userId) {
            if (count <= 0) count = 1;
            return Microblog.find( "User.Id=" + userId + showCondition() ).list( count );
        }

        public virtual DataPage<Microblog> GetPageList(long userId, int pageSize) {

            DataPage<Microblog> list = Microblog.findPage( "UserId=" + userId + showCondition(), pageSize );
            return list;
        }


        public virtual DataPage<Microblog> GetFollowingPage(long ownerId, int pageSize) {

            String followingIds = getFriendAndFollowingIds( ownerId );

            return Microblog.findPage( "UserId in (" + followingIds + ")" + showCondition(), pageSize );

        }

        public virtual DataPage<Microblog> GetFollowingPage(long ownerId, string searchKey) {

            String followingIds = getFriendAndFollowingIds( ownerId );
            searchKey = strUtil.SqlClean( searchKey, 10 );
            if( strUtil.IsNullOrEmpty( searchKey) )
                return Microblog.findPage( "UserId in (" + followingIds + ")" + showCondition() );
            return Microblog.findPage( "UserId in (" + followingIds + ") and Content like '%" + searchKey + "%'" + showCondition() );

        }

        private string getFriendAndFollowingIds(long userId) {
            String friendIds = friendService.FindFriendsIds( userId );
            String followingIds = followerService.GetFollowingIds( userId );

            String ids = strUtil.Join( friendIds, followingIds, "," );
            ids = strUtil.Join( ids, userId.ToString(), "," );
            ids = ids.Trim().TrimEnd( ',' ).TrimStart( ',' );
            return ids;
        }

        public virtual void Add(User creator, string msg, string dataType, long dataId, string ip) {

            Microblog x = new Microblog();
            x.User = creator;
            x.Content = msg;
            x.Ip = ip;

            x.DataType = dataType;
            x.DataId = dataId;

            this.Insert( x, 0 );
        }

        /// <summary>
        /// 纯粹插入数据库，不检查表情、at用户、不处理tag；不处理转发
        /// </summary>
        /// <param name="creator"></param>
        /// <param name="msg"></param>
        /// <param name="dataType"></param>
        /// <param name="dataId"></param>
        /// <param name="ip"></param>
        public virtual void AddSimple(User creator, string msg, string dataType, long dataId, string ip) {

            Microblog x = new Microblog();
            x.User = creator;
            x.Content = msg;
            x.Ip = ip;

            x.DataType = dataType;
            x.DataId = dataId;

            x.insert();
        }

        /// <summary>
        /// 不展示在信息流中的数据，可以供管理员和自己查看，但朋友看不到
        /// </summary>
        /// <param name="creator"></param>
        /// <param name="msg"></param>
        /// <param name="dataType"></param>
        /// <param name="dataId"></param>
        /// <param name="ip"></param>
        public virtual void AddSimplePrivate(User creator, string msg, string dataType, long dataId, string ip) {

            Microblog x = new Microblog();
            x.User = creator;
            x.Content = msg;
            x.Ip = ip;

            x.DataType = dataType;
            x.DataId = dataId;

            x.SaveStatus = SaveStatus.Private;

            x.insert();
        }

        public virtual void Insert( Microblog blog ) {

            blog.Content = strUtil.SubString( blog.Content, MicroblogAppSetting.Instance.MicroblogContentMax );

            Insert( blog, 0 );
        }


        public virtual void Insert( Microblog blog, int i ) {


            String rcontent = blog.Content;

            MicroblogBinder smbinder = new MicroblogBinder();

            MicroblogParser mp = new MicroblogParser( blog.Content, smbinder );
            mp.Process();

            blog.Content = mp.ToString();

            blog.Content = processEmotions( blog.Content );

            Result result = blog.insert();
            
            if( i==0 ) {

                // 保存tag
                TagService.SaveDataTag( blog, mp.GetTagList() );

                // 发通知
                addNotification( smbinder.GetValidUsers(), blog );
            }

            // 转发需要刷新原帖的转发量
            if (blog.ParentId > 0) {
                Microblog parent = GetById( blog.ParentId );
                if (parent != null) {
                    parent.Reposts = Microblog.count( "ParentId=" + parent.Id );
                    parent.update( "Reposts" );
                }
            }

            //if (result.IsValid) addFeedInfo( blog );

        }

        private string processEmotions( string content ) {
            Dictionary<string, string> map = WebHelper.GetEmotions();
            foreach (KeyValuePair<string, string> kv in map) {
                content = content.Replace( "[" + kv.Key + "]", string.Format( "<img src=\"{0}\"/>", kv.Value ) );
            }
            return content;
        }

        private void addNotification( List<User> users, Microblog blog ) {

            // 给@用户发通知
            foreach (User u in users) {

                MicroblogAt mat = new MicroblogAt();
                mat.Microblog = blog;
                mat.User = u;
                mat.insert();

                u.MicroblogAt++;
                u.update( "MicroblogAt" );

                u.MicroblogAtUnread++;
                u.update( "MicroblogAtUnread" );

            }
        }

        //----------------------------------------------------------------------------

        public virtual void Delete( Microblog blog ) {

            if (blog == null) throw new ArgumentNullException( "blog" );

            blog.SaveStatus = SaveStatus.Delete;
            blog.delete();

        }

        public virtual void DeleteBatch( string ids ) {

            int[] arrIds = cvt.ToIntArray( ids );
            if (arrIds.Length == 0) return;

            Microblog.updateBatch( "SaveStatus=" + SaveStatus.Delete, "id in (" + ids + ")" );
        }

    }

}
