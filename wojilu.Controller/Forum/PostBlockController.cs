/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Web.Utils;

using wojilu.Common.AppBase;
using wojilu.Common.Money.Service;
using wojilu.Common.Money.Domain;
using wojilu.Common.Security;
using wojilu.Common.Money.Interface;

using wojilu.Members.Users.Interface;
using wojilu.Members.Users.Service;
using wojilu.Members.Users.Domain;
using wojilu.Members.Sites.Domain;

using wojilu.Apps.Forum.Domain;
using wojilu.Apps.Forum.Service;
using wojilu.Apps.Forum.Interface;

using wojilu.Web.Controller.Common;
using wojilu.Web.Controller.Forum.Utils;
using wojilu.Common;

namespace wojilu.Web.Controller.Forum {

    public class PostBlockController : ControllerBase {

        public IForumTopicService topicService { get; set; }
        public IForumBoardService boardService { get; set; }
        public IUserService userService { get; set; }
        public IForumRateService rateService { get; set; }
        public ICurrencyService currencyService { get; set; }
        public IForumBuyLogService buylogService { get; set; }

        public IModeratorService moderatorService { get; set; }


        public PostBlockController() {
            topicService = new ForumTopicService();
            boardService = new ForumBoardService();
            userService = new UserService();
            rateService = new ForumRateService();
            currencyService = new CurrencyService();
            buylogService = new ForumBuyLogService();
            moderatorService = new ModeratorService();
        }

        private ForumBoard board = null;

        [NonVisit]
        public void Show() {


            List<ForumPost> posts = ctx.GetItem( "posts" ) as List<ForumPost>;
            List<Attachment> attachs = ctx.GetItem( "attachs" ) as List<Attachment>;
            int pageSize = cvt.ToInt( ctx.GetItem( "pageSize" ) );

            this.board = ctx.GetItem( "forumBoard" ) as ForumBoard;
            ForumTopic topic = ctx.GetItem( "forumTopic" ) as ForumTopic;

            IBlock block = getBlock( "posts" );

            for (int i = 0; i < posts.Count; i++) {

                ForumPost data = posts[i];
                if (data.Creator == null) continue;

                getPostTopic( data );

                block.Set( "post.Id", data.Id );
                block.Set( "post.MemberName", data.Creator.Name );
                block.Set( "post.MemberUrl", Link.ToMember( data.Creator ) );

                String face = "";
                if (strUtil.HasText( data.Creator.Pic )) {
                    face = string.Format( "<img src=\"{0}\"/>", data.Creator.PicMedium );
                }

                block.Set( "post.MemberFace", face );


                block.Set( "post.MemberRank", getRankStr( data ) );
                block.Set( "post.StarList", data.Creator.Rank.StarHtml );
                block.Set( "post.IncomeList", data.Creator.IncomeInfo );
                block.Set( "post.MemberTitle", getUserTitle( data ) );
                block.Set( "post.MemberGender", data.Creator.GenderString );
                block.Set( "post.MemberPostCount", data.Creator.PostCount );
                block.Set( "post.MemberCreateTime", data.Creator.Created.ToShortDateString() );
                block.Set( "post.UserLastLogin", data.Creator.LastLoginTime.ToShortDateString() );

                String signature = strUtil.HasText( data.Creator.Signature ) ? "<div class=\"posC\">" + data.Creator.Signature + "</div>" : "";
                block.Set( "post.UserSignature", signature );

                block.Set( "post.OnlyOneMember", Link.To( new TopicController().Show, data.TopicId ) + "?userId=" + data.Creator.Id );
                block.Set( "post.FloorNo", getFloorNumber( pageSize, i ) );
                block.Set( "post.Title", addRewardInfo( data, data.Title ) );

                block.Set( "post.TitleText", data.Title );

                block.Set( "post.Admin", getAdminString( data ) );
                block.Set( "post.CreateTime", data.Created );

                List<Attachment> attachList = getAttachByPost( attachs, data.Id );

                if (data.ParentId == 0) {
                    bindTopicOne( block, data, attachList );
                }
                else {
                    bindPostOne( block, data, attachList );
                }

                block.Set( "adForumPosts", AdItem.GetAdById( AdCategory.ForumPosts ) );


                block.Next();
            }

            set( "moderatorJson", moderatorService.GetModeratorJson( board ) );
            set( "creatorId", topic.Creator.Id );
            set( "tagAction", to( new Edits.TagController().SaveTag, topic.Id ) );
        }

