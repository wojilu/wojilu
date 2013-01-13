/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Web.Utils;

using wojilu.Members.Groups.Service;
using wojilu.Members.Groups.Domain;
using wojilu.Common.Msg.Interface;
using wojilu.Common.Msg.Service;
using wojilu.Web.Controller.Groups;
using wojilu.Members.Sites.Domain;
using wojilu.Members.Users.Domain;
using wojilu.Members.Groups.Interface;
using wojilu.Common;
using wojilu.Apps.Forum.Domain;
using wojilu.Web.Controller.Forum;
using wojilu.Members.Interface;
using wojilu.DI;
using wojilu.Apps.Forum.Interface;
using wojilu.Apps.Forum.Service;
using wojilu.Members.Groups;

namespace wojilu.Web.Controller.Users.Admin {


    public class MyGroupController : ControllerBase {

        public IGroupService groupService { get; set; }
        public IMessageService msgService { get; set; }
        public IMemberGroupService mgrService { get; set; }
        public IInviteService inviteService { get; set; }
        public IGroupPostService gpostService { get; set; }

        public MyGroupController() {
            msgService = new MessageService();
            groupService = new GroupService();
            mgrService = new MemberGroupService();
            inviteService = new InviteService();

            gpostService = new GroupPostService();
        }

        public override void CheckPermission() {

            if (Component.IsEnableGroup() == false) {
                echo( "对不起，本功能已经停用" );
            }
        }

        public override void Layout() {

            set( "searchActionUrl", to( SearchResult ) );
            set( "allGroupLink", Link.To( Site.Instance, new Groups.MainController().List, -1 ) );

            set( "groupHome", to( My ) );
            set( "lnkMyGroup", to( GroupMy ) );
            set( "lnkMyPost", to( MyPost ) );

            set( "lnkFriendGroup", to( GroupFriend ) );

            IBlock block = getBlock( "addGroup" );
            if (isEnableUserCreateGroup()) {
                block.Set( "addGroupUrl", to( New ) );
                block.Next();
            }

        }

        public void My() {

            List<Group> lists = mgrService.GetJoinedGroup( ctx.owner.Id, 10 );

            String ids = mgrService.GetJoinedGroupIds( ctx.owner.Id );

            List<ForumTopic> topics = gpostService.GetMyTopic( ctx.owner.Id, ids, 20 );
            IBlock block = getBlock( "list" );
            foreach (ForumTopic t in topics) {
                bindTopicOne( block, t );
            }


            bindMyGroup( lists, "mygroup" );

            set( "myGroupLink", to( GroupMy ) );

        }

        public void GroupMy() {
            DataPage<Group> lists = mgrService.GetGroupByUser( ctx.owner.Id );
            bindMyGroup( lists.Results, "list" );
            set( "page", lists.PageBar );
        }

        public void GroupFriend() {
            List<Group> flist = mgrService.GetGroupByFriends( ctx.owner.Id, 20 );
            bindMyGroup( flist, "friends" );
        }

        public void MyPost() {

            String ids = mgrService.GetJoinedGroupIds( ctx.owner.Id );
            DataPage<ForumTopic> topics = gpostService.GetMyTopicPage( ctx.owner.Id, ids, 20 );
            IBlock block = getBlock( "list" );
            foreach (ForumTopic t in topics.Results) {
                bindTopicOne( block, t );
            }

            set( "page", topics.PageBar );
        }


