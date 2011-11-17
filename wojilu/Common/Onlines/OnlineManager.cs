/*
 * Copyright 2010 www.wojilu.com
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *      http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Context;
using wojilu.Web;
using wojilu.Web.Mvc;

namespace wojilu.Common.Onlines {

    /// <summary>
    /// 在线管理器
    /// </summary>
    public class OnlineManager {

        private static readonly ILog logger = LogManager.GetLogger( typeof( OnlineManager ) );

        public static void Refresh( MvcContext ctx ) {

            if (isAgentSpider( ctx )) return;

            String sid = getSessionId( ctx );
            List<OnlineUser> users = cdb.findByName<OnlineUser>( sid );
            Boolean isNew = (users.Count == 0 ? true : false);

            if (isNew) {
                addNewVisitor( ctx, sid );
            }
            else {
                updateVisitor( ctx, users[0] );
            }

            
            deleteSameUser( ctx, sid );
        }


        private static Boolean isAgentSpider( MvcContext ctx ) {

            String agent = ctx.web.ClientAgent;
            if (agent == null) return true;

            foreach (String spiderName in config.Instance.Site.Spider) {

                if (agent.ToLower().IndexOf( spiderName.ToLower() ) >= 0) {
                    return true; // TODO 记录spider在线情况
                }
            }

            return false;
        }


        private static void updateVisitor( MvcContext ctx, OnlineUser myOnline ) {
            populateOnline( myOnline, ctx );
            Dictionary<String, Object> dic = getIndexMap( myOnline );
            myOnline.updateByIndex( dic );
        }

        private static void addNewVisitor( MvcContext ctx, String sid ) {
            OnlineUser visitor = new OnlineUser();
            visitor.Name = sid;
            visitor.StartTime = DateTime.Now;

            populateOnline( visitor, ctx );

            Dictionary<String, Object> dic = getIndexMap( visitor );
            visitor.insertByIndex( dic );


            if (ctx.viewer != null && ctx.viewer.Id > 0) OnlineStats.Instance.AddMemberCount();
        }
        
        // 检查是否有同名的登录用户
        private static void deleteSameUser( MvcContext ctx, String sid ) {

            if (ctx.viewer == null) return;
            if (ctx.viewer.Id <= 0) return;

            List<OnlineUser> sameUsers = cdb.findBy<OnlineUser>( "UserName", ctx.viewer.obj.Name );
            if (sameUsers.Count == 0) return;

            foreach (OnlineUser u in sameUsers) {
                if (u.Name == sid) continue;
                cdb.delete( u );
                OnlineStats.Instance.SubtractMemberCount();
            }
        }

        private static Dictionary<String, Object> getIndexMap( OnlineUser visitor ) {
            Dictionary<String, Object> dic = new Dictionary<String, Object>();
            dic.Add( "Name", visitor.Name );
            dic.Add( "UserName", visitor.UserName );
            return dic;
        }

        private static void populateOnline( OnlineUser visitor, MvcContext ctx ) {

            visitor.Role = "";
            visitor.Ip = ctx.Ip;
            visitor.TrueIp = ctx.Ip;
            visitor.Agent = strUtil.CutString( ctx.web.ClientAgent, 240 );

            if (ctx.viewer != null && ctx.viewer.obj != null) {

                visitor.UserName = ctx.viewer.obj.Name;
                visitor.UserId = ctx.viewer.Id;
                visitor.UserUrl = Link.ToMember( ctx.viewer.obj );
                visitor.UserPicUrl = ctx.viewer.obj.PicSmall;
            }
            else { // 游客

                visitor.UserName = "guest";
                visitor.UserId = 0;
                visitor.UserUrl = "";
                visitor.UserPicUrl = "";

            }

            if (ctx.owner != null && ctx.owner.obj != null)
                visitor.Target = ctx.owner.obj.Name;

            visitor.IsHidden = 0;

            String referrer = "";
            if (ctx.web.PathReferrer != null) referrer = ctx.web.PathReferrer;
            visitor.Referrer = referrer;

            visitor.LastActive = DateTime.Now;
            visitor.Location = getLocation( ctx );
        }


        private static String getSessionId( MvcContext ctx ) {

            String sidstring = "sessionId";

            String cookieValue = ctx.web.CookieGet( sidstring );

            if (strUtil.HasText( cookieValue )) {

                return cookieValue;
            }

            else {
                return setGuestSessionId( ctx, sidstring );

            }
        }

        private static String setGuestSessionId( MvcContext ctx, String sidstring ) {
            String guidSessionId = getGuidString();
            ctx.web.CookieSet( sidstring, guidSessionId );
            return guidSessionId;
        }


        private static String getGuidString() {
            return Guid.NewGuid().ToString().Replace( "-", "" );
        }

        // TODO 精确描述当前位置
        private static String getLocation( MvcContext ctx ) {
            String result = ctx.url.PathAndQuery;
            return result;
        }


    }
}