        private static String getRankStr( ForumPost data ) {

            if (data.Creator.RoleId != SiteRole.NormalMember.Id)
                return data.Creator.Role.Name;
            else
                return data.Creator.Rank.Name;
        }

        private void bindTopicOne( IBlock block, ForumPost data, List<Attachment> attachList ) {

            String quoteLink = Link.To( new Users.PostController().QuoteTopic, data.TopicId ) + "?boardId=" + data.ForumBoardId;
            String replyLink = Link.To( new Users.PostController().ReplyTopic, data.TopicId ) + "?boardId=" + data.ForumBoardId;
            String topicUrl = LinkUtil.appendListPageToTopic( Link.To( new TopicController().Show, data.TopicId ), ctx );

            block.Set( "post.ReplyQuoteUrl", quoteLink );
            block.Set( "post.ReplyUrl", replyLink );
            block.Set( "post.TitleStyle", string.Empty );
            block.Set( "post.PostUrl", topicUrl );
            String content = getTopicContent( data, attachList );
            block.Set( "post.Body", content );

            block.Set( "post.PostFullUrl", getFullUrl( topicUrl ) );

            block.Set( "post.AdBody", AdItem.GetAdById( AdCategory.ForumTopicInner ) );

            block.Set( "nofollow", "" );
        }

        private String getFullUrl( String url ) {
            if (url == null) return "";
            if (url.StartsWith( "http" )) return url;
            return strUtil.Join( ctx.url.SiteAndAppPath, url );
        }

        private void bindPostOne( IBlock block, ForumPost data, List<Attachment> attachList ) {

            String quoteLink = Link.To( new Users.PostController().QuotePost, data.Id ) + "?boardId=" + data.ForumBoardId;
            String replyLink = Link.To( new Users.PostController().ReplyPost, data.Id ) + "?boardId=" + data.ForumBoardId;
            String postUrl = alink.ToAppData( data );

            block.Set( "post.ReplyQuoteUrl", quoteLink );
            block.Set( "post.ReplyUrl", replyLink );
            block.Set( "post.TitleStyle", "font-weight:normal" );
            block.Set( "post.PostUrl", postUrl );
            String content = getPostContent( data, attachList );
            block.Set( "post.Body", content );

            block.Set( "post.PostFullUrl", getFullUrl( postUrl ) );

            block.Set( "post.AdBody", AdItem.GetAdById( AdCategory.ForumTopicInner ) );

            block.Set( "nofollow", "rel=\"nofollow\"" );
        }

        //----------------------------------------------------------------------------------------------------------------------------------


        private String getAdminString( ForumPost post ) {
            return getAdminActions( post ) + " " + getEditAction( post );
            //if (hasAdminPermission( post )) {
            //    return getAdminActions( post );
            //}
            //if (post.Creator.Id == ctx.viewer.Id) {
            //    return getEditAction( post );
            //}
            //return string.Empty;
        }

        //private Boolean canAdminTag( ForumPost post ) {
        //    return hasAdminPermission( post ) || post.Creator.Id == ctx.viewer.Id;
        //}

        private Boolean hasAdminPermission( ForumPost post ) {

            ForumBoard board = boardService.GetById( post.ForumBoardId, ctx.owner.obj );

            IList adminCmds = SecurityHelper.GetTopicAdminCmds( (User)ctx.viewer.obj, board, ctx );

            return adminCmds.Count > 0;
        }

