/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

using wojilu.Web.Mvc;
using wojilu.Members.Users.Domain;
using wojilu.Apps.Blog.Domain;
using wojilu.Common.Resource;

namespace wojilu.Web.Controller.Blog {

    public partial class MainController : ControllerBase {

        private void bindUsers( List<User> userRanks ) {

            IBlock block = getBlock( "users" );
            foreach (User user in userRanks) {
                block.Set( "user.Name", user.Name );
                block.Set( "user.Face", user.PicSmall );
                block.Set( "user.Link", toUser( user ) );
                block.Next();
            }

        }

        private int i = 1;

        private void bindSidebar( List<BlogPost> tops, List<BlogPost> hits, List<BlogPost> replies ) {
            i = 1;

            bindList( "tops", "post", tops, bindLink );

            i = 1;
            bindList( "hits", "post", hits, bindLink );

            i = 1;
            bindList( "replies", "post", replies, bindLink );
        }

        private void bindLink( IBlock tpl, String lbl, object obj ) {

            BlogPost post = obj as BlogPost;
            String userLink = toUser( post.CreatorUrl );

            String userFace = "";
            if (strUtil.HasText( post.Creator.Pic )) {
                userFace = string.Format( "<a href='{0}'><img src='{1}'/></a><br/>", userLink, post.Creator.PicSmall );
            }
            userFace += string.Format( "<a href='{0}'>{1}</a>", userLink, post.Creator.Name );

            String abs = strUtil.HasText( post.Abstract ) ? post.Abstract : strUtil.ParseHtml( post.Content, 200 );
            tpl.Set( "post.Face", userFace );
            tpl.Set( "post.Abstract", abs );
            tpl.Set( "post.LinkShow", alink.ToAppData( post ) );
            tpl.Set( "post.Number", i );

            i++;
        }


        private String getDropList( int val ) {
            PropertyCollection plist = new PropertyCollection();
            plist.Add( new PropertyItem( lang( "title" ), 1 ) );
            plist.Add( new PropertyItem( lang( "author" ), 2 ) );

            return Html.DropList( plist, "qtype", "Name", "Value", val );
        }

    }

}
