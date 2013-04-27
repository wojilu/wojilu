/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Text;

using wojilu.Web.Mvc;
using wojilu.Web.Context;
using wojilu.Apps.Forum.Domain;
using wojilu.Members.Users.Domain;

namespace wojilu.Web.Controller.Forum.Utils {

    public class ForumValidator {

        public static void AddCreateTime( MvcContext ctx ) {
            ctx.web.SessionSet( "__forumLastPublish", DateTime.Now );
        }

        public static Boolean IsIntervalShort( MvcContext ctx ) {
            Object objLast = ctx.web.SessionGet( "__forumLastPublish" );
            if (objLast == null) return false;

            ForumApp app = ctx.app.obj as ForumApp;
            ForumSetting setting = app.GetSettingsObj();

            return DateTime.Now.Subtract( (DateTime)objLast ).TotalSeconds <= setting.ReplyInterval;
        }

        public static ForumBoard ValidateBoard( MvcContext ctx ) {
            return ValidateBoard( null, ctx );
        }

        public static ForumBoard ValidateBoard( ForumBoard board, MvcContext ctx ) {

            if (board == null) board = new ForumBoard();

            String name = ctx.Post( "Name" );
            if (strUtil.IsNullOrEmpty( name )) {
                ctx.errors.Add( lang.get( "exName" ) );
            }

            String description = ctx.Post( "Description" );
            int parentId = ctx.PostInt( "ParentId" );
            String notice = ctx.PostHtml( "Notice" );


            board.ParentId = parentId;
            board.Name = name;
            board.Description = description;
            board.Notice = notice;

            board.AppId = ctx.app.Id;
            board.Creator = (User)ctx.viewer.obj;
            board.CreatorUrl = ctx.viewer.obj.Url;
            board.OwnerId = ctx.owner.Id;
            board.OwnerUrl = ctx.owner.obj.Url;
            board.OwnerType = ctx.owner.obj.GetType().FullName;
            board.Ip = ctx.Ip;

            board.IsCategory = ctx.PostInt( "IsCategory" );
            board.ViewId = ctx.PostInt( "ViewId" );

            return board;
        }

        public static ForumCategory ValidateCategory( MvcContext ctx ) {
            return ValidateCategory( null, ctx );
        }

        public static ForumCategory ValidateCategory( ForumCategory category, MvcContext ctx ) {

            if (category == null) category = new ForumCategory();

            String name = ctx.Post( "Name" );
            if (strUtil.IsNullOrEmpty( name ))
                ctx.errors.Add( lang.get( "exName" ) );

            String nameColor = ctx.Post( "NameColor" );
            if (strUtil.HasText( nameColor )) {
                String errorInfo = alang( ctx, "exColorFormat" );
                if (nameColor.Length != 7) ctx.errors.Add( errorInfo );
                if (nameColor.StartsWith( "#" ) == false) ctx.errors.Add( errorInfo );
            }

            int boardId = ctx.PostInt( "BoardId" );

            category.Name = name;
            category.NameColor = nameColor;
            category.BoardId = boardId;
            category.OwnerId = ctx.owner.Id;
            category.OwnerType = ctx.owner.obj.GetType().FullName;
            category.Creator = (User)ctx.viewer.obj;

            return category;
        }

        public static ForumLink ValidateLink( MvcContext ctx ) {
            return ValidateLink( null, ctx );
        }

        public static ForumLink ValidateLink( ForumLink link, MvcContext ctx ) {

            if (link == null) link = new ForumLink();

            String name = ctx.Post( "Name" );
            String description = ctx.Post( "Description" );
            String url = ctx.Post( "Url" );
            String logo = ctx.Post( "Logo" );

            if (strUtil.IsNullOrEmpty( name )) ctx.errors.Add( lang.get( "exName" ) );

            if (strUtil.IsNullOrEmpty( url ))
                ctx.errors.Add( lang.get( "exUrl" ) );
            else if (!url.ToLower().StartsWith( "http" ))
                url = "http://" + url;

            //if (strUtil.IsNullOrEmpty( logo ))
            //    ctx.errors.Add( alang( ctx, "exLogo" ) );
            //else if (!logo.ToLower().StartsWith( "http://" ))
            //    logo = "http://" + logo;

            link.Name = name;
            link.Description = description;
            link.Url = url;
            link.Logo = logo;
            link.AppId = ctx.app.Id;
            link.OwnerId = ctx.owner.Id;
            link.OwnerType = ctx.owner.obj.GetType().FullName;

            return link;
        }

