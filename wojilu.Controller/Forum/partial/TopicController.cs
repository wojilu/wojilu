/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.ORM;

using wojilu.Common;
using wojilu.Common.Money.Domain;
using wojilu.Common.Security;
using wojilu.Common.Tags;
using wojilu.Common.AppBase.Interface;

using wojilu.Members.Sites.Domain;
using wojilu.Members.Users.Domain;

using wojilu.Web.Controller.Forum.Utils;
using wojilu.Apps.Forum.Domain;


namespace wojilu.Web.Controller.Forum {

    public partial class TopicController : ControllerBase {

        [NonVisit]
        public void PostLoop() {

            List<ForumPost> posts = ctx.GetItem( "posts" ) as List<ForumPost>;
            List<Attachment> attachs = ctx.GetItem( "attachs" ) as List<Attachment>;
            int pageSize = cvt.ToInt( ctx.GetItem( "pageSize" ) );

            ForumBoard board = ctx.GetItem( "forumBoard" ) as ForumBoard;
            ForumTopic topic = ctx.GetItem( "forumTopic" ) as ForumTopic;

            IBlock block = getBlock( "posts" );

            for (int i = 0; i < posts.Count; i++) {

                ForumPost data = posts[i];
                if (data.Creator == null) continue;

                populatePostTopic( data );

                block.Set( "post.Id", data.Id );
                block.Set( "post.TopicId", data.TopicId );
                block.Set( "post.MemberName", data.Creator.Name );
                block.Set( "post.MemberUrl", toUser( data.Creator ) );

                String face = "";
                if (strUtil.HasText( data.Creator.Pic )) {
                    face = string.Format( "<img src=\"{0}\"/>", data.Creator.PicM );
                }

                block.Set( "post.MemberFace", face );

                block.Set( "post.MemberRank", data.Creator.Rank.Name );
                block.Set( "post.StarList", data.Creator.Rank.StarHtml );
                block.Set( "post.IncomeList", data.Creator.IncomeInfo );
                block.Set( "post.MemberTitle", getUserTitle( data ) );
                block.Set( "post.MemberGender", data.Creator.GenderString );
                block.Set( "post.MemberPostCount", data.Creator.PostCount );
                block.Set( "post.MemberCreateTime", data.Creator.Created.ToShortDateString() );
                block.Set( "post.UserLastLogin", data.Creator.LastLoginTime.ToShortDateString() );

                String signature = strUtil.HasText( data.Creator.Signature ) ? "<div class=\"posC\">" + data.Creator.Signature + "</div>" : "";
                block.Set( "post.UserSignature", signature );

                block.Set( "post.OnlyOneMember", to( new TopicController().Show, data.TopicId ) + "?userId=" + data.Creator.Id );
                block.Set( "post.FloorNo", getFloorNumber( pageSize, i ) );
                block.Set( "post.Title", addRewardInfo( data, data.Title ) );

                block.Set( "post.TitleText", data.Title );

                block.Set( "post.Admin", getAdminString( data ) );
                block.Set( "post.CreateTime", data.Created );

                List<Attachment> attachList = getAttachByPost( attachs, data.Id );

                if (data.ParentId == 0) {
                    bindTopicOne( block, data, board, attachList );
                }
                else {
                    bindPostOne( block, data, board, attachList );
                }

                block.Set( "relativePosts", getRelativePosts( data ) );

                block.Set( "adForumPosts", AdItem.GetAdById( AdCategory.ForumPosts ) );


                block.Next();
            }

        }

