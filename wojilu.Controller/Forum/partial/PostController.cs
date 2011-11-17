/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Apps.Forum.Domain;
using wojilu.Web.Controller.Forum.Utils;

namespace wojilu.Web.Controller.Forum {

    public partial class PostController : ControllerBase {

        private void bindReplyList( DataPage<ForumPost> results, int currentPostId ) {

            IBlock block = getBlock( "replypost" );
            List<ForumPost> replyList = results.Results;
            foreach (ForumPost post in replyList) {

                if (post.Creator == null) continue;
                block.Set( "p.MemberUrl", Link.ToMember( post.Creator ) );
                block.Set( "p.MemberName", post.Creator.Name );

                block.Set( "p.Title", post.Title );
                block.Set( "p.Length", post.Content.Length );
                block.Set( "p.CreateTime", post.Created );
                block.Set( "p.Hits", post.Hits );

                String plink = post.ParentId == 0 ? Link.To( new TopicController().Show, post.TopicId ) : alink.ToAppData( post );
                block.Set( "p.Url", plink );

                String pclass = post.Id == currentPostId ? "red strong" : "";
                block.Set( "p.Class", pclass );


                block.Next();
            }

            String page = results.PageCount > 1 ? results.PageBar : "";
            set( "replyPage", page );
        }


    }
}

