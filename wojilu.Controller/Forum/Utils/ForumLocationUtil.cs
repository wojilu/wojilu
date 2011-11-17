/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using wojilu.Web.Mvc;
using wojilu.Web.Context;
using wojilu.Apps.Forum.Domain;
using wojilu.Apps.Forum.Service;
using wojilu.Members.Groups.Domain;

namespace wojilu.Web.Controller.Forum.Utils {


    public class ForumLocationUtil {

        public static readonly String separator = "&rsaquo;&rsaquo;";

        private static String alang( MvcContext ctx, String key ) {
            return ctx.controller.alang( key );
        }


        public static String GetBoard( List<ForumBoard> boards, MvcContext ctx ) {
            StringBuilder sb = getBuilder( boards, ctx );
            int length = separator.Length + 1;
            sb.Remove( sb.Length - length, separator.Length );
            sb.Append( "</div></div>" );
            return sb.ToString();
        }

        public static String GetRecent( List<ForumBoard> boards, MvcContext ctx ) {
            StringBuilder sb = getBuilder( boards, ctx );
            sb.Append( alang( ctx, "recentPostList" ) );
            sb.Append( "</div></div>" );
            return sb.ToString();
        }

        public static String GetTopicRecent( List<ForumBoard> boards, MvcContext ctx ) {
            StringBuilder sb = getBuilder( boards, ctx );
            sb.Append( alang( ctx, "recentTopicList" ) );
            sb.Append( "</div></div>" );
            return sb.ToString();
        }

        public static String GetPostRecent( List<ForumBoard> boards, MvcContext ctx ) {
            StringBuilder sb = getBuilder( boards, ctx );
            sb.Append( alang( ctx, "allRecentPost" ) );
            sb.Append( "</div></div>" );
            return sb.ToString();
        }

        //------------------------------------------

        public static String GetPollAdd( List<ForumBoard> boards, MvcContext ctx ) {
            StringBuilder sb = getBuilder( boards, ctx );
            sb.Append( "<span style=\"font-weight:normal;margin-left:5px;\">" + alang( ctx, "pAddPoll" ) + "</span>" );
            sb.Append( "</div></div>" );
            return sb.ToString();
        }

        public static String GetQuestionAdd( List<ForumBoard> boards, MvcContext ctx ) {
            StringBuilder sb = getBuilder( boards, ctx );
            sb.Append( "<span style=\"font-weight:normal;margin-left:5px;\">" + alang( ctx, "pAddQuestion" ) + "</span>" );
            sb.Append( "</div></div>" );
            return sb.ToString();
        }

        public static String GetReply( List<ForumBoard> boards, ForumTopic topic, MvcContext ctx ) {
            StringBuilder sb = getBuilder( boards, ctx );
            sb.AppendFormat( "<a href=\"{0}\" style=\"font-weight:normal;margin-left:5px;\">" + alang( ctx, "pReply" ) + ": {1}</a>", alink.ToAppData( topic ), topic.Title );
            sb.Append( "</div></div>" );
            return sb.ToString();
        }

        public static String GetSetReward( List<ForumBoard> boards, ForumTopic topic, MvcContext ctx ) {
            StringBuilder sb = getBuilder( boards, ctx );
            sb.AppendFormat( "<span style=\"font-weight:normal;margin-left:5px;\">" + alang( ctx, "pReward" ) + ": <a href=\"{0}\">{1}</a></span>", alink.ToAppData( topic ), topic.Title );
            sb.Append( "</div></div>" );
            return sb.ToString();
        }

        //--------------------

        public static String GetPost( List<ForumBoard> boards, ForumPost post, MvcContext ctx ) {
            StringBuilder sb = getBuilder( boards, ctx );
            sb.AppendFormat( "<a href=\"{0}\" style=\"margin-left:5px;\">" + alang( ctx, "pPost" ) + ": {1}</a>", alink.ToAppData( post ), post.Title );
            sb.Append( "</div></div>" );
            return sb.ToString();
        }

        public static String GetPostEdit( List<ForumBoard> boards, ForumPost post, MvcContext ctx ) {
            StringBuilder sb = getBuilder( boards, ctx );
            sb.AppendFormat( "<span style=\"margin-left:5px;\">" + alang( ctx, "pPostEdit" ) + ": <a href=\"{0}\">{1}</a></span>", alink.ToAppData( post ), post.Title );
            sb.Append( "</div></div>" );
            return sb.ToString();
        }

        //-------------------------

        public static String GetTopic( List<ForumBoard> boards, ForumTopic topic, MvcContext ctx ) {
            StringBuilder sb = getBuilder( boards, ctx );
            String url = LinkUtil.appendListPageToTopic( alink.ToAppData( topic ), ctx );
            sb.AppendFormat( "<a href=\"{0}\" style=\"font-weight:normal;margin-left:5px;\">" + alang( ctx, "pTopic" ) + ": {1}</a>", url, topic.Title );

            sb.Append( "</div></div>" );
            return sb.ToString();
        }


