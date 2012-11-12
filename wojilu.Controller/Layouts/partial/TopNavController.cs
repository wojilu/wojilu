/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;

using wojilu.Web.Mvc;

using wojilu.Web.Controller.Admin;
using wojilu.Web.Controller.Admin.Sys;

using wojilu.Members.Sites.Domain;
using wojilu.Members.Users.Domain;
using wojilu.Common.MemberApp.Interface;
using System.Collections.Generic;

namespace wojilu.Web.Controller.Layouts {

    public partial class TopNavController : ControllerBase {

        private String getAdminCmd() {
            String siteAdminCmd = "";

            if( SiteRole.IsInAdminGroup( ctx.viewer.obj.RoleId) ) {

                String lk = string.Format( "<img src=\"{0}lock.gif\"/> ", sys.Path.Img );

                //siteAdminCmd += string.Format( "<a href=\"{0}\" class=\"quickCmd\">采集</a> ", Link.T2( ctx.viewer.obj, new Users.Admin.Spiders.ArticleController().List, 0 ) );

                if (AdminSecurityUtils.HasSession( ctx ))
                    siteAdminCmd += string.Format( "<a href='{0}'>{2}{1}</a>", Link.To( Site.Instance, new Admin.MainController().Welcome ), lang( "siteAdmin" ), lk );
                else
                    siteAdminCmd += string.Format( "<a href='{0}'>{2}{1}</a>", Link.To( Site.Instance, new Admin.MainController().Login ), lang( "siteAdmin" ), lk );
            }
            return siteAdminCmd;
        }

        private String getMsgCount() {
            User viewer = (User)ctx.viewer.obj;
            msgService.CheckSiteMsg( viewer );
            return viewer.MsgNewCount > 0 ? "<span id=\"newMsgCount\">(" + viewer.MsgNewCount + ")</span>" : "";
        }

        private String getNewNotificationCount() {

            int nCount = ((User)ctx.viewer.obj).NewNotificationCount;

            String msg = string.Format( lang( "notificationInfo" ), nCount );

            return nCount > 0 ? "<div class=\"NewNotificationCount\"><a href='" + Link.To( ctx.viewer.obj, new Users.Admin.NotificationController().List ) + "'>" + msg + "</a></div>" : "";
        }

        private String getNewMicroblogAtCount() {

            int nCount = ((User)ctx.viewer.obj).MicroblogAtUnread;

            if (nCount > 0) {
                return string.Format( "<div class=\"NewNotificationCount\"><a href=\"{1}\">{0}条at我的微博</a></div>", nCount, Link.To( ctx.viewer.obj, new Microblogs.My.MicroblogController().Atme ) );
            }


            return "";
        }



        private void bindUserAppList(  IBlock ablock, IList userAppList ) {
            IBlock block = ablock.GetBlock( "apps" );
            foreach (IMemberApp app in userAppList) {
                block.Set( "app.NameAndUrl", getNameAndUrl( app ) );
                block.Next();
            }
        }

        private String getNameAndUrl( IMemberApp app ) {

            String iconPath = strUtil.Join( sys.Path.Img, "app/s" );
            iconPath = strUtil.Join( iconPath, app.AppInfo.TypeFullName ) + ".png";

            if (app.IsStop == 1) return ("<span class=\"stop\"><img src=\"" + iconPath + "\"/> " + app.Name + "</span>");

            return string.Format( "<a href=\"{1}\"><img src=\"{2}\"/> {0}</a>", app.Name, alink.ToAppAdmin( ctx.viewer.obj, app ), iconPath );


        }

    }

}
