/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;

using wojilu.Members.Users.Domain;
using wojilu.Members.Users.Service;
using wojilu.Members.Users.Interface;

using wojilu.Apps.Blog.Domain;
using wojilu.Apps.Blog.Service;
using wojilu.Apps.Blog.Interface;
using wojilu.Web.Controller.Blog.Caching;
using wojilu.Common.Picks;
using wojilu.Apps.Blog;

namespace wojilu.Web.Controller.Blog {

    [App( typeof( BlogApp ) )]
    public partial class MainController : ControllerBase {

        public IBlogPostService postService { get; set; }
        public IUserService userService { get; set; }
        public IPickedService pickedService { get; set; }
        public ISysBlogService sysblogService { get; set; }
        public IBlogSysCategoryService categoryService { get; set; }
        public BlogPickService pickService { get; set; }
        public BlogPicService picService { get; set; }

        public MainController() {
            postService = new BlogPostService();
            userService = new UserService();
            pickedService = new PickedService();
            sysblogService = new SysBlogService();
            categoryService = new BlogSysCategoryService();
            pickService = new BlogPickService();

            picService = new BlogPicService();

            HideLayout( typeof( LayoutController ) );
        }

        [CacheAction( typeof( BlogMainLayoutCache ) )]
        public override void Layout() {

            // 当前app/module所有页面，所属的首页
            ctx.SetItem( "_moduleUrl", to( Index ) );

            List<BlogPost> tops = pickedService.GetTop( 10 );
            List<BlogPost> hits = sysblogService.GetSysHit( 15 );
            List<BlogPost> replies = sysblogService.GetSysReply( 15 );

            bindSidebar( tops, hits, replies );

            // 博客之星
            bindBlogStar();

            String qword = ctx.Get( "qword" );
            int qtype = ctx.GetInt( "qtype" );
            set( "qword", qword );
            set( "qtype", qtype );

            set( "recentLink", to( Recent ) );

            bindAdminLink();
        }

        private void bindAdminLink() {
            set( "listLink", to( new wojilu.Web.Controller.Admin.Apps.Blog.MainController().Index, 0 ) );
            set( "pickedLink", to( new wojilu.Web.Controller.Admin.Apps.Blog.BlogPickController().Index ) );
            set( "trashLink", to( new wojilu.Web.Controller.Admin.Apps.Blog.TrashController().Trash ) );
            set( "commentLink", to( new wojilu.Web.Controller.Admin.Apps.Blog.CommentController().List ) + "?type=" + typeof( BlogPostComment ).FullName );
            set( "categoryLink", to( new wojilu.Web.Controller.Admin.Apps.Blog.SysCategoryController().List ) );
            set( "settingLink", to( new wojilu.Web.Controller.Admin.Apps.Blog.SettingController().Index ) );
            set( "fileLink", to( new wojilu.Web.Controller.Admin.Apps.Blog.BlogFileController().Index, 0 ) );
        }

        [CachePage( typeof( BlogMainPageCache ) )]
        [CacheAction( typeof( BlogMainCache ) )]
        public void Index() {

            ctx.Page.Title = BlogAppSetting.Instance.MetaTitle;
            ctx.Page.Keywords = BlogAppSetting.Instance.MetaKeywords;
            ctx.Page.Description = BlogAppSetting.Instance.MetaDescription;

            // TODO 博客排行
            List<User> userRanks = User.find( "order by Hits desc, id desc" ).list( 14 );
            bindUsers( userRanks );

            set( "recentLink", to( Recent ) );
            set( "recentLink2", PageHelper.AppendNo( to( Recent ), 2 ) );

            // 头条
            load( "blogPickedList", TopList );

            // 图片博客
            bindPicBlog();

            List<BlogSysCategory> categories = categoryService.GetAll();
            IBlock block = getBlock( "categories" );
            int i = 0;
            foreach (BlogSysCategory c in categories) {
                //给博客的聚合入口中博客系统分类的展示增加逻辑：每行显示2个 
                if (i % 2 == 0) {

                    block.Set( "c.Begin", "<tr><td>" );
                    block.Set( "c.End", "</td>" );
                    if (categories.Count % 2 != 0 && categories.Count == (i + 1)) {
                        block.Set( "c.End", "</td></tr>" );
                    }

                }
                else {
                    block.Set( "c.Begin", "<td>" );
                    block.Set( "c.End", "</td></tr>" );
                }

                List<BlogPost> list = sysblogService.GetByCategory( c.Id, 8 );
                bindOneCategory( block, list );

                block.Set( "c.Index", i % 2 );
                block.Set( "c.Name", c.Name );
                block.Set( "c.LinkShow", to( Recent ) + "?cid=" + c.Id );
                block.Next();
                i++;
            }

        }

