/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Text;
using System.Reflection;

using wojilu.ORM;
using wojilu.Web.Mvc;

using wojilu.Apps.Forum.Domain;
using wojilu.Apps.Forum.Service;
using wojilu.Web.Controller.Forum.Utils;

namespace wojilu.Web.Controller.Forum {

    public class SecurityController : ControllerBase {

        public Random rd = new Random();

        public override void CheckPermission() {

            if (!checkRegisterTimeValid()) return;

            ForumApp forum = ctx.app.obj as ForumApp;
            refreshTodayStats( forum );
            
        }

        private void refreshTodayStats( ForumApp forum ) {


            if (time.IsNextDay( forum.TodayTime ) == false) return;

            int all = forum.TodayTopicCount + forum.TodayPostCount;
            if (all > forum.PeakPostCount) {
                forum.PeakPostCount = all;
            }

            forum.YestodayPostCount = forum.TodayTopicCount + forum.TodayPostCount;

            forum.TodayPostCount = 0;
            forum.TodayTopicCount = 0;
            forum.TodayVisitCount = 0;
            forum.TodayTime = DateTime.Now;

            forum.update();

            ForumBoard.updateBatch( "set TodayTopics=0, TodayPosts=0", "AppId=" + forum.Id );
        }

        //--------------------------------------------------------------------------------------

        // TODO 需要配置
        private Boolean checkRegisterTimeValid() {

            if (ctx.HttpMethod.Equals( "GET" )) return true;

            int validHours = 0;
            TimeSpan span = DateTime.Now.Subtract( ctx.viewer.obj.Created );
            if (span.Hours < validHours) {
                echoRedirect( string.Format( lang( "exPostAfter" ), validHours ) );
                return false;
            }
            return true;
        }


    }

}
