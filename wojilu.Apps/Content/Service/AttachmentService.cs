/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;

using wojilu.IO;
using wojilu.Drawing;
using wojilu.Web.Utils;

using wojilu.Members.Users.Domain;
using wojilu.Members.Interface;
using wojilu.Apps.Content.Interface;
using wojilu.Apps.Content.Domain;
using wojilu.Web;

namespace wojilu.Apps.Content.Service {


    public class AttachmentService : IAttachmentService {

        public virtual Result SaveFile( HttpFile postedFile ) {

            Result result = Uploader.SaveFileOrImage( postedFile );
            if (result.HasErrors)return result;

            ContentAttachment uploadFile = new ContentAttachment();
            uploadFile.FileSize = postedFile.ContentLength;
            uploadFile.Type = postedFile.ContentType;
            uploadFile.Name = result.Info.ToString();
            uploadFile.Description = System.IO.Path.GetFileName( postedFile.FileName );
            uploadFile.Guid = Guid.NewGuid().ToString();

            uploadFile.insert();

            result.Info = uploadFile;

            return result;
        }

        public virtual List<ContentAttachment> GetAttachmentsByPost( int postId ) {
            return ContentAttachment.find( "PostId=" + postId + " order by OrderId desc, Id asc" ).list();
        }

        public virtual ContentAttachment GetById( int id, String guid ) {
            ContentAttachment a = db.findById<ContentAttachment>( id );
            if (a != null && a.Guid != guid) {
                return null;
            }
            return a;
        }

        public virtual ContentAttachment GetById( int id ) {
            return ContentAttachment.findById( id );
        }

        public virtual void AddHits( ContentAttachment attachment ) {
            attachment.Downloads++;
            db.update( attachment, "Downloads" );
        }

        public virtual void UpdateName( ContentAttachment attachment, string name ) {
            attachment.Description = name;
            attachment.update( "Description" );
        }

        public virtual void UpdateFile( ContentAttachment a, String oldFilePath ) {

            a.update();

            if (a.IsImage)
                Img.DeleteImgAndThumb( oldFilePath );
            else
                Img.DeleteFile( oldFilePath );

        }

        //--------------------------------------------------------------------------------------------------------------


        public virtual Result CreateTemp( ContentAttachmentTemp a, User user, IMember owner ) {

            a.OwnerId = owner.Id;
            a.OwnerType = owner.GetType().FullName;
            a.OwnerUrl = owner.Url;
            a.Creator = user;
            a.CreatorUrl = user.Url;

            a.Guid = Guid.NewGuid().ToString();

            return db.insert( a );
        }


        public virtual Result Create( ContentAttachment a, User user, IMember owner ) {

            a.OwnerId = owner.Id;
            a.OwnerType = owner.GetType().FullName;
            a.OwnerUrl = owner.Url;
            a.Creator = user;
            a.CreatorUrl = user.Url;

            a.Guid = Guid.NewGuid().ToString();

            Result result = db.insert( a );

            refreshAttachmentCount( a );

            return result;
        }

        public void CreateByTemp( String ids, ContentPost post ) {

            int[] arrIds = cvt.ToIntArray( ids );
            if (arrIds.Length == 0) return;
            
            int attachmentCount = 0;
            foreach (int id in arrIds) {

                ContentAttachmentTemp at = ContentAttachmentTemp.findById( id );
                if (at == null) continue;

                ContentAttachment a = new ContentAttachment();

                a.AppId = at.AppId;
                a.Guid = at.Guid;

                a.FileSize = at.FileSize;
                a.Type = at.Type;
                a.Name = at.Name;
                a.Description = at.Description;
                a.PostId = post.Id;

                a.OwnerId = post.OwnerId;
                a.OwnerType = post.OwnerType;
                a.OwnerUrl = post.OwnerUrl;
                a.Creator = post.Creator;
                a.CreatorUrl = post.CreatorUrl;

                a.insert();

                at.delete();

                attachmentCount++;
            }

            post.Attachments = attachmentCount;
            post.update( "Attachments" );

        }

        public virtual void UpdateAtachments( int[] arrAttachmentIds, ContentPost post ) {

            if (post == null || arrAttachmentIds.Length == 0) return;

            foreach (int id in arrAttachmentIds) {

                ContentAttachment a = ContentAttachment.findById( id );
                if (a == null) continue;

                a.OwnerId = post.OwnerId;
                a.OwnerType = post.OwnerType;
                a.OwnerUrl = post.OwnerUrl;
                a.Creator = post.Creator;
                a.CreatorUrl = post.CreatorUrl;

                a.PostId = post.Id;
                a.AppId = post.AppId;

                a.update();
            }

            int count = ContentAttachment.count( "PostId=" + post.Id );
            post.Attachments = count;
            post.update();

        }

        //--------------------------------------------------------------------------------------------------------------

        public virtual void Delete( int id ) {
            ContentAttachment at = ContentAttachment.findById( id );
            if (at == null) return;

            at.delete();
            Img.DeleteImgAndThumb( strUtil.Join( sys.Path.DiskPhoto, at.Name ) );

            // 重新统计所属主题的附件数
            refreshAttachmentCount( at );
        }

        private static void refreshAttachmentCount( ContentAttachment at ) {
            ContentPost post = ContentPost.findById( at.PostId );
            if (post == null) return;
            int count = ContentAttachment.count( "PostId=" + post.Id );
            post.Attachments = count;
            post.update();
        }


        public virtual void DeleteTempAttachment( int id ) {

            ContentAttachmentTemp at = ContentAttachmentTemp.findById( id );
            if (at == null) return;

            at.delete();

            Img.DeleteImgAndThumb( at.FileUrl );
        }


        public virtual void DeleteByPost( int postId ) {
            List<ContentAttachment> attachments = this.GetAttachmentsByPost( postId );
            foreach (ContentAttachment attachment in attachments) {
                Img.DeleteImgAndThumb( attachment.FileUrl );
            }
            ContentAttachment.deleteBatch( "PostId=" + postId );
        }



    }
}