        private void bindTopicOne( IBlock block, ForumTopic topic ) {

            String typeImg = string.Empty;
            if (strUtil.HasText( topic.TypeName )) {
                typeImg = string.Format( "<img src='{0}apps/forum/{1}.gif'>", sys.Path.Skin, topic.TypeName );
            }

            block.Set( "p.Id", topic.Id );
            block.Set( "p.TypeImg", typeImg );
            block.Set( "p.TitleStyle", topic.TitleStyle );
            block.Set( "p.Titile", strUtil.CutString( topic.Title, 30 ) );
            block.Set( "p.Url", Link.To( topic.OwnerType, topic.OwnerUrl, new TopicController().Show, topic.Id, topic.AppId ) );

            block.Set( "p.BoardName", strUtil.SubString( topic.ForumBoard.Name, 10 ) );
            block.Set( "p.BoardUrl", Link.To( topic.OwnerType, topic.OwnerUrl, new BoardController().Show, topic.ForumBoard.Id, topic.AppId ) );


            String ownerName = getOwnerName( topic );
            block.Set( "p.OwnerName", ownerName );
            block.Set( "p.OwnerLink", Link.ToMember( topic.OwnerType, topic.OwnerUrl ) );

            block.Set( "p.CreateTime", topic.Created );
            block.Set( "p.Replied", topic.Replied );
            block.Set( "p.RepliedUserName", topic.RepliedUserName );

            block.Set( "p.ReplyCount", topic.Replies );
            block.Set( "p.Hits", topic.Hits.ToString() );

            String attachments = topic.Attachments > 0 ? "<img src='" + sys.Path.Img + "attachment.gif'/>" : "";
            block.Set( "p.Attachments", attachments );

            block.Next();
        }

        private string getOwnerName( ForumTopic topic ) {
            Type ownerType;
            ObjectContext.Instance.TypeList.TryGetValue( topic.OwnerType, out ownerType );
            if (ownerType == null) return "";

            IMember owner = ndb.findById( ownerType, topic.OwnerId ) as IMember;
            return owner == null ? "" : owner.Name;
        }

        //---------------------------------------------------------------------


        public void New() {

            if (isEnableUserCreateGroup() == false) {
                echoError( "禁止创建群组" );
                return;
            }

            target( StepTwo );

            set( "siteUrl", strUtil.TrimEnd( ctx.url.SiteAndAppPath, "/" ) );
            set( "urlExt", MvcConfig.Instance.UrlExt );
            set( "groupPath", MemberPath.GetPath( typeof( Group ).Name ) );

            int categoryId = (ctx.PostInt( "Category" ) > 0) ? ctx.PostInt( "Category" ) : 1;
            dropList( "Category", GroupCategory.GetAll(), "Name=Id", categoryId );

            setAccessStatus();

            set( "isNameValidLink", to( CheckNameExist ) );
            set( "isUrlValidLink", to( CheckUrlExist ) );

        }

        [HttpPost, DbTransaction]
        public void CheckNameExist() {

            String name = ctx.Post( "Name" );
            if (groupService.IsNameReservedOrExist( name )) {
                echoJsonMsg( lang( "exNameFound" ), false, null );
            }
            else {
                echoJsonMsg( "验证成功", true, null );
            }

        }

        [HttpPost, DbTransaction]
        public void CheckUrlExist() {

            String url = ctx.Post( "FriendUrl" );
            if (groupService.IsUrlReservedOrExist( url )) {
                echoJsonMsg( lang( "exUrlFound" ), false, null );
            }
            else {
                echoJsonMsg( "验证成功", true, null );
            }
        }

        [HttpPost, DbTransaction]
        public void StepTwo() {

            if (isEnableUserCreateGroup() == false) {
                echoError( "禁止创建群组" );
                return;
            }

            String name = ctx.Post( "Name" );
            int categoryId = ctx.PostInt( "Category" );
            int accessStats = ctx.PostInt( "AccessStatus" );
            String description = ctx.Post( "Description" );
            String url = ctx.Post( "FriendUrl" );

            if (strUtil.IsNullOrEmpty( name )) errors.Add( lang( "exGroupName" ) );
            if (strUtil.HasText( name ) && (name.Length > 30)) errors.Add( lang( "exGroupNameMaxLength" ) );
            if (strUtil.IsAbcNumberAndChineseLetter( name ) == false) errors.Add( lang( "exGroupNameFormat" ) );

            if (categoryId <= 0) errors.Add( lang( "exGroupCategory" ) );
            if (accessStats < 0) errors.Add( lang( "exGroupSecurity" ) );
            if (strUtil.IsNullOrEmpty( description )) errors.Add( lang( "exGroupDescription" ) );

            if (strUtil.IsNullOrEmpty( url )) errors.Add( lang( "exGroupUrl" ) );
            if (!strUtil.IsUrlItem( url )) errors.Add( lang( "exGroupUrlFormat" ) );

            if (errors.HasErrors) { run( New ); return; }

            Result result = groupService.Create( (User)ctx.viewer.obj, name, url, description, categoryId, accessStats, ctx );
            if (result.HasErrors) { errors.Join( result ); run( New ); return; }

            // 添加论坛
            Group group = result.Info as Group;
            new GroupUtil().CreateAppAndMenu( group, ctx );

            run( showStepTwo, group.Id );
        }

