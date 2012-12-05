/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using wojilu.Common.Microblogs.Domain;
using wojilu.Common.Feeds.Interface;
using wojilu.Common.Msg.Interface;
using wojilu.Members.Users.Domain;

namespace wojilu.Common.Microblogs.Interface {

    public interface IMicroblogService {

        IFeedService feedService { get; set; }

        Microblog GetById( int id );
        Microblog GetFirst( int userId );
        List<Microblog> GetCurrent( int count, int userId );

        DataPage<Microblog> GetPageList( int userId, int pageSize );
        DataPage<Microblog> GetFollowingPage( int ownerId, int pageSize );
        DataPage<Microblog> GetFollowingPage( int ownerId, string searchKey );

        void Insert( Microblog blog );

        int CountByUser( int userId );

        void Delete( Microblog blog );
        void DeleteBatch( string ids );

    }

}
