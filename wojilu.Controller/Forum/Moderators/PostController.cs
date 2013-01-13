/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Apps.Forum.Domain;
using wojilu.Apps.Forum.Interface;
using wojilu.Apps.Forum.Service;
using wojilu.Common.Money.Domain;
using wojilu.Common.Money.Interface;
using wojilu.Common.Money.Service;
using wojilu.Common.Resource;
using wojilu.Members.Users.Domain;
using wojilu.Web.Controller.Forum.Utils;

namespace wojilu.Web.Controller.Forum.Moderators {

    [App( typeof( ForumApp ) )]
    public class PostController : ControllerBase {

        public IForumBoardService boardService { get; set; }
        public ICurrencyService currencyService { get; set; }
        public IForumTopicService topicService { get; set; }
        public IForumPostService postService { get; set; }
        public IForumRateService rateService { get; set; }

        public PostController() {
            topicService = new ForumTopicService();
            boardService = new ForumBoardService();
            postService = new ForumPostService();
            currencyService = new CurrencyService();
            rateService = new ForumRateService();
        }



        private Boolean boardError( ForumTopic topic ) {
            if (ctx.GetInt( "boardId" ) != topic.ForumBoard.Id) {
                echoRedirect( lang( "exNoPermission" ) + ": borad id error" );
                return true;
            }
            return false;
        }

        private Boolean boardError( ForumPost post ) {
            if (ctx.GetInt( "boardId" ) != post.ForumBoardId) {
                echoRedirect( lang( "exNoPermission" ) + ": borad id error" );
                return true;
            }
            return false;
        }

        //--------------------------------------------------------------------------


        public void AddCredit( int id ) {

            String msg = "<div style=\"font-size:22px;color:red;font-weight:bold;margin-top:30px; text-align:center;\">{0}</div>";

            if (rateService.HasRate( ctx.viewer.Id, id )) {
                content( string.Format( msg, alang( "exRewarded" ) ) );
                return;
            }

            ForumPost post = postService.GetById( id, ctx.owner.obj );
            if (post == null) {
                content( string.Format( msg, alang( "exPostNotFound" ) ) );
                return;
            }

            if (post.Creator.Id == ctx.viewer.Id) {
                content( string.Format( msg, alang( "exNotAllowSelfCredit" ) ) );
                return;
            }

            if (boardError( post )) return;


            ForumBoard board = boardService.GetById( post.ForumBoardId, ctx.owner.obj );

            set( "ActionLink", to( new PostSaveController().SaveCredit, id ) + "?boardId=" + board.Id );


            IList currencyList = currencyService.GetForumRateCurrency();
            dropList( "CurrencyId", currencyList, "Name=Id", 0 );

            List<PropertyItem> values = getCurrencyValues();
            dropList( "CurrencyValue", values, "Value=Value", 2 );
        }


        private List<PropertyItem> getCurrencyValues() {
            List<PropertyItem> values = new List<PropertyItem>();
            int rateMaxValue = ((ForumApp)ctx.app.obj).MaxRateValue;
            for (int i = rateMaxValue / 2; i > 0; i--) {
                values.Add( new PropertyItem( "CurencyValue", -i * 2 ) );
            }
            for (int i = 1; i <= (rateMaxValue / 2); i++) {
                values.Add( new PropertyItem( "CurencyValue", i * 2 ) );
            }
            return values;
        }

        private int getPageSize() { return 100; }

        private Tree<ForumBoard> _tree;

        private Tree<ForumBoard> getTree() {
            if (_tree == null) _tree = new Tree<ForumBoard>( boardService.GetBoardAll( ctx.app.Id, ctx.viewer.IsLogin ) );
            return _tree;
        }
    }

}
