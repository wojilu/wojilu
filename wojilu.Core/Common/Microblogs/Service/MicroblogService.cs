/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Text;
using System.Collections.Generic;

using wojilu.Common.Feeds.Service;
using wojilu.Common.Microblogs.Domain;
using wojilu.Common.Feeds.Domain;
using wojilu.Common.Msg.Service;
using wojilu.Common.Msg.Enum;
using wojilu.Common.Feeds.Interface;
using wojilu.Common.Msg.Interface;
using wojilu.Common.Microblogs.Interface;
using wojilu.Members.Users.Domain;
using wojilu.Serialization;
using wojilu.Common.Microblogs.Parser;
using wojilu.Common.Tags;
using wojilu.Members.Users.Interface;
using wojilu.Members.Users.Service;
using wojilu.Web.Utils;

namespace wojilu.Common.Microblogs.Service {

    public class MicroblogService : IMicroblogService {


        public virtual IFeedService feedService { get; set; }
        public virtual INotificationService nfService { get; set; }
        public virtual IFriendService friendService { get; set; }
        public virtual IFollowerService followerService { get; set; }

        public MicroblogService() {
            feedService = new FeedService();
            nfService = new NotificationService();
            friendService = new FriendService();
            followerService = new FollowerService();
        }

        public virtual int CountByUser( int userId ) {
            return Microblog.count( "UserId=" + userId );
        }

        public virtual List<Microblog> GetByUser( int count, int userId ) {
            if (count <= 0) count = 10;
            return Microblog.find( "User.Id=" + userId ).list( count );
        }

        public virtual List<IBinderValue> GetMyRecent( int count, int userId ) {

            return SysMicroblogService.populatePost( GetByUser( count, userId ) );
        }

        public virtual Microblog GetById( int id ) {
            return Microblog.findById( id );
        }

        public virtual Microblog GetFirst( int userId ) {
            return Microblog.find( "User.Id=" + userId ).first();
        }

        public virtual List<Microblog> GetCurrent( int count, int userId ) {
            if (count <= 0) count = 1;
            return Microblog.find( "User.Id=" + userId ).list( count );
        }

        public virtual DataPage<Microblog> GetPageList( int userId, int pageSize ) {

            DataPage<Microblog> list = Microblog.findPage( "UserId=" + userId, pageSize );
            return list;
        }

        public virtual DataPage<Microblog> GetPageListAll( int pageSize ) {

            DataPage<Microblog> list = Microblog.findPage( "", pageSize );
            return list;
        }

        public virtual DataPage<Microblog> GetFollowingPage( int ownerId, int pageSize ) {

            String followingIds = getFriendAndFollowingIds( ownerId );

            return Microblog.findPage( "UserId in (" + followingIds + ")", pageSize );

        }

        public virtual DataPage<Microblog> GetFollowingPage( int ownerId, string searchKey ) {

            String followingIds = getFriendAndFollowingIds( ownerId );
            searchKey = strUtil.SqlClean( searchKey, 10 );
            if( strUtil.IsNullOrEmpty( searchKey) )
                return Microblog.findPage( "UserId in (" + followingIds + ")" );
            return Microblog.findPage( "UserId in (" + followingIds + ") and Content like '%"+searchKey+"%'" );

        }

        private String getFriendAndFollowingIds( int userId ) {
            String friendIds = friendService.FindFriendsIds( userId );
            String followingIds = followerService.GetFollowingIds( userId );

            String ids = strUtil.Join( friendIds, followingIds, "," );
            ids = strUtil.Join( ids, userId.ToString(), "," );
            ids = ids.Trim().TrimEnd( ',' ).TrimStart( ',' );
            return ids;
        }

        //--------------------------------------

        //public virtual void InsertBig( Microblog blog ) {

        //    List<String> arrContent = strUtil.SplitByNum( blog.Content, config.Instance.Site.MicroblogContentMax );

        //    for (int i = 0; i < arrContent.Count; i++) {

        //        saveMicroblogOther( blog, arrContent, i );
        //    }
        //}

        //private void saveMicroblogOther( Microblog ob, List<String> arrContent, int i ) {

        //    Microblog blog = new Microblog();

        //    if (arrContent.Count > 1) {
        //        blog.Content = "(" + (i + 1) + ")" + arrContent[i];
        //    }
        //    else {
        //        blog.Content = arrContent[i];
        //    }
        //    if (i == 0) {
        //        blog.Pic = ob.Pic;
        //        blog.FlashUrl = ob.FlashUrl;
        //        blog.PageUrl = ob.PageUrl;
        //        blog.PicUrl = ob.PicUrl;
        //    }

        //    blog.Ip = ob.Ip;
        //    blog.User = ob.User;




        //    Insert( blog, i );
        //}

        public virtual void Insert( Microblog blog ) {

            blog.Content = strUtil.SubString( blog.Content, config.Instance.Site.MicroblogContentMax );

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

            if (result.IsValid) addFeedInfo( blog );

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

                //String msg = string.Format( "有微博提到了您:<a href=\"{0}\">{1}</a>", lnk, strUtil.ParseHtml( blog.Content, 30 ) );
                //nfService.send( u.Id, msg );

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

        private void addFeedInfo( Microblog log ) {
            Feed feed = new Feed();
            feed.Creator = log.User;
            feed.DataType = typeof( Microblog ).FullName;
            feed.DataId = log.Id;

            // 转发微博信息
            String pbody = "";
            if (log.ParentId > 0) {
                Microblog parent = GetById( log.ParentId );
                if (parent == null) {
                    pbody = " [被转微博已被原作者删除]";
                }
                else {
                    pbody = ": [转]" + parent.Content;
                }
            }

            feed.TitleTemplate = "{*actor*} :" + strUtil.SubString( log.Content + pbody, 230 );

            if (strUtil.HasText( log.Pic )) {

                Dictionary<String, String> data = new Dictionary<string, string>();
                data.Add( "pic", "<img src=\"" + log.PicSmall + "\" />" );

                feed.BodyTemplate = "{*pic*}";
                feed.BodyData = JsonString.ConvertDictionary( data );
            }

            feedService.publishUserAction( feed );
        }


        public virtual void Delete( Microblog blog ) {
            blog.delete();
        }

        public virtual void DeleteBatch( string ids ) {

            int[] arrIds = cvt.ToIntArray( ids );
            if (arrIds.Length == 0) return;

            Microblog.deleteBatch( "id in (" + ids + ")" );
        }

    }

}
