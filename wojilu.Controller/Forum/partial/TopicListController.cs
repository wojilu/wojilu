/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;

using wojilu.ORM;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;

using wojilu.Apps.Forum.Domain;
using wojilu.Common.Security;
using wojilu.Common.Money.Domain;
using wojilu.Web.Controller.Forum.Utils;
using wojilu.Members.Users.Domain;

namespace wojilu.Web.Controller.Forum {

    public partial class TopicListController : ControllerBase {


        private void bindAll( int id, List<ForumTopic> stickyList, DataPage<ForumTopic> topicList, List<ForumCategory> categories, Boolean isAdmin ) {

            set( "slink", to( new BoardController().Show, id ) );
            set( "slinkReplied", to( new BoardController().Replied, id ) );
            set( "slinkCreated", to( new BoardController().Created, id ) );
            set( "slinkReplies", to( new BoardController().Replies, id ) );
            set( "slinkViews", to( new BoardController().Views, id ) );

            set( "slinkAll", to( new BoardController().All, id ) );
            set( "slinkDay", to( new BoardController().Day, id ) );
            set( "slinkDayTwo", to( new BoardController().DayTwo, id ) );
            set( "slinkWeek", to( new BoardController().Week, id ) );
            set( "slinkMonth", to( new BoardController().Month, id ) );
            set( "slinkMonthThree", to( new BoardController().MonthThree, id ) );
            set( "slinkMonthSix", to( new BoardController().MonthSix, id ) );

            set( "forumBoard.PollUrl", to( Polls, id ) );
            set( "forumBoard.PickedUrl", to( Picked, id ) );

            int replySize = ((ForumApp)ctx.app.obj).GetSettingsObj().ReplySize;

            bindBoardInfo();

            bindCategories( id, categories );

            bindTopicList( stickyList, getBlock( "stickyList" ), isAdmin, true, replySize );

            bindTopicList( topicList.Results, getBlock( "postlist" ), isAdmin, false, replySize );

            // 分页和表单
            bindPagerAndForm( id, topicList );

            bindToolbar( topicList );

            bindAdminToolbar( isAdmin );
        }


        private void bindTopicList( List<ForumTopic> list, IBlock postBlock, Boolean isAdmin, Boolean isSticky, int replySize ) {
            if (list == null) return;
            foreach (ForumTopic topic in list) {
                bindTopicOne( postBlock, topic, isAdmin, isSticky, replySize );
            }
        }

        private void bindCategories( int id, List<ForumCategory> categories ) {

            IBlock panelBlock = getBlock( "categoryPanel" );
            if (categories.Count > 0) {

                IBlock block = panelBlock.GetBlock( "categories" );
                foreach (ForumCategory category in categories) {
                    block.Set( "c.Name", category.Name );
                    block.Set( "c.NameColor", category.NameColor );
                    block.Set( "c.LinkShow", to( new BoardController().Category, category.BoardId ) + "?categoryId=" + category.Id );
                    block.Next();
                }

                panelBlock.Set( "boardLink", to( new BoardController().Show, id ) );
                panelBlock.Next();
            }
        }

        private void bindToolbar( DataPage<ForumTopic> topicList ) {
            ctx.SetItem( "topicListPagebar", topicList.PageBar );
            String toolbarHtml = loadHtml( Toolbar );
            set( "toolbar", toolbarHtml );
            String toolbarBottomHtml = topicList.RecordCount > 10 ? toolbarHtml : "";
            set( "toolbarBottom", toolbarBottomHtml );
        }

        private void bindAdminToolbar( Boolean isAdmin ) {
            load( "adminToolbar", AdminToolbar );
        }

        //------------------------------------------------------------------------------------------------------

        private void bindBoardInfo() {


            set( "app.Url", ctx.app.Url );
            set( "forumBoard.Title", fb.Name );
            set( "forumBoard.Url", to( new BoardController().Show, fb.Id ) );

            set( "forumBoard.TodayPosts", fb.TodayPosts );
            set( "forumBoard.Topics", fb.Topics );
            set( "forumBoard.Posts", fb.Posts );


            set( "forumBoard.Notice", strUtil.HasText( fb.Notice ) ? "<div class=\"board-info-notice clearfix\">" + fb.Notice + "</div>" : "" );
            set( "forumBoard.Moderator", moderatorService.GetModeratorHtml( fb ) );

            set( "moderatorJson", moderatorService.GetModeratorJson( fb ) );
        }


