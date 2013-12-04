/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web;
using wojilu.Web.Mvc;

using wojilu.Common;
using wojilu.Common.AppBase;
using wojilu.Common.Jobs;
using wojilu.Common.MemberApp.Interface;
using wojilu.Common.Tags;
using wojilu.Common.Upload;

using wojilu.Members.Users.Interface;
using wojilu.Members.Users.Domain;
using wojilu.Members.Users.Service;

using wojilu.Common.Money.Domain;
using wojilu.Common.Money.Service;
using wojilu.Common.Money.Interface;

using wojilu.Apps.Blog.Domain;
using wojilu.Apps.Blog.Interface;
using wojilu.Common.Microblogs.Service;
using wojilu.Common.Microblogs.Interface;
using wojilu.Common.Microblogs;

namespace wojilu.Apps.Blog.Service {

    public class BlogPostService : IBlogPostService {

        public virtual IFriendService friendService { get; set; }
        public virtual IUserIncomeService incomeService { get; set; }
        public virtual IMicroblogService microblogService { get; set; }

        public BlogPostService() {
            friendService = new FriendService();
            incomeService = new UserIncomeService();
            microblogService = new MicroblogService();
        }

        public virtual void AddHits( BlogPost post ) {
            HitsJob.Add( post );
        }

        public virtual BlogPost GetById_ForAdmin(long id) {
            return db.findById<BlogPost>( id );
        }

        public virtual BlogPost GetById(long id, long ownerId) {
            BlogPost post = GetById_ForAdmin( id );
            if (post == null) return null;
            if (post.SaveStatus == SaveStatus.Delete || post.SaveStatus == SaveStatus.SysDelete) return null;
            if (post.OwnerId != ownerId) return null;
            return post;
        }

        public virtual BlogPost GetDraft(long postId) {
            BlogPost post = GetById_ForAdmin( postId );
            if (post.SaveStatus != SaveStatus.Draft) return null;
            return post;
        }

        public virtual List<BlogPost> GetNewBlog(long appId, int count) {
            return db.find<BlogPost>( "AppId=" + appId + " and SaveStatus=" + SaveStatus.Normal + " order by Created desc, Id desc" ).list( count );
        }

        public virtual List<BlogPost> GetByApp(long appId) {
            String condition = "AppId=" + appId + " and SaveStatus=" + SaveStatus.Normal + " order by Created desc, Id desc";
            return db.find<BlogPost>( condition ).list();
        }

        public virtual DataPage<BlogPost> GetPage() {
            return db.findPage<BlogPost>( "SaveStatus=" + SaveStatus.Normal );
        }

        public virtual DataPage<BlogPost> GetPage(long appId, int pageSize) {
            String condition = "AppId=" + appId + " and SaveStatus=" + SaveStatus.Normal + " and IsTop=0 order by Id desc";
            return db.findPage<BlogPost>( condition, pageSize );
        }



        public virtual DataPage<BlogPost> GetDraft(long appId, int pageSize) {

            String condition = "AppId=" + appId;
            condition = condition + " and (SaveStatus=" + SaveStatus.Draft + " ) order by Created desc, Id desc";
            return db.findPage<BlogPost>( condition, pageSize );
        }

        public virtual DataPage<BlogPost> GetTrash(long appId, int pageSize) {

            String condition = "AppId=" + appId;
            condition = condition + " and (SaveStatus=" + SaveStatus.Delete + " ) order by Created desc, Id desc";
            return db.findPage<BlogPost>( condition, pageSize );
        }

        public virtual DataPage<BlogPost> GetPageByCategory(long appId, long categoryId, int pageSize) {

            String condition = "AppId=" + appId;
            if (categoryId > 0) condition = condition + " and Category.Id=" + categoryId;
            condition = condition + " and SaveStatus=" + SaveStatus.Normal + " order by Created desc, Id desc";
            return db.findPage<BlogPost>( condition, pageSize );
        }