        public static ForumPost ValidatePost( MvcContext ctx ) {

            ForumPost post = new ForumPost();
            int boardId = ctx.PostInt( "forumId" );
            int topicId = ctx.PostInt( "topicId" );
            int parentId = ctx.PostInt( "parentId" );
            String title = ctx.Post( "Title" );
            String content = ctx.PostHtml( "Content" );

            if (boardId <= 0) {
                ctx.errors.Add( alang( ctx, "exBoardSet" ) );
            }
            else {
                post.ForumBoardId = boardId;
            }

            if (topicId <= 0) {
                ctx.errors.Add( ctx.controller.alang( "exTopicNotFound" ) );
            }
            else {
                ForumTopic topic = ForumTopic.findById( topicId );
                if (topic == null) {
                    ctx.errors.Add( ctx.controller.alang( "exTopicNotFound" ) );
                }
                else {
                    post.Topic = topic;
                }
            }

            if (strUtil.IsNullOrEmpty( title )) ctx.errors.Add( lang.get( "exTitle" ) );
            if (strUtil.IsNullOrEmpty( content )) ctx.errors.Add( lang.get( "exContent" ) );

            post.ForumBoardId = boardId;
            post.TopicId = topicId;
            post.ParentId = parentId;
            post.Title = title;
            post.Content = content;
            post.Ip = ctx.Ip;

            return post;
        }

        public static ForumPost ValidatePostEdit( ForumPost post, MvcContext ctx ) {

            String title = ctx.Post( "Title" );
            String content = ctx.PostHtml( "Content" );

            if (strUtil.IsNullOrEmpty( title )) ctx.errors.Add( lang.get( "exTitle" ) );
            if (strUtil.IsNullOrEmpty( content )) ctx.errors.Add( lang.get( "exContent" ) );

            post.Title = title;
            post.Content = content;

            return post;
        }

        public static ForumTopic ValidateTopic( MvcContext ctx ) {
            return ValidateTopic( null, ctx );
        }

        public static ForumTopic ValidateTopic( ForumTopic topic, MvcContext ctx ) {

            if (topic == null) topic = new ForumTopic();

            String title = ctx.Post( "Title" );
            String content = ctx.PostHtml( "Content" );

            if (strUtil.IsNullOrEmpty( title )) ctx.errors.Add( lang.get( "exTitle" ) );
            if (strUtil.IsNullOrEmpty( content )) ctx.errors.Add( lang.get( "exContent" ) );

            topic.Title = title;
            topic.Content = content;
            topic.Category = new ForumCategory( ctx.PostInt( "CategoryId" ) );
            topic.Reward = ctx.PostInt( "Reward" );
            topic.RewardAvailable = topic.Reward;
            topic.ReadPermission = ctx.PostInt( "ReadPermission" );
            topic.Price = ctx.PostInt( "Price" );
            topic.TagRawString = ctx.Post( "TagList" );

            topic.IsAttachmentLogin = ctx.PostIsCheck( "IsAttachmentLogin" );

            return topic;
        }

        public static ForumTopic ValidateTopicEdit( ForumTopic topic, MvcContext ctx ) {

            if (topic == null) topic = new ForumTopic();

            String title = ctx.Post( "Title" );
            String content = ctx.PostHtml( "Content" );

            if (strUtil.IsNullOrEmpty( title )) ctx.errors.Add( lang.get( "exTitle" ) );
            if (strUtil.IsNullOrEmpty( content )) ctx.errors.Add( lang.get( "exContent" ) );

            topic.Title = title;
            topic.Content = content;
            topic.Category = new ForumCategory( ctx.PostInt( "CategoryId" ) );
            topic.TagRawString = ctx.Post( "TagList" );

            return topic;
        }

        private static String alang( MvcContext ctx, String key ) {
            return ctx.controller.alang( key );
        }

    }
}

