/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Apps.Shop.Domain;
using wojilu.Common.AppBase;
using wojilu.Apps.Shop.Interface;

namespace wojilu.Apps.Shop.Service {

    public class SysItemService : ISysItemService {

        public DataPage<ShopItem> GetPage() {
            DataPage<ShopItem> list = ShopItem.findPage( "SaveStatus<>" + SaveStatus.SysDelete, 20 );
            return list;
        }


        public void Delete( String ids ) {
            db.updateBatch<ShopItem>( "set SaveStatus=" + SaveStatus.SysDelete + "", "Id in (" + ids + ")" );
        }


        public int GetDeleteCount() {
            return db.count<ShopItem>( "SaveStatus=" + SaveStatus.SysDelete );
        }


        public DataPage<ShopItem> GetPageTrash() {
            DataPage<ShopItem> list = ShopItem.findPage( "SaveStatus=" + SaveStatus.SysDelete, 20 );
            return list;
        }

        public ShopItem GetById_ForAdmin( int id ) {
            return ShopItem.findById( id );
        }

        public void UnDelete( ShopItem post ) {
            post.SaveStatus = SaveStatus.Normal;
            post.update();
        }

        public void DeleteTrue( String ids ) {
            db.deleteBatch<ShopItem>( "Id in (" + ids + ")" );
        }

    }
}
