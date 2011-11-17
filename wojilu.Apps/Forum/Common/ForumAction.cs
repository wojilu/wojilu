/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;

using wojilu.Data;
using wojilu.Web.Mvc;
using wojilu.Common.Security;
using System.Collections.Generic;

namespace wojilu.Apps.Forum.Domain {

    /*
[
    { Id:1, Name:"访问板块", Url:"Forum/Forum/List", IsMenu:0, Format:"", IsTopicAdmin:0 },
    { Id:2, Name:"访问主题列表", Url:"Forum/Board/Show", IsMenu:0, Format:"", IsTopicAdmin:0 },
    { Id:3, Name:"浏览主题", Url:"Forum/Topic/Show", IsMenu:0, Format:"", IsTopicAdmin:0 },
    { Id:4, Name:"浏览单帖", Url:"Forum/Post/Show", IsMenu:0, Format:"", IsTopicAdmin:0 },
    { Id:5, Name:"查看附件", Url:"Forum/Topic/Attachement", IsMenu:0, Format:"", IsTopicAdmin:0 },
     
    { Id:6, Name:"查看精华列表", Url:"Forum/Board/Picked", IsMenu:0, Format:"", IsTopicAdmin:0 },
    { Id:7, Name:"查看投票列表", Url:"Forum/Board/Polls", IsMenu:0, Format:"", IsTopicAdmin:0 },     
    { Id:8, Name:"发布主题", Url:"Forum/Topic/NewTopic;Forum/Topic/Create", IsMenu:0, Format:"", IsTopicAdmin:0 },
    { Id:9, Name:"悬赏提问", Url:"Forum/Topic/NewQ", IsMenu:0, Format:"", IsTopicAdmin:0 },
    { Id:10, Name:"发布投票", Url:"Survey/Poll/ForumNew;Survey/Poll/ForumCreate", IsMenu:0, Format:"", IsTopicAdmin:0 },
     
    { Id:11, Name:"回复帖子", Url:"Forum/Post/ReplyTopic;Forum/Post/QuoteTopic;Forum/Post/ReplyPost;Forum/Post/QuotePost;Forum/Post/Create", IsMenu:0, Format:"", IsTopicAdmin:0 },
    { Id:12, Name:"帖子管理", Url:"Forum/Topic/Edit;Forum/Topic/Update;Forum/Post/Edit;Forum/Post/Update;Forum/Moderators/Admin/Delete;Forum/Moderators/Admin/Sticky;Forum/Moderators/AdminSave/StickyUndo;Forum/Moderators/Admin/SortSticky;Forum/Moderators/AdminSave/SaveStickySort;Forum/Moderators/Admin/GlobalSticky;Forum/Moderators/AdminSave/GlobalStickyUndo;Forum/Moderators/Admin/GlobalSortSticky;Forum/Moderators/AdminSave/SaveGlobalStickySort;Forum/Moderators/Admin/Picked;Forum/Moderators/AdminSave/PickedUndo;Forum/Moderators/Admin/Highlight;Forum/Moderators/AdminSave/HighlightUndo;Forum/Moderators/Admin/Lock;Forum/Moderators/AdminSave/LockUndo;Forum/Moderators/Admin/Move;Forum/Moderators/AdminSave/MoveSave;Forum/Moderators/Admin/Category;Forum/PostAdmin/DeleteTopic;Forum/PostAdmin/DeletePost;Forum/PostAdmin/AddCredit;Forum/PostAdmin/SaveCredit;Forum/PostAdmin/Ban;Forum/PostAdmin/UnBan;Forum/Attachment/Admin;Forum/Attachment/SetPermission;Forum/Attachment/SavePermission;Forum/Attachment/SaveSort;Forum/Attachment/Add;Forum/Attachment/SaveAdd;Forum/Attachment/Rename;Forum/Attachment/SaveRename;Forum/Attachment/Upload;Forum/Attachment/SaveUpload;Forum/Attachment/Delete;", IsMenu:0, Format:"", IsTopicAdmin:1 }
]
    */

    [Serializable]
    public class ForumAction : ISecurityAction {

        private static readonly List<ForumAction> dbs = loadActions();

