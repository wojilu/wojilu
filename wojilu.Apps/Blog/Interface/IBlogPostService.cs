/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web;
using wojilu.Members.Users.Service;
using wojilu.Apps.Blog.Domain;
using wojilu.Members.Users.Interface;
using wojilu.Common;

namespace wojilu.Apps.Blog.Interface {

    public interface IBlogPostService {

        IFriendService friendService { get; set; }

        void AddHits( BlogPost post );
        void ChangeCategory( int categoryId, String ids, int appId );
        void Delete( String ids, int appId );
        void DeleteTrue( String ids, int appId );
        BlogPost GetById( int id, int ownerId );
        BlogPost GetById_ForAdmin( int id );
        int GetCountByCategory( int id );
        int GetCountByUser( int userId );
        BlogPost GetDraft( int postId );
        DataPage<BlogPost> GetDraft( int appId, int pageSize );
        DataPage<BlogPost> GetFriendsBlog( int userId, int friendId );
        List<IBinderValue> GetMyRecent( int count, int userId );
        List<BlogPost> GetNewBlog( int appId, int count );

        DataPage<BlogPost> GetPage();
        DataPage<BlogPost> GetPage( int appId, int pageSize );
        DataPage<BlogPost> GetPageByCategory( int appId, int categoryId, int pageSize );

        RssChannel getRssByApp( int appId, int count );
        RssChannel GetRssByAppId( int appId, int count );
        List<BlogPost> GetTop( int appId, int count );
        List<IBinderValue> GetTopNew( int count );
        DataPage<BlogPost> GetTrash( int appId, int pageSize );

        Result Insert( BlogPost post );
        Result InsertDraft( BlogPost blogPost );
        Result PublishDraft( BlogPost post );
        Result Insert( BlogPost data, int[] ids );

        void SetPick( String ids, int appId );
        void SetTop( String ids, int appId );
        void SetUnpick( String ids, int appId );
        void SetUntop( String ids, int appId );
        void UnDelete( String ids, int appId );
        Result UpdateDraft( BlogPost blogPost );


        List<BlogPost> GetByApp( int appId );
    }
}
