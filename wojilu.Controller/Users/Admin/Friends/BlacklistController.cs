/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;

using wojilu.Members.Users.Service;
using wojilu.Members.Users.Domain;
using wojilu.Members.Users.Interface;

namespace wojilu.Web.Controller.Users.Admin.Friends {

    public class BlacklistController : ControllerBase {

        // 【消息屏蔽】某些好友愿意保留，但想屏蔽它的动态消息

        // 访问脚印：谁来看过我+我看过谁；
        // 用户名、头像、微博、来访时间+访问时间

        public IBlacklistService blacklistService { get; set; }

        public BlacklistController() {
            base.LayoutControllerType = typeof( FriendController );
            blacklistService = new BlacklistService();
        }

        public void Index() {

            set( "addLink", to( Add ) );

            DataPage<Blacklist> list = blacklistService.GetPage( ctx.owner.Id, 20 );

            IBlock block = getBlock( "list" );
            foreach (Blacklist b in list.Results) {
                block.Set( "m.Name", b.Target.Name );
                block.Set( "m.UrlFull", toUser( b.Target ) );
                block.Set( "m.FaceFull", b.Target.PicSmall );
                block.Set( "m.DeleteUrl", to( Delete, b.Id ) );
                block.Next();
            }

            set( "page", list.PageBar );
        }

        public void Add() {
            target( Create );
        }

        [HttpPost, DbTransaction]
        public void Create() {

            String targetUserName = strUtil.CutString( ctx.Post( "UserName" ), 20 );

            Result result = blacklistService.Create( ctx.owner.Id, targetUserName );

            if (result.HasErrors) {
                errors.Join( result );
                echoError();
                return;
            }

            echoRedirect( lang( "opok" ), Index );
        }

        [HttpDelete]
        public void Delete( int id ) {

            Result result = blacklistService.Delete( id, ctx.owner.Id );
            if (result.HasErrors ) {
                errors.Join( result );
                echoError( );
                return;
            }

            echoRedirect( lang( "opok" ) );
        }


    }

}
