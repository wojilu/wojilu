/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Apps.Forum.Domain;

namespace wojilu.Web.Controller.Forum {

    public partial class BoardController : ControllerBase {


        private void bindChildBoards() {

            if (ctx.route.page > 1) {
                set( "childrenBoards", "" );
                return;
            }

            List<ForumBoard> children = getTree().GetChildren( fb.Id );

            if (children.Count == 0) {
                set( "childrenBoards", "" );
            }
            else {
                ctx.SetItem( "currentBoard", fb );
                ctx.SetItem( "childForumBoards", children );
                load( "childrenBoards", List );
            }
        }

        [NonVisit]
        public void List() {

            ForumBoard fbCategory = ctx.GetItem( "currentBoard" ) as ForumBoard;
            List<ForumBoard> childForums = ctx.GetItem( "childForumBoards" ) as List<ForumBoard>;

            String viewName = fbCategory.ViewId == 0 ? "List" : "List" + fbCategory.ViewId;
            view( viewName );

            IBlock fblock = getBlock( "forum" );

            set( "forumCategory.Name", fbCategory.Name );
            set( "forumCategory.Url", alink.ToAppData( fbCategory ) );

            foreach (ForumBoard board in childForums) {

                fblock.Set( "f.Name", board.Name );
                fblock.Set( "f.Url", alink.ToAppData( board ) );

                String description = getDescription( board );
                fblock.Set( "f.Description", description );

                fblock.Set( "f.TodayPosts", getTadayTopics( board.TodayPosts ) );
                fblock.Set( "f.Posts", board.Posts );
                fblock.Set( "f.Topics", board.Topics );
                fblock.Set( "f.Moderator", moderatorService.GetModeratorHtml( board ) );
                fblock.Set( "f.UpdateInfo", getLastUpdateInfo( board.LastUpdateInfo ) );
                fblock.Set( "f.Updated", board.LastUpdateInfo.UpdateTime );

                String statusImg = getStatusImg( board );
                fblock.Set( "statusImg", statusImg );
                fblock.Next();
            }
        }

        private String getStatusImg( ForumBoard board ) {

            if (strUtil.HasText( board.Logo )) return sys.Path.GetPhotoOriginal( board.Logo );

            //if (board.TodayPosts > 0) return strUtil.Join( sys.Path.Skin, "apps/forum/normalNew.gif" );
            //return strUtil.Join( sys.Path.Skin, "apps/forum/normal.gif" );

            if (board.TodayPosts > 0) return strUtil.Join( sys.Path.Skin, "site/new/board-new.png" );
            return strUtil.Join( sys.Path.Skin, "site/new/board.png" );

        }

        private String getDescription( ForumBoard board ) {

            List<ForumBoard> subboards = getTree().GetChildren( board.Id );
            if (subboards.Count == 0) {
                return board.Description;
            }
            else {
                StringBuilder sb = new StringBuilder();
                sb.Append( board.Description );
                sb.Append( " <span class=\"subBoardLabel\">" );
                sb.Append( alang( "subBoard" ) );
                sb.Append( "</span>: " );
                for (int i = 0; i < subboards.Count; i++) {
                    ForumBoard bd = subboards[i];
                    sb.AppendFormat( "<a href=\"{0}\">{1}</a>", alink.ToAppData( bd ), bd.Name );
                    if (i < subboards.Count - 1) sb.Append( ", " );
                }
                sb.Append( "" );
                return sb.ToString();
            }
        }

        private String getLastUpdateInfo( LastUpdateInfo info ) {

            if (info.IsEmpty()) return string.Empty;

            String lnkPost = "";

            if (info.PostType.Equals( typeof( ForumTopic ).Name ))
                lnkPost = to( new TopicController().Show, info.PostId );
            else if (info.PostType.Equals( typeof( ForumPost ).Name ))
                lnkPost = to( new PostController().Show, info.PostId );

            return string.Format( "<div class=\"fblastTitle\"><a href='{0}'>{1}</a></div><div class=\"fblastTime\">by <a href='{3}'>{2}</a> at {4}</div>",
                lnkPost,
                strUtil.CutString( info.PostTitle, 30 ),
                info.CreatorName,
                toUser( info.CreatorUrl ),
                info.UpdateTime.ToShortDateString()
                );

        }

        private String getTadayTopics( int todayPosts ) {
            if (todayPosts > 0) {
                return string.Format( "<span class=\"todayInfo left5\">(" + alang( "today" ) + ":<span class=\"todayPosts\">{0}</span>)</span>", todayPosts );
            }
            return string.Empty;
        }

    }
}