        private String getAdminActions( ForumPost data ) {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat( " <span class=\"menuMore postAdmin\" id=\"postAdmin{0}\" list=\"menuItems_postAdmin{0}\">", data.Id );
            sb.AppendFormat( "<img src=\"{0}tools.gif\"/> {1} <img src=\"{0}down.gif\"/>", sys.Path.Img, alang( "admin" ) );
            sb.AppendFormat( "<ul class=\"menuItems\" id=\"menuItems_postAdmin{0}\">", data.Id );
            setEditActionItem( data, sb );
            setCreditAction( data, sb );
            setBanAction( data, sb );
            setLockAction( data, sb );
            setDeleteAction( data, sb );
            sb.Append( "</ul></span>" );
            return sb.ToString();
        }

        private List<Attachment> getAttachByPost( List<Attachment> attachs, int postId ) {
            List<Attachment> list = new List<Attachment>();
            foreach (Attachment attachment in attachs) {
                if (attachment.PostId == postId) {
                    list.Add( attachment );
                }
            }
            return list;
        }

        private int getFloorNumber( int pageSize, int i ) {
            return ((((ctx.route.page - 1) * pageSize) + i) + 1);
        }

        private String getTopicContent( ForumPost data, List<Attachment> attachList ) {

            if (data.Status == 1) return "<div class=\"banned\">" + alang( "postBanned" ) + "</div>";

            ForumTopic topic = getPostTopic( data );
            if (((topic.Price > 0) && (topic.Creator.Id != ctx.viewer.Id)) && !buylogService.HasBuyed( ctx.viewer.Id, topic )) {
                return getPricedContent( topic, data );
            }

            String content = data.Content;
            content = addOtherDataContent( addLockedInfo( data, content ), topic );
            if (data.EditTime.Subtract( data.Created ).TotalMinutes > ForumConfig.Instance.ShowEditInfoTime) {
                content = addEditInfo( data, content );
            }
            content = addRateLog( data, content );

            if (topic.IsAttachmentLogin == 1 && ctx.viewer.IsLogin == false) {
                content += "<div class=\"downloadWarning\"><div>" + alang( "downloadNeedLogin" ) + "</div></div>";
            }
            else {
                content = addAttachment( data, attachList, content );
            }

            return appendTags( topic, data, content );
        }

        private String appendTags( ForumTopic topic, ForumPost post, String content ) {

            //if (canAdminTag( post )) {
            //    _canAdminTag = true;
            return string.Format( "<div>{0}</div><div class=\"tagBar\" style=\"font-size:12px;\">" + alang( "tag" ) + ": <span id=\"tag{2}\" postId=\"{2}\">{1}</span> <span class=\"cmdTag cmd\">{3}tag</span></div>", content, topic.Tag.HtmlString, topic.Id, lang( "edit" ) );
            //}

            //if (strUtil.HasText( topic.Tag.HtmlString ))
            //    return string.Format( "<div>{0}</div><div style=\"font-size:12px;\">" + alang( "tag" ) + ": {1}</div>", content, topic.Tag.HtmlString );

            //return content;
        }


        private String getPostContent( ForumPost data, List<Attachment> attachList ) {
            if (data.Status == 1) {
                return "<div class=\"banned\">" + alang( "postBanned" ) + "</div>";
            }
            String content = data.Content;
            if (data.EditTime.Subtract( data.Created ).TotalMinutes > ForumConfig.Instance.ShowEditInfoTime) {
                content = addEditInfo( data, content );
            }
            content = addRateLog( data, content );
            return addAttachment( data, attachList, content );
        }

        private ForumTopic getPostTopic( ForumPost post ) {
            ForumTopic topic = topicService.GetById( post.TopicId, ctx.owner.obj );
            post.Topic = topic;
            return topic;
        }