        private string getRelativePosts( ForumPost data ) {

            if (data.ParentId > 0) return "";
            ForumTopic topic = data.Topic;
            if (topic == null) return "";

            String tagIds = topic.Tag.TagIds;
            if (strUtil.IsNullOrEmpty( tagIds )) return "";

            List<DataTagShip> list = DataTagShip.find( "TagId in (" + tagIds + ")" ).list( 21 );
            if (list.Count <= 1) return "";

            StringBuilder sb = new StringBuilder();
            sb.AppendLine( "<div class=\"relative-post\">" );
            sb.AppendLine( "<div class=\"relative-title\">相关文章</div>" );
            sb.AppendLine( "<ul>" );

            List<IAppData> addList = new List<IAppData>();

            foreach (DataTagShip dt in list) {

                if (dt.DataId == topic.Id && dt.TypeFullName == typeof( ForumTopic ).FullName) continue;

                EntityInfo ei = Entity.GetInfo( dt.TypeFullName );
                if (ei == null) continue;

                IAppData obj = ndb.findById( ei.Type, dt.DataId ) as IAppData;
                if (obj == null) continue;

                if (hasAdded( addList, obj )) continue;

                sb.AppendFormat( "<li><div><a href=\"{0}\">{1}</a></div></li>", alink.ToAppData( obj ), obj.Title );
                sb.AppendLine();

                addList.Add( obj );
            }

            sb.AppendLine( "</ul>" );
            sb.AppendLine( "<div style=\"clear:both;\"></div>" );
            sb.AppendLine( "</div>" );

            return sb.ToString();
        }

        private bool hasAdded( List<IAppData> xlist, IAppData obj ) {

            foreach (IAppData x in xlist) {
                if (x.Id == obj.Id && x.GetType() == obj.GetType()) return true;
            }
            return false;
        }

        private void bindTopicOne( IBlock block, ForumPost data, ForumBoard board, List<Attachment> attachList ) {

            String quoteLink = to( new Users.PostController().QuoteTopic, data.TopicId ) + "?boardId=" + data.ForumBoardId;
            String replyLink = to( new Users.PostController().ReplyTopic, data.TopicId ) + "?boardId=" + data.ForumBoardId;
            String topicUrl = LinkUtil.appendListPageToTopic( to( new TopicController().Show, data.TopicId ), ctx );

            block.Set( "post.ReplyQuoteUrl", quoteLink );
            block.Set( "post.ReplyUrl", replyLink );
            block.Set( "post.TitleStyle", string.Empty );
            block.Set( "post.PostUrl", topicUrl );
            String content = getTopicContent( board, data, attachList );
            block.Set( "post.Body", content );
            block.Set( "post.TagHtml", data.Topic.Tag.HtmlString );
            block.Set( "post.TagStyle", "" );

            block.Set( "post.PostFullUrl", getFullUrl( topicUrl ) );

            block.Set( "post.AdBody", AdItem.GetAdById( AdCategory.ForumTopicInner ) );

            block.Set( "nofollow", "" );
        }

        private String getFullUrl( String url ) {
            if (url == null) return "";
            if (url.StartsWith( "http" )) return url;
            return strUtil.Join( ctx.url.SiteAndAppPath, url );
        }

        private void bindPostOne( IBlock block, ForumPost data, ForumBoard board, List<Attachment> attachList ) {

            String quoteLink = to( new Users.PostController().QuotePost, data.Id ) + "?boardId=" + data.ForumBoardId;
            String replyLink = to( new Users.PostController().ReplyPost, data.Id ) + "?boardId=" + data.ForumBoardId;
            String postUrl = alink.ToAppData( data );

            block.Set( "post.ReplyQuoteUrl", quoteLink );
            block.Set( "post.ReplyUrl", replyLink );
            block.Set( "post.TitleStyle", "font-weight:normal" );
            block.Set( "post.PostUrl", postUrl );
            String content = getPostContent( board, data, attachList );
            block.Set( "post.Body", content );
            block.Set( "post.TagStyle", "display:none" );

            block.Set( "post.PostFullUrl", getFullUrl( postUrl ) );

            block.Set( "post.AdBody", AdItem.GetAdById( AdCategory.ForumTopicInner ) );

            block.Set( "nofollow", "rel=\"nofollow\"" );
        }

        //----------------------------------------------------------------------------------------------------------------------------------


        private String getAdminString( ForumPost post ) {
            return getAdminActions( post ) + " " + getEditAction( post );
        }

        private Boolean hasAdminPermission( ForumPost post ) {

            ForumBoard board = boardService.GetById( post.ForumBoardId, ctx.owner.obj );

            IList adminCmds = SecurityHelper.GetTopicAdminCmds( (User)ctx.viewer.obj, board, ctx );

            return adminCmds.Count > 0;
        }

