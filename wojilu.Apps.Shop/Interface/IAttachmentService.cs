/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Members.Interface;
using wojilu.Members.Users.Domain;
using wojilu.Apps.Shop.Domain;
using wojilu.Web;

namespace wojilu.Apps.Shop.Interface {

    public interface IAttachmentService {

        void AddHits( ShopItemAttachment attachment );

        ShopItemAttachment GetById( int id );
        ShopItemAttachment GetById( int id, String guid );
        List<ShopItemAttachment> GetAttachmentsByPost( int postId );

        Result Create( ShopItemAttachment a, User user, IMember owner );
        Result CreateTemp( ShopItemAttachmentTemp a, User user, IMember owner );

        void DeleteByPost( int id );
        void DeleteTempAttachment( int id );
        void CreateByTemp( String ids, ShopItem topic );

        void UpdateName( ShopItemAttachment attachment, string name );
        void UpdateFile( ShopItemAttachment attachment, String oldFilePath );
        void Delete( int id );


        Result SaveFile( HttpFile httpFile );


        void UpdateAtachments( int[] arrAttachmentIds, ShopItem post );
    }

}
