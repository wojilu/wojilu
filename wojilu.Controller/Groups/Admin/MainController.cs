/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Web;

using wojilu.Web.Utils;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Members.Groups.Domain;
using wojilu.Members.Groups.Service;
using wojilu.Members.Groups.Interface;
using wojilu.Members.Interface;
using wojilu.Members.Users.Domain;
using wojilu.Web.Controller.Security;
using wojilu.Members.Groups;

namespace wojilu.Web.Controller.Groups.Admin {

    public partial class MainController : ControllerBase {

        public IGroupService groupService { get; set; }
        public IMemberGroupService mgrService { get; set; }
        public IAdminLogService<GroupLog> logService { get; set; }

        public MainController() {
            groupService = new GroupService();
            mgrService = new MemberGroupService();
            logService = new GroupLogService();
        }

        public void Index() {
            target( SaveInfo );
            Group group = ctx.owner.obj as Group;
            bindGroupInfo( group );
        }

        [HttpPost, DbTransaction]
        public void SaveInfo() {

            //String target = ctx.Post( "Name" );
            //String url = ctx.Post( "Url" );
            int categoryId = ctx.PostInt( "Category" );
            int accessId = ctx.PostInt( "AccessStatus" );
            String description = ctx.Post( "Description" );

            //if (strUtil.IsNullOrEmpty( target )) errors.Add( lang( "exGroupName" ) );
            //if (strUtil.IsNullOrEmpty( url )) errors.Add( lang( "exGroupUrl" ) );
            //if (strUtil.HasText( target ) && (target.Length > 30)) errors.Add( lang( "exGroupNameMaxLength" ) );
            if (categoryId <= 0) errors.Add( lang( "exGroupCategory" ) );
            if (accessId < 0) errors.Add( lang( "exGroupSecurity" ) );
            if (strUtil.IsNullOrEmpty( description )) errors.Add( lang( "exGroupDescription" ) );

            if (errors.HasErrors) {
                run( Index ); return;
            }

            Group group = ctx.owner.obj as Group;
            
            group.Category = new GroupCategory(categoryId);
            group.AccessStatus = accessId;
            group.Description = description;

            group.IsCloseJoinCmd = ctx.PostIsCheck( "IsCloseJoinCmd" );

            Result result = db.update( group );
            if (result.IsValid) {
                log( SiteLogString.UpdateGroupInfo(), group );
                echoRedirect( lang( "opok" ), Index );
            }
            else {
                errors.Join( result );
                run( Index );
            }
        }

        public void Logo() {
            target( SaveLogo );
            Group group = ctx.owner.obj as Group;
            bindLogo( group );
        }


        [HttpPost, DbTransaction]
        public void SaveLogo() {
            Group group = ctx.owner.obj as Group;

            Result result = GroupHelper.SaveGroupLogo( ctx.GetFileSingle(), group.Url );
            if (result.HasErrors) {
                errors.Join( result );
                run( Logo );
            }
            else {
                group.Logo = result.Info.ToString();
                db.update( group, "Logo" );
                log( SiteLogString.UpdateGroupLogo(), group );

                echoRedirect( lang( "opok" ), Logo );
            }
        }



        public void Members( int roleId ) {

            IBlock block = getBlock( "list" );


            DataPage<GroupUser> list;
            if (roleId == 0)
                list = mgrService.GetMembersAll( ctx.owner.Id );
            else {
                if (roleId == 999) roleId = GroupRole.Member.Id;
                list = mgrService.GetMembersAll( ctx.owner.Id, roleId );
            }

            bindMemberList( block, list );

            set( "lnkAll", to( Members, 0 ) );
            set( "lnkMembers", to( Members, 999 ) );
            set( "lnkAdmins", to( Members, GroupRole.Administrator.Id ) );
            set( "lnkApprovings", to( Members, GroupRole.Approving.Id ) );

            target( SaveMember  );
        }


        [HttpPost, DbTransaction]
        public void SaveMember() {

            Group group = ctx.owner.obj as Group;

            String action = ctx.Post( "action" );
            String ids = ctx.Post( "choice" );
            if ((strUtil.IsNullOrEmpty( action ) || !cvt.IsIdListValid( ids ))) {
                echoText( "error" );
                return;
            }

            if (action.Equals( "pass" )) {
                mgrService.ApproveUser( ctx.owner.obj as Group, ids );
                log( SiteLogString.ApproveUser(), group, ids );
                echoAjaxOk();
            }

            else if (action.Equals( "deletetrue" )) {
                mgrService.DeleteUser( ctx.owner.obj as Group, ids );
                log( SiteLogString.DeleteUser(), group, ids );
                echoAjaxOk();
            }
            else if (action.Equals( "addadmin" )) {
                mgrService.AddOfficer( ctx.owner.obj as Group, ids );
                log( SiteLogString.AddOfficer(), group, ids );
                echoAjaxOk();
            }
            else if (action.Equals( "deleteadmin" )) {

                if (mgrService.GetOfficer( ctx.owner.Id ).Count == 1) {
                    echoError( lang( "exDeleteOnlyGroupAdmin" ) );
                    return;
                }

                mgrService.RemoveOfficer( ctx.owner.obj as Group, ids );
                log( SiteLogString.RemoveOfficer(), group, ids );
                echoAjaxOk();
            }
            else {
                content( "error" );
            }
        }


        public void AdminLog() {
        }

    }

}
