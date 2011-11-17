/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;

using wojilu.IO;
using wojilu.Apps.Forum.Domain;
using wojilu.Members.Users.Domain;
using wojilu.Members.Interface;
using wojilu.Apps.Forum.Interface;
using wojilu.Drawing;
using wojilu.Web.Utils;

namespace wojilu.Apps.Forum.Service {


    public class AttachmentService : IAttachmentService {

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
            return AttachmentTemp.findPage( "OwnerId="+userId, pageSize );
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

        public virtual void AddHits( Attachment attachment ) {
            attachment.Downloads++;
            db.update( attachment, "Downloads" );
        }

        public virtual void UpdateName( Attachment attachment, string name ) {
            attachment.Description = name;
            attachment.update( "Description" );
        }

        public virtual void UpdateFile( Attachment a, String oldFilePath ) {

            a.update();

            if (a.IsImage)
                Img.DeleteImgAndThumb( oldFilePath );
            else
                Img.DeleteFile( oldFilePath );

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

            return result;
        }

        public void CreateByTemp( String ids, ForumTopic topic ) {

            int[] arrIds = cvt.ToIntArray( ids );
            if (arrIds.Length == 0) return;

            ForumTopicService topicService = new ForumTopicService();

            ForumPost post = topicService.GetPostByTopic( topic.Id );
            
            int attachmentCount = 0;
            foreach (int id in arrIds) {

                AttachmentTemp at = AttachmentTemp.findById( id );
                if (at == null) continue;

                Attachment a = new Attachment();

                a.AppId = at.AppId;
                a.Guid = at.Guid;

                a.FileSize = at.FileSize;
                a.Type = at.Type;
                a.Name = at.Name;

                a.Description = at.Description;
                a.ReadPermission = at.ReadPermission;
                a.Price = at.Price;


                a.TopicId = topic.Id;
                a.PostId = post.Id;

                a.OwnerId = topic.OwnerId;
                a.OwnerType = topic.OwnerType;
                a.OwnerUrl = topic.OwnerUrl;
                a.Creator = topic.Creator;
                a.CreatorUrl = topic.CreatorUrl;

                a.insert();

                at.delete();

                attachmentCount++;

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

