/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Members.Users.Interface;
using wojilu.Members.Users.Service;
using wojilu.Members.Users.Domain;
using wojilu.Common.Microblogs.Service;
using wojilu.Common.Microblogs.Domain;
using wojilu.Common.Microblogs.Interface;

namespace wojilu.Web.Controller.Microblogs.My {

    public partial class MicroblogCommentsController : ControllerBase {

        public IMicroblogService microblogService { get; set; }
        public IFollowerService followService { get; set; }
        public MicroblogCommentService commentService { get; set; }

        public MicroblogCommentsController() {
            microblogService = new MicroblogService();
            followService = new FollowerService();
            commentService = new MicroblogCommentService();

            LayoutControllerType = typeof( MicroblogController );
        }

        public void My() {

            load( "publisher", new Microblogs.My.MicroblogController().Publisher );


            DataPage<MicroblogComment> list = commentService.GetPageByUser( ctx.owner.Id, 25 );

            IBlock block = getBlock( "list" );

            foreach (MicroblogComment c in list.Results) {

                if (c.Root == null) continue;

                block.Set( "c.UserName", c.User.Name );
                block.Set( "c.UserFace", c.User.PicSmall );
                block.Set( "c.UserLink", toUser( c.User ) );
                block.Set( "c.UserName", c.User.Name );

                block.Set( "c.Created", c.Created );
                block.Set( "c.Content", c.Content );

                block.Set( "c.Microblog", strUtil.CutString( c.Root.Content, 20 ) );
                block.Set( "c.MicroblogLink", to( new wojilu.Web.Controller.Microblogs.MicroblogController().Show, c.Root.Id ) );

                block.Next();

            }

            set( "page", list.PageBar );
        }

        [HttpDelete, DbTransaction]
        public void Delete( int id ) {

            if (hasPermission()==false) {
                echoText( lang( "exNoPermission" ) );
                return;
            }

            MicroblogComment c = MicroblogComment.findById( id );
            if (c == null) {
                echoText( lang( "exDataNotFound" ) );
                return;
            }

            c.delete();

            echoAjaxOk();

        }

        private bool hasPermission() {
            if (ctx.viewer.IsAdministrator()) return true;
            if (ctx.viewer.Id == ctx.owner.Id) return true;
            return false;
        }




    }
}