        private static List<ForumAction> loadActions() {

            List<ForumAction> list = new List<ForumAction>();

            list.Add( new ForumAction() { Id = 1, Name = "访问板块", Url = "Forum/Forum/List" } );
            list.Add( new ForumAction() { Id = 1, Name = "访问主题列表", Url = "Forum/Board/Show" } );
            list.Add( new ForumAction() { Id = 1, Name = "浏览主题", Url = "Forum/Topic/Show" } );
            list.Add( new ForumAction() { Id = 1, Name = "浏览单帖", Url = "Forum/Post/Show" } );
            list.Add( new ForumAction() { Id = 1, Name = "查看附件", Url = "Forum/Topic/Attachement" } );

            list.Add( new ForumAction() { Id = 1, Name = "查看精华列表", Url = "Forum/Board/Picked" } );
            list.Add( new ForumAction() { Id = 1, Name = "查看投票列表", Url = "Forum/Board/Polls" } );
            list.Add( new ForumAction() { Id = 1, Name = "发布主题", Url = "Forum/Topic/NewTopic;Forum/Topic/Create" } );
            list.Add( new ForumAction() { Id = 1, Name = "悬赏提问", Url = "Forum/Topic/NewQ" } );
            list.Add( new ForumAction() { Id = 1, Name = "发布投票", Url = "Survey/Poll/ForumNew;Survey/Poll/ForumCreate" } );

            list.Add( new ForumAction() { Id = 1, Name = "回复帖子", Url = "Forum/Post/ReplyTopic;Forum/Post/QuoteTopic;Forum/Post/ReplyPost;Forum/Post/QuotePost;Forum/Post/Create" } );
            list.Add( new ForumAction() { Id = 1, Name = "帖子管理", Url = "Forum/Topic/Edit;Forum/Topic/Update;Forum/Post/Edit;Forum/Post/Update;Forum/Moderators/Admin/Delete;Forum/Moderators/Admin/Sticky;Forum/Moderators/AdminSave/StickyUndo;Forum/Moderators/Admin/SortSticky;Forum/Moderators/AdminSave/SaveStickySort;Forum/Moderators/Admin/GlobalSticky;Forum/Moderators/AdminSave/GlobalStickyUndo;Forum/Moderators/Admin/GlobalSortSticky;Forum/Moderators/AdminSave/SaveGlobalStickySort;Forum/Moderators/Admin/Picked;Forum/Moderators/AdminSave/PickedUndo;Forum/Moderators/Admin/Highlight;Forum/Moderators/AdminSave/HighlightUndo;Forum/Moderators/Admin/Lock;Forum/Moderators/AdminSave/LockUndo;Forum/Moderators/Admin/Move;Forum/Moderators/AdminSave/MoveSave;Forum/Moderators/Admin/Category;Forum/PostAdmin/DeleteTopic;Forum/PostAdmin/DeletePost;Forum/PostAdmin/AddCredit;Forum/PostAdmin/SaveCredit;Forum/PostAdmin/Ban;Forum/PostAdmin/UnBan;Forum/Attachment/Admin;Forum/Attachment/SetPermission;Forum/Attachment/SavePermission;Forum/Attachment/SaveSort;Forum/Attachment/Add;Forum/Attachment/SaveAdd;Forum/Attachment/Rename;Forum/Attachment/SaveRename;Forum/Attachment/Upload;Forum/Attachment/SaveUpload;Forum/Attachment/Delete;", IsTopicAdmin = 1 } );

            return list;
        }


        public int Id { get; set; }
        public string Name { get; set; }

        public IList findAll() { return ForumAction.dbs; }
        public void insert() {}
        public Result update() {}
        public void delete() {}

        public ISecurityAction GetById( int id ) {
            return this.findById( id ) as ISecurityAction;
        }

        //----------------------------------------------------------------------------


        public String Url { get; set; }
        public int IsMenu { get; set; }
        public String Format { get; set; }

        //是否主题管理的action
        public int IsTopicAdmin { get; set; } 



        //-----------------------------------------------------------------------------

        public static ForumAction GetByUrl( String url ) {
            IList actions = new ForumAction().findAll();
            foreach (ForumAction a in actions) {
                if (a.Url.IndexOf( url ) >= 0) return a;
            }
            return null;
        }

        public static ForumAction Get( aAction action, String rootNamespace ) {

            String typeFullName = action.Target.GetType().FullName;
            return getForumAction( typeFullName, action.Method.Name, rootNamespace );
        }

        public static ForumAction Get( aActionWithId action, String rootNamespace ) {

            String typeFullName = action.Target.GetType().FullName;
            return getForumAction( typeFullName, action.Method.Name, rootNamespace );
        }

        private static ForumAction getForumAction( String typeFullName, String actionName, String rootNamespace ) {

            String controllerAndAction = strUtil.TrimStart( typeFullName, rootNamespace );
            controllerAndAction = strUtil.TrimStart( controllerAndAction, "." ).Replace( ".", "/" );
            controllerAndAction = strUtil.TrimEnd( controllerAndAction, "Controller" );
            controllerAndAction = controllerAndAction + "/" + actionName;

            IList actions = new ForumAction().findAll();
            foreach (ForumAction a in actions) {
                if (a.Url.IndexOf( controllerAndAction ) >= 0) return a;
            }
            return null;
        }


    }

}

