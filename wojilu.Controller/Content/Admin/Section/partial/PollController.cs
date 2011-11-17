/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

using wojilu.Web.Mvc;

using wojilu.Common.AppBase;
using wojilu.Common.AppBase.Interface;
using wojilu.Apps.Content.Domain;

namespace wojilu.Web.Controller.Content.Admin.Section {

    public partial class PollController : ControllerBase, IPageSection {

        private void bindPollSection( int sectionId, ContentPoll c ) {
            set( "addLink", to( Add, sectionId ) );
            set( "listLink", to( List, sectionId ) );
            set( "section.Id", sectionId );

            if (c != null) {
                set( "poll.Title", c.Title );

                set( "poll.Question", c.Question );

                // 显示投票选项
                ctx.SetItem( "sectionId", sectionId );
                String html = ExtData.GetExtView( c.Id, typeof( ContentPost ).FullName, typeof( ContentPoll ).FullName, ctx );

                set( "poll.Html", html );
            }
            else {
                set( "poll.Title", alang( "exPollNotAdd" ) );
                set( "poll.Question", "" );
                set( "poll.Html", "" );
            }
        }

        private void bindPostList( DataPage<ContentPost> list ) {
            IBlock block = getBlock( "list" );
            foreach (ContentPost p in list.Results) {
                block.Set( "post.Title", p.Title );
                //block.Set( "post.ShowLink", to( new Content.Section.PollController().Show, p.Id ) );
                block.Set( "post.ShowLink", alink.ToAppData( p ) );

                block.Set( "post.Created", p.Created );
                block.Set( "post.DeleteLink", to( Delete, p.Id ) );
                block.Next();
            }

            set( "page", list.PageBar );
        }

    }
}
