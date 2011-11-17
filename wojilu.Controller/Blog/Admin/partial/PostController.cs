/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Members.Users.Domain;
using wojilu.Common.AppBase;
using wojilu.Apps.Blog.Domain;

namespace wojilu.Web.Controller.Blog.Admin {

    public partial class PostController : ControllerBase {







        private void bindTrashList( DataPage<BlogPost> blogpostList ) {
            IBlock block = getBlock( "list" );
            foreach (BlogPost post in blogpostList.Results) {
                block.Set( "post.CategoryName", post.Category.Name );
                block.Set( "post.CategoryUrl", to( ListByCategory, post.Category.Id ) );
                block.Set( "post.Id", post.Id );
                block.Set( "post.Title", post.Title );
                block.Set( "post.Created", post.Created );
                block.Next();
            }

            set( "page", blogpostList.PageBar );
        }


        private void bindAdd( List<BlogCategory> categories ) {
            set( "categoryAddUrl", to( new CategoryController().New ) );
            set( "DraftActionUrl", to( SaveDraft ) );

            //String dropList = Html.DropList( categories, "CategoryId", "Name", "Id", null );
            //set( "categoryDropList", dropList );
            dropList( "CategoryId", categories, "Name=Id", null );

            editor( "Content", "", "400px" );
        }


        private void bindEdit( BlogPost data, List<BlogCategory> categories ) {
            //String categoryDropList = Html.DropList( categories, "CategoryId", "Name", "Id", data.Category.Id );
            //set( "data.CatetgoryId", categoryDropList );
            dropList( "CategoryId", categories, "Name=Id", data.Category.Id );

            set( "data.Id", data.Id );
            set( "data.Abstract", data.Abstract );
            set( "data.TagList", data.Tag.TextString );
            set( "data.Title", data.Title );

            editor( "Content", data.Content, "400px" );

            set( "data.AccessStatus", AccessStatusUtil.GetRadioList( data.AccessStatus ) );
            set( "data.IsCloseComment", Html.CheckBox( "IsCloseComment", lang( "closeComment" ), "1", cvt.ToBool( data.CommentCondition ) ) );
        }

    }

}
