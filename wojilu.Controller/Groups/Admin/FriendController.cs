/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Members.Groups.Service;
using wojilu.Members.Groups.Domain;
using wojilu.Web.Mvc.Attr;
using wojilu.Members.Groups.Interface;
using wojilu.Members.Interface;
using wojilu.Members.Users.Domain;
using wojilu.Web.Controller.Security;

namespace wojilu.Web.Controller.Groups.Admin {

    public class FriendController : ControllerBase {

        public IGroupFriendService gfService { get; set; }
        public IGroupService groupService { get; set; }
        public IAdminLogService<GroupLog> logService { get; set; }

        public FriendController() {
            gfService = new GroupFriendService();
            groupService = new GroupService();
            logService = new GroupLogService();
        }

        public void Index() {

            DataPage<Group> list = gfService.GetPage( ctx.owner.Id, 20 );

            IBlock block = getBlock( "list" );
            foreach (Group g in list.Results) {
                block.Set( "g.Name", g.Name );
                block.Set( "g.Logo", g.LogoSmall );
                block.Set( "g.Link", Link.ToMember(g) );
                block.Set( "g.DeleteLink", to( Delete, g.Id ) );
                block.Next();
            }

            set( "page", list.PageBar );

            set( "addLink", to( Add ) );

        }

        public void Add() {
            target( Create );
        }

        [HttpPost, DbTransaction]
        public void Create() {

            String name = ctx.Post( "Name" );
            if (strUtil.IsNullOrEmpty( name )) {
                errors.Add( lang( "exName" ) );
                run( Add );
                return;
            }

            Group friend = groupService.GetByName( name );
            if (friend == null) {
                errors.Add( lang( "exFriendGroupNotFound" ) );
                run( Add );
                return;
            }

            Result result = gfService.AddFriend( ctx.owner.obj, name );
            if (result.HasErrors) {
                errors.Join( result );
                run( Add );
            }
            else {
                log( SiteLogString.AddFriendGroup(), friend );
                echoToParentPart( lang( "opok" ) );
            }

        }

        [HttpDelete, DbTransaction]
        public void Delete( int id ) {

            GroupFriends gf = gfService.GetFriend( ctx.owner.Id, id );
            if (gf == null) {
                echoRedirect( lang( "exGroupNotFound" ) );
            }
            else {
                gfService.Delete( gf );
                log( SiteLogString.DeleteFriendGroup(), gf.Friend );
                redirect( Index );
            }

        }

        private void log( String msg, Group g ) {
            String dataInfo = "{Id:" + g.Id + ", Name:'" + g.Name + "'}";
            logService.Add( (User)ctx.viewer.obj, msg, dataInfo, typeof( Group ).FullName, ctx.Ip );
        }

    }

}
