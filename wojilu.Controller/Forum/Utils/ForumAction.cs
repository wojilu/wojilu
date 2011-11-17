/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;

using wojilu.Data;
using wojilu.Web.Mvc;
using wojilu.Common.Security;
using System.Collections.Generic;
using System.Reflection;


namespace wojilu.Web.Controller.Forum.Utils {

    [Serializable]
    public class SecurityAction : ISecurityAction {

        private static readonly List<SecurityAction> dbs = loadActions();

        private static List<SecurityAction> loadActions() {

            List<SecurityAction> list = new List<SecurityAction>();

            list.Add( new SecurityAction() { Id = 1, Name = "论坛首页", Url = SecurityActionHelper.getAction( new ForumController().Index ) } );
            list.Add( new SecurityAction() { Id = 2, Name = "访问主题列表", Url = SecurityActionHelper.getAction( new BoardController().Show ) } );
            list.Add( new SecurityAction() { Id = 3, Name = "浏览主题", Url = SecurityActionHelper.getAction( new TopicController().Show ) } );
            list.Add( new SecurityAction() { Id = 4, Name = "浏览单帖", Url = SecurityActionHelper.getAction( new PostController().Show ) } );
            list.Add( new SecurityAction() { Id = 5, Name = "查看附件", Url = SecurityActionHelper.getAction( new AttachmentController().Show ) } );

            list.Add( new SecurityAction() { Id = 6, Name = "查看精华列表", Url = SecurityActionHelper.getAction( new TopicListController().Picked ) } );
            list.Add( new SecurityAction() { Id = 7, Name = "查看投票列表", Url = SecurityActionHelper.getAction( new TopicListController().Polls ) } );
            list.Add( new SecurityAction() { Id = 8, Name = "发布主题", Url = SecurityActionHelper.getTopicNew_Actions() } );
            list.Add( new SecurityAction() { Id = 9, Name = "悬赏提问", Url = SecurityActionHelper.getQuestion_Actions() } );
            list.Add( new SecurityAction() { Id = 10, Name = "发布投票", Url = SecurityActionHelper.getPoll_Actions() } );

            list.Add( new SecurityAction() { Id = 11, Name = "回复帖子", Url = SecurityActionHelper.getReply_Actions() } );
            list.Add( new SecurityAction() { Id = 12, Name = "帖子管理", Url = SecurityActionHelper.getAdminActions(), IsTopicAdmin = 1 } );

            return list;
        }


        public int Id { get; set; }
        public string Name { get; set; }

        public IList findAll() { return SecurityAction.dbs; }
        public void insert() { }
        public Result update() { return new Result(); }
        public void delete() { }

        public ISecurityAction GetById( int id ) {
            return this.findById( id ) as ISecurityAction;
        }

        public SecurityAction findById( int id ) {
            foreach (SecurityAction a in SecurityAction.dbs) {
                if (a.Id == id) return a;
            }
            return null;
        }

        //----------------------------------------------------------------------------


        public String Url { get; set; }
        public int IsMenu { get; set; }
        public String Format { get; set; }

        //是否主题管理的action
        public int IsTopicAdmin { get; set; }

        //-----------------------------------------------------------------------------

        public static SecurityAction GetByAction( aAction a ) {

            return GetByUrl( SecurityActionHelper.getAction( a ) );
        }

        public static SecurityAction GetByAction( aActionWithId a ) {

            return GetByUrl( SecurityActionHelper.getAction( a ) );
        }

        private static SecurityAction GetByUrl( String url ) {
            IList actions = new SecurityAction().findAll();
            foreach (SecurityAction a in actions) {
                if (a.Url.IndexOf( url ) >= 0) return a;
            }
            return null;
        }



    }

}

