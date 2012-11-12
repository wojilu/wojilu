/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Text;
using System.Collections.Generic;

using wojilu.DI;
using wojilu.Web.Mvc;
using wojilu.Web.Context;
using wojilu.Members.Users.Domain;
using wojilu.Apps.Forum.Domain;
using wojilu.Common.Polls.Domain;

namespace wojilu.Web.Controller.Poll.Utils {

    public class PollValidator<T> where T : PollBase {


        private static String alang( MvcContext ctx, String key ) {
            return ctx.controller.alang( key );
        }

        //--------------------------------------------


        public T Validate( MvcContext ctx ) {

            String title = ctx.Post( "Title" );
            String question = ctx.PostHtml( "Question" );

            string[] answers = ctx.web.postValuesByKey( "Answer" );
            StringBuilder builder = new StringBuilder();
            int acount = 0;
            foreach (String answer in answers) {
                if ((strUtil.HasText( answer ) && (answer.Length > 0)) && (answer.Length < 80)) {
                    builder.Append( answer );
                    builder.Append( "\n" );
                    acount++;
                }
            }
            String strAnswer = builder.ToString();

            int polltype = ctx.PostInt( "PollType" );
            int days = ctx.PostInt( "Days" );
            int voterRank = ctx.PostInt( "VoterRank" );
            String strIsVisible = ctx.Post( "IsVisible" );
            String openVoter = ctx.Post( "OpenVoter" );

            if (strUtil.IsNullOrEmpty( title )) ctx.errors.Add( lang.get( "exTitle" ) );
            if (strUtil.IsNullOrEmpty( strAnswer )) ctx.errors.Add( alang( ctx, "exPollOptions" ) );

            if (strUtil.HasText( strAnswer )) {
                if (acount < 2) {
                    ctx.errors.Add( alang( ctx, "exPollMinTwoOptions" ) );
                }
                if (acount > 20) {
                    ctx.errors.Add( alang( ctx, "exPollMaxTwoOptions" ) );
                }
            }
            if (days > 365 * 10) {
                ctx.errors.Add( alang( ctx, "exPollMaxDay" ) );
                days = 0;
            }

            T poll = (T)ObjectContext.CreateObject( typeof( T ) );
            poll.Title = title;
            poll.Question = question;
            poll.Answer = strAnswer;
            poll.ExpiryDate = DateTime.Now.AddDays( (double)days );
            poll.VoterRank = voterRank;

            int isVisible = 0;
            if (strUtil.HasText( strIsVisible ) && strIsVisible.Equals( "on" )) {
                isVisible = 1;
            }

            int isOpenVoter = 1;
            if (strUtil.HasText( openVoter ) && openVoter.Equals( "on" )) {
                isOpenVoter = 0;
            }

            poll.Type = polltype;
            poll.IsVisible = isVisible;
            poll.IsOpenVoter = isOpenVoter;

            poll.Creator = (User)ctx.viewer.obj;
            poll.CreatorUrl = ctx.viewer.obj.Url;

            poll.OwnerId = ctx.owner.Id;
            poll.OwnerType = ctx.owner.obj.GetType().FullName;
            poll.OwnerUrl = ctx.owner.obj.Url;

            poll.AppId = ctx.app.Id;
            poll.Ip = ctx.Ip;

            return poll;
        }

    }
}

