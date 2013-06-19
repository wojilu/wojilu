/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Common.Pages.Service;
using wojilu.Common.Pages.Domain;
using wojilu.Members.Users.Domain;
using wojilu.Common.Pages.Interface;

namespace wojilu.Web.Controller.Admin.Sys {

    public partial class PageController : ControllerBase {


        private String plink( int id ) {
            return strUtil.Join( ctx.url.SiteUrl, to( new wojilu.Web.Controller.Common.PageController().Show, id ) );
        }

        private void bindCategoryLink( IBlock tpl, int id ) {
            tpl.Set( "data.LinkByCategory", to( List, id ) );
        }

        private void bindPages( List<Page> list ) {
            Tree<Page> tree = new Tree<Page>( list );

            IBlock block = getBlock( "list" );
            foreach (Page data in tree.GetAllOrdered()) {

                block.Set( "data.Id", data.Id );
                block.Set( "data.OrderId", data.OrderId );

                block.Set( "data.AddSubLink", to( AddSubPage, data.Id ) );

                int indentLength = tree.GetDepth( data.Id ) * 20;
                String indent = "padding-left:" + indentLength + "px";
                block.Set( "data.Indent", indent );

                block.Set( "data.Title", data.Title );
                block.Set( "data.Created", data.Created );
                block.Set( "data.Hits", data.Hits );
                block.Set( "data.ReplyCount", data.Replies );
                block.Set( "data.IsAllowReplyStr", data.IsAllowReplyStr );
                block.Set( "data.IsCollapseStr", data.IsCollapseStr );


                block.Set( "data.ViewUrl", to( ViewUrl, data.Id ) );

                block.Set( "data.LinkShow", plink( data.Id ) );
                block.Set( "data.LinkDelete", to( Delete, data.Id ) );
                block.Set( "data.LinkEdit", to( Edit, data.Id ) );

                block.Next();

            }

        }


        private void bindForm( Page data, Tree<Page> tree ) {
            set( "category", data.Category.Name );
            set( "p.Title", data.Title );
            set( "p.Logo", data.Logo );
            String drop = tree.DropList( "ParentId", data.ParentId, data.Id, "---" );
            set( "dropParent", drop );
            set( "Content", data.Content );

            String chk = "checked=\"checked\"";
            set( "IsAllowReply", data.IsAllowReply == 1 ? chk : "" );
            set( "chkIsCollapse", data.IsCollapse == 1 ? chk : "" );
            set( "chkIsTextNode", data.IsTextNode == 1 ? chk : "" );
        }

        //--------------------------------------------------------------------------

        private void golist( int categoryId ) {
            echoRedirect( lang( "opok" ), to( List, categoryId ) );
        }

        private Page validate( Page data ) {

            data.Title = ctx.Post( "Title" );
            data.Logo = ctx.Post( "Logo" );
            data.Content = ctx.PostHtml( "Content" );
            data.EditReason = ctx.Post( "editReason" );

            if (strUtil.IsNullOrEmpty( data.Title )) errors.Add( lang( "exTitle" ) );
            if (strUtil.IsNullOrEmpty( data.Content )) errors.Add( lang( "exContent" ) );
            if (strUtil.IsNullOrEmpty( data.EditReason )) errors.Add( "请填写编辑原因" );

            data.IsAllowReply = ctx.PostIsCheck( "IsAllowReply" );
            data.IsCollapse = ctx.PostIsCheck( "IsCollapse" );
            data.IsTextNode = ctx.PostIsCheck( "IsTextNode" );


            return data;
        }

    }

}