        private String getAdminActions( ForumPost data ) {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat( "<span class=\"dropdown postAdmin\"><a href=\"#\" class=\"dropdown-toggle\" id=\"postAdmin{0}\" data-toggle=\"dropdown\" data-hover=\"dropdown\"><i class=\"icon-cog\"></i> {1} <span class=\"caret\"></span></a>", data.Id, alang( "admin" ) );
            sb.AppendFormat( "<ul class=\"dropdown-menu post-admin-items\" id=\"menuItems_postAdmin{0}\">", data.Id );
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
            return (ctx.route.page - 1) * pageSize + i + 1;
        }

        private String getTopicContent( ForumBoard board, ForumPost data, List<Attachment> attachList ) {

            if (data.Status == 1) return "<div class=\"banned\">" + alang( "postBanned" ) + "</div>";

            ForumTopic topic = populatePostTopic( data );

            String content = data.Content;

            if (topic.Price > 0) {

                if (ctx.viewer.IsAdministrator() || ctx.viewer.Id == topic.Creator.Id) {
                    content = getPriceContentStats( topic, data ) + content;
                }
                else if (buylogService.HasBuyed( ctx.viewer.Id, topic )) {
                    content = getBuyedContent( topic, data ) + content;
                }
                else {
                    return getNonBuyedContent( topic, data );
                }
            }

            content = addOtherDataContent( addLockedInfo( data, content ), topic );
            if (data.EditTime.Subtract( data.Created ).TotalMinutes > ForumConfig.Instance.ShowEditInfoTime) {
                content = addEditInfo( data, content );
            }
            content = addRateLog( data, content );

            if (topic.IsAttachmentLogin == 1 && ctx.viewer.IsLogin == false) {
                content += "<div class=\"downloadWarning\"><div>" + alang( "downloadNeedLogin" ) + "</div></div>";
            }
            else {
                content = addAttachment( board, data, attachList, content );
            }

            return content;
        }

        private String getPostContent( ForumBoard board, ForumPost data, List<Attachment> attachList ) {
            if (data.Status == 1) {
                return "<div class=\"banned\">" + alang( "postBanned" ) + "</div>";
            }
            String content = data.Content;
            if (data.EditTime.Subtract( data.Created ).TotalMinutes > ForumConfig.Instance.ShowEditInfoTime) {
                content = addEditInfo( data, content );
            }
            content = addRateLog( data, content );
            return addAttachment( board, data, attachList, content );
        }

        private ForumTopic populatePostTopic( ForumPost post ) {
            ForumTopic topic = topicService.GetById( post.TopicId, ctx.owner.obj );
            post.Topic = topic;
            return topic;
        }

        private String getUserTitle( ForumPost data ) {

            if (data.Creator.RoleId != SiteRole.NormalMember.Id) {
                return data.Creator.Role.Name;
            }

            if (moderatorService.IsModerator( data.AppId, data.Creator.Name )) {
                return "版主";
            }

            return data.Creator.Rank.Name;
        }

        //----------------------------------------------------------------------------------------------------------------------------------

        private String getNonBuyedContent( ForumTopic topic, ForumPost post ) {
            StringBuilder builder = getPriceContentPrefix( topic );
            builder.AppendFormat( "<div class=\"forum-price-cmd\" style=\"margin-top:10px;\"><a class=\"frmBox btn btn-primary\" href=\"{0}\"><i class=\"icon-hand-right icon-white\"></i> {1}</a></div>", to( new Users.PostController().Buy, post.Id ) + "?boardId=" + topic.ForumBoard.Id, alang( "buyTopic" ) );
            builder.Append( "</div></div>" );
            return builder.ToString();
        }

        private String getBuyedContent( ForumTopic topic, ForumPost post ) {
            StringBuilder builder = getPriceContentPrefix( topic );
            builder.AppendFormat( "<div class=\"forum-price-cmd\" style=\"margin-top:10px;\"><span class=\"btn btn-primary\"><i class=\"icon-ok icon-white\"></i> 您已经购买</a></div>" );
            builder.Append( "</div></div>" );
            return builder.ToString();
        }

        private String getPriceContentStats( ForumTopic topic, ForumPost post ) {
            StringBuilder builder = getPriceContentPrefix( topic );
            builder.Append( "</div></div>" );
            return builder.ToString();
        }