        public static String GetTopicAdd( List<ForumBoard> boards, MvcContext ctx ) {
            StringBuilder sb = getBuilder( boards, ctx );
            sb.Append( "<span style=\"font-weight:normal;margin-left:5px;\">" + alang( ctx, "pAddTopic" ) + "</span>" );
            sb.Append( "</div></div>" );
            return sb.ToString();
        }

        public static String GetTopicSort( List<ForumBoard> boards, MvcContext ctx ) {
            StringBuilder sb = getBuilder( boards, ctx );
            sb.Append( "<span style=\"font-weight:normal;margin-left:5px;\">" + alang( ctx, "pSortStickyTopic" ) + "</span>" );
            sb.Append( "</div></div>" );
            return sb.ToString();
        }

        public static String GetGlobalTopicSort( MvcContext ctx ) {
            StringBuilder sb = getBuilder( null, ctx );
            sb.Append( "<span style=\"font-weight:normal;margin-left:5px;\">" + alang( ctx, "pSortStickyTopic" ) + "</span>" );
            sb.Append( "</div></div>" );
            return sb.ToString();
        }

        public static String GetTopicEdit( List<ForumBoard> boards, ForumTopic topic, MvcContext ctx ) {
            StringBuilder sb = getBuilder( boards, ctx );


            sb.AppendFormat( "<span style=\"font-weight:normal;margin-left:5px;\">" + alang( ctx, "pTopicEdit" ) + ": <a href=\"{0}\">{1}</a></span>", alink.ToAppData( topic ), topic.Title );
            //sb.AppendFormat( "<span style=\"font-weight:normal;margin-left:5px;\">" + alang( ctx, "pTopicEdit" ) + ": {0}</span>", topic.Title );

            sb.Append( "</div></div>" );
            return sb.ToString();
        }

        public static String GetTopicAttachment( List<ForumBoard> boards, ForumTopic topic, MvcContext ctx ) {
            StringBuilder sb = getBuilder( boards, ctx );
            sb.AppendFormat( "<a href=\"{0}\" style=\"font-weight:normal;margin-left:5px;\">{1}</a>", alink.ToAppData( topic ), topic.Title );
            sb.Append( "</div></div>" );
            return sb.ToString();
        }

        //----------------------------------------------------------------------------------------------------------------------------

        private static StringBuilder getBuilder( List<ForumBoard> boards, MvcContext ctx ) {
            StringBuilder sb = new StringBuilder( "<div id=\"forumLocationContainer\"><div id=\"forumLocation\">" );
            appendAppBoard( sb, boards, ctx );
            return sb;
        }


        private static void appendAppBoard( StringBuilder sb, List<ForumBoard> boards, MvcContext ctx ) {
            appendApp( sb, ctx );
            appendBoard( sb, boards, ctx );
        }

        private static void appendApp( StringBuilder sb, MvcContext ctx ) {
            if (ctx.owner.obj.GetType() == typeof( Group )) {
            }
            else {

                sb.Append( "<span style=\"\" class=\"menuMore\" list=\"locationBoards\">" );
                sb.AppendFormat( "<a href=\"{0}\" id=\"locationHome\">{1} <img src=\"{2}down.gif\" /></a>", ctx.GetLink().To( new ForumController().Index ), ((AppContext)ctx.app).UserApp.Name, sys.Path.Img );
                addLocationMenu( sb, ctx );
                sb.Append( "</span>" );
                sb.Append( " " );
                sb.Append( separator );
                sb.Append( " " );


            }
        }


        private static void addLocationMenu( StringBuilder sb, MvcContext ctx ) {

            sb.Append( "<div class=\"menuItems\" id=\"locationBoards\">" );

            ForumBoardService service = new ForumBoardService();
            Tree<ForumBoard> tree = new Tree<ForumBoard>( service.GetBoardAll( ctx.app.Id, ctx.viewer.IsLogin ) );
            List<ForumBoard> categories = tree.GetRoots();

            foreach (ForumBoard category in categories) {
                sb.AppendFormat( "<div class=\"forum_location_category\">{0}</div>", category.Name );
                sb.Append( "<div class=\"forum_location_forums\">" );
                List<ForumBoard> children = tree.GetChildren( category.Id );
                foreach (ForumBoard board in children) {
                    sb.AppendFormat( "<a href=\"{0}\">{1}</a> ", alink.ToAppData( board ), board.Name );
                }
                sb.Append( "</div>" );
            }
            sb.Append( "</div>" );
        }


        private static void appendBoard( StringBuilder sb, List<ForumBoard> boards, MvcContext ctx ) {
            if (boards == null) return;
            for (int i = 0; i < boards.Count; i++) {

                String url = alink.ToAppData( boards[i] );
                if (i == boards.Count - 1) url = LinkUtil.appendListPageToBoard( url, ctx );

                sb.AppendFormat( "<a href=\"{0}\" class=\"left5 right5\">{1}</a> {2} ", url, boards[i].Name, separator );
            }
        }





    }
}

