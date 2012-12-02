using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Apps.Forum.Domain;
using wojilu.Web.Mvc.Attr;
using wojilu.Apps.Forum.Service;
using wojilu.Data;
using System.Data;
using wojilu.Members.Users.Domain;
using wojilu.Members.Sites.Domain;
using wojilu.Common.AppBase.Interface;

namespace wojilu.Web.Controller.Forum.Admin {

    [App( typeof( ForumApp ) )]
    public class StatsController : ControllerBase {

        private static readonly ILog logger = LogManager.GetLogger( typeof( StatsController ) );

        public ForumBoardService boardService { get; set; }
        public ForumTopicService topicService { get; set; }
        public ForumPostService postService { get; set; }

        public StatsController() {
            boardService = new ForumBoardService();
            topicService = new ForumTopicService();
            postService = new ForumPostService();
        }

        public void Index() {
            target( BeginStats );
        }

        public void BeginStats() {

            List<ForumBoard> list = ForumBoard.find( "AppId=" + ctx.app.Id ).list();
            foreach (ForumBoard fb in list) {
                recountSingle( fb );
            }

            echoRedirect( "统计成功" );
        }

        private void recountSingle( ForumBoard fb ) {

            boardService.UpdateStats( fb );
            boardService.UpdateLastInfo( fb );

        }


    }

}
