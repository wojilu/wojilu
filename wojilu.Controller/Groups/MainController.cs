/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Mvc;

using wojilu.Apps.Forum.Service;
using wojilu.Apps.Forum.Domain;
using wojilu.Apps.Forum.Interface;

using wojilu.Members.Groups.Domain;
using wojilu.Members.Groups.Service;
using wojilu.Members.Groups.Interface;

using wojilu.Members.Users.Domain;
using wojilu.Members.Users.Interface;
using wojilu.Members.Users.Service;
using wojilu.Web.Controller.Common;
using wojilu.Web.Mvc.Attr;
using wojilu.Web.Controller.Groups.Caching;
using wojilu.Members.Groups;

namespace wojilu.Web.Controller.Groups {

    public class MainController : ControllerBase {

        private static readonly ILog logger = LogManager.GetLogger( typeof( MainController ) );

        public IGroupService groupService { get; set; }
        public IGroupPostService postService { get; set; }
        public IUserService userService { get; set; }

        public MainController() {
            groupService = new GroupService();
            postService = new GroupPostService();
            userService = new UserService();
        }

        [CacheAction( typeof( GroupMainLayoutCache ) )]
        public override void Layout() {


            // 当前app/module所有页面，所属的首页
            ctx.SetItem( "_moduleUrl", to( Index ) );

            List<GroupCategory> categories = GroupCategory.GetAll();
            IBlock block = getBlock( "categories" );
            foreach (GroupCategory c in categories) {
                block.Set( "c.Name", c.Name );
                block.Set( "c.Link", to( List, c.Id ) );
                block.Next();
            }

            String name = ctx.Get( "name" );
            set( "s.Name", name );

            set( "SearchAction", to( Search ) );

            set( "allGroupLink", to( List, -1 ) );

            bindAdminLink();
        }

        private void bindAdminLink() {
            set( "groupAdminHome", to( new wojilu.Web.Controller.Admin.Groups.GroupController().Index ) );
            set( "postLink", to( new wojilu.Web.Controller.Admin.Groups.GroupController().PostAdmin ) );
            set( "groupLink", to( new wojilu.Web.Controller.Admin.Groups.GroupController().GroupAdmin, -1 ) );
            set( "groupCategoryLink", to( new wojilu.Web.Controller.Admin.Groups.CategoryController().List ) );
            set( "settingLink", to( new wojilu.Web.Controller.Admin.Groups.SettingController().Index ) );
        }

        [CachePage( typeof( GroupMainPageCache ) )]
        [CacheAction( typeof( GroupMainActionCache ) )]
        public void Index() {

            ctx.Page.Title = GroupSetting.Instance.MetaTitle;
            ctx.Page.Keywords = GroupSetting.Instance.MetaKeywords;
            ctx.Page.Description = GroupSetting.Instance.MetaDescription;

            List<ForumTopic> posts = postService.GetHotTopic( 10 );
            bindPosts( posts, "list" );

            List<Group> hots = groupService.GetHots( 8 );
            bindGroups( hots, "hots" );

            List<Group> recent = groupService.GetRecent( 12 );
            bindGroups( recent, "recent" );

            set( "allGroupLink", to( List, -1 ) );

        }

        public void List( int id ) {

            ctx.Page.Title = "群组列表";

            GroupCategory category = db.findById<GroupCategory>( id );
            if (category != null) {
                String groupsByCategory = string.Format( lang( "groupsByCategory" ), category.Name );
                set( "groupsByCategory", groupsByCategory );
            }
            else {
                set( "groupsByCategory", lang( "all" ) );
            }

            DataPage<Group> groups = groupService.GetByCategory( id );
            bindGroups( groups.Results, "list" );
            set( "page", groups.PageBar );

        }

        public void Search() {

            DataPage<Group> list = getResults();
            bindGroups( list.Results, "list" );
            set( "page", list.PageBar );

        }

        private DataPage<Group> getResults() {

            String condition = "";

            String name = ctx.Get( "name" );

            if (strUtil.IsNullOrEmpty( name )) return DataPage<Group>.GetEmpty();

            name = strUtil.SqlClean( name, 20 );

            String queryType = ctx.Get( "queryType" );

            if ("name".Equals( queryType )) {
                condition += string.Format( " and Name like '%{0}%' ", name );
            }
            else if ("creator".Equals( queryType )) {


                String creatorIds = getCreators( name );
                if (strUtil.IsNullOrEmpty( creatorIds )) return DataPage<Group>.GetEmpty();
                condition += " and CreatorId in (" + creatorIds + ")";
            }
            else if ("description".Equals( queryType )) {
                condition += string.Format( " and Description like '%{0}%' ", name );
            }

            condition = condition.Trim();
            condition = strUtil.TrimStart( condition, "and" );

            return groupService.SearchByCondition( condition );
        }

        //----------------------------------------------------------------------------

        private void bindPosts( List<ForumTopic> posts, String blockName ) {
            IBlock block = getBlock( blockName );
            foreach (ForumTopic post in posts) {

                if (post.Creator == null) continue;

                block.Set( "post.Title", post.Title );
                block.Set( "post.Link", alink.ToAppData( post ) );
                block.Set( "post.Replies", post.Replies );

                Group g = getGroup( post );
                if (g == null) continue;

                block.Set( "post.OwnerName", g.Name );
                block.Set( "post.OwnerLink", Link.ToMember( g ) );

                block.Next();

            }
        }

        private void bindGroups( List<Group> groups, String blockName ) {
            IBlock gblock = getBlock( blockName );
            foreach (Group g in groups) {

                gblock.Set( "g.Name", g.Name );
                gblock.Set( "g.Link", Link.ToMember( g ) );
                gblock.Set( "g.Logo", g.LogoSmall );
                gblock.Set( "g.Created", cvt.ToTimeString( g.Created ) );
                gblock.Set( "g.MemberCount", g.MemberCount );

                gblock.Next();
            }
        }


        private Group getGroup( IPost post ) {
            return groupService.GetById( post.OwnerId );
        }

        private String getCreators( String name ) {

            List<User> users = userService.SearchByName( name );
            String ids = "";
            foreach (User user in users) ids += user.Id + ",";
            return ids.TrimEnd( ',' );
        }

    }

}