        [NonVisit]
        public void showStepTwo( int id ) {

            if (isEnableUserCreateGroup() == false) {
                echoError( "禁止创建群组" );
                return;
            }

            target( StepThree );
            set( "newGroupId", id );
        }

        [HttpPost, DbTransaction]
        public void StepThree() {

            if (isEnableUserCreateGroup() == false) {
                echoError( "禁止创建群组" );
                return;
            }

            int newGroupId = ctx.PostInt( "newGroupId" );
            if (newGroupId <= 0) { errors.Add( lang( "exGroupNull" ) ); run( New ); return; }

            Group group = groupService.GetById( newGroupId );
            if (group == null) { errors.Add( lang( "exGroupNull" ) ); run( New ); return; }

            HttpFile postedFile = ctx.GetFileSingle();
            Result result = GroupHelper.SaveGroupLogo( postedFile, group.Url );
            if (result.HasErrors) { errors.Join( result ); run( showStepTwo, group.Id ); return; }

            group.Logo = result.Info.ToString();
            groupService.UpdateLogo( group );

            echoRedirect( "群组创建成功", Link.To( group, new Groups.Admin.InviteController().Add ) );

        }

        public void SearchResult() {

            String term = ctx.Post( "term" );
            List<Group> mygroups = this.groupService.Search( term );

            String resultLabel = string.Format( lang( "resultLabel" ), term, mygroups.Count );
            set( "resultLabel", resultLabel );

            this.bindMyGroup( mygroups, "list" );
        }

        private void setAccessStatus() {

            int val = (ctx.PostInt( "AccessStatus" ) > 0) ? ctx.PostInt( "AccessStatus" ) : 0;

            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic["ac0"] = "";
            dic["ac1"] = "";
            dic["ac2"] = "";

            dic["ac" + val] = "checked=\"checked\"";

            set( "accessChecked0", dic["ac0"] );
            set( "accessChecked1", dic["ac1"] );
            set( "accessChecked2", dic["ac2"] );

        }

        private void bindMyGroup( List<Group> mygroups, String blockName ) {
            IBlock block = getBlock( blockName );
            StringBuilder builder = new StringBuilder();
            foreach (Group group in mygroups) {
                builder.Remove( 0, builder.Length );
                block.Set( "g.Name", group.Name );
                block.Set( "g.Logo", group.LogoSmall );
                block.Set( "g.Url", Link.ToMember( group ) );
                builder.AppendFormat( "<div>{0}: {1} </div>", lang( "groupMember" ), group.MemberCount );
                builder.AppendFormat( "<div>{0}: {1}</div>", lang( "groupCreated" ), group.Created.ToShortDateString() );
                builder.AppendFormat( "<div>{0}: {1}</div>", lang( "groupLastUpdated" ), group.LastUpdated.ToShortDateString() );
                builder.AppendFormat( "<div>{0}: {1}</div>", lang( "groupCategory" ), group.Category.Name );

                block.Set( "g.Info", builder.ToString() );
                builder.Remove( 0, builder.Length );
                block.Set( "g.OtherUrl", builder.ToString() );
                block.Next();
            }
        }

        private Boolean isEnableUserCreateGroup() {
            if (ctx.viewer.IsAdministrator()) return true;
            return Component.IsEnableUserCreateGroup();
        }

    }
}

