/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Members.Groups.Domain;
using wojilu.Members.Users.Domain;

namespace wojilu.Web.Controller.Groups.Admin {

    public partial class MainController : ControllerBase {


        private void bindGroupInfo( Group group ) {
            set( "g.Name", group.Name );
            set( "g.Description", group.Description );
            set( "g.Url", group.Url );

            set( "siteUrl", strUtil.TrimEnd( ctx.url.SiteAndAppPath, "/" ) );
            set( "cfg.UrlExt", MvcConfig.Instance.UrlExt );
            set( "groupPath", MemberPath.GetPath( typeof( Group ).Name ) );

            int categoryId = (ctx.PostInt( "Category" ) > 0) ? ctx.PostInt( "Category" ) : group.Category.Id;
            dropList( "Category", GroupCategory.GetAll(), "Name=Id", categoryId );
            this.setAccessStatus();

            String chk = group.IsCloseJoinCmd == 1 ? "checked=\"checked\"" : "";
            set( "g.IsCloseJoinCmd", chk );
        }

        private void bindLogo( Group group ) {
            if (strUtil.HasText( group.Logo ))
                set( "g.Logo", string.Format( "<img src='{0}' />", group.LogoSmall ) );
            else
                set( "g.Logo", string.Empty );
        }


        private void bindMemberList( IBlock block, DataPage<GroupUser> list ) {
            foreach (GroupUser mgr in list.Results) {
                if (mgr.Member == null) continue;
                block.Set( "user.Id", mgr.Member.RealId );
                block.Set( "user.Name", mgr.Member.Name );
                block.Set( "user.Url", toUser( mgr.Member ) );
                block.Set( "user.Status", mgr.RoleString );
                block.Set( "user.LastLoginTime", mgr.Member.LastLoginTime );

                block.Set( "user.Msg", mgr.Msg );

                String style = getUserStyle( mgr );
                block.Set( "user.Style", style );

                block.Next();
            }
            set( "page", list.PageBar );
        }

        private string getUserStyle( GroupUser mgr ) {

            if (mgr.Status == GroupRole.Approving.Id)
                return "color:red;";

            if (mgr.Status == GroupRole.Administrator.Id)
                return "color:blue;";

            return "";
        }


        private void setAccessStatus() {

            Group group = ctx.owner.obj as Group;

            int accessStatus = group.AccessStatus;
            if (ctx.PostInt( "AccessStatus" ) > 0)
                accessStatus = ctx.PostInt( "AccessStatus" );

            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic["ac0"] = "";
            dic["ac1"] = "";
            dic["ac2"] = "";

            dic["ac" + accessStatus] = "checked=\"checked\"";

            set( "accessChecked0", dic["ac0"] );
            set( "accessChecked1", dic["ac1"] );
            set( "accessChecked2", dic["ac2"] );
        }


        private void log( String msg, Type t ) {
            logService.Add( (User)ctx.viewer.obj, msg, "", t.FullName, ctx.Ip );
        }

        private void log( String msg, Group g ) {
            String dataInfo = "{Id:" + g.Id + ", Name:'" + g.Name + "'}";
            logService.Add( (User)ctx.viewer.obj, msg, dataInfo, typeof( Group ).FullName, ctx.Ip );
        }

        private void log( String msg, Group g, String ids ) {
            String dataInfo = "{Id:" + g.Id + ", Name:'" + g.Name + "', UserIds:[" + ids + "]}";
            logService.Add( (User)ctx.viewer.obj, msg, dataInfo, typeof( Group ).FullName, ctx.Ip );
        }

    }

}
