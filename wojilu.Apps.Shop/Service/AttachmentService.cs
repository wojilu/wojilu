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
using wojilu.Apps.Shop.Interface;
using wojilu.Apps.Shop.Domain;
using wojilu.Web;

namespace wojilu.Apps.Shop.Service {


    public class AttachmentService : IAttachmentService {

        public virtual Result SaveFile( HttpFile postedFile ) {

            Result result = Uploader.SaveFileOrImage( postedFile );
            if (result.HasErrors)return result;

            ShopItemAttachment uploadFile = new ShopItemAttachment();
            uploadFile.FileSize = postedFile.ContentLength;
            uploadFile.Type = postedFile.ContentType;
            uploadFile.Name = result.Info.ToString();
            uploadFile.Description = System.IO.Path.GetFileName( postedFile.FileName );
            uploadFile.Guid = Guid.NewGuid().ToString();

            uploadFile.insert();

            result.Info = uploadFile;

            return result;
        }

        public virtual List<ShopItemAttachment> GetAttachmentsByPost( int ItemId ) {
            return ShopItemAttachment.find( "ItemId=" + ItemId + " order by OrderId desc, Id asc" ).list();
        }

        public virtual ShopItemAttachment GetById( int id, String guid ) {
            ShopItemAttachment a = db.findById<ShopItemAttachment>( id );
            if (a != null && a.Guid != guid) {
                return null;
            }
            return a;
        }

        public virtual ShopItemAttachment GetById( int id ) {
            return ShopItemAttachment.findById( id );
        }

        public virtual void AddHits( ShopItemAttachment attachment ) {
            attachment.Downloads++;
            db.update( attachment, "Downloads" );
        }

        public virtual void UpdateName( ShopItemAttachment attachment, string name ) {
            attachment.Description = name;
            attachment.update( "Description" );
        }

        public virtual void UpdateFile( ShopItemAttachment a, String oldFilePath ) {

            a.update();

            if (a.IsImage)
                Img.DeleteImgAndThumb( oldFilePath );
            else
                Img.DeleteFile( oldFilePath );

        }

        //--------------------------------------------------------------------------------------------------------------


        public virtual Result CreateTemp( ShopItemAttachmentTemp a, User user, IMember owner ) {

            a.OwnerId = owner.Id;
            a.OwnerType = owner.GetType().FullName;
            a.OwnerUrl = owner.Url;
            a.Creator = user;
            a.CreatorUrl = user.Url;

            a.Guid = Guid.NewGuid().ToString();

            return db.insert( a );
        }


        public virtual Result Create( ShopItemAttachment a, User user, IMember owner ) {

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

        public void CreateByTemp( String ids, ShopItem post ) {

            int[] arrIds = cvt.ToIntArray( ids );
            if (arrIds.Length == 0) return;
            
            int attachmentCount = 0;
            foreach (int id in arrIds) {

                ShopItemAttachmentTemp at = ShopItemAttachmentTemp.findById( id );
                if (at == null) continue;

                ShopItemAttachment a = new ShopItemAttachment();

                a.AppId = at.AppId;
                a.Guid = at.Guid;

                a.FileSize = at.FileSize;
                a.Type = at.Type;
                a.Name = at.Name;
                a.Description = at.Description;
                a.ItemId = post.Id;

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

        public virtual void UpdateAtachments( int[] arrAttachmentIds, ShopItem post ) {

            if (post == null || arrAttachmentIds.Length == 0) return;

            foreach (int id in arrAttachmentIds) {

                ShopItemAttachment a = ShopItemAttachment.findById( id );
                if (a == null) continue;

                a.OwnerId = post.OwnerId;
                a.OwnerType = post.OwnerType;
                a.OwnerUrl = post.OwnerUrl;
                a.Creator = post.Creator;
                a.CreatorUrl = post.CreatorUrl;

                a.ItemId = post.Id;
                a.AppId = post.AppId;

                a.update();
            }

            int count = ShopItemAttachment.count( "ItemId=" + post.Id );
            post.Attachments = count;
            post.update();

        }

        //--------------------------------------------------------------------------------------------------------------

        public virtual void Delete( int id ) {
            ShopItemAttachment at = ShopItemAttachment.findById( id );
            if (at == null) return;

            at.delete();
            Img.DeleteImgAndThumb( strUtil.Join( sys.Path.DiskPhoto, at.Name ) );

            // 重新统计所属主题的附件数
            refreshAttachmentCount( at );
        }

        private static void refreshAttachmentCount( ShopItemAttachment at ) {
            ShopItem post = ShopItem.findById(at.ItemId);
            if (post == null) return;
            int count = ShopItemAttachment.count( "ItemId=" + post.Id );
            post.Attachments = count;
            post.update();
        }


        public virtual void DeleteTempAttachment( int id ) {

            ShopItemAttachmentTemp at = ShopItemAttachmentTemp.findById( id );
            if (at == null) return;

            at.delete();

            Img.DeleteImgAndThumb( at.FileUrl );
        }


        public virtual void DeleteByPost( int ItemId ) {
            List<ShopItemAttachment> attachments = this.GetAttachmentsByPost( ItemId );
            foreach (ShopItemAttachment attachment in attachments) {
                Img.DeleteImgAndThumb( attachment.FileUrl );
            }
            ShopItemAttachment.deleteBatch( "ItemId=" + ItemId );
        }



    }
}

