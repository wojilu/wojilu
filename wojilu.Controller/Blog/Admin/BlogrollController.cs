/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;

using wojilu.Web;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;

using wojilu.Apps.Blog.Domain;
using wojilu.Apps.Blog.Interface;
using wojilu.Apps.Blog.Service;


namespace wojilu.Web.Controller.Blog.Admin {

    [App( typeof( BlogApp ) )]
    public class BlogrollController : ControllerBase {

        public IBlogrollService rollService { get; set; }

        public BlogrollController() {
            rollService = new BlogrollService();
        }

        public void AdminList() {

            set( "addLink", to( Add ) );

            IList blogrolls = rollService.GetByApp( ctx.app.Id, ctx.owner.obj.Id );
            IBlock block = getBlock( "list" );
            foreach (Blogroll blogroll in blogrolls) {
                block.Set( "roll.Name", strUtil.SubString( blogroll.Name, 10 ) );
                block.Set( "roll.Link", blogroll.Link );
                block.Set( "roll.OrderId", blogroll.OrderId );
                block.Set( "roll.EditUrl", to( Edit, blogroll.Id ) );
                block.Set( "roll.DeleteUrl", to( Delete, blogroll.Id ) );
                block.Next();
            }
        }

        public void Add() {
            target( Create );
        }

        [HttpPost, DbTransaction]
        public void Create() {
            Blogroll roll = Validate( null );
            if (errors.HasErrors) {
                echoError();
            }
            else {
                rollService.Insert( roll, ctx.owner.obj.Id, ctx.app.Id );
                echoRedirectPart( lang( "opok" ), to( AdminList ) );
            }
        }

        public void Edit( int id ) {
            Blogroll blogroll = rollService.GetById( id, ctx.app.Id );
            if (blogroll == null) {
                echoRedirect( lang( "exDataNotFound" ) );
                return;
            }
            set( "roll.Name", blogroll.Name );
            set( "roll.Link", blogroll.Link );
            set( "roll.OrderId", blogroll.OrderId );
            set( "roll.Description", blogroll.Description );
            target( Update, id );
        }

        [HttpPost, DbTransaction]
        public void Update( int id ) {
            Blogroll blogroll = rollService.GetById( id, ctx.app.Id );
            blogroll = Validate( blogroll );
            if (errors.HasErrors) {
                echoError();
            }
            else {
                rollService.Update( blogroll );
                echoRedirectPart( lang( "opok" ), to( AdminList ) );
            }
        }

        [HttpDelete, DbTransaction]
        public void Delete( int id ) {
            Blogroll blogroll = rollService.GetById( id, ctx.app.Id );
            if (blogroll != null) {
                rollService.Delete( blogroll );
                redirect( AdminList );
            }
            else {
                echoRedirect( lang( "exDataNotFound" ) );
            }
        }

        private Blogroll Validate( Blogroll roll ) {

            if (roll == null) roll = new Blogroll();

            roll.Name = ctx.Post( "Name" );
            roll.OrderId = ctx.PostInt( "OrderId" );
            roll.Link = ctx.Post( "Link" );
            roll.Description = ctx.Post( "Description" );

            if (strUtil.IsNullOrEmpty( roll.Name )) errors.Add( lang( "exName" ) );
            if (strUtil.IsNullOrEmpty( roll.Link )) errors.Add( lang( "exUrl" ) );

            if (!(!strUtil.HasText( roll.Link ) || roll.Link.ToLower().StartsWith( "http://" ))) {
                roll.Link = "http://" + roll.Link;
            }

            return roll;
        }
    }
}
