using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Apps.Forum.Interface;
using wojilu.Apps.Forum.Service;
using wojilu.Apps.Forum.Domain;
using wojilu.Web.Controller.Forum.Utils;
using wojilu.Members.Users.Domain;

namespace wojilu.Web.Controller.Forum.Edits {

    [App( typeof( ForumApp ) )]
    public class TagController : ControllerBase {

        public IForumTopicService topicService { get; set; }

        public TagController() {
            topicService = new ForumTopicService();
        }


        [HttpPost, DbTransaction]
        public void SaveTag( int topicId ) {

            String tagValue = ctx.Post( "tagValue" );

            if (strUtil.IsNullOrEmpty( tagValue )) {
                echoText( "请输入内容" );
                return;
            }

            ForumTopic topic = topicService.GetById( topicId, ctx.owner.obj );
            if (topic == null) { echo( lang( "exDataNotFound" ) ); return; }
            ForumPost post = topicService.GetPostByTopic( topic.Id );

            topic.Tag.Save( tagValue );

            echoAjaxOk();
        }



    }

}
