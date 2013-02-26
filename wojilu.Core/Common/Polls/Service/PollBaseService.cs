/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using wojilu.Common.Feeds.Domain;
using wojilu.Common.Feeds.Interface;
using wojilu.Common.Feeds.Service;
using wojilu.Common.Money.Domain;
using wojilu.Common.Money.Interface;
using wojilu.Common.Money.Service;
using wojilu.Common.Polls.Domain;

using wojilu.Members.Users.Domain;

namespace wojilu.Common.Polls.Service {

    public class PollBaseService<TP, TR> where TP : PollBase where TR : PollResultBase  {

        public virtual IUserIncomeService incomeService { get; set; }
        public virtual IFeedService feedService { get; set; }

        public PollBaseService() {
            incomeService = new UserIncomeService();
            feedService = new FeedService();
        }

        public virtual void AddHits( TP poll ) {
            poll.Hits++;
            db.update( poll, "Hits" );
        }


        public virtual Result CreateResult( TR pr, String postLink ) {

            TP poll = getPoll( pr );

            Result result = db.insert( pr );
            if (result.IsValid) {

                this.updatePollResult( pr );

                String msg = string.Format( "参与投票 <a href=\"{0}\">{1}</a>, 获得奖励", postLink, poll.Title );
                incomeService.AddIncome( pr.User, UserAction.Forum_Vote.Id, msg );

                addFeedInfo( pr, postLink );
            }
            return result;
        }


        private void addFeedInfo( TR pr, String lnkpost ) {

            TP poll = getPoll( pr );

            Feed feed = new Feed();
            feed.Creator = pr.User;
            feed.DataType = typeof( TP ).FullName;
            feed.TitleTemplate = "{*actor*} " + lang.get( "hasvoted" ) + " {*poll*}";
            //feed.TitleData = "{poll : '<a href=\"" + lnkpost + "\">" + JSON.Encode( poll.Title ) + "</a>'}";
            feed.TitleData = getTitleData( lnkpost, poll );

            feed.Ip = pr.Ip;

            feedService.publishUserAction( feed );
        }

        private string getTitleData( string lnkpost, TP poll ) {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            String lnkPoll = string.Format( "<a href=\"{0}\">{1}</a>", lnkpost, poll.Title );
            dic.Add( "poll", lnkPoll );
            return Json.ToString( dic );
        }

        public void PubCreatedFeed( TP poll, String lnkPoll ) {
            addPubFeedInfo( poll, lnkPoll );
        }

        private void addPubFeedInfo( TP poll, String lnkPost ) {

            String templateData = getTitleData( lnkPost, poll );

            Feed feed = new Feed();

            String strPollFeed = lang.get( "strPollFeed" );
            String tt = string.Format( strPollFeed, "{*actor*}", "{*poll*}" );
            feed.TitleTemplate = tt;

            feed.TitleData = templateData;
            feed.Creator = poll.Creator;
            feed.DataType = typeof( TP ).FullName;

            feed.Ip = poll.Ip;

            feedService.publishUserAction( feed );
        }

        private static TP getPoll( TR pr ) {
            return db.findById<TP>( pr.PollId );
        }


        public virtual TP GetById( int id ) {
            return db.findById<TP>( id );
        }

        public virtual List<TP> GetByTopicList( IList posts ) {
            if (posts.Count == 0) return new List<TP>();
            String ids = "";
            foreach (IEntity d in posts) ids += d.Id + ",";
            ids = ids.TrimEnd( ',' );
            String typeFullName = posts[0].GetType().FullName;
            return GetByTopicIds( ids, typeFullName ); 
        }

        public virtual List<TP> GetByTopicIds( String ids, String typeFullName ) {
            if (cvt.IsIdListValid( ids ) == false) return new List<TP>();
            return db.find<TP>( "TopicId in (" + ids + ")" )
             .list();
        }

        public virtual TP GetByTopicId( int id ) {
            return db.find<TP>( "TopicId=:id" )
                .set( "id", id )
                .first();
        }

        public virtual TP GetByTopicId( List<TP> polls, int topicId ) {
            foreach (TP p in polls) {
                if (p.TopicId == topicId) return p;
            }
            return null;
        }

        public void DeleteByTopicId( int id ) {

            TP poll = this.GetByTopicId( id );
            if (poll != null) poll.delete();
        }

        private String getEmptyResult( int optionCount ) {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < optionCount; i++) {
                builder.Append( "0" );
                builder.Append( "/" );
            }
            return builder.ToString().TrimEnd( '/' );
        }

        private String getNewVote( String memberResults, int optionCount, String choice ) {
            if (strUtil.IsNullOrEmpty( memberResults )) {
                memberResults = this.getEmptyResult( optionCount );
            }
            string[] arrItem = memberResults.Split( '/' );
            if (arrItem.Length != optionCount) {
                memberResults = this.getEmptyResult( optionCount );
                arrItem = memberResults.Split( '/' );
            }
            return this.getPollResult( arrItem, choice );
        }

        private String getOptionNewVoteCount( string[] arrChoice, int oldVotes, int i ) {
            foreach (String str in arrChoice) {
                if (cvt.IsInt( str ) && (cvt.ToInt( str ) == (i + 1))) {
                    int newCount = oldVotes + 1;
                    return newCount.ToString();
                }
            }
            return oldVotes.ToString();
        }

        private String getPollResult( string[] arrItem, String choiceString ) {
            string[] arrChoice = choiceString.Split( new char[] { ',' } );
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < arrItem.Length; i++) {
                if (!cvt.IsInt( arrItem[i] )) {
                    builder.Append( "0" );
                }
                else {
                    String str = this.getOptionNewVoteCount( arrChoice, cvt.ToInt( arrItem[i] ), i );
                    builder.Append( str );
                    builder.Append( "/" );
                }
            }
            return builder.ToString().TrimEnd( '/' );
        }

        public virtual DataPage<TR> GetVoterList( int pollId ) {
            return db.findPage<TR>( "PollId=" + pollId );
        }

        public virtual DataPage<TR> GetVoterList( int pollId, int pageSize ) {
            return db.findPage<TR>( "PollId=" + pollId, pageSize );
        }

        public virtual Result Insert( TP poll ) {
            return db.insert(poll);
        }

        private void updatePollResult( TR pr ) {

            User user = pr.User;
            TP poll = getPoll( pr );

            if (user.IsRegisterUser) {
                poll.MemberResult = this.getNewVote( poll.MemberResult, poll.OptionList.Length, pr.Answer );
            }
            else {
                poll.AnonymousResult = this.getNewVote( poll.AnonymousResult, poll.OptionList.Length, pr.Answer );
            }

            poll.VoteCount++;
            db.update( poll );

        }

        public void Update( TP poll ) {
            if (poll == null) throw new ArgumentNullException( "poll" );
            poll.update();
        }

    }
}

