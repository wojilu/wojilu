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

        public virtual IMicroblogService microblogService { get; set; }
        public virtual IFollowerService followService { get; set; }
        public virtual MicroblogCommentService commentService { get; set; }

        public MicroblogCommentsController() {
            microblogService = new MicroblogService();
            followService = new FollowerService();
            commentService = new MicroblogCommentService();

            LayoutControllerType = typeof( MicroblogController );
        }

        public virtual void My() {

            content( loadHtml( new wojilu.Web.Controller.Users.Admin.HomeController().Comment ) );

        }

        //[HttpDelete, DbTransaction]
        //public virtual void Delete( long id ) {

        //    if (hasPermission() == false) {
        //        echoText( lang( "exNoPermission" ) );
        //        return;
        //    }

        //    MicroblogComment c = MicroblogComment.findById( id );
        //    if (c == null) {
        //        echoText( lang( "exDataNotFound" ) );
        //        return;
        //    }

        //    c.delete();

        //    echoAjaxOk();

        //}

        private bool hasPermission() {
            if (ctx.viewer.IsAdministrator()) return true;
            if (ctx.viewer.Id == ctx.owner.Id) return true;
            return false;
        }




    }
}
