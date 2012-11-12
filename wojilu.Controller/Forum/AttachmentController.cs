/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Apps.Forum.Domain;
using wojilu.Apps.Forum.Service;
using wojilu.Apps.Forum.Interface;
using wojilu.Web.Utils;
using wojilu.Members.Users.Domain;
using wojilu.Common.AppBase.Interface;
using wojilu.Web.Controller.Forum.Utils;
using wojilu.Common.AppBase;
using wojilu.Common.Money.Interface;
using wojilu.Common.Money.Service;
using wojilu.Common.Money.Domain;

namespace wojilu.Web.Controller.Forum {

    [App( typeof( ForumApp ) )]
    public class AttachmentController : ControllerBase {

        public IAttachmentService attachmentService { get; set; }
        public IForumTopicService topicService { get; set; }
        public IAttachmentService attachService { get; set; }
        public IForumBoardService boardService { get; set; }
        public ICurrencyService currencyService { get; set; }
        public IUserIncomeService incomeService { get; set; }

        public AttachmentController() {
            boardService = new ForumBoardService();
            attachmentService = new AttachmentService();
            topicService = new ForumTopicService();
            attachService = new AttachmentService();
            currencyService = new CurrencyService();
            incomeService = new UserIncomeService();
        }

        private Tree<ForumBoard> _tree;

        private Tree<ForumBoard> getTree() {
            if (_tree == null) _tree = new Tree<ForumBoard>( boardService.GetBoardAll( ctx.app.Id, ctx.viewer.IsLogin ) );
            return _tree;
        }

        public void Show( int id ) {

            String guid = ctx.Get( "id" );

            Attachment attachment = attachmentService.GetById( id, guid );
            if (attachment == null) {
                echoRedirect( lang( "exDataNotFound" ) );
                return;
            }

            // 权限控制
            ForumTopic topic = topicService.GetById( attachment.TopicId, ctx.owner.obj );
            if (SecurityHelper.Check( this, topic.ForumBoard ) == false) return;

            // 积分不够不能下载
            User user = ctx.viewer.obj as User;
            if (getUserDownloadMoney( user ) < getDownloadRequirement()) {
                echoRedirect( "对不起，您的积分不够" );
                return;
            }

            // 增加下载记录，扣除用户的下载币
            attachmentService.AddHits( attachment, ctx.viewer.obj as User );

            // 检查盗链
            if (isDownloadValid() == false) {
                echoRedirect( alang( "exDownload" ) );
                return;
            }

            // 转发
            redirectUrl( attachment.FileUrl );
        }

        // 用户持有的下载币
        private int getUserDownloadMoney( User user ) {

            if (user == null || user.Id <= 0) return 0;

            int currencyId = getDownloadCurrency();
            if (currencyId <= 0) return 0;

            UserIncome income = incomeService.GetUserIncome( user.Id, currencyId );
            return income == null ? 0 : income.Income;
        }

        // 系统对下载设置的货币要求
        private int getDownloadRequirement() {
            int currencyId = getDownloadCurrency();
            if (currencyId <= 0) return 0;
            IncomeRule rule = currencyService.GetRuleByActionAndCurrency( UserAction.Forum_DownloadAttachment.Id, currencyId );
            return rule == null ? 0 : -rule.Income; // 负值转为正值
        }

        // 下载币
        private int getDownloadCurrency() {
            Currency x = Currency.DownloadCurrency();
            return x == null ? 0 : x.Id;
        }

        private Boolean isDownloadValid() {

            if (ctx.web.PathReferrer == null) return false;

            return ctx.web.PathReferrerHost.Equals( ctx.url.Host );
        }




    }

}
