/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;

using wojilu.Apps.Poll.Service;
using wojilu.Apps.Poll.Domain;
using wojilu.Members.Users.Domain;
using wojilu.Web.Controller.Poll.Utils;

namespace wojilu.Web.Controller.Poll.Admin {

    [App(typeof(PollApp))]
    public partial class PollController : ControllerBase {

        public PollDataService pollService { get; set; }

        public PollController() {
            pollService = new PollDataService();
        }

        public override void Layout() {

            set( "friendsPollLink", to( Index ) );
            set( "myPollLink", to(List ));
            set( "addPollLink", to( Add ) );

        }

        public void Index() {
            view( "List" );
            DataPage<PollData> polls = pollService.GetFriendsPage( ctx.owner.Id );
            bindPollList( polls );
        }

        public void List() {
            DataPage<PollData> polls = pollService.GetPageByApp( ctx.app.Id );
            bindPollList( polls );
        }

        public void Add() {

            target( Create );
            set( "optionCount", 5 );
        }

        [HttpPost, DbTransaction]
        public void Create() {

            PollData poll = new PollValidator<PollData>().Validate( ctx );
            if (errors.HasErrors) {
                run( Add );
                return;
            }

            pollService.Insert( poll );

            String lnkPoll = to( new wojilu.Web.Controller.Poll.PollController().Show, poll.Id );
            pollService.PubCreatedFeed( poll, lnkPoll );

            echoRedirect( lang( "opok" ), List );
        }

        public void Show( int id ) {

            PollData poll = pollService.GetById( id );

            pollService.AddHits( poll );

            bindDetail( poll );
            bindComment( poll );
        }



    }

}