        private void bindTopicOne( IBlock block, ForumTopic topic, Boolean isAdmin, Boolean isSticky, int replySize ) {

            if (topic.Creator == null) return;

            String stickyIconName = topic.IsGlobalSticky ? "gsticky" : "sticky";
            if (isSticky) block.Set( "stickyIconName", stickyIconName );

            String rewardInfo = string.Empty;
            if (topic.Reward > 0) rewardInfo = getRewardInfo( topic, rewardInfo );

            String lblNew = string.Empty;
            ForumApp app = ctx.app.obj as ForumApp;
            int newDays = app.GetSettingsObj().NewDays;
            if (DateTime.Now.Subtract( topic.Created ).Days < newDays) lblNew = "<span class=\"supNew\">new</span>";


            String lblCategory = string.Empty;
            if (topic.Category != null && topic.Category.Id > 0) {
                String lnkCategory = to( new BoardController().Category, topic.ForumBoard.Id ) + "?categoryId=" + topic.Category.Id;
                lblCategory = string.Format( "<a href=\"{0}\" target=\"_blank\"><span style=\"color:{2}\">[{1}]</span></a>&nbsp;", lnkCategory, topic.Category.Name, topic.Category.NameColor );
            }

            String typeImg = string.Empty;
            if (strUtil.HasText( topic.TypeName )) {
                typeImg = string.Format( "<img src=\"{0}apps/forum/{1}.gif\">", sys.Path.Skin, topic.TypeName );
            }

            String priceInfo = string.Empty;
            if (topic.Price > 0) {
                priceInfo = alang( "price" ) + " :" + topic.Price + " ";
            }

            String permissionInfo = string.Empty;
            if (topic.ReadPermission > 0) {
                permissionInfo = alang( "readPermission" ) + ":" + topic.ReadPermission + "";
            }

            //String chkId = "";
            //if (isAdmin) chkId = "<input type=\"checkbox\" name=\"postSelect\" id=\"checkbox" + topic.Id + "\" value=\"" + topic.Id + "\" />";
            String chkId = "<input type=\"checkbox\" name=\"postSelect\" id=\"checkbox" + topic.Id + "\" value=\"" + topic.Id + "\" />";


            block.Set( "p.CheckBox", chkId );

            block.Set( "p.Id", topic.Id );
            block.Set( "p.Category", lblCategory );
            block.Set( "p.TypeImg", typeImg );
            block.Set( "p.Reward", rewardInfo );
            block.Set( "p.Price", priceInfo );
            block.Set( "p.ReadPermission", permissionInfo );
            block.Set( "p.Titile", strUtil.CutString( topic.Title, 40 ) );
            block.Set( "p.TitleStyle", topic.TitleStyle );
            block.Set( "p.LabelNew", lblNew );

            String lnk = LinkUtil.appendListPage( to( new TopicController().Show, topic.Id ), ctx );

            block.Set( "p.Url", lnk );

            block.Set( "p.Pages", getPostPagesString( alink.ToAppData( topic ), topic.Replies, replySize ) );
            block.Set( "p.MemberName", topic.Creator.Name );
            block.Set( "p.MemberUrl", toUser( topic.Creator ) );
            block.Set( "p.CreateTime", topic.Created.ToShortDateString() );
            block.Set( "p.ReplyCount", topic.Replies );
            block.Set( "p.Hits", topic.Hits );
            block.Set( "p.LastUpdate", topic.Replied.GetDateTimeFormats( 'g' )[0] );
            block.Set( "p.LastReplyUrl", toUser( topic.RepliedUserFriendUrl ) );
            block.Set( "p.LastReplyName", topic.RepliedUserName );

            String attachments = topic.Attachments > 0 ? "<img src='" + sys.Path.Img + "attachment.gif'/>" : "";
            block.Set( "p.Attachments", attachments );

            String statusImg;

            if (topic.IsLocked == 1)
                statusImg = sys.Path.Skin + "apps/forum/lock.gif";
            else if (topic.IsPicked == 1)
                statusImg = sys.Path.Skin + "apps/forum/pick.gif";
            else
                statusImg = sys.Path.Skin + "apps/forum/topic.gif";

            block.Set( "postStatusImage", statusImg );
            block.Next();
        }

        //------------------------------------------------------------------------------------------------------


        private void bindPagerAndForm( int id, DataPage<ForumTopic> topicList ) {

            IBlock formBlock = getBlock( "form" );

            List<ForumCategory> categories = categoryService.GetByBoard( id );
            if (categories.Count > 1) {
                categories.Insert( 0, new ForumCategory( 0, alang( "plsSelectCategory" ) ) );
            }

            bindFormNew( id, formBlock, categories );

        }

        private void bindFormNew( int boardId, IBlock formBlock, List<ForumCategory> categories ) {

            formBlock.Set( "ActionLink", to( new Users.TopicController().Create ) + "?boardId=" + boardId );

            formBlock.Set( "loginLink", t2( new MainController().Login ) );
            formBlock.Set( "regLink", t2( new RegisterController().Register ) );

            String categoryHtml = "";
            if (categories.Count > 0) categoryHtml = "<div id=\"forum-form-cat\">" + Html.DropList( categories, "CategoryId", "Name", "Id", 0 ) + "</div>";
            formBlock.Set( "Category", categoryHtml );

            formBlock.Next();
        }

        //-------------------------------------------------------------------------------------------------------------------

