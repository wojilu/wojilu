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

        void Add( User creator, String msg, String dataType, int dataId, String ip );

        /// <summary>
        /// 纯粹插入数据库，不检查表情、at用户、不处理tag；不处理转发
        /// </summary>
        /// <param name="creator"></param>
        /// <param name="msg"></param>
        /// <param name="dataType"></param>
        /// <param name="dataId"></param>
        /// <param name="ip"></param>
        void AddSimple( User creator, String msg, String dataType, int dataId, String ip );

        void AddSimplePrivate( User user, String msg, String dataType, int dataId, String ip );

        Microblog GetById( int id );
        Microblog GetFirst( int userId );
        List<Microblog> GetCurrent( int count, int userId );

        DataPage<Microblog> GetPageList( int userId, int pageSize );
        DataPage<Microblog> GetFollowingPage( int ownerId, int pageSize );
        DataPage<Microblog> GetFollowingPage( int ownerId, String searchKey );

        void Insert( Microblog blog );

        int CountByUser( int userId );

        void Delete( Microblog blog );
        void DeleteBatch( String ids );


    }

}
