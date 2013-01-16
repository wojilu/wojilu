/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Apps.Forum.Domain;
using wojilu.Members.Interface;
using wojilu.Members.Users.Domain;

namespace wojilu.Apps.Forum.Interface {

    public interface IAttachmentService {

        void AddHits( Attachment attachment, User user );

        Result Create( Attachment a, User user, IMember owner );
        Result CreateTemp( AttachmentTemp a, User user, IMember owner );

        Attachment GetById( int id );
        Attachment GetById( int id, String guid );
        List<Attachment> GetByPost( int postId );
        List<Attachment> GetByTopic( List<ForumPost> list );

        void DeleteByPost( int id );

        void DeleteTempAttachment( int id );

        void CreateByTemp( String ids, ForumTopic topic );

        DataPage<AttachmentTemp> GetByUser( int userId, int pageSize );


        void UpdateName( Attachment attachment, string name );

        void UpdateFile( User user, Attachment attachment, String oldFilePath );

        void Delete( int id );

        List<Attachment> GetTopicAttachments( int topicId );

    }

}
