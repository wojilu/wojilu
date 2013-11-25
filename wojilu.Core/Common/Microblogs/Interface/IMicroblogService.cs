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

        void Add(User creator, string msg, string dataType, long dataId, string ip);

        /// <summary>
        /// 纯粹插入数据库，不检查表情、at用户、不处理tag；不处理转发
        /// </summary>
        /// <param name="creator"></param>
        /// <param name="msg"></param>
        /// <param name="dataType"></param>
        /// <param name="dataId"></param>
        /// <param name="ip"></param>
        void AddSimple(User creator, string msg, string dataType, long dataId, string ip);

        void AddSimplePrivate(User user, string msg, string dataType, long dataId, string ip);

        Microblog GetById(long id);
        Microblog GetFirst(long userId);
        List<Microblog> GetCurrent(int count, long userId);

        DataPage<Microblog> GetPageList(long userId, int pageSize);
        DataPage<Microblog> GetFollowingPage(long ownerId, int pageSize);
        DataPage<Microblog> GetFollowingPage(long ownerId, string searchKey);

        void Insert( Microblog blog );

        int CountByUser(long userId);

        void Delete( Microblog blog );
        void DeleteBatch( String ids );


    }

}
