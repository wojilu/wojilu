/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Mvc.Attr;
using wojilu.Apps.Blog.Domain;
using wojilu.Apps.Blog.Interface;
using wojilu.Apps.Blog.Service;
using wojilu.Web.Controller.Common;

namespace wojilu.Web.Controller.Admin.Apps.Blog {

    [App( typeof( BlogApp ) )]
    public class BlogPickController : PickDataBaseController<BlogPost, BlogPick> {

        public ISysBlogService postService { get; set; }

        public BlogPickController() {
            postService = new SysBlogService();
        }

        public override List<BlogPost> GetNewPosts() {
            return postService.GetSysNew( 0, 21 );
        }

        public override string GetImgAddLink() {
            return to( new PickedImgController().Add );
        }

        public override string GetImgListLink() {
            return to( new PickedImgController().Index );
        }

        public override int GetImgCount() {
            return 6;
        }
    }

}
