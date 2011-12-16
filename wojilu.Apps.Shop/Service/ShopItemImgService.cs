/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.ORM;
using wojilu.Drawing;
using wojilu.Common.AppBase;
using wojilu.Apps.Shop.Domain;
using wojilu.Apps.Shop.Interface;

namespace wojilu.Apps.Shop.Service {

    public class ShopItemImgService : IShopItemImgService {

        public virtual List<ShopItem> GetByType(int sectionId, int typeId, int appId)
        {
            return GetByType( sectionId, typeId, appId, 2 );
        }

        public virtual List<ShopItem> GetByType(int sectionId, int typeId, int appId, int count)
        {
            List<ShopItem> list = ShopItem.find("AppId=" + appId + " and PageSection.Id=" + sectionId + " and MethodId=" + typeId + " and SaveStatus=" + SaveStatus.Normal + " order by Id desc").list(count);
            list.Sort();
            return list;
        }

        public virtual ShopItem GetTopImg( int sectionId, int typeId, int appId ) {
            return ShopItem.find("AppId=" + appId + " and PageSection.Id=" + sectionId + " and MethodId=" + typeId + " and SaveStatus=" + SaveStatus.Normal + " order by Id desc").first();
        }

        public virtual List<ShopItem> GetByCategory(int sectionId, int categoryId, int appId)
        {
            return GetByCategory(sectionId, categoryId, appId, 2);
        }

        public virtual List<ShopItem> GetByCategory(int sectionId, int categoryId, int appId, int count)
        {
            List<ShopItem> list = ShopItem.find("AppId=" + appId + " and PageSection.Id=" + sectionId + " and CategoryId=" + categoryId + " and SaveStatus=" + SaveStatus.Normal + " order by Id desc").list(count);
            list.Sort();
            return list;
        }

        public virtual ShopItem GetTopImgByCategory(int sectionId, int categoryId, int appId)
        {
            return ShopItem.find("AppId=" + appId + " and PageSection.Id=" + sectionId + " and CategoryId=" + categoryId + " and SaveStatus=" + SaveStatus.Normal + " order by Id desc").first();
        }

        public virtual ShopItemImg GetImgById( int imgId ) {
            return db.findById<ShopItemImg>( imgId );
        }

        public virtual List<ShopItemImg> GetImgList( int ItemId ) {
            return db.find<ShopItemImg>("ItemId=" + ItemId + " order by Id").list();
        }

        public virtual int GetImgCount( int ItemId ) {
            return db.count<ShopItemImg>( "ItemId=" + ItemId );
        }

        public virtual DataPage<ShopItemImg> GetImgPage( int ItemId ) {
            return db.findPage<ShopItemImg>( "ItemId=" + ItemId + " order by Id", 10 );
        }

        public virtual DataPage<ShopItemImg> GetImgPage( int ItemId, int currentPage ) {
            return db.findPage<ShopItemImg>("ItemId=" + ItemId + " order by Id", currentPage, 10);
        }

        private void setNextImgLogo( ShopItemImg articleImg ) {
            ShopItemImg img = db.find<ShopItemImg>( "Id>" + articleImg.Id + " order by Id" ).first();
            if (img != null) {
                articleImg.Item.ImgLink = img.ImgUrl;
                this.UpdateImgLogo(articleImg.Item);
            }
            else {
                articleImg.Item.ImgLink = "";
                this.UpdateImgLogo(articleImg.Item);
            }
        }

        private void setPreImgLogo( ShopItemImg articleImg ) {
            ShopItemImg img = db.find<ShopItemImg>("Id<" + articleImg.Id + " and ItemId=" + articleImg.Item.Id + " order by Id desc").first();
            if (img != null) {
                articleImg.Item.ImgLink = img.ImgUrl;
                this.UpdateImgLogo(articleImg.Item);
            }
            else {
                this.setNextImgLogo( articleImg );
            }
        }


        public virtual void UpdateImgLogo( ShopItem post ) {
            db.update( post, "ImgLink" );
        }


        public virtual void CreateImg( ShopItemImg img ) {
            db.insert( img );
        }

        public virtual void DeleteImg( ShopItem post ) {
            db.delete( post );
        }


        public virtual void DeleteImgOne( ShopItemImg articleImg ) {
            db.delete( articleImg );
            Img.DeleteImgAndThumb( strUtil.Join( sys.Path.DiskPhoto, articleImg.ImgUrl ) );
            if (articleImg.ImgUrl.Equals(articleImg.Item.ImgLink))
            {
                this.setPreImgLogo( articleImg );
            }
        }

    }
}
