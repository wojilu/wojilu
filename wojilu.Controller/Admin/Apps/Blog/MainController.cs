/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;

using wojilu.Members.Interface;
using wojilu.Members.Sites.Domain;
using wojilu.Members.Sites.Service;

using wojilu.Apps.Blog.Domain;
using wojilu.Apps.Blog.Interface;
using wojilu.Apps.Blog.Service;
using wojilu.Web.Controller.Security;

namespace wojilu.Web.Controller.Admin.Apps.Blog {

    [App( typeof( BlogApp ) )]
    public partial class MainController : ControllerBase {

        public IBlogPostService postService { get; set; }
        public IPickedService pickedService { get; set; }
        public ISysBlogService sysblogService { get; set; }
        public IAdminLogService<SiteLog> logService { get; set; }
        public IBlogSysCategoryService categoryService { get; set; }

        public MainController() {
            postService = new BlogPostService();
            pickedService = new PickedService();
            sysblogService = new SysBlogService();
            logService = new SiteLogService();
            categoryService = new BlogSysCategoryService();
        }

        public override void Layout() {

            IList categories = categoryService.GetAll();
            bindList( "categories", "c", categories, bindLink );

        }

        // TODO 搜索功能：根据作者、根据时间(最近一个月)、根据阅读量、根据评论数、
        public void Index( int id ) {

            target( Admin );

            DataPage<BlogPost> list = sysblogService.GetSysPageByCategory( id, 36 );
            bindPosts( list );

            setCategoryDropList();


        }

        private void bindLink( IBlock tpl, int id ) {
            tpl.Set( "c.LinkCategory", to( new MainController().Index, id ) );
        }

        private void setCategoryDropList() {
            List<BlogSysCategory> categories = categoryService.GetAll();
            List<BlogSysCategory> list = addSelectInfo( categories );
            dropList( "adminDropCategoryList", list, "Name=Id", null );
        }

        private static readonly int zeroCatId = 99999999;

        private List<BlogSysCategory> addSelectInfo( List<BlogSysCategory> categories ) {
            BlogSysCategory category = new BlogSysCategory();
            category.Id = -1;
            category.Name = lang( "setCategory" );

            BlogSysCategory nullCat = new BlogSysCategory();
            nullCat.Id = zeroCatId;
            nullCat.Name = "--无分类--";

            List<BlogSysCategory> list = new List<BlogSysCategory>();
            list.Add( category );
            list.Add( nullCat );
            foreach (BlogSysCategory cat in categories) {
                list.Add( cat );
            }
            return list;
        }


        public void Picked() {
            target( Admin );
            DataPage<BlogPost> list = pickedService.GetAll();
            bindPosts( list );
        }

        [HttpPost, DbTransaction]
        public void Admin() {

            String ids = ctx.PostIdList( "choice" );
            String cmd = ctx.Post( "action" );
            int categoryId = ctx.PostInt( "categoryId" );

            String condition = string.Format( "Id in ({0}) ", ids );

            if (strUtil.IsNullOrEmpty( cmd ) ) {
                echoText( lang( "exCmd" ) );
                return;
            }

            if (strUtil.IsNullOrEmpty( ids )) {
                echoText( lang( "plsSelect" ) );
                return;
            }

            if ("pick".Equals( cmd )) {
                pickedService.PickPost( ids );
                log( SiteLogString.PickBlogPost(), ids );
                echoAjaxOk();
            }
            else if ("unpick".Equals( cmd )) {
                pickedService.UnPickPost( ids );
                log( SiteLogString.UnPickBlogPost(), ids );
                echoAjaxOk();
            }
            else if ("delete".Equals( cmd )) {
                sysblogService.Delete( ids );
                log( SiteLogString.DeleteBlogPost(), ids );
                echoAjaxOk();
            }
            else if ("category".Equals( cmd )) {
                if (categoryId < 0) {
                    content( lang( "exCategoryNotFound" ) );
                    return;
                }

                if (categoryId == zeroCatId) categoryId = 0;

                BlogPost.updateBatch( "set SysCategoryId=" + categoryId, condition );
                log( SiteLogString.MoveBlogPost(), ids );

                echoAjaxOk();
            }
            else
                echoText( lang( "exUnknowCmd" ) );

        }


        [HttpDelete, DbTransaction]
        public void Delete( int id ) {

            BlogPost post = postService.GetById_ForAdmin( id );
            if (post == null) {
                echoRedirect( lang( "exDataNotFound" ) );
                return;
            }

            sysblogService.SystemDelete( post );
            log( SiteLogString.SystemDeleteBlogPost(), post );

            redirect( Index, 0 );
        }



    }

}
