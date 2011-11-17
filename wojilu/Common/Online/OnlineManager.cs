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
            Boolean isSpider = isAgentSpider( ctx );
            if (isSpider) return;

            UpdateOnline( ctx );

            DeleteTimeoutVisitor( ctx );// TODO job异步处理
            UpdateMaxOnline( ctx );// TODO job异步处理

            CountOnlineData( ctx );// 在UpdateOnline时候即算出，不需要额外计算

        }

        #region refresh private

        private static Boolean isAgentSpider( MvcContext ctx ) {

            String agent = ctx.web.ClientAgent;
            if (agent == null) return true;

            foreach (String spiderName in config.Instance.Site.Spider) {

                if (agent.ToLower().IndexOf( spiderName.ToLower() ) >= 0) {
                    // TODO 记录spider在线情况
                    return true;
                }
            }

            return false;
        }

        private static void CountOnlineData( MvcContext ctx ) {
            List<OnlineUser> allVisitors = cdb.findAll<OnlineUser>();
            OnlineStats.Instance.Count = allVisitors.Count;
            int memberCount = 0;
            for (int i = 0; i < allVisitors.Count; i++) {
                OnlineUser online = allVisitors[i] as OnlineUser;
                if (online == null) continue;
                if (online.UserId > 0) {
                    memberCount++;
                }
            }
            OnlineStats.Instance.MemberCount = memberCount;
            OnlineStats.Instance.GuestCount = OnlineStats.Instance.Count - OnlineStats.Instance.MemberCount;
        }


        private static void UpdateOnline( MvcContext ctx ) {


            String sid = getSessionId( ctx );
            List<OnlineUser> result = cdb.findByName<OnlineUser>( sid );
            Boolean isNew = (result.Count == 0 ? true : false);

            if (isNew) {
                OnlineUser myOnline = new OnlineUser();
                myOnline.Name = sid;
                myOnline.StartTime = DateTime.Now;

                populateOnline( myOnline, ctx );

                Dictionary<String, Object> dic = getIndexMap( myOnline );
                myOnline.insertByIndex( dic );


            }
            else {
                OnlineUser myOnline = result[0];
                populateOnline( myOnline, ctx );
                Dictionary<String, Object> dic = getIndexMap( myOnline );
                myOnline.updateByIndex( dic );

            }

            // 检查是否有同名的登录用户
            if ( ctx.viewer != null && ctx.viewer.IsLogin) {

                List<OnlineUser> sameUsers = cdb.findBy<OnlineUser>( "UserName", ctx.viewer.obj.Name );
                if (sameUsers.Count > 1) {
                    foreach (OnlineUser u in sameUsers) {
                        if (u.Name == sid) continue;
                        cdb.delete( u );
                    }
                }
            }

        }

        private static Dictionary<String, Object> getIndexMap( OnlineUser myOnline ) {
            Dictionary<String, Object> dic = new Dictionary<String, Object>();
            dic.Add( "Name", myOnline.Name );
            dic.Add( "UserName", myOnline.UserName );
            return dic;
        }

        private static void populateOnline( OnlineUser myOnline, MvcContext ctx ) {

            myOnline.Role = String.Empty;
            myOnline.Ip = ctx.Ip;
            myOnline.TrueIp = ctx.Ip;
            myOnline.Agent = strUtil.CutString( ctx.web.ClientAgent, 240 );


            if (ctx.viewer != null && ctx.viewer.obj != null) {

                myOnline.UserName = ctx.viewer.obj.Name;
                myOnline.UserId = ctx.viewer.Id;
                myOnline.UserUrl = Link.ToMember( ctx.viewer.obj );
                myOnline.UserPicUrl = ctx.viewer.obj.PicSmall;
            }
            else
                myOnline.UserName = "guest";

            if (ctx.owner != null && ctx.owner.obj != null)
                myOnline.Target = ctx.owner.obj.Name;

            myOnline.IsHidden = 0;

            String referrer = "";
            if (ctx.web.PathReferrer != null) referrer = ctx.web.PathReferrer;
            myOnline.Referrer = referrer;

            myOnline.LastActive = DateTime.Now;
            myOnline.Location = getLocation( ctx );
        }



        private static void DeleteTimeoutVisitor( MvcContext ctx ) {
            List<OnlineUser> allVisitors = cdb.findAll<OnlineUser>();
            for (int i = 0; i < allVisitors.Count; i++) {

                OnlineUser online = allVisitors[i] as OnlineUser;
                if (online == null) continue;
                TimeSpan span = DateTime.Now.Subtract( online.LastActive );
                try {
                    if (span.TotalMinutes > 20) online.delete();
                }
                catch (Exception ex) {
                    logger.Error( "DeleteTimeoutVisitor:" + ex );
                }
            }
        }

        private static void UpdateMaxOnline( MvcContext ctx ) {
            if (OnlineStats.Instance.Count > config.Instance.Site.MaxOnline) {

                config.Instance.Site.MaxOnline = OnlineStats.Instance.Count;
                config.Instance.Site.MaxOnlineTime = DateTime.Now;

                config.Instance.Site.Update( "MaxOnline", OnlineStats.Instance.Count );
                config.Instance.Site.Update( "MaxOnlineTime", DateTime.Now );
            }

            OnlineStats.Instance.MaxCount = config.Instance.Site.MaxOnline;
            OnlineStats.Instance.MaxTime = config.Instance.Site.MaxOnlineTime;
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

        private static String getLocation( MvcContext ctx ) {
            String result = ctx.url.PathAndQuery;
            return result;
        }



        #endregion
    }
}
