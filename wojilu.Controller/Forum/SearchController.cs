using System;
using System.Collections.Generic;
using System.Text;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Apps.Forum.Interface;
using wojilu.Apps.Forum.Service;
using wojilu.Apps.Forum.Domain;

namespace wojilu.Web.Controller.Forum {

    [App( typeof( ForumApp ) )]
    public class SearchController : ControllerBase {

        public IForumTopicService topicService { get; set; }

        public SearchController() {
            topicService = new ForumTopicService();
        }

        public void Results() {

            String key = ctx.Get( "q" );
            set( "searchKey", key );

            set( "searchAction", to( Results ) );

            DataPage<ForumTopic> list = topicService.Search( ctx.app.Id, key, 50 );
            bintTopics( list.Results, list.PageBar );
        }


        private void bintTopics( List<ForumTopic> results, String pageBar ) {
            IBlock block = getBlock( "list" );
            foreach (ForumTopic t in results) {
                bindTopicOne( block, t );
            }
            set( "page", pageBar );
        }

        private void bindTopicOne( IBlock block, ForumTopic topic ) {

            String typeImg = string.Empty;
            if (strUtil.HasText( topic.TypeName )) {
                typeImg = string.Format( "<img src='{0}apps/forum/{1}.gif'>", sys.Path.Skin, topic.TypeName );
            }

            block.Set( "p.Id", topic.Id );
            block.Set( "p.TypeImg", typeImg );
            block.Set( "p.TitleStyle", topic.TitleStyle );
            block.Set( "p.Titile", topic.Title );
            block.Set( "p.Author", topic.Creator.Name );

            block.Set( "p.Url", Link.To( topic.OwnerType, topic.OwnerUrl, new TopicController().Show, topic.Id, topic.AppId ) );

            block.Set( "p.Created", topic.Created );
            block.Set( "p.ReplyCount", topic.Replies );
            block.Set( "p.Hits", topic.Hits.ToString() );

            String attachments = topic.Attachments > 0 ? "<img src='" + sys.Path.Img + "attachment.gif'/>" : "";
            block.Set( "p.Attachments", attachments );

            block.Next();
        }

    }

}