        private StringBuilder getPriceContentPrefix( ForumTopic topic ) {
            int buyerCount = buylogService.GetBuyerCount( topic.Id );
            StringBuilder builder = new StringBuilder();
            builder.Append( "<div id=\"forumPrice\" class=\"alert alert-warning\"><div class=\"forum-price-inner\">" );
            builder.AppendFormat( "<div class=\"forum-price-tip\">{0}</div>", string.Format( alang( "exPlsBuy" ), KeyCurrency.Instance.Name ) );
            builder.AppendFormat( "<div class=\"forum-price-price\">{0}: {1} {2}</div>", alang( "price" ), topic.Price, KeyCurrency.Instance.Unit );
            builder.AppendFormat( "<div class=\"forum-price-buyers\">{0}: {1}</div>", alang( "buyUsersCount" ), buyerCount );
            return builder;
        }

        //----------------------------------------------------------------------------------------------------------------------------------

        private void setBanAction( ForumPost data, StringBuilder sb ) {
            String strLock = "<li cmdurl=\"{0}\"><a href='#' class=\"postAdminItem putCmd\"><i class=\"icon-ban-circle\"></i> " + alang( "cmdBan" ) + "</a></li>";
            String strUnlock = "<li cmdurl=\"{0}\"><a href='#' class=\"postAdminItem putCmd\"><i class=\"icon-ban-circle\"></i> " + alang( "cmdUnban" ) + "</a></li>";
            if (data.Status == 0)
                sb.AppendFormat( strLock, to( new Moderators.PostSaveController().Ban, data.Id ) + "?boardId=" + data.ForumBoardId );
            else
                sb.AppendFormat( strUnlock, to( new Moderators.PostSaveController().UnBan, data.Id ) + "?boardId=" + data.ForumBoardId );
        }

        private void setCreditAction( ForumPost data, StringBuilder sb ) {
            sb.AppendFormat( "<li cmdurl=\"{0}\"><a href='#' class=\"postAdminItem frmBox\"><i class=\"icon-gift\"></i> {1}</a></li>", to( new Moderators.PostController().AddCredit, data.Id ) + "?boardId=" + data.ForumBoardId, alang( "cmdCredit" ) );
        }

        private void setDeleteAction( ForumPost data, StringBuilder sb ) {
            String strDelete = "<li cmdurl=\"{0}\"><a href='#' class=\"postAdminItem deleteCmd\"><i class=\"icon-trash\"></i> " + alang( "cmdDelete" ) + "</a></li>";
            if (data.ParentId == 0)
                sb.AppendFormat( strDelete, to( new Moderators.PostSaveController().DeleteTopic, data.TopicId ) + "?boardId=" + data.ForumBoardId );
            else
                sb.AppendFormat( strDelete, to( new Moderators.PostSaveController().DeletePost, data.Id ) + "?boardId=" + data.ForumBoardId );
        }

        private String getEditAction( ForumPost data ) {
            String strEdit = " <a href='{0}' class=\"editCmd {2}\" data-creatorId=\"{4}\" title=\"{3}\" xwidth=\"700\" xheight=\"430\"><img src=\"{1}edit.gif\"/> {3}</a>";
            if (data.ParentId == 0) {
                if (data.Topic.Reward > 0) {
                    return string.Format( strEdit, to( new Edits.TopicController().EditQ, data.TopicId ), sys.Path.Img, "", lang( "edit" ), data.Creator.Id );
                }
                else {
                    return string.Format( strEdit, to( new Edits.TopicController().Edit, data.TopicId ), sys.Path.Img, "", lang( "edit" ), data.Creator.Id );
                }
            }
            return string.Format( strEdit, to( new Edits.PostController().Edit, data.Id ), sys.Path.Img, "frmBox", lang( "edit" ), data.Creator.Id );
        }

