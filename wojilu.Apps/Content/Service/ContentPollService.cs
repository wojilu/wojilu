/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Apps.Content.Domain;
using wojilu.Common.Polls.Service;
using wojilu.Common.AppBase;

namespace wojilu.Apps.Content.Service {

    public class ContentPollService : PollBaseService<ContentPoll, ContentPollResult> {

        /// <summary>
        /// 快速创建投票，没有其他SEO等高级数据
        /// </summary>
        /// <param name="sectionId"></param>
        /// <param name="poll"></param>
        /// <returns></returns>
        public Result CreatePoll( int sectionId, ContentPoll poll ) {

            // 1) insert post
            ContentPost post = new ContentPost();
            Result result = insertPostByPoll( poll, post );
            if (result.HasErrors) return result;

            // 2) insert post_section
            // 新的多对多关系
            insertPostSectionShip( sectionId, post );

            // 3) insert poll
            poll.TopicId = post.Id;
            poll.insert();

            return result;
        }

        private static Result insertPostByPoll( ContentPoll poll, ContentPost post ) {
            post.OwnerId = poll.OwnerId;
            post.OwnerType = poll.OwnerType;
            post.OwnerUrl = poll.OwnerUrl;

            post.Creator = poll.Creator;
            post.CreatorUrl = poll.CreatorUrl;

            post.TypeName = typeof( ContentPoll ).FullName;

            post.AppId = poll.AppId;
            post.Ip = poll.Ip;

            post.Title = poll.Title;
            post.Content = poll.Question;

            return post.insert();
        }

        //-------------------------------------------------------------------------------------------------------

        /// <summary>
        /// 完整创建投票，包括客户端提供的SEO等表单数据
        /// </summary>
        /// <param name="sectionId"></param>
        /// <param name="poll"></param>
        /// <param name="post"></param>
        /// <returns></returns>
        public Result CreatePoll( int sectionId, ContentPoll poll, ContentPost post, String tagList ) {

            // 1) insert post
            post.TypeName = typeof( ContentPoll ).FullName;
            post.Content = poll.Question;
            Result result = post.insert();
            if (result.HasErrors) return result;

            post.Tag.Save( tagList );

            // 2) insert post_section
            // 新的多对多关系
            insertPostSectionShip( sectionId, post );

            // 3) insert poll
            poll.TopicId = post.Id;
            poll.insert();

            return result;
        }


        private static void insertPostSectionShip( int sectionId, ContentPost post ) {

            // page section
            ContentSection section = new ContentSection();
            section.Id = sectionId;

            // 缓存原始section
            post.PageSection = section;
            post.update();

            // 多对多关系
            ContentPostSection ps = new ContentPostSection();
            ps.Post = post;
            ps.Section = section;
            ps.insert();
        }


    }
}
