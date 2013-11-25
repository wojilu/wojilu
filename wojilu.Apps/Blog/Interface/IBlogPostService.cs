/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web;
using wojilu.Apps.Blog.Domain;
using wojilu.Members.Users.Interface;
using wojilu.Common;
using wojilu.Common.Microblogs.Interface;

namespace wojilu.Apps.Blog.Interface {

    public interface IBlogPostService {

        IMicroblogService microblogService { get; set; }
        IFriendService friendService { get; set; }

        void AddHits( BlogPost post );
        void ChangeCategory(long categoryId, string ids, long appId);
        void Delete(string ids, long appId);
        void DeleteTrue(string ids, long appId);
        BlogPost GetById(long id, long ownerId);
        BlogPost GetById_ForAdmin(long id);
        int GetCountByCategory(long id);
        int GetCountByUser(long userId);
        BlogPost GetDraft(long postId);
        DataPage<BlogPost> GetDraft(long appId, int pageSize);
        DataPage<BlogPost> GetFriendsBlog(long userId, long friendId);
        List<IBinderValue> GetMyRecent(int count, long userId);
        List<BlogPost> GetNewBlog(long appId, int count);

        DataPage<BlogPost> GetPage();
        DataPage<BlogPost> GetPage(long appId, int pageSize);
        DataPage<BlogPost> GetPageByCategory(long appId, long categoryId, int pageSize);

        RssChannel getRssByApp(long appId, int count);
        RssChannel GetRssByAppId(long appId, int count);
        List<BlogPost> GetTop(long appId, int count);
        List<IBinderValue> GetTopNew( int count );
        DataPage<BlogPost> GetTrash(long appId, int pageSize);

        void AddFeedInfo( BlogPost data );
        Result Insert( BlogPost post );
        Result InsertDraft( BlogPost blogPost );
        Result PublishDraft( BlogPost post );
        Result Insert(BlogPost data, long[] ids);

        void SetPick(string ids, long appId);
        void SetTop(string ids, long appId);
        void SetUnpick(string ids, long appId);
        void SetUntop(string ids, long appId);
        void UnDelete(string ids, long appId);
        Result UpdateDraft( BlogPost blogPost );


        List<BlogPost> GetByApp(long appId);
    }
}
