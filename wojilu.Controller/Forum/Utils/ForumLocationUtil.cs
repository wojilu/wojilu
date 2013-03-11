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

        public static readonly String separator = "<div class=\"pull-left forum-location-sp\"></div>";

        private static String alang( MvcContext ctx, String key ) {
            return ctx.controller.alang( key );
        }


        public static String GetBoard( List<ForumBoard> boards, MvcContext ctx ) {
            StringBuilder sb = getBuilder( boards, ctx );
            int length = separator.Length + 1;
            sb.Remove( sb.Length - length, separator.Length + 1 );
            appendEnd( sb );
            return sb.ToString();
        }

        public static String GetRecent( List<ForumBoard> boards, MvcContext ctx ) {
            StringBuilder sb = getBuilder( boards, ctx );
            sb.AppendFormat( "<div class=\"pull-left\">{0}</div>", alang( ctx, "recentPostList" ) );
            appendEnd( sb );
            return sb.ToString();
        }

        //------------------------------------------

        public static String GetPollAdd( List<ForumBoard> boards, MvcContext ctx ) {
            StringBuilder sb = getBuilder( boards, ctx );
            sb.Append( "<div class=\"pull-left\">" + alang( ctx, "pAddPoll" ) + "</div>" );
            appendEnd( sb );
            return sb.ToString();
        }

        public static String GetQuestionAdd( List<ForumBoard> boards, MvcContext ctx ) {
            StringBuilder sb = getBuilder( boards, ctx );
            sb.Append( "<div class=\"pull-left\">" + alang( ctx, "pAddQuestion" ) + "</div>" );
            appendEnd( sb );
            return sb.ToString();
        }

        public static String GetReply( List<ForumBoard> boards, ForumTopic topic, MvcContext ctx ) {
            StringBuilder sb = getBuilder( boards, ctx );
            sb.AppendFormat( "<div class=\"pull-left\"><a href=\"{0}\">" + alang( ctx, "pReply" ) + ": {1}</a></div>", alink.ToAppData( topic ), topic.Title );
            appendEnd( sb );
            return sb.ToString();
        }

        public static String GetSetReward( List<ForumBoard> boards, ForumTopic topic, MvcContext ctx ) {
            StringBuilder sb = getBuilder( boards, ctx );
            sb.AppendFormat( "<div class=\"pull-left\">" + alang( ctx, "pReward" ) + ": <a href=\"{0}\">{1}</a></div>", alink.ToAppData( topic ), topic.Title );
            appendEnd( sb );
            return sb.ToString();
        }

        //--------------------

        public static String GetPost( List<ForumBoard> boards, ForumPost post, MvcContext ctx ) {
            StringBuilder sb = getBuilder( boards, ctx );
            sb.AppendFormat( "<div class=\"pull-left\"><a href=\"{0}\">" + alang( ctx, "pPost" ) + ": {1}</a></div>", alink.ToAppData( post ), post.Title );
            appendEnd( sb );
            return sb.ToString();
        }

        public static String GetPostEdit( List<ForumBoard> boards, ForumPost post, MvcContext ctx ) {
            StringBuilder sb = getBuilder( boards, ctx );
            sb.AppendFormat( "<div class=\"pull-left\">" + alang( ctx, "pPostEdit" ) + ": <a href=\"{0}\">{1}</a></div>", alink.ToAppData( post ), post.Title );
            appendEnd( sb );
            return sb.ToString();
        }

        //-------------------------

        public static String GetTopic( List<ForumBoard> boards, ForumTopic topic, MvcContext ctx ) {
            StringBuilder sb = getBuilder( boards, ctx );
            String url = LinkUtil.appendListPageToTopic( alink.ToAppData( topic ), ctx );
            sb.AppendFormat( "<div class=\"pull-left\"><a href=\"{0}\">" + alang( ctx, "pTopic" ) + ": {1}</a></div>", url, strUtil.CutString( topic.Title, 35 ) );

            appendEnd( sb );
            return sb.ToString();
        }


        public static String GetTopicAdd( List<ForumBoard> boards, MvcContext ctx ) {
            StringBuilder sb = getBuilder( boards, ctx );
            sb.Append( "<div class=\"pull-left\">" + alang( ctx, "pAddTopic" ) + "</div>" );
            appendEnd( sb );
            return sb.ToString();
        }

        public static String GetTopicSort( List<ForumBoard> boards, MvcContext ctx ) {
            StringBuilder sb = getBuilder( boards, ctx );
            sb.Append( "<div class=\"pull-left\">" + alang( ctx, "pSortStickyTopic" ) + "</div>" );
            appendEnd( sb );
            return sb.ToString();
        }

        public static String GetGlobalTopicSort( MvcContext ctx ) {
            StringBuilder sb = getBuilder( null, ctx );
            sb.Append( "<div class=\"pull-left\">" + alang( ctx, "pSortStickyTopic" ) + "</div>" );
            appendEnd( sb );
            return sb.ToString();
        }

        public static String GetTopicEdit( List<ForumBoard> boards, ForumTopic topic, MvcContext ctx ) {
            StringBuilder sb = getBuilder( boards, ctx );


            sb.AppendFormat( "<div class=\"pull-left\">" + alang( ctx, "pTopicEdit" ) + ": <a href=\"{0}\">{1}</a></div>", alink.ToAppData( topic ), topic.Title );

            appendEnd( sb );
            return sb.ToString();
        }

        public static String GetTopicAttachment( List<ForumBoard> boards, ForumTopic topic, MvcContext ctx ) {
            StringBuilder sb = getBuilder( boards, ctx );
            sb.AppendFormat( "<div class=\"pull-left\"><a href=\"{0}\">{1}</a></div>", alink.ToAppData( topic ), topic.Title );
            appendEnd( sb );
            return sb.ToString();
        }

        private static void appendEnd( StringBuilder sb ) {
            sb.Append( "</div></div>" );
        }

        //----------------------------------------------------------------------------------------------------------------------------

        private static StringBuilder getBuilder( List<ForumBoard> boards, MvcContext ctx ) {
            StringBuilder sb = new StringBuilder( "<div class=\"row\"><div class=\"span12 forum-location\">" );
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

                String homePath = strUtil.Join( sys.Path.Skin, "site/new/home.png" );
                sb.AppendFormat( "<div class=\"pull-left dropdown\"><img src=\"{0}\" /> ", homePath );
                sb.AppendFormat( "<a class=\"dropdown-toggle\" data-toggle=\"dropdown\" data-hover=\"dropdown\" href=\"{0}\">{1} <span class=\"caret\"></span></a>", ctx.link.To( new ForumController().Index ), ((AppContext)ctx.app).UserApp.Name );

                addLocationMenu( sb, ctx );
                sb.Append( "</div>" );
                sb.Append( " " );
                sb.Append( separator );
                sb.Append( " " );


            }
        }


        private static void addLocationMenu( StringBuilder sb, MvcContext ctx ) {

            sb.Append( "<ul class=\"dropdown-menu\" id=\"locationBoards\">" );

            ForumBoardService service = new ForumBoardService();
            Tree<ForumBoard> tree = new Tree<ForumBoard>( service.GetBoardAll( ctx.app.Id, ctx.viewer.IsLogin ) );
            List<ForumBoard> categories = tree.GetRoots();

            foreach (ForumBoard category in categories) {
                sb.AppendFormat( "<li class=\"forum_location_category\">{0}</li>", category.Name );
                sb.Append( "<li class=\"forum_location_forums\">" );
                List<ForumBoard> children = tree.GetChildren( category.Id );
                foreach (ForumBoard board in children) {
                    sb.AppendFormat( "<a href=\"{0}\">{1}</a> ", alink.ToAppData( board ), board.Name );
                }
                sb.Append( "</li>" );
            }
            sb.Append( "</ul>" );
        }


        private static void appendBoard( StringBuilder sb, List<ForumBoard> boards, MvcContext ctx ) {
            if (boards == null) return;
            for (int i = 0; i < boards.Count; i++) {

                String url = alink.ToAppData( boards[i] );
                if (i == boards.Count - 1) url = LinkUtil.appendListPageToBoard( url, ctx );

                sb.AppendFormat( "<div class=\"pull-left\"><a href=\"{0}\">{1}</a></div> {2}", url, boards[i].Name, separator );
            }
        }





    }
}

