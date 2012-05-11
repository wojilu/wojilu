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
        //INotificationService nfService { get; set; }

        Microblog GetById( int id );

        List<Microblog> GetCurrent( int count, int userId );
        Microblog GetFirst( int userId );

        DataPage<Microblog> GetPageList( int userId, int pageSize );

        void Insert( Microblog log );
        //void InsertBig( Microblog blog );


        void Delete( Microblog blog );

        DataPage<Microblog> GetPageListAll( int pageSize );

        int CountByUser( int userId );

        /// <summary>
        /// 得到指定日期内用户发微博数
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="filter">filter可选值 today,week,month,month3，注意大小写</param>
        /// <returns></returns>
        int CountByUserTime(int userId, string filter);

        DataPage<Microblog> GetFollowingPage( int ownerId, int pageSize );

        DataPage<Microblog> GetFollowingPage( int p, string searchKey );

        //DataPage<Microblog> GetPageList( int p, int pageSize );


        void DeleteBatch( string ids );
    }

}