        private String getRewardInfo( ForumTopic topic, String rewardInfo ) {
            if (topic.RewardAvailable == 0) {
                rewardInfo = "(" + alang( "resolved" ) + ")";
            }
            else {
                rewardInfo = string.Format( "({0}{1})", topic.Reward, KeyCurrency.Instance.Unit );
            }
            if (DateTime.Now.Subtract( topic.Created ).Days >= ForumConfig.Instance.QuestionExpiryDay) {
            }
            return rewardInfo;
        }

        private String getPostPagesString( String url, int replies, int pageSize ) {

            if (replies < pageSize) return string.Empty;

            int beginsize = 5;
            int endsize = 1;
            int pcount = getPageCount( replies, pageSize );

            StringBuilder builder = new StringBuilder( "[ " );

            if (pcount <= (beginsize + endsize)) {
                for (int i = 1; i <= pcount; i++) {
                    appendLink( url, builder, i );
                }
            }
            else {
                int i = 1;
                while (i <= beginsize) {
                    appendLink( url, builder, i );
                    i++;
                }
                builder.Append( " .. " );
                for (i = endsize - 1; i >= 0; i--) {
                    int no = pcount - i;
                    appendLink( url, builder, no );
                }
            }
            builder.Append( " ]" );
            return builder.ToString();
        }

        private void appendLink( String url, StringBuilder builder, int i ) {
            builder.AppendFormat( "<a href=\"{0}\" target=\"_blank\">{1}</a> ", LinkUtil.appendListPage( PageHelper.AppendNo( url, i ), ctx ), i );
        }

        private static int getPageCount( int replies, int pageSize ) {

            int topicAndReplies = replies + 1;
            int mod = topicAndReplies % pageSize;
            if (mod == 0) {
                return (topicAndReplies / pageSize);
            }
            return ((topicAndReplies / pageSize) + 1);
        }

        //-------------------------------------------------------------------------------------------------------------------

        [NonVisit]
        public void Toolbar() {

            int id = fb.Id;

            set( "newPostUrl", to( new Users.TopicController().NewTopic ) + "?boardId=" + id );
            set( "newPollUrl", to( new Users.PollController().Add ) + "?boardId=" + id );
            set( "newQUrl", to( new Users.TopicController().NewQ ) + "?boardId=" + id );
            set( "page", ctx.GetItem( "topicListPagebar" ) );
        }

        [NonVisit]
        public void AdminToolbar() {

            int id = fb.Id;

            Moderators.TopicController t = new Forum.Moderators.TopicController();
            Moderators.TopicSaveController ts = new Forum.Moderators.TopicSaveController();

            set( "adminSticky", urlto( t.Sticky, id ) );
            set( "adminPicked", urlto( t.Picked, id ) );
            set( "adminHighlight", urlto( t.Highlight, id ) );
            set( "adminLock", urlto( t.Lock, id ) );

            set( "adminStickyUndo", urlto( ts.StickyUndo, id ) );
            set( "adminPickedUndo", urlto( ts.PickedUndo, id ) );
            set( "adminHighlightUndo", urlto( ts.HighlightUndo, id ) );
            set( "adminLockUndo", urlto( ts.LockUndo, id ) );
            set( "adminCategory", urlto( t.Category, id ) );
            set( "adminDelete", urlto( t.Delete, id ) );

            set( "stickyOrderLink", urlto( t.SortSticky, id ) );


            String cmdGsticky = string.Format( "<i class=\"icon-circle-arrow-up\"></i> <span class=\"cmdGsticky\"> {0}</span>", alang( "cmdGlobalSticky" ) );
            String cmd = "<span class=\"ajaxCmd btn\" url=\"{0}\">{1}</span>";
            String adminGlobalSticky = string.Format( cmd, urlto( t.GlobalSticky, id ), cmdGsticky );

            String gstickyOrderLink = urlto( t.GlobalSortSticky, id );
            adminGlobalSticky += " <a href=\"" + gstickyOrderLink + "\">&rsaquo;&rsaquo; " + alang( "pSortStickyTopic" ) + "</a>";

            String gstickyUndoLink = urlto( ts.GlobalStickyUndo, id );
            String cmdPost = "<span class=\"ajaxForumPost btn\" url=\"{0}\"><i class=\"icon-circle-arrow-down\"></i> {1}</span>";
            String adminGlobalStickyUndo = string.Format( cmdPost, gstickyUndoLink, alang( "cmdGlobalStickyUndo" ) );

            String moveLink = urlto( t.Move, id );
            String adminMove = string.Format( cmd, moveLink, "<i class=\"icon-move\"></i> " + alang( "moveTopic" ) );

            set( "adminGsticky", adminGlobalSticky );
            set( "adminGstickyUndo", adminGlobalStickyUndo );
            set( "adminMove", adminMove );

        }

        private String urlto( aAction action, int id ) {
            return to( action ) + "?boardId=" + id;
        }


    }

}
