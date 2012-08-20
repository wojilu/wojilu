/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;

using wojilu.Apps.Forum.Domain;
using wojilu.Apps.Forum.Service;
using wojilu.Web.Controller.Forum.Utils;
using wojilu.Common.AppBase;
using wojilu.Common.AppBase.Interface;
using wojilu.Members.Users.Domain;
using wojilu.Apps.Forum.Interface;

namespace wojilu.Web.Controller.Forum.Admin {

    [App( typeof( ForumApp ) )]
    public class ForumLinkController : ControllerBase {

        public IForumLinkService linkService { get; set; }
        public IForumLogService logService { get; set; }

        public ForumLinkController() {
            linkService = new ForumLinkService();
            logService = new ForumLogService();
        }

        public void List() {
            set( "addLink", to( New ) );
            set( "sortAction", to( SaveSort ) );
            bindForumLink( linkService.GetByApp( ctx.app.Id, ctx.owner.Id ), getBlock( "list" ) );
        }

        private void bindForumLink( List<ForumLink> list, IBlock block ) {
            foreach (ForumLink link in list) {
                block.Set( "link.Id", link.Id );
                block.Set( "link.Name", link.Name );

                String logo = strUtil.HasText( link.Logo ) ? string.Format( "<img src=\"{0}\"/>", link.Logo ) : "";

                block.Set( "link.Logo", logo );
                block.Set( "link.Url", link.Url );
                block.Set( "link.Created", link.Created );
                block.Set( "link.EditUrl", to( Edit, link.Id ) );
                block.Set( "link.DeleteUrl", to( Delete, link.Id ) );
                block.Next();
            }
        }

        public void New() {
            target( Create  );
            bind( "link", new ForumLink() );
        }

        [HttpPost, DbTransaction]
        public void Create() {
            ForumLink link = ForumValidator.ValidateLink(ctx);
            if (errors.HasErrors) {
                run( New );
                return;
            }

            Result result = linkService.Insert( link );
            if (result.HasErrors) {
                errors.Join( result );
                run( New );
                return;
            }

            logService.Add( (User)ctx.viewer.obj, ctx.app.Id, alang( "logAddLink" ) + ":" + link.Name, ctx.Ip );
            redirect( List );
        }

        public void Edit( int id ) {
            view( "New" );
            ForumLink link = linkService.GetById( id, ctx.owner.obj );
            if (link == null) {
                echoRedirect( lang( "exDataNotFound" ) );
                return;
            }

            bind( "link", link );
            target( Update, id  );
        }

        [HttpPost, DbTransaction]
        public void Update( int id ) {

            ForumLink link = linkService.GetById( id, ctx.owner.obj );
            if (link == null) {
                echoRedirect( lang( "exDataNotFound" ) );
                return;
            }

            link = ForumValidator.ValidateLink( link, ctx );
            if (errors.HasErrors) {
                run( Edit, id );
                return;
            }

            Result result = linkService.Update( link );
            if (result.HasErrors) {
                errors.Join( result );
                run( Edit, id );
                return;
            }

            logService.Add( (User)ctx.viewer.obj, ctx.app.Id, alang( "logEditLink" ) + ":" + link.Name, ctx.Ip );
            redirect( List );
        }

        [HttpDelete, DbTransaction]
        public void Delete( int id ) {

            ForumLink link = linkService.GetById( id, ctx.owner.obj );
            if (link == null) {
                echoRedirect( lang( "exDataNotFound" ) );
                return;
            }

            linkService.Delete( link );
            redirect( List );
        }

        [HttpPost, DbTransaction]
        public void SaveSort() {

            int id = ctx.PostInt( "id" );
            String cmd = ctx.Post( "cmd" );

            ForumLink link = linkService.GetById( id, ctx.owner.obj );
            if (link == null) {
                echoRedirect( lang( "exDataNotFound" ) );
                return;
            }

            List<ForumLink> list = linkService.GetByApp( ctx.app.Id, ctx.owner.Id );

            if (cmd == "up") {
                new SortUtil<ForumLink>( link, list ).MoveUp();
                echoJsonOk();
            }
            else if (cmd == "down") {
                new SortUtil<ForumLink>( link, list ).MoveDown();
                echoJsonOk();
            }
            else {
                echoError( lang( "exUnknowCmd" ) );
            }
        }


    }
}

