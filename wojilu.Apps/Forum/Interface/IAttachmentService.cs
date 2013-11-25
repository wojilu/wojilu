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

        Attachment GetById(long id);
        Attachment GetById(long id, string guid);
        List<Attachment> GetByPost(long postId);
        List<Attachment> GetByTopic( List<ForumPost> list );

        void DeleteByPost(long id);

        void DeleteTempAttachment(long id);

        void CreateByTemp( String ids, ForumTopic topic );

        DataPage<AttachmentTemp> GetByUser(long userId, int pageSize);


        void UpdateName( Attachment attachment, string name );

        void UpdateFile( User user, Attachment attachment, String oldFilePath );

        void Delete(long id);

        List<Attachment> GetTopicAttachments(long topicId);

    }

}
