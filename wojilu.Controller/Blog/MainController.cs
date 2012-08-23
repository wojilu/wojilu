/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;
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
        public IBlogSysCategoryService categoryService { get; set; }

        public MainController() {
            postService = new BlogPostService();
            userService = new UserService();
            pickedService = new PickedService();
            sysblogService = new SysBlogService();
            categoryService = new BlogSysCategoryService();

            HideLayout( typeof( LayoutController ) );
        }

        [CacheAction( typeof( BlogMainLayoutCache ) )]
        public override void Layout() {

            List<BlogPost> tops = pickedService.GetTop( 10 );
            List<BlogPost> hits = sysblogService.GetSysHit( 20 );
            List<BlogPost> replies = sysblogService.GetSysReply( 30 );

            bindSidebar( tops, hits, replies );

            String qword = ctx.Get( "qword" );
            int qtype = ctx.GetInt( "qtype" );
            set( "qword", qword );
            set( "qtype", qtype );

            set( "recentLink", to( Recent ) );
        }

        [CachePage( typeof( BlogMainPageCache ) )]
        [CacheAction( typeof( BlogMainCache ) )]
        public void Index() {

            WebUtils.pageTitle( this, lang( "blog" ) );

            // TODO 博客排行
            List<User> userRanks = User.find( "order by Hits desc, id desc" ).list( 14 );
            bindUsers( userRanks );

            set( "recentLink", to( Recent ) );
            set( "recentLink2", Link.AppendPage( to( Recent ), 2 ) );

            List<BlogSysCategory> categories = categoryService.GetAll();
            IBlock block = getBlock( "categories" );
            foreach (BlogSysCategory c in categories) {

                List<BlogPost> list = sysblogService.GetByCategory( c.Id, 8 );
                bindOneCategory( block, list );

                block.Set( "c.Name", c.Name );
                block.Set( "c.LinkShow", to( Recent ) + "?cid=" + c.Id );
                block.Next();
            }

        }

        private void bindOneCategory( IBlock cblock, List<BlogPost> list ) {

            IBlock block = cblock.GetBlock( "list" );
            foreach (BlogPost x in list) {
                block.Set( "x.Id", x.Id );
                block.Set( "x.Title", x.Title );
                block.Set( "x.LinkShow", alink.ToAppData( x ) );
                block.Set( "x.Created", x.Created );
                block.Set( "x.CreatorName", x.Creator.Name );
                block.Set( "x.CreatorLink", Link.ToMember( x.Creator ) );
                block.Next();
            }
        }


        public void Recent() {

            WebUtils.pageTitle( this, alang( "allBlogPost" ) );

            set( "blogLink", to( Index ) );

            DataPage<BlogPost> blogs = null;

            String qword = ctx.Get( "qword" );
            int qtype = ctx.GetInt( "qtype" );
            int categoryId = ctx.GetInt( "cid" );


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

                set( "listName", "搜索结果" );

            }
            else if (categoryId > 0) {
                blogs = sysblogService.GetSysPageByCategory( categoryId, 30 );

                BlogSysCategory c = categoryService.GetById( categoryId );
                if (c == null) {
                    set( "listName", "分类不存在，没有结果" );
                }
                else {
                    set( "listName", c.Name );
                }
            }
            else {
                blogs = sysblogService.GetSysPage( 30 );
                set( "listName", alang( "allBlogPost" ) );
            }


            bindList( "list", "post", blogs.Results, bindLink );
            set( "page", blogs.PageBar );
            set( "recentLink", to( Recent ) );
        }



    }

}