        public virtual RssChannel GetRssByAppId(long appId, int count) {
            return getRssByApp( appId, count );
        }

        public virtual RssChannel getRssByApp(long appId, int count) {

            List<BlogPost> newListAll = null;

            if (appId > 0)
                newListAll = this.GetNewBlog( appId, count );
            else
                newListAll = this.GetNewListAll( count );

            BlogApp app = BlogApp.findById( appId );
            User user = User.findById( app.OwnerId );
            IMemberApp uapp = new UserAppService().GetByApp( app );

            RssChannel channel = new RssChannel();
            channel.Title = uapp.Name + " -- " + user.Name + "'s blog";
            channel.Link = Link.ToMember( user );

            foreach (BlogPost post in newListAll) {

                RssItem rssItem = new RssItem();
                rssItem.Title = post.Title;
                rssItem.Description = post.Content;
                rssItem.Category = post.Category.Name;
                rssItem.PubDate = post.Created;
                rssItem.Author = post.Creator.Name;

                rssItem["CreatorLink"] = channel.Link;
                rssItem["CreatorFace"] = post.Creator.PicSmall;
                rssItem.Link = alink.ToAppData( post );

                channel.RssItems.Add( rssItem );
            }


            return channel;
        }

        public virtual List<BlogPost> GetTop(long appId, int count) {
            return db.find<BlogPost>( "IsTop=1 and AppId=" + appId ).list( count );
        }

        //------------------------------------------ 聚合数据 --------------------------------------------------

        public virtual List<IBinderValue> GetTopNew( int count ) {

            List<BlogPost> list = GetNewListAll( count );
            return SysBlogService.getResult( list );
        }

        private List<BlogPost> GetNewListAll( int count ) {
            if (count <= 0) count = 10;
            return db.find<BlogPost>( "SaveStatus=" + SaveStatus.Normal ).list( count );
        }

        public virtual List<IBinderValue> GetMyRecent(int count, long userId) {
            if (count == 0) count = 5;
            List<BlogPost> list = db.find<BlogPost>( "Creator.Id=" + userId + " and SaveStatus=" + SaveStatus.Normal ).list( count );
            return SysBlogService.getResult( list );
        }



        //------------------------------------------ 好友数据 --------------------------------------------------

        public virtual DataPage<BlogPost> GetFriendsBlog(long userId, long friendId) {

            String condition;
            if (friendId > 0)
                condition = "CreatorId=" + friendId;
            else {
                String ids = friendService.FindFriendsIds( userId );
                if (strUtil.IsNullOrEmpty( ids )) return DataPage<BlogPost>.GetEmpty();
                condition = "CreatorId in (" + ids + ")";
            }

            condition = condition + " and SaveStatus=" + SaveStatus.Normal;

            return db.findPage<BlogPost>( condition );
        }

        //------------------------------------------ 插入与更新 --------------------------------------------------


        public virtual Result Insert(BlogPost post, long[] attachmentIds) {

            Result result = db.insert( post );
            if (result.IsValid) {

                saveAttachments( post, attachmentIds );

                updateAppCount( post );
                TagService.SaveDataTag( post, post.Tags );
                AddFeedInfo( post );

            }

            return result;
        }




        private void saveAttachments(BlogPost post, long[] attachmentIds) {

            if (attachmentIds == null || attachmentIds.Length == 0) return;

            int count = 0;
            foreach (int id in attachmentIds) {

                UserFile att = UserFile.findById( id );
                if (att == null) continue;

                att.DataId = post.Id;
                att.DataType = typeof( BlogPost ).FullName;

                att.Creator = post.Creator;
                att.CreatorUrl = post.CreatorUrl;

                att.OwnerId = post.OwnerId;
                att.OwnerType = post.OwnerType;
                att.OwnerUrl = post.OwnerUrl;

                att.update();

                count++;
            }

            if (count > 0) {
                post.AttachmentCount = count;
                post.update();
            }

        }

