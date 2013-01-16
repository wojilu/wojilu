/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;

using wojilu.Drawing;
using wojilu.Web.Mvc;

using wojilu.Common.Money.Domain;
using wojilu.Common.Money.Interface;
using wojilu.Common.Money.Service;

using wojilu.Members.Interface;
using wojilu.Members.Users.Domain;

using wojilu.Apps.Forum.Domain;
using wojilu.Apps.Forum.Interface;

namespace wojilu.Apps.Forum.Service {


    public class AttachmentService : IAttachmentService {

        public virtual IUserIncomeService incomeService { get; set; }

        public AttachmentService() {
            incomeService = new UserIncomeService();
        }

        public virtual List<Attachment> GetByPost( int postId ) {
            return Attachment.find( "PostId=" + postId + " order by OrderId desc, Id asc" ).list();
        }

        public virtual List<Attachment> GetTopicAttachments( int topicId ) {
            return Attachment.find( "TopicId=" + topicId + " order by OrderId desc, Id asc" ).list();
        }

        public virtual List<Attachment> GetByTopic( List<ForumPost> list ) {

            if (list.Count == 0) return new List<Attachment>();

            StringBuilder builder = new StringBuilder();
            foreach (ForumPost post in list) {
                builder.Append( post.Id );
                builder.Append( "," );
            }
            return Attachment.find( "PostId in (" + builder.ToString().TrimEnd( ',' ) + ") order by OrderId desc, Id asc" ).list();
        }

        public virtual DataPage<AttachmentTemp> GetByUser( int userId, int pageSize ) {
            return AttachmentTemp.findPage( "OwnerId=" + userId, pageSize );
        }

        public virtual Attachment GetById( int id, String guid ) {
            Attachment a = db.findById<Attachment>( id );
            if (a != null && a.Guid != guid) {
                return null;
            }
            return a;
        }

        public virtual Attachment GetById( int id ) {
            return Attachment.findById( id );
        }

        public virtual void AddHits( Attachment attachment, User downloader ) {
            attachment.Downloads++;
            db.update( attachment, "Downloads" );
            this.SubstractIncome( attachment.TopicId, downloader );
        }

        public virtual void SubstractIncome( int topicId, User viewer ) {

            if (viewer == null || viewer.Id <= 0) return; // 跳过游客

            ForumTopic topic = ForumTopic.findById( topicId );
            if (topic == null) return;

            // 附件作者不用扣除积分
            if (viewer.Id == topic.Creator.Id) return;

            // 如果曾经下载过，不再扣除积分
            if (!isFirstDownload( viewer, topicId )) return;

            // 第一次下载:做记录
            addFirstDownloadLog( viewer, topicId );

            // 第一次下载:扣除积分
            String msg = string.Format( "下载帖子 <a href=\"{0}\">{1}</a> 的附件", alink.ToAppData( topic ), topic.Title );
            incomeService.AddIncome( viewer, UserAction.Forum_DownloadAttachment.Id, msg );
        }

        private Boolean isFirstDownload( User user, int topicId ) {
            AttachmentDownload x = AttachmentDownload.find( "UserId=" + user.Id + " and TopicId=" + topicId ).first();
            return x == null;
        }

        private void addFirstDownloadLog( User user, int topicId ) {
            AttachmentDownload x = new AttachmentDownload();
            x.UserId = user.Id;
            x.TopicId = topicId;
            x.insert();
        }

        //--------------------------------------------------------------------------------------------------------------

        public virtual void UpdateName( Attachment attachment, string name ) {
            attachment.Description = name;
            attachment.update( "Description" );
        }

        public virtual void UpdateFile( User user, Attachment a, String oldFilePath ) {

            a.Created = DateTime.Now;
            a.update();

            if (a.IsImage) {
                Img.DeleteImgAndThumb( oldFilePath );
            }
            else {
                Img.DeleteFile( oldFilePath );
            }

            ForumTopicService topicService = new ForumTopicService();
            ForumPost post = ForumPost.findById( a.PostId );
            topicService.UpdateLastEditInfo( user, post );
        }