        private void bindBlogStar() {

            IBlock sblock = getBlock( "blockStar" );

            BlogAppSetting s = BlogAppSetting.Instance;
            sblock.Set( "BlogStarColumnName", s.BlogStarColumnName );

            User user = User.findById( s.BlogStarUserId );
            if (user != null) {
                sblock.Set( "x.UserTitle", s.BlogStarUserTitle );
                sblock.Set( "x.Pic", user.PicM );
                sblock.Set( "x.Link", Link.ToMember( user ) );
                sblock.Set( "x.Description", s.BlogStarUserDescription );
                sblock.Next();
            }
        }

        private void bindPicBlog() {
            List<BlogPicPick> pics = picService.GetSysNew( 5 );
            pics.ForEach( x => x.data.show = alink.ToAppData( x.BlogPost ) );
            bindList( "picList", "x", pics );
        }

        private void bindOneCategory( IBlock cblock, List<BlogPost> list ) {

            IBlock block = cblock.GetBlock( "list" );
            foreach (BlogPost x in list) {
                block.Set( "x.Id", x.Id );
                block.Set( "x.Title", x.Title );
                block.Set( "x.LinkShow", alink.ToAppData( x ) );
                block.Set( "x.Created", x.Created );
                block.Set( "x.CreatorName", x.Creator.Name );
                block.Set( "x.CreatorLink", toUser( x.Creator ) );
                block.Next();
            }
        }

        //-------------------------------------------------------------------------------------------


        [NonVisit]
        public void TopList() {

            int imgCount = BlogAppSetting.Instance.PickImgCount;
            int dataCount = BlogAppSetting.Instance.PickDataCount;

            List<BlogPickedImg> pickedImg = db.find<BlogPickedImg>( "order by Id desc" ).list( imgCount );
            bindImgs( pickedImg );

            List<BlogPost> newPosts = sysblogService.GetSysNew( 0, dataCount );
            List<MergedData> results = pickService.GetAll( newPosts, 0 );

            bindCustomList( results );
        }

        private void bindImgs( List<BlogPickedImg> list ) {
            IBlock block = getBlock( "pickedImg" );
            foreach (BlogPickedImg f in list) {
                block.Set( "f.Title", f.Title );
                block.Set( "f.Url", f.Url );
                block.Set( "f.ImgUrl", f.ImgUrl );
                block.Next();
            }
        }

        private void bindCustomList( List<MergedData> list ) {

            IBlock hBlock = getBlock( "hotPick" );
            IBlock pBlock = getBlock( "pickList" );

            // 绑定第一个
            if (list.Count == 0) return;
            bindPick( list[0], hBlock, 1 );

            // 绑定列表
            if (list.Count == 1) return;
            for (int i = 1; i < list.Count; i++) {
                bindPick( list[i], pBlock, i + 1 );

                if (i > BlogAppSetting.Instance.PickDataCount - 1) break;
            }
        }

        private void bindPick( MergedData x, IBlock block, int index ) {

            block.Set( "x.Title", x.Title );
            block.Set( "x.Summary", x.Summary );

            String lnk = x.Topic == null ? x.Link : alink.ToAppData( x.Topic );
            block.Set( "x.LinkShow", lnk );

            block.Next();
        }


        //--------------------------------------------------------------------------------------------

        public void Recent() {

            ctx.Page.Title = alang( "allBlogPost" );

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
