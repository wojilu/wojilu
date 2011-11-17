/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Apps.Forum.Domain;
using wojilu.Web.Controller.Forum.Utils;

namespace wojilu.Web.Controller.Forum {

    public partial class PollController : ControllerBase {

        private void bindAddForm( int id, List<ForumBoard> pathboards ) {

            set( "location", ForumLocationUtil.GetPollAdd( pathboards, ctx ) );
            set( "optionCount", 5 );
            set( "forumId", id );
            editor( "Question", "", "180px" );

            if ((ctx.Post( "PollType" ) == null) || (ctx.Post( "PollType" ) == "0")) {
                set( "singleCheck", " checked=\"checked\"" );
                set( "multiCheck", "" );
            }
            else if (ctx.Post( "PollType" ) == "1") {
                set( "singleCheck", "" );
                set( "multiCheck", " checked=\"checked\"" );
            }
        }

        private void bindVoter( ForumPoll poll, DataPage<ForumPollResult> voterList ) {

            setCurrentBoard( poll );

            IBlock block = getBlock( "list" );
            foreach (ForumPollResult result in voterList.Results) {
                block.Set( "user.Name", result.User.Name );
                block.Set( "user.Choice", result.Answer );
                block.Set( "user.Created", result.Created );
                block.Next();
            }

            set( "page", voterList.PageBar );
        }



    }
}

