/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Members.Interface;
using wojilu.Members.Users.Domain;
using wojilu.Apps.Content.Domain;
using wojilu.Web;

namespace wojilu.Apps.Content.Interface {

    public interface IAttachmentService {

        void AddHits( ContentAttachment attachment );

        ContentAttachment GetById( int id );
        ContentAttachment GetById( int id, String guid );
        List<ContentAttachment> GetAttachmentsByPost( int postId );

        Result Create( ContentAttachment a, User user, IMember owner );
        Result CreateTemp( ContentAttachmentTemp a, User user, IMember owner );

        void DeleteByPost( int id );
        void DeleteTempAttachment( int id );
        void CreateByTemp( String ids, ContentPost topic );

        void UpdateName( ContentAttachment attachment, string name );
        void UpdateFile( ContentAttachment attachment, String oldFilePath );
        void Delete( int id );


        Result SaveFile( HttpFile httpFile );


        void UpdateAtachments( int[] arrAttachmentIds, ContentPost post );
    }

}
