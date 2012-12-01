using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Apps.Forum.Domain;
using wojilu.Apps.Forum.Interface;
using wojilu.Apps.Forum.Service;
using wojilu.Web.Controller.Common;

namespace wojilu.Web.Controller.Forum.Admin {

    [App( typeof( ForumApp ) )]
    public class ForumPickController : PickDataBaseController<ForumTopic, ForumPick> {

        public override List<ForumTopic> GetNewPosts() {
            IForumTopicService topicService = new ForumTopicService();
            return topicService.GetByApp( ctx.app.Id, 21 );
        }

        public override string GetImgAddLink() {
            return to( new PickedImgController().Add );
        }

        public override string GetImgListLink() {
            return to( new PickedImgController().Index );
        }

        public override int GetImgCount() {
            ForumApp app = ctx.app.obj as ForumApp;
            ForumSetting s = app.GetSettingsObj();
            return s.HomeImgCount;
        }

    }


}