        //--------------------------------------------------------------------------------------------------------------


        public virtual Result CreateTemp( AttachmentTemp a, User user, IMember owner ) {

            a.OwnerId = owner.Id;
            a.OwnerType = owner.GetType().FullName;
            a.OwnerUrl = owner.Url;
            a.Creator = user;
            a.CreatorUrl = user.Url;

            a.Guid = Guid.NewGuid().ToString();

            return db.insert( a );
        }


        public virtual Result Create( Attachment a, User user, IMember owner ) {

            a.OwnerId = owner.Id;
            a.OwnerType = owner.GetType().FullName;
            a.OwnerUrl = owner.Url;
            a.Creator = user;
            a.CreatorUrl = user.Url;

            a.Guid = Guid.NewGuid().ToString();

            ForumTopicService topicService = new ForumTopicService();
            ForumPost post = topicService.GetPostByTopic( a.TopicId );
            a.PostId = post.Id;

            Result result = db.insert( a );

            refreshTopicCount( a );

            topicService.UpdateLastEditInfo( user, post );

            return result;
        }

        public void CreateByTemp( String ids, ForumTopic topic ) {

            int[] arrIds = cvt.ToIntArray( ids );
            if (arrIds.Length == 0) return;

            ForumTopicService topicService = new ForumTopicService();

            ForumPost post = topicService.GetPostByTopic( topic.Id );

            int attachmentCount = 0;
            foreach (int id in arrIds) {

                AttachmentTemp _temp = AttachmentTemp.findById( id );
                if (_temp == null) continue;

                Attachment a = new Attachment();

                a.AppId = _temp.AppId;
                a.Guid = _temp.Guid;

                a.FileSize = _temp.FileSize;
                a.Type = _temp.Type;
                a.Name = _temp.Name;

                a.Description = _temp.Description;
                a.ReadPermission = _temp.ReadPermission;
                a.Price = _temp.Price;


                a.TopicId = topic.Id;
                a.PostId = post.Id;

                a.OwnerId = topic.OwnerId;
                a.OwnerType = topic.OwnerType;
                a.OwnerUrl = topic.OwnerUrl;
                a.Creator = topic.Creator;
                a.CreatorUrl = topic.CreatorUrl;

                a.insert();

                _temp.delete();

                attachmentCount++;

            }

            if (attachmentCount > 0) {
                String msg = string.Format( "上传附件 <a href=\"{0}\">{1}</a>，获得奖励", alink.ToAppData( topic ), topic.Title );
                incomeService.AddIncome( topic.Creator, UserAction.Forum_AddAttachment.Id, msg );
            }

            topicService.UpdateAttachments( topic, attachmentCount );
        }

        //--------------------------------------------------------------------------------------------------------------

        public virtual void Delete( int id ) {
            Attachment at = Attachment.findById( id );
            if (at == null) return;

            at.delete();
            Img.DeleteImgAndThumb( at.FileUrl );

            // 重新统计所属主题的附件数
            refreshTopicCount( at );
        }

        private static void refreshTopicCount( Attachment at ) {
            ForumTopic topic = ForumTopic.findById( at.TopicId );
            int count = Attachment.count( "TopicId=" + topic.Id );
            topic.Attachments = count;
            topic.update();
        }


        public virtual void DeleteTempAttachment( int id ) {

            AttachmentTemp at = AttachmentTemp.findById( id );
            if (at == null) return;

            at.delete();

            Img.DeleteImgAndThumb( at.FileUrl );
        }


        public virtual void DeleteByPost( int postId ) {
            List<Attachment> attachments = this.GetByPost( postId );
            foreach (Attachment attachment in attachments) {
                Img.DeleteImgAndThumb( attachment.FileUrl );
            }
            Attachment.deleteBatch( "PostId=" + postId );
        }



    }
}

