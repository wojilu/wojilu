/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;

using wojilu.Common.Onlines;
using wojilu.Common.Resource;

using wojilu.Members.Sites.Service;
using wojilu.Members.Users.Domain;
using wojilu.Members.Users.Interface;
using wojilu.Members.Users.Service;

using wojilu.Web.Controller.Users.Caching;

namespace wojilu.Web.Controller.Users {

    public class MainController : ControllerBase {

        public IUserService userService;
        public ISiteRoleService roleService;
        public UserTagService userTagService { get; set; }


        public MainController() {
            userService = new UserService();
            roleService = new SiteRoleService();
            userTagService = new UserTagService();
        }

        [CacheAction( typeof( UserMainLayoutCache ) )]
        public override void Layout() {
            set( "SearchAction", to( Search ) );

            bindSimple();

            List<User> picked = userService.GetPickedList( 20 );
            bindUsers( picked, "picked" );

            // 当前app/module所有页面，所属的首页
            ctx.SetItem( "_moduleUrl", to( Index ) );

            bindAdminLink();
        }

        private void bindAdminLink() {
            set( "feedLink", to( new wojilu.Web.Controller.Admin.FeedAdminController().Index ) );

            set( "userListLink", to( new wojilu.Web.Controller.Admin.Members.UserController().Index ) );
            set( "siteMsgLink", to( new wojilu.Web.Controller.Admin.Members.SiteMsgController().Index ) );
            set( "importLink", to( new wojilu.Web.Controller.Admin.Members.ImportController().Index ) );
            set( "settingLink", to( new wojilu.Web.Controller.Admin.Members.UserSettingController().Index ) );
            set( "regLink", to( new wojilu.Web.Controller.Admin.Members.UserRegController().Index ) );
            set( "templateLink", to( new wojilu.Web.Controller.Admin.Members.EmailConfirmController().EditTemplate ) );
        }

        [CachePage( typeof( UserMainPageCache ) )]
        [CacheAction( typeof( UserMainIndexCache ) )]
        public void Index() {

            ctx.Page.Title = lang( "user" );
            ctx.Page.Keywords = config.Instance.Site.UserPageKeywords;
            ctx.Page.Description = config.Instance.Site.UserPageDescription;

            set( "lnkRank", to( Rank ) );
            set( "lnkOnlineAll", to( OnlineAll ) );
            set( "linkAll", to( ListAll ) );

            //bindOnlineInfos( OnlineService.GetRecent( 20 ) );
            set( "onlineMemberLink", to( OnlineUserData ) );

            List<User> ranks = userService.GetRanked( 21 );
            List<User> newList = userService.GetNewListValid( 21 );
            bindUsers( ranks, "ranks" );
            bindUsers( newList, "users" );
        }

        public void OnlineUserData() {

            List<OnlineUser> users = OnlineService.GetRecent( 20 );
            echoJson( users );
        }

        public void Rank() {

            ctx.Page.Title = lang( "userCharts" );

            int rankCount = 20;

            List<User> rankCredit = userService.GetRanked( "credit", rankCount );
            List<User> rankHits = userService.GetRanked( "hits", rankCount );
            List<User> rankPosts = userService.GetRanked( "posts", rankCount );

            int indexCount = rankCredit.Count;
            if (rankHits.Count > indexCount) indexCount = rankHits.Count;
            if (rankPosts.Count > indexCount) indexCount = rankPosts.Count;

            IBlock iblock = getBlock( "rankIndex" );
            for (int i = 1; i < indexCount + 1; i++) {
                iblock.Set( "rIndex", i );
                iblock.Next();
            }

            bindUsers( rankCredit, "Credit" );
            bindUsers( rankHits, "Hits" );
            bindUsers( rankPosts, "Posts" );
        }

