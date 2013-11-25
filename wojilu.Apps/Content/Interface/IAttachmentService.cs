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

        ContentAttachment GetById(long id);
        ContentAttachment GetById(long id, string guid);
        List<ContentAttachment> GetAttachmentsByPost(long postId);

        Result Create( ContentAttachment a, User user, IMember owner );
        Result CreateTemp( ContentAttachmentTemp a, User user, IMember owner );

        void DeleteByPost(long id);
        void DeleteTempAttachment(long id);
        void CreateByTemp( String ids, ContentPost topic );

        void UpdateName( ContentAttachment attachment, string name );
        void UpdateFile( ContentAttachment attachment, String oldFilePath );
        void Delete(long id);


        Result SaveFile( HttpFile httpFile );


        void UpdateAtachments(long[] arrAttachmentIds, ContentPost post);
    }

}
