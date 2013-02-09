using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Apps.Forum.Service;
using wojilu.Apps.Forum.Interface;
using wojilu.Apps.Forum.Domain;
using wojilu.Web.Mvc.Attr;
using wojilu.Members.Users.Domain;
using wojilu.Common.AppBase.Interface;

namespace wojilu.Web.Controller.Forum {

    public class PostVo {
        public int Id { get; set; }
        public String User { get; set; }
        public String Title { get; set; }
        public DateTime Created { get; set; }
    }

    public class PostDetailVo {
        public int Id { get; set; }
        public String User { get; set; }
        public String Title { get; set; }
        public DateTime Created { get; set; }
        public String Content { get; set; }
    }

    [App( typeof( ForumApp ) )]
    public class ServiceController : ControllerBase {

        public IForumPostService postService { get; set; }
        public IForumTopicService topicService { get; set; }

        public ServiceController() {
            postService = new ForumPostService();
            topicService = new ForumTopicService();
        }

        public void Post() {

            List<ForumPost> xlist = postService.GetRecentByApp( ctx.app.Id, 50 );

            List<PostVo> list = new List<PostVo>();
            foreach (ForumPost x in xlist) {
                list.Add( new PostVo {
                    Id = x.Id,
                    User = x.Creator.Name,
                    Title = x.Title,
                    Created = x.Created
                } );
            }

            echoJson( list );
        }

        public void Show( int id ) {

            ForumPost post = ForumPost.findById( id );

            PostDetailVo x = new PostDetailVo {
                Id = id,
                Title = post.Title,
                User = post.Creator.Name,
                Created = post.Created,
                Content = strUtil.ParseHtml( post.Content )
            };

            echoJson( x );

        }

        public void Topic() {

            List<ForumTopic> xlist = topicService.GetByApp( ctx.app.Id, 50 );

            List<PostVo> list = new List<PostVo>();
            foreach (ForumTopic x in xlist) {
                list.Add( new PostVo {
                    User = x.Creator.Name,
                    Title = x.Title,
                    Created = x.Created
                } );
            }

            echoJson( list );
        }

        [HttpPost]
        public void Reply( int topicId ) {

            ForumTopic topic = ForumTopic.findById( topicId );
            ForumPost post = postService.GetPostByTopic( topicId );

            ForumPost x = new ForumPost();

            x.ForumBoardId = topic.ForumBoard.Id;
            x.TopicId = topicId;
            x.ParentId = post.Id;


            x.Title = ctx.Post( "title" );
            x.Content = ctx.PostHtml( "content" );
            x.Ip = ctx.Ip;

            User user = (User)ctx.viewer.obj;

            Result result = postService.Insert( x, user, ctx.owner.obj, (IApp)ctx.app.obj );

            if (result.IsValid) {
                echoAjaxOk();
            }
            else {
                echoText( "error" );
            }
        }

    }


}
