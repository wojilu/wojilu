/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Text;

using wojilu.ORM;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;


using wojilu.Members.Users.Domain;
using wojilu.Members.Users.Service;
using wojilu.Members.Users.Interface;
using wojilu.Common.Resource;

using wojilu.Apps.Blog.Domain;
using wojilu.Apps.Blog.Service;
using wojilu.Apps.Blog.Interface;
using wojilu.Web.Controller.Common;
using wojilu.Web.Controller.Blog.Caching;

namespace wojilu.Web.Controller.Blog {

    [App( typeof( BlogApp ) )]
    public partial class MainController : ControllerBase {

        public IBlogPostService postService { get; set; }
        public IUserService userService { get; set; }
        public IPickedService pickedService { get; set; }
        public ISysBlogService sysblogService { get; set; }


        public MainController() {
            postService = new BlogPostService();
            userService = new UserService();
            pickedService = new PickedService();
            sysblogService = new SysBlogService();

            HideLayout( typeof( LayoutController ) );
        }

        [CacheAction( typeof( BlogMainLayoutCache ) )]
        public override void Layout() {

            IList tops = pickedService.GetTop( 10 );
            IList hits = sysblogService.GetSysHit( 20 );
            IList replies = sysblogService.GetSysReply( 30 );

            bindSidebar( tops, hits, replies );
        }

        [CachePage( typeof( BlogMainPageCache ) )]
        [CacheAction( typeof( BlogMainCache ) )]
        public void Index() {

            WebUtils.pageTitle( this, lang( "blog" ) );

            // TODO 博客排行
            IList userRanks = User.find( "order by Hits desc, id desc" ).list( 21 );
            bindUsers( userRanks );

            IList blogs = sysblogService.GetSysNew( -1, 30 );
            bindList( "list", "post", blogs, bindLink );

            set( "recentLink", to( Recent ) );
            set( "recentLink2", Link.AppendPage( to( Recent ), 2 ) );
            set( "droplist", getDropList( 0 ) );
        }


        public void Recent() {

            WebUtils.pageTitle( this, alang( "allBlogPost" ) );

            DataPage<BlogPost> blogs = null;

            String qword = ctx.Get( "qword" );
            int qtype = ctx.GetInt( "qtype" );


            if (strUtil.HasText( qword ) && qtype > 0) {

                qword = strUtil.SqlClean( qword, 20 );

                String condition = "";
                if (qtype == 1) {
                    condition = "Title like '%" + qword + "%'";
                }
                else if (qtype == 2) {
                    User user = userService.GetByName( qword );
                    if (user == null) {
                        echoRedirect( lang( "exUserNotFound" ) + " : " + qword );
                        return;
                    }

                    condition = "CreatorId=" + user.Id;
                }
                else {
                    echoRedirect( lang( "exop" ) );
                    return;
                }

                blogs = sysblogService.GetSysPageBySearch( condition );

            }
            else {
                blogs = sysblogService.GetSysPage( 30 );
            }

            set( "qword", qword );
            set( "droplist", getDropList( qtype ) );

            bindList( "list", "post", blogs.Results, bindLink );
            set( "page", blogs.PageBar );
            set( "recentLink", to( Recent ) );
        }



    }

}
