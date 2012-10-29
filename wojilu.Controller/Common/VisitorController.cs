/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Text;

using wojilu.Apps;
using wojilu.Web.Mvc;
using wojilu.ORM;

using wojilu.Members.Users.Domain;
using wojilu.Common.Visitors;
using wojilu.Web.Mvc.Attr;
using wojilu.Common.AppBase.Interface;
using wojilu.Members.Users.Interface;
using wojilu.Members.Users.Service;

namespace wojilu.Web.Controller.Common {

    public class VisitorController : ControllerBase {

        public wojilu.Common.Visitors.VisitorService visitorService { get; set; }
        public IBlacklistService blacklistService { get; set; }

        public VisitorController() {
            visitorService = new wojilu.Common.Visitors.VisitorService();
            blacklistService = new BlacklistService();
        }

        public void List() {

            IAppData post = ctx.GetItem( "visitTarget" ) as IAppData;
            IDataVisitor visitor = ctx.GetItem( "visitor" ) as IDataVisitor;

            String visitLink = to( Visit, post.Id ) + "?type=" + visitor.GetType().FullName;
            set( "visitLink", visitLink );

            visitorService.setVisitor( visitor );
            IList visitorList = visitorService.GetRecent( post.Id, 8 );

            IBlock block = getBlock( "visitors" );
            foreach (User user in visitorList) {
                block.Set( "user.Name", user.Name );
                block.Set( "user.Face", user.PicSmall );
                block.Set( "user.Link", toUser( user ) );
                block.Next();
            }
        }

        [HttpPut, DbTransaction]
        public void Visit( int id ) {

            if (ctx.viewer.IsLogin == false) {
                echoRedirect( lang( "exPlsLogin" ) );
                return;
            }

            if (blacklistService.IsBlack( ctx.owner.Id, ctx.viewer.Id )) {
                echoRedirect( lang( "backVisitPost" ) );
                return;
            }

            if (ctx.viewer.Id == ctx.owner.obj.Id && ctx.owner.obj.GetType() == typeof( User )) {
                echoRedirect( lang( "exVisitSelf" ) );
                return;
            }

            String visitorType = ctx.Get( "type" );
            if (MappingClass.Instance.FactoryList[visitorType] == null) {
                echoRedirect( lang( "exTypeError" ) );
                return;
            }

            IDataVisitor visitor = Entity.New( visitorType ) as IDataVisitor;
            if (visitor == null) {
                echoRedirect( lang( "exTypeError" ) );
                return;
            }

            User owner = ctx.owner.obj as User;
            if (ctx.viewer.HasPrivacyPermission( owner, UserPermission.VisitLog.ToString() ) == false) {
                echoRedirect( lang( "exVisitLogNoPermission" ) );
                return;
            }


            visitorService.setVisitor( visitor );

            IEntity post = ndb.findById( visitor.getTargetType(), id );
            if (post == null) {
                echoRedirect( lang( "exDataNotFound" ) );
                return;
            }

            IDataVisitor v = visitorService.Visit( ctx.viewer.Id, post as IAppData );

            String url = ctx.web.PathReferrer + "#visitorList";
            redirectUrl( url );
        }



    }

}
