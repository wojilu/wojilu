/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

using wojilu.Web.Mvc;
using wojilu.Web.Context;
using wojilu.Web.Controller.Users;
using wojilu.Members.Users.Service;
using wojilu.Common.Feeds.Interface;
using wojilu.Members.Sites.Domain;
using wojilu.Common;
using System.Collections.Generic;

namespace wojilu.Web.Controller.Common {

    public class WebUtils {

        private static String pageTitleSeparator = "_";

        public static void pageTitle( ControllerBase controller, String title ) {
            controller.Page.Title = title + pageTitleSeparator + pageTitlePostfix( controller.ctx );
        }

        public static void pageTitle( ControllerBase controller, String item1, String item2 ) {
            controller.Page.Title = item1 + pageTitleSeparator + item2 + pageTitleSeparator + pageTitlePostfix( controller.ctx );
        }

        private static String pageTitlePostfix( MvcContext ctx ) {
            if (ctx.owner.obj.GetType() != typeof( Site ))
                return ctx.owner.obj.Name + pageTitleSeparator + config.Instance.Site.SiteName;
            else
                return config.Instance.Site.SiteName;
        }

        //--------------------------------------------------------------------------------------------------------


        //public static String getShareLink( MvcContext ctx, IShareData data, String name ) {
        //    String queryParam = "?dataType=" + data.GetType().FullName + "&name=" + ctx.web.UrlDecode( name );
        //    return ctx.GetLink().To( new wojilu.Web.Controller.ShareController().Add ) + queryParam;
        //}

        //public static String getShareLink( MvcContext ctx, IShareData data, String name, String dataLink ) {
        //    String queryParam = "?dataType=" + data.GetType().FullName + "&name=" + ctx.web.UrlDecode( name )
        //        + "&dataLink=" + dataLink;
        //    return ctx.GetLink().To( new wojilu.Web.Controller.ShareController().Add ) + queryParam;
        //}

        public static String getFavoriteLink( MvcContext ctx, IShareData data, String name ) {
            String queryParam = "?dataType=" + data.GetType().FullName + "&name=" + ctx.web.UrlDecode( name );
            return ctx.GetLink().To( new Common.ShareController().Add, data.Id ) + queryParam;
        }

        public static String getFavoriteLink( MvcContext ctx, IShareData data, String name, String dataLink ) {
            String queryParam = "?dataType=" + data.GetType().FullName + "&name=" + ctx.web.UrlDecode( name )
                + "&dataLink=" + dataLink;
            return ctx.GetLink().To( new Common.ShareController().Add, data.Id ) + queryParam;

        }

        //--------------------------------------------------------------------------------------------------------

        public static String getFriendCmd( MvcContext ctx ) {

            int targetId = ctx.owner.Id;

            if (ctx.viewer.Id == targetId) return "";
            if (ctx.viewer.IsFriend( targetId )) return deleteFriendCmd( ctx, targetId );
            if (isWaitingFriendApproving( ctx.viewer.Id, targetId )) return waitingApprovingCmd( ctx, targetId );

            return friendAndFollowCmd( ctx, targetId );
        }

        private static String deleteFriendCmd( MvcContext ctx, int targetId ) {
            return "<a href='" + ctx.GetLink().T2( new FriendController().DeleteFriend, targetId ) + "' class=\"deleteCmd cmd\"><span>" + lang.get( "canelFriend" ) + "</span></a>";
        }

        private static String waitingApprovingCmd( MvcContext ctx, int targetId ) {

            String cmd = "<span>" + lang.get( "inApproveFriend" ) + "...</span>";
            String delpic = string.Format( "<img src=\"{0}\" />", strUtil.Join( sys.Path.Img, "delete.gif" ) );
            String cancelLink = ctx.GetLink().T2( new FriendController().CancelAddFriend, targetId );
            String str = "<span class=\"left5 deleteCmd\" style=\"cursor:pointer\" title=\"{0}\" href=\"{1}\">{2}</span>";
            cmd += string.Format( str, lang.get( "canelFriend" ), cancelLink, delpic );
            return cmd;
        }

        private static String friendAndFollowCmd( MvcContext ctx, int targetId ) {
            String cmd = "<a href=\"" + ctx.GetLink().T2( new FriendController().AddFriend, targetId ) + "\" class=\"frmBox cmd\" xwidth=\"500\" title=\"" + lang.get( "addAsFriend" ) + "\"><span>" + lang.get( "addAsFriend" ) + "</span></a>";

            if (ctx.viewer.IsFollowing( targetId )) {
                cmd += "<a href='" + ctx.GetLink().T2( new FriendController().DeleteFollow, targetId ) + "' class=\"deleteCmd cmd left10\"><span>" + lang.get( "cancelFollow" ) + "</span></a>";
            }
            else {
                cmd += "<a href='" + ctx.GetLink().T2( new FriendController().AddFollow, targetId ) + "' class=\"frmBox cmd left10\" title=\"" + lang.get( "followcmd" ) + "\"><span>" + lang.get( "followcmd" ) + "</span></a>";
            }
            return cmd;
        }


        private static Boolean isWaitingFriendApproving( int userId, int targetId ) {
            FriendService friendService = new FriendService();
            return friendService.IsWaitingFriendApproving( userId, targetId );
        }


        public static String getMailLink( string mail ) {

            if (strUtil.IsNullOrEmpty( mail )) return "";
            if (mail.IndexOf( '@' ) <= 0) return "";

            String[] arrItem = mail.Split( '@' );
            if (arrItem.Length != 2) return "";

            String result = "";
            getMailHostMap().TryGetValue( arrItem[1], out result );

            return result;
        }

        public static Dictionary<String, String> getMailHostMap() {

            Dictionary<String, String> dic = new Dictionary<string, string>();
            dic.Add( "126.com", "http://mail.126.com" );
            dic.Add( "163.com", "http://mail.163.com" );
            dic.Add( "qq.com", "http://mail.qq.com" );
            dic.Add( "gmail.com", "https://mail.google.com" );
            dic.Add( "sohu.com", "http://mail.sohu.com" );
            dic.Add( "hotmail.com", "http://mail.live.com" );
            dic.Add( "live.com", "http://mail.live.com" );
            dic.Add( "live.cn", "http://mail.live.com" );
            dic.Add( "msn.com", "http://mail.live.com" );
            dic.Add( "yahoo.com.cn", "http://mail.yahoo.com.cn" );
            dic.Add( "yahoo.cn", "http://mail.yahoo.cn" );
            dic.Add( "sina.com", "http://mail.sina.com" );
            dic.Add( "sina.cn", "http://mail.sina.com" );

            return dic;
        }



    }

}
