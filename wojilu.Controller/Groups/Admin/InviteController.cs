/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Members.Users.Domain;
using wojilu.Members.Groups.Domain;
using wojilu.Members.Users.Interface;
using wojilu.Members.Users.Service;
using wojilu.Common.Msg.Interface;
using wojilu.Common.Msg.Service;
using wojilu.Members.Groups.Interface;
using wojilu.Members.Groups.Service;
using wojilu.Web.Mvc.Attr;

namespace wojilu.Web.Controller.Groups.Admin {

    public class InviteController : ControllerBase {

        public wojilu.Members.Groups.Service.InviteService inviteService { get; set; }

        public InviteController() {
            inviteService = new wojilu.Members.Groups.Service.InviteService();
        }

        public override void Layout() {
            set( "addUrl", to( Add ) );
            set( "listUrl", to( List ) );
        }

        public void Add() {
            target( Create );
            set( "lnkSelectFriends", Link.To( ctx.viewer.obj, new Users.Admin.Friends.FriendController().SelectBox ) );
        }

        [HttpPost, DbTransaction]
        public void Create() {

            String receiver = ctx.Post( "receiver" );

            String lnkInvite = to( new JoinController().Invite, 999 );

            Result result = inviteService.Invite( ctx.viewer.obj as User, receiver, ctx.owner.obj as Group, ctx.PostHtml( "msg" ), lnkInvite );

            if (result.HasErrors) {
                echoError( result );
            }
            else {
                echoRedirect( lang( "opok" ) );
            }
        }

        public void List() {

            DataPage<GroupInvite> list = inviteService.GetPage( ctx.owner.Id );

            bindInvite( list.Results );
            set( "page", list.PageBar );
            set( "addUrl", to( Add ) );
        }

        private void bindInvite( List<GroupInvite> list ) {

            IBlock block = getBlock( "list" );
            foreach (GroupInvite g in list) {

                block.Set( "g.Inviter", g.Inviter.Name );
                block.Set( "g.InviterLink", toUser( g.Inviter ) );

                block.Set( "g.Receiver", g.Receiver.Name );
                block.Set( "g.ReceiverLink", toUser( g.Receiver ) );

                block.Set( "g.Created", g.Created );

                block.Set( "g.StatusStr", g.StatusStr );
                block.Next();
            }

        }


    }

}