        private String getPricedContent( ForumTopic topic, ForumPost post ) {
            int buyerCount = buylogService.GetBuyerCount( topic.Id );
            StringBuilder builder = new StringBuilder();
            builder.Append( "<div id=\"forumPrice\"><div class=\"price\">" );
            builder.AppendFormat( "<div>{0}</div>", alang( "exPlsBuy" ) );
            builder.AppendFormat( "<div>{0}: {1}</div>", alang( "price" ), topic.Price );
            builder.AppendFormat( "<div>{0}: {1}</div>", alang( "buyUsersCount" ), buyerCount );
            builder.AppendFormat( "<div><a class=\"frmBox cmd\" href=\"{0}\">{1}</a></div>", Link.To( new Moderators.PostSaveController().Buy, post.Id ) + "?boardId=" + topic.ForumBoard.Id, alang( "buyTopic" ) );
            builder.Append( "</div></div>" );
            return builder.ToString();
        }

        private String getUserTitle( ForumPost data ) {
            if (strUtil.IsNullOrEmpty( data.Creator.Title )) return "";
            return string.Format( "<div>{0}: {1}</div>", alang( "userTitle" ), data.Creator.Title );
        }

        //----------------------------------------------------------------------------------------------------------------------------------

        private void setBanAction( ForumPost data, StringBuilder sb ) {
            String strLock = "<li cmdurl=\"{0}\"><a href='#' class=\"postAdminItem putCmd\">" + alang( "cmdBan" ) + "</a></li>";
            String strUnlock = "<li cmdurl=\"{0}\"><a href='#' class=\"postAdminItem putCmd\">" + alang( "cmdUnban" ) + "</a></li>";
            if (data.Status == 0)
                sb.AppendFormat( strLock, Link.To( new Moderators.PostSaveController().Ban, data.Id ) + "?boardId=" + data.ForumBoardId );
            else
                sb.AppendFormat( strUnlock, Link.To( new Moderators.PostSaveController().UnBan, data.Id ) + "?boardId=" + data.ForumBoardId );
        }

        private void setCreditAction( ForumPost data, StringBuilder sb ) {
            sb.AppendFormat( "<li cmdurl=\"{0}\"><a href='#' class=\"postAdminItem frmBox\">{1}</a></li>", Link.To( new Moderators.PostController().AddCredit, data.Id ) + "?boardId=" + data.ForumBoardId, alang( "cmdCredit" ) );
        }

        private void setDeleteAction( ForumPost data, StringBuilder sb ) {
            String strDelete = "<li cmdurl=\"{0}\"><a href='#' class=\"postAdminItem deleteCmd\">" + alang( "cmdDelete" ) + "</a></li>";
            if (data.ParentId == 0)
                sb.AppendFormat( strDelete, Link.To( new Moderators.PostSaveController().DeleteTopic, data.TopicId ) + "?boardId=" + data.ForumBoardId );
            else
                sb.AppendFormat( strDelete, Link.To( new Moderators.PostSaveController().DeletePost, data.Id ) + "?boardId=" + data.ForumBoardId );
        }

        private String getEditAction( ForumPost data ) {
            String strEdit = " <a href='{0}' class=\"editCmd {2}\" data-creatorId=\"{4}\" title=\"{3}\" xwidth=\"700\" xheight=\"430\"><img src=\"{1}edit.gif\"/> {3}</a>";
            if (data.ParentId == 0) {
                return string.Format( strEdit, Link.To( new Edits.TopicController().Edit, data.TopicId ), sys.Path.Img, "", lang( "edit" ), data.Creator.Id );
            }
            return string.Format( strEdit, Link.To( new Edits.PostController().Edit, data.Id ), sys.Path.Img, "frmBox", lang( "edit" ), data.Creator.Id );
        }

        private void setEditActionItem( ForumPost data, StringBuilder sb ) {
            String str = "<li cmdurl=\"{0}\"><a href='#' class=\"postAdminItem {1}\" title=\"{2}\" xwidth=\"700\" xheight=\"430\">{2}</a></li>";
            if (data.ParentId == 0)
                sb.AppendFormat( str, Link.To( new Edits.TopicController().Edit, data.TopicId ), "", alang( "cmdEdit" ) );
            else
                sb.AppendFormat( str, Link.To( new Edits.PostController().Edit, data.Id ), "frmBox", alang( "cmdEdit" ) );
        }