        private void setEditActionItem( ForumPost data, StringBuilder sb ) {
            String str = "<li cmdurl=\"{0}\"><a href='#' class=\"postAdminItem {1}\" title=\"{2}\" xwidth=\"700\" xheight=\"430\"><i class=\"icon-edit\"></i> {2}</a></li>";
            if (data.ParentId == 0) {
                if (data.Topic.Reward > 0) {
                    sb.AppendFormat( str, to( new Edits.TopicController().EditQ, data.TopicId ), "", alang( "cmdEdit" ) );
                }
                else {
                    sb.AppendFormat( str, to( new Edits.TopicController().Edit, data.TopicId ), "", alang( "cmdEdit" ) );
                }
            }
            else {
                sb.AppendFormat( str, to( new Edits.PostController().Edit, data.Id ), "frmBox", alang( "cmdEdit" ) );
            }
        }

        private void setLockAction( ForumPost data, StringBuilder sb ) {
            if (data.ParentId <= 0) {
                ForumTopic topic = populatePostTopic( data );
                if (topic.IsLocked == 1) {
                    sb.AppendFormat( "<li cmdurl=\"{0}\"><a href='#' class=\"postAdminItem putCmd\"><i class=\"icon-lock\"></i> {1}</a></li>", to( new Moderators.PostSaveController().UnLock, topic.Id ) + "?boardId=" + data.ForumBoardId, alang( "cmdUnlock" ) );
                }
                else {
                    sb.AppendFormat( "<li cmdurl=\"{0}\"><a href='#' class=\"postAdminItem putCmd\"><i class=\"icon-lock\"></i> {1}</a></li>", to( new Moderators.PostSaveController().Lock, topic.Id ) + "?boardId=" + data.ForumBoardId, alang( "cmdLock" ) );
                }
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------


        private String addAttachment( ForumBoard board, ForumPost data, List<Attachment> attachList, String content ) {

            if (attachList.Count <= 0) return content;

            ISecurityAction action = SecurityAction.GetByAction( new AttachmentController().Show );

            Boolean hasAction = SecurityHelper.HasAction( (User)ctx.viewer.obj, board, action, ctx );
            if (!hasAction) {
                String amsg = string.Format( alang( "attachmentsInfo" ), attachList.Count );
                return content + "<div class=\"attachmentForbidden\"><span class=\"afText\">" + alang( "exAttachmentView" ) + "</span>(<span class=\"afInfo\">" + amsg + "</span>)</div>";
            }

            StringBuilder sb = new StringBuilder();
            String created = getAttachmentLastUpdateTime( attachList ).ToString();
            sb.Append( "<div class=\"hr\"></div><div class=\"attachmentTitleWrap\"><div class=\"attachmentTitle\">" + alang( "attachment" ) + " <span class=\"note\">(" + created + ")</span> " );
            if (ctx.viewer.Id == data.Creator.Id || hasAdminPermission( data )) {
                sb.AppendFormat( "<a href=\"{0}\">" + alang( "adminAttachment" ) + "</a>", to( new Edits.AttachmentController().Admin, data.TopicId ) );
            }

            sb.Append( "</div></div><ul class=\"attachmentList\">" );

            foreach (Attachment attachment in attachList) {

                string fileName = attachment.GetFileShowName();

                if (attachment.IsImage) {

                    sb.AppendFormat( "<li><div>{0} <span class=\"note\">({1}KB, {2})</span></div>", fileName, attachment.FileSizeKB, attachment.Created );
                    sb.AppendFormat( "<div><a href=\"{0}\" target=\"_blank\"><img src=\"{1}\" /></a></div></li>",
                        attachment.FileUrl, attachment.FileMediuUrl );
                }
                else {

                    sb.AppendFormat( "<li><div>{0} <span class=\"note right10\">({1}KB, {2})</span>", fileName, attachment.FileSizeKB, attachment.Created );

                    sb.AppendFormat( "<img src=\"{1}\" /><a href=\"{0}\" target=\"_blank\">" + alang( "hitDownload" ) + "</a></div>", to( new AttachmentController().Show, attachment.Id ) + "?id=" + attachment.Guid, strUtil.Join( sys.Path.Img, "/s/download.png" ) );
                }
            }
            sb.Append( "</ul>" );

            content = string.Format( "<div>{0}</div><div id=\"attachmentPanel\">{1}</div>", content, sb.ToString() );

            return content;
        }

        private DateTime getAttachmentLastUpdateTime( List<Attachment> attachList ) {

            DateTime result = attachList[0].Created;
            foreach (Attachment x in attachList) {
                if (x.Created > result) result = x.Created;
            }

            return result;
        }

        private String addEditInfo( ForumPost data, String content ) {
            User user = userService.GetById( data.EditMemberId );
            String str = string.Format( "<div class=\"updateNote\">" + alang( "lastEditInfo" ) + "</div>",
                data.EditTime, toUser( user ), user.Name );
            content = content + str;
            return content;
        }

        private String addLockedInfo( ForumPost data, String content ) {
            if (populatePostTopic( data ).IsLocked == 1) {
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
            builder.Append( "<div class=\"alert\">" );
            if (topic.RewardAvailable == 0) {
                builder.AppendFormat( "<img src=\"{0}{1}\"/>", sys.Path.Skin, "apps/forum/question_m.gif" );
                builder.AppendFormat( " {2}<span class=\"font12 left10\">({3}{0} {1}", topic.Reward, KeyCurrency.Instance.Unit, alang( "qResolved" ), alang( "reward" ) );
                builder.AppendFormat( " <a href=\"{0}\" class=\"frmBox\">{1}</a>)</span>", to( new Users.PostController().RewardList, topic.Id ) + "?boardId=" + topic.ForumBoard.Id, alang( "viewReward" ) );
            }
            else {
                builder.AppendFormat( "<img src=\"{0}{1}\"/>", sys.Path.Skin, "apps/forum/question_m.gif" );
                builder.AppendFormat( " {0} {1} {2}", alang( "rewardLable" ), topic.Reward, KeyCurrency.Instance.Unit );
                if (topic.RewardAvailable < topic.Reward) {
                    builder.AppendFormat( "，<span class=\"font12\">" + alang( "scoreStats" ) + "</span> ", topic.Reward - topic.RewardAvailable, topic.RewardAvailable );
                }
                if (topic.Creator.Id == ctx.viewer.Id) {
                    builder.AppendFormat( "<a href=\"{0}\" class=\"left20 btn\"><i class=\"icon-check\"></i> {1}</a>", to( new Users.PostController().SetReward, topic.Id ) + "?boardId=" + topic.ForumBoard.Id, alang( "cmdReward" ) );
                }
            }
            builder.Append( "</div>" );
            content = builder.ToString() + content;
            return content;
        }

        private String addContentInfo( ForumTopic data, String content ) {
            return content + "<div class=\"extDataPanel\">" + wojilu.Common.AppBase.ExtObject.GetExtView( data.Id, typeof( ForumTopic ).FullName, data.TypeName, ctx ) + "</div>";
        }

        private String addRateLog( ForumPost data, String content ) {
            if (data.Rate == 0) {
                return content;
            }
            List<ForumRateLog> logs = rateService.GetByPost( data.Id );
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat( "<div class=\"forum-rate-list\"><div class=\"forum-rate-title\">{0}</div>", alang( "creditLog" ) );
            sb.Append( "<table class=\"forum-rate-table table table-condensed\">" );
            foreach (ForumRateLog x in logs) {

                ICurrency currency = currencyService.GetICurrencyById( x.CurrencyId );

                sb.Append( "<tr>" );
                sb.AppendFormat( "<td class=\"forum-rate-user\"><a href=\"{0}\" target=\"_blank\">{1}</a></td>", toUser( x.User ), x.User.Name );
                sb.AppendFormat( "<td class=\"forum-rate-name\">{0} <span class=\"forum-rate-value\">+{1}</span></td>", currency.Name, x.Income );

                sb.AppendFormat( "<td class=\"forum-rate-note\">{0}</td>", x.Reason );
                sb.AppendFormat( "<td class=\"forum-rate-time\">{0}</td>", x.Created.ToString( "g" ) );

                sb.Append( "</tr>" );

            }
            sb.Append( "</table></div>" );
            return content + sb.ToString();
        }

        private String addRewardInfo( ForumPost data, String title ) {
            if (data.Reward > 0) {
                title = title + string.Format( "<span class=\"note\">({0}:{1})</span>", alang( "creditLabel" ), (int)data.Reward );
            }
            return title;
        }
    }
}
