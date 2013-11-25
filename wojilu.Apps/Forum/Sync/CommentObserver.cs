/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Reflection;

using wojilu.Aop;
using wojilu.Apps.Forum.Domain;
using wojilu.Apps.Forum.Interface;
using wojilu.Apps.Forum.Service;

using wojilu.Common.AppBase.Interface;
using wojilu.Common.Comments;

using wojilu.Members.Interface;
using wojilu.Members.Sites.Domain;
using wojilu.Members.Users.Domain;

namespace wojilu.Apps.Forum.Sync {

    public class CommentObserver : MethodObserver {

        public IForumPostService postService { get; set; }
        public IOpenCommentService commentService { get; set; }

        public CommentObserver() {
            postService = new ForumPostService();
            commentService = new OpenCommentService();
        }

        public override void ObserveMethods() {

            observe( typeof( OpenCommentService ), "Create" );

            observe( typeof( OpenCommentService ), "Delete" );

            // 批量删除禁止同步，需要手动检查，逐个删除
            //observe( typeof( OpenCommentService ), "DeleteBatch" );
            //observe( typeof( OpenCommentService ), "DeleteAll" );

        }

        public override void After( object returnValue, MethodInfo method, object[] args, object target ) {
            if (method.Name == "Create") {
                // 添加论坛回帖
                addPost( args, target );
            }
            else if (method.Name == "Delete") {
                deletePost( args, target );
            }

        }

        private void addPost( object[] args, object target ) {

            OpenComment comment = args[0] as OpenComment;
            if (comment == null || comment.Id <= 0) return;

            // 只监控论坛评论，其他所有评论跳过
            if (comment.TargetDataType != typeof( ForumTopic ).FullName) return;

            // 附属信息
            ForumTopic topic = commentService.GetTarget( comment ) as ForumTopic;
            User creator = comment.Member;
            IMember owner = getOwner( topic );
            IApp app = ForumApp.findById( topic.AppId );

            // 内容
            ForumPost post = new ForumPost();

            post.ForumBoardId = topic.ForumBoard.Id;
            post.TopicId = topic.Id;
            post.ParentId = getParentId( comment, topic );
            post.Title = "re:" + topic.Title;
            post.Content = comment.Content;
            post.Ip = comment.Ip;


            // 保存
            // 因为comment本身已有通知，所以论坛不再发通知
            postService.InsertNoNotification( post, creator, owner, app );

            // 同步表
            CommentSync objSync = new CommentSync();
            objSync.Post = post;
            objSync.Comment = comment;
            objSync.insert();
        }

        private long getParentId(OpenComment comment, ForumTopic topic) {

            ForumPost post = postService.GetPostByTopic( topic.Id );

            // 获取topic对应的post的Id
            if (comment.ParentId == 0) {
                return post.Id;
            }
            else {

                CommentSync objSync = CommentSync.find( "CommentId=" + comment.ParentId ).first();
                if (objSync == null || objSync.Post == null) return post.Id;

                return objSync.Post.Id;

            }
        }

        private IMember getOwner( ForumTopic topic ) {
            if (topic.OwnerType == typeof( Site ).FullName) return Site.Instance;
            return ndb.findById( Entity.GetType( topic.OwnerType ), topic.OwnerId ) as IMember;
        }

        private void deletePost( object[] args, object target ) {

            OpenComment comment = args[0] as OpenComment;

            CommentSync objSync = CommentSync.find( "CommentId=" + comment.Id ).first();
            if (objSync == null) return;

            // 删除帖子
            postService.DeleteToTrash( objSync.Post, comment.Member, "" );

            // 删除同步表
            objSync.delete();

            // 删除 ForumPost 对应的 Microblog


        }
    }

}