        private void setLockAction( ForumPost data, StringBuilder sb ) {
            if (data.ParentId <= 0) {
                ForumTopic topic = getPostTopic( data );
                if (topic.IsLocked == 1) {
                    sb.AppendFormat( "<li cmdurl=\"{0}\"><a href='#' class=\"postAdminItem putCmd\">{1}</a></li>", Link.To( new Moderators.PostSaveController().UnLock, topic.Id ) + "?boardId=" + data.ForumBoardId, alang( "cmdUnlock" ) );
                }
                else {
                    sb.AppendFormat( "<li cmdurl=\"{0}\"><a href='#' class=\"postAdminItem putCmd\">{1}</a></li>", Link.To( new Moderators.PostSaveController().Lock, topic.Id ) + "?boardId=" + data.ForumBoardId, alang( "cmdLock" ) );
                }
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------


        private String addAttachment( ForumPost data, List<Attachment> attachList, String content ) {

            if (attachList.Count <= 0) return content;

            ISecurityAction action = SecurityAction.GetByAction( new AttachmentController().Show );

            Boolean hasAction = SecurityHelper.HasAction( (User)ctx.viewer.obj, board, action, ctx );
            if (!hasAction) {
                String amsg = string.Format( alang( "attachmentsInfo" ), attachList.Count );
                return content + "<div class=\"attachmentForbidden\"><span class=\"afText\">" + alang( "exAttachmentView" ) + "</span>(<span class=\"afInfo\">" + amsg + "</span>)</div>";
            }

            StringBuilder sb = new StringBuilder();
            String created = attachList[0].Created.ToString();
            sb.Append( "<div class=\"hr\"></div><div class=\"attachmentTitleWrap\"><div class=\"attachmentTitle\">" + alang( "attachment" ) + " <span class=\"note\">(" + created + ")</span> " );
            if (ctx.viewer.Id == data.Creator.Id || hasAdminPermission( data )) {
                sb.AppendFormat( "<a href=\"{0}\">" + alang( "adminAttachment" ) + "</a>", to( new Edits.AttachmentController().Admin, data.TopicId ) );
            }

            sb.Append( "</div></div><ul class=\"attachmentList\">" );

            foreach (Attachment attachment in attachList) {

                string fileName = attachment.GetFileShowName();

                if (attachment.IsImage) {

                    sb.AppendFormat( "<li><div>{0} <span class=\"note\">({1}KB)</span></div>", fileName, attachment.FileSizeKB );
                    sb.AppendFormat( "<div><a href=\"{0}\" target=\"_blank\"><img src=\"{1}\" /></a></div></li>",
                        attachment.FileUrl, attachment.FileMediuUrl );
                }
                else {


                    sb.AppendFormat( "<li><div>{0} <span class=\"note right10\">({1}KB)</span>", fileName, attachment.FileSizeKB );

                    sb.AppendFormat( "<img src=\"{1}\" /><a href=\"{0}\" target=\"_blank\">" + alang( "hitDownload" ) + "</a></div>", to( new AttachmentController().Show, attachment.Id ) + "?id=" + attachment.Guid, strUtil.Join( sys.Path.Img, "/s/download.png" ) );
                }
            }
            sb.Append( "</ul>" );

            content = string.Format( "<div>{0}</div><div id=\"attachmentPanel\">{1}</div>", content, sb.ToString() );

            return content;
        }

        private String addEditInfo( ForumPost data, String content ) {
            User user = userService.GetById( data.EditMemberId );
            String str = string.Format( "<div class=\"updateNote\">" + alang( "lastEditInfo" ) + "</div>",
                data.EditTime, Link.ToMember( user ), user.Name );
            content = content + str;
            return content;
        }

        private String addLockedInfo( ForumPost data, String content ) {
            if (getPostTopic( data ).IsLocked == 1) {
                return ("<div class=\"locked\">" + alang( "exLockTip" ) + "</div>" + content);
            }
            return content;
        }

        private String addOtherDataContent( String content, ForumTopic topic ) {

            if (strUtil.IsNullOrEmpty( topic.TypeName )) return content;
            if (topic.TypeName.Equals( "question" )) return addQuestionInfo( content, topic );

            return addContentInfo( topic, content );
        }

        private String addQuestionInfo( String content, ForumTopic topic ) {
            StringBuilder builder = new StringBuilder();
            builder.Append( "<div class=\"question\">" );
            if (topic.RewardAvailable == 0) {
                builder.AppendFormat( "<img src=\"{0}{1}\"/>", sys.Path.Skin, "apps/forum/question_m.gif" );
                builder.AppendFormat( " {2}<span class=\"font12 left10\">({3}{0} {1}", topic.Reward, KeyCurrency.Instance.Unit, alang( "qResolved" ), alang( "reward" ) );
                builder.AppendFormat( " <a href=\"{0}\" class=\"frmBox\">{1}</a>)</span>", Link.To( new Moderators.PostController().RewardList, topic.Id ) + "?boardId=" + topic.ForumBoard.Id, alang( "viewReward" ) );
            }
            else {
                builder.AppendFormat( "<img src=\"{0}{1}\"/>", sys.Path.Skin, "apps/forum/question_m.gif" );
                builder.AppendFormat( " {0} {1} {2}", alang( "rewardLable" ), topic.Reward, KeyCurrency.Instance.Unit );
                if (topic.RewardAvailable < topic.Reward) {
                    builder.AppendFormat( "，<span class=\"font12\">" + alang( "scoreStats" ) + "</span> ", topic.Reward - topic.RewardAvailable, topic.RewardAvailable );
                }
                if (topic.Creator.Id == ctx.viewer.Id) {
                    builder.AppendFormat( "<a href=\"{0}\" class=\"left20\">+ {1}</a>", Link.To( new Moderators.PostController().SetReward, topic.Id ) + "?boardId=" + topic.ForumBoard.Id, alang( "cmdReward" ) );
                }
            }
            builder.Append( "</div>" );
            content = builder.ToString() + content;
            return content;
        }

        private String addContentInfo( ForumTopic data, String content ) {
            return content + "<div class=\"extDataPanel\">" + ExtData.GetExtView( data.Id, typeof( ForumTopic ).FullName, data.TypeName, ctx ) + "</div>";
        }

        private String addRateLog( ForumPost data, String content ) {
            if (data.Rate == 0) {
                return content;
            }
            List<UserIncomeLog> logs = rateService.GetByPost( data.Id );
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat( "<fieldset class=\"forumRateLogs\"><legend>{0}</legend>", alang( "creditLog" ) );
            foreach (UserIncomeLog log in logs) {
                builder.AppendFormat( "<div><span>{0}</span><span>", log.OperatorName );
                ICurrency currency = currencyService.GetICurrencyById( log.CurrencyId );
                builder.Append( "</span><span>" );
                builder.Append( currency.Name );
                if (log.Income > 0) {
                    builder.Append( "+" );
                }
                builder.Append( log.Income );
                builder.Append( "</span><span>" );
                builder.Append( log.Note );
                builder.Append( "</span><span>" );
                builder.Append( log.Created.ToString( "g" ) );
                builder.Append( "</span></div>" );
            }
            builder.Append( "</fieldset>" );
            return (content + builder.ToString());
        }

        private String addRewardInfo( ForumPost data, String title ) {
            if (data.Reward > 0) {
                title = title + string.Format( "<span class=\"note\">({0}:{1})</span>", alang( "creditLabel" ), (int)data.Reward );
            }
            return title;
        }

    }

}