        public virtual Result Insert( BlogPost post ) {

            Result result = db.insert( post );
            if (result.IsValid) {
                String msg = string.Format( "发布博客 <a href=\"{0}\">{1}</a>，得到奖励", alink.ToAppData( post ), post.Title );
                incomeService.AddIncome( post.Creator, UserAction.Blog_CreatePost.Id, msg );
                updateAppCount( post );
                TagService.SaveDataTag( post, post.Tags );
                AddFeedInfo( post );
            }

            return result;
        }

        private static void updateAppCount( BlogPost post ) {
            int count = db.count<BlogPost>( "AppId=" + post.AppId );
            BlogApp app = BlogApp.findById( post.AppId );
            app.BlogCount = count;
            app.update( "BlogCount" );
        }

        public virtual void AddFeedInfo( BlogPost data ) {

            String msg = GetFeedMsg( data );

            microblogService.AddSimple( data.Creator, msg, typeof( BlogPost ).FullName, data.Id, data.Ip );
        }

        public virtual String GetFeedMsg( BlogPost data ) {
            String lnkPost = alink.ToAppData( data );
            String summary = strUtil.SubString( data.SummaryInfo, MicroblogAppSetting.Instance.MicroblogContentMax );

            return MbTemplate.GetFeed( "写了博客", data.Title, lnkPost, summary, null );
        }


        public virtual Result InsertDraft( BlogPost post ) {
            Result result = db.insert( post );
            if (result.IsValid) {
                TagService.SaveDataTag( post, post.Tags );
            }
            return result;
        }

        public virtual Result UpdateDraft( BlogPost post ) {
            Result result = db.update( post );
            if (result.IsValid) {
                TagService.SaveDataTag( post, post.Tags );
            }
            return result;
        }

        public virtual Result PublishDraft( BlogPost post ) {

            post.SaveStatus = SaveStatus.Normal;

            Result result = db.update( post );
            if (result.IsValid) {
                updateAppCount( post );
                TagService.SaveDataTag( post, post.Tags );
                AddFeedInfo( post );
            }
            return result;
        }

        //-----------------------------------------------------------------------------------------------------

        public virtual void SetTop(string ids, long appId) {
            db.updateBatch<BlogPost>( "set IsTop=1", condition( ids, appId ) );
        }

        public virtual void SetUntop(string ids, long appId) {
            db.updateBatch<BlogPost>( "set IsTop=0", condition( ids, appId ) );
        }

        public virtual void SetPick(string ids, long appId) {
            db.updateBatch<BlogPost>( "set IsPick=1", condition( ids, appId ) );
        }

        public virtual void SetUnpick(string ids, long appId) {
            db.updateBatch<BlogPost>( "set IsPick=0", condition( ids, appId ) );
        }

        public virtual void ChangeCategory(long categoryId, string ids, long appId) {
            db.updateBatch<BlogPost>( "set CategoryId=" + categoryId, condition( ids, appId ) );
        }

        public virtual void Delete(string ids, long appId) {
            db.updateBatch<BlogPost>( "set SaveStatus=" + SaveStatus.Delete, condition( ids, appId ) );
        }

        public virtual void UnDelete(string ids, long appId) {
            db.updateBatch<BlogPost>( "set SaveStatus=" + SaveStatus.Normal, condition( ids, appId ) );
        }

        public virtual void DeleteTrue(string ids, long appId) {
            db.deleteBatch<BlogPost>( condition( ids, appId ) );
            // TODO restats user blog count
        }

        private string condition(string ids, long appId) {
            if (cvt.IsIdListValid( ids ) == false) throw new ArgumentException( "ids is invalid" );
            return string.Format( "Id in ({0}) and AppId={1}", ids, appId );
        }


        public virtual int GetCountByCategory(long id) {
            return db.count<BlogPost>( "Category.Id=" + id + " " );
        }


        public virtual int GetCountByUser(long userId) {
            return db.count<BlogPost>( "OwnerId=" + userId + " and SaveStatus=" + SaveStatus.Normal );
        }
    }
}