        public void OnlineUser() {

            ctx.Page.Title = lang( "onlineUsers" );

            DataPage<OnlineUser> users = DataPage<OnlineUser>.GetPage( OnlineService.GetLoggerUser(), 70 );
            bindOnlineInfos( users.Results );

            int userCount = users.RecordCount;
            int totalCount = OnlineService.GetAll().Count;
            int guestCount = totalCount - userCount;

            set( "userCount", userCount );
            set( "totalCount", totalCount );
            set( "guestCount", guestCount );

            set( "allOnlinLink", to( OnlineAll ) );

            set( "page", users.PageBar );
        }

        private void bindOnlineInfos( List<OnlineUser> users ) {
            IBlock block = getBlock( "onlines" );
            foreach (OnlineUser user in users) {
                bindUserSingle( block, user );
                block.Next();
            }
        }

        public void OnlineAll() {

            ctx.Page.Title = "所有在线用户";

            DataPage<OnlineUser> users = DataPage<OnlineUser>.GetPage( OnlineService.GetAll(), 70 ); // 已排序

            set( "onlineCount", users.RecordCount );

            IBlock block = getBlock( "onlines" );
            foreach (OnlineUser user in users.Results) {

                bindUserSingle( block, user );

                block.Next();
            }


            set( "page", users.PageBar );
        }

        private void bindUserSingle( IBlock block, OnlineUser user ) {

            String ip = ctx.viewer.IsAdministrator() ? user.Ip : user.GetIp( 1 );

            String lblValue = "【" + lang( "ipAddress" ) + "】" + ip +
"\n【" + lang( "osInfo" ) + "】" + user.Agent +
"\n【" + lang( "startTime" ) + "】" + user.StartTime.ToString() +
"\n【" + lang( "lastActive" ) + "】" + user.LastActive.ToString() +
"\n【" + lang( "clocation" ) + "】" + user.Location;


            if (user.UserId > 0) {
                block.Set( "u.Name", user.UserName );
                block.Set( "u.Face", user.UserPicUrl );
                block.Set( "u.Link", user.UserUrl );
            }
            else {
                block.Set( "u.Name", UserFactory.Guest.Name );
                block.Set( "u.Face", UserFactory.Guest.PicSmall );
                block.Set( "u.Link", "javascript:;" );
            }

            block.Set( "u.Info", lblValue );
        }

        public void ListAll() {

            ctx.Page.Title = lang( "allUser" );

            DataPage<User> list = userService.GetAllValid( 56 );
            bindUsers( list.Results, "list" );
            set( "page", list.PageBar );
        }


        public void Tag( int id ) {

            Page.Title = "根据tag搜索用户";
            target( Tag, 0 );



            String tagName = "";
            DataPage<User> list = new DataPage<User>();

            UserTag t = null;
            String tName = strUtil.SqlClean( ctx.Get( "tagName" ), 10 );
            if (strUtil.HasText( tName )) {
                t = userTagService.GetTagByName( tName );

            }
            else {
                t = userTagService.GetTagById( id );
            }

            if (t != null) {
                tagName = t.Name;
                list = userTagService.GetPageByTag( t.Id );
            }


            set( "tagName", tagName );
            IBlock block = getBlock( "users" );
            foreach (User u in list.Results) {
                block.Set( "user.Name", u.Name );
                block.Set( "user.Link", toUser( u ) );
                block.Set( "user.LinkT", alink.ToUserMicroblog( u ) );
                block.Set( "user.PicSmall", u.PicSmall );

                block.Set( "user.Description", u.Profile.Description );

                block.Next();
            }

            set( "page", list.PageBar );

        }

        public void Search() {

            HideLayout( typeof( MainController ) );

            // 当前app/module所有页面，所属的首页
            ctx.SetItem( "_moduleUrl", to( Index ) );

            set( "userMainLink", to( Index ) );

            ctx.Page.Title = lang( "searchUser" );

            set( "ActionLink", ctx.url.Path );
            bindDropList();

            String name = getTerm( "name" );
            String city1 = getTerm( "city1" );
            String city2 = getTerm( "city2" );
            set( "name", name );
            set( "city1", city1 );
            set( "city2", city2 );


            String condition = getCondition();
            DataPage<User> list = userService.SearchBy( condition, 40 );

            bindUsers( list.Results, "list" );
            set( "page", list.PageBar );
        }

