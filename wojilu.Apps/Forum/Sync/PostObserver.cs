/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

using wojilu.Aop;
using wojilu.Apps.Forum.Domain;
using wojilu.Apps.Forum.Service;

using wojilu.Common.AppBase.Interface;
using wojilu.Common.Comments;
using wojilu.Common.Microblogs.Domain;

using wojilu.Members.Interface;
using wojilu.Members.Users.Domain;
using wojilu.Web.Mvc;
using wojilu.Apps.Forum.Interface;

namespace wojilu.Apps.Forum.Sync {

    public class PostObserver : MethodObserver {

        public IOpenCommentService commentService { get; set; }

        public PostObserver() {
            commentService = new OpenCommentService();
        }

        public override void ObserveMethods() {

            observe( typeof( ForumPostService ), "Insert" );

            observe( typeof( ForumPostService ), "Update" );

            observe( typeof( ForumPostService ), "BanPost" );
            observe( typeof( ForumPostService ), "DeleteToTrash" );

        }

        public override void After( object returnValue, MethodInfo method, object[] args, object target ) {
            if (method.Name == "Insert") {
                // 添加评论
                addComment( args, target );
            }
            else if (method.Name == "Update") {
                updateComment( args, target );
            }

            else if (method.Name == "BanPost") {
                ForumPost post = args[0] as ForumPost;
                deleteCommentByPost( post );
            }
            else if (method.Name == "DeleteToTrash") {
                ForumPost post = args[0] as ForumPost;
                deleteCommentByPost( post );
            }
        }

        private void addComment( object[] args, object target ) {

            ForumPost post = args[0] as ForumPost;
            ForumTopic topic = ForumTopic.findById( post.TopicId );
            User creator = args[1] as User;
            IMember owner = args[2] as IMember;
            IApp app = args[3] as IApp;

            OpenComment c = new OpenComment();
            c.Content = strUtil.ParseHtml( post.Content );
            c.TargetUrl = alink.ToAppData( topic );

            c.TargetDataType = typeof( ForumTopic ).FullName;
            c.TargetDataId = topic.Id;
            c.TargetTitle = topic.Title;
            c.TargetUserId = topic.Creator.Id;

            c.OwnerId = owner.Id;
            c.AppId = app.Id;
            c.FeedId = getFeedId( topic );

            c.Ip = post.Ip;
            c.Author = creator.Name;
            c.ParentId = 0;
            c.AtId = 0;

            c.Member = creator;

            Result result = commentService.CreateNoNotification( c );

            // 修复comment额外的replies更新
            IForumTopicService topicService = ObjectContext.Create<IForumTopicService>( typeof( ForumTopicService ) );
            topic.Replies = topicService.CountReply( post.TopicId );
            topic.update( "Replies" );

            // 同步表
            CommentSync x = new CommentSync();
            x.Post = post;
            x.Comment = result.Info as OpenComment;
            x.insert();
        }

        private long getFeedId(ForumTopic topic) {

            Microblog mblog = Microblog.find( "DataId=:id and DataType=:dtype" )
                    .set( "id", topic.Id )
                    .set( "dtype", typeof( ForumTopic ).FullName )
                    .first();

            if (mblog != null) return mblog.Id;
            return 0;
        }

        private void updateComment( object[] args, object target ) {

            ForumPost post = args[0] as ForumPost;
            User editor = args[1] as User;
            ForumTopic topic = ForumTopic.findById( post.TopicId );

            // 更新评论
            CommentSync objSync = CommentSync.find( "PostId=" + post.Id ).first();
            if (objSync == null) return;
            OpenComment comment = objSync.Comment;
            comment.Content = strUtil.ParseHtml( post.Content );
            comment.update();

            // 更新此ForumPost对应的Microblog
            Microblog mblog = Microblog.find( "DataId=:id and DataType=:dtype" )
                .set( "id", post.Id )
                .set( "dtype", typeof( ForumPost ).FullName )
                .first();
            if (mblog != null) {
                mblog.Content = getPostContent( post );
                mblog.update();
            }
        }

        private String getPostContent( ForumPost data ) {

            String lnkPost = alink.ToAppData( data );

            String msg = string.Format( "<div class=\"feed-item-title\">回复了论坛帖子 <a href=\"{0}\">{1}</a></div>", lnkPost, data.Title );
            msg += string.Format( "<div class=\"feed-item-body\"><div class=\"feed-item-quote\">{0}</div></div>", strUtil.ParseHtml( data.Content, 200 ) );
            return msg;
        }


        private void deleteCommentByPost( ForumPost post ) {

            CommentSync objSync = CommentSync.find( "PostId=" + post.Id ).first();
            if (objSync == null) return;

            // 删除此评论
            OpenComment comment = objSync.Comment;
            commentService.Delete( comment );

            // 删除同步表 CommentSync
            objSync.delete();

        }

    }

}