        private String getCondition() {

            String sql = "";

            String name = getTerm( "name" );
            if (strUtil.HasText( name )) sql += " and Name like '%" + name + "%'";

            int provinceId1 = ctx.GetInt( "p1" );
            if (provinceId1 > 0) sql += " and ProvinceId1=" + provinceId1;
            int provinceId2 = ctx.GetInt( "p2" );
            if (provinceId2 > 0) sql += " and provinceId2=" + provinceId2;

            String city1 = getTerm( "city1" );
            if (strUtil.HasText( city1 )) sql += " and City1='" + city1 + "'";
            String city2 = getTerm( "city2" );
            if (strUtil.HasText( city2 )) sql += " and City2='" + city2 + "'";

            int Gender = ctx.GetInt( "g" );
            if (Gender > 0) sql += " and Gender=" + Gender;

            int Relationship = ctx.GetInt( "r" );
            if (Relationship > 0) sql += " and Relationship=" + Relationship;

            int Blood = ctx.GetInt( "b" );
            if (Blood > 0) sql += " and Blood=" + Blood;

            int Zodiac = ctx.GetInt( "z" );
            if (Zodiac > 0) sql += " and Zodiac=" + Zodiac;

            int Degree = ctx.GetInt( "d" );
            if (Degree > 0) sql += " and Degree=" + Degree;

            int age1 = ctx.GetInt( "a1" );
            if (age1 > 0) sql += " and BirthYear<=" + (DateTime.Now.Year - age1);
            int age2 = ctx.GetInt( "a2" );
            if (age2 > 0) sql += " and BirthYear>=" + (DateTime.Now.Year - age2);

            sql = strUtil.TrimStart( sql.Trim(), "and" );

            return sql;
        }

        private String getTerm( String qname ) {
            String name = ctx.Get( qname );
            name = strUtil.SqlClean( name, 15 );
            return name;
        }

        private void bindSimple() {
            dropList( "g", AppResource.Gender, "Name=Value", ctx.Get( "g" ) );
            dropList( "a1", AppResource.GetInts( 10, 99 ), "Name=Value", ctx.Get( "a1" ) );
            dropList( "a2", AppResource.GetInts( 10, 99 ), "Name=Value", ctx.Get( "a2" ) );
            dropList( "p2", AppResource.Province, "Name=Value", ctx.Get( "p2" ) );
        }

        private void bindDropList() {

            String kv = "Name=Value";
            dropList( "g", AppResource.Gender, kv, ctx.Get( "g" ) );

            dropList( "r", AppResource.Relationship, kv, ctx.Get( "r" ) );
            dropList( "b", AppResource.Blood, kv, ctx.Get( "b" ) );
            dropList( "z", AppResource.Zodiac, kv, ctx.Get( "z" ) );
            dropList( "d", AppResource.Degree, kv, ctx.Get( "d" ) );

            dropList( "a1", AppResource.GetInts( 10, 99 ), kv, ctx.Get( "a1" ) );
            dropList( "a2", AppResource.GetInts( 10, 99 ), kv, ctx.Get( "a2" ) );
            dropList( "p1", AppResource.Province, kv, ctx.Get( "p1" ) );
            dropList( "p2", AppResource.Province, kv, ctx.Get( "p2" ) );
        }


        private void bindUsers( List<User> users, String blockName ) {
            IBlock block = getBlock( blockName );
            int i = 1;
            foreach (User member in users) {
                block.Set( "u.Index", i );
                block.Set( "u.Name", member.Name );
                block.Set( "u.Face", member.PicSmall );
                block.Set( "u.Link", toUser( member ) );
                block.Next();
                i++;
            }
        }


    }
}

