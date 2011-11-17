/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

using wojilu.DI;
using wojilu.ORM;
using wojilu.Members.Users.Domain;
using wojilu.Common.Tags;
using wojilu.Common.AppBase.Interface;

namespace wojilu.Common.Polls.Domain {

    [Serializable]
    public abstract class PollBase : ObjectBase<PollBase>, IAppData {

        public int AppId { get; set; }

        public int OwnerId { get; set; }
        public String OwnerType { get; set; }
        public String OwnerUrl { get; set; }

        public User Creator { get; set; }
        public String CreatorUrl { get; set; }

        public int Type { get; set; }

        [NotNull( Lang = "exTitle" )]
        public String Title { get; set; }

        public int TopicId { get; set; }

        [LongText]
        public String Question { get; set; }
        public String Answer { get; set; }

        public int VoterRank { get; set; }
        public int IsOpenVoter { get; set; }
        public int IsVisible { get; set; }
        public DateTime ExpiryDate { get; set; }

        public String AnonymousResult { get; set; }
        public String MemberResult { get; set; }

        public int AccessStatus { get; set; }
        public int VoteCount { get; set; }
        public int Hits { get; set; }

        [Column( Length = 40 )]
        public String Ip { get; set; }

        public DateTime Created { get; set; }


        [NotSave]
        public string[] OptionList {
            get {
                if (strUtil.HasText( Answer )) {
                    return Answer.Split( new char[] { '\n' } );
                }
                return null;
            }
        }

        [NotSave]
        public int TotalVotes {
            get {
                return (AnonymousVotes + MemberVotes);
            }
        }

        private int _memberCount = -1;
        [NotSave]
        public int MemberVotes {
            get {
                if (_memberCount == -1) {
                    _memberCount = getVoteCount( MemberResult );
                }
                return _memberCount;
            }
        }

        private int _anonymousCount = -1;
        [NotSave]
        public int AnonymousVotes {
            get {
                if (_anonymousCount == -1) {
                    _anonymousCount = getVoteCount( AnonymousResult );
                }
                return _anonymousCount;
            }
        }

        private int getVoteCount( String target ) {
            if (strUtil.IsNullOrEmpty( target )) {
                return 0;
            }
            string[] strArray = target.Split( new char[] { '/' } );
            int num = 0;
            foreach (String str in strArray) {
                num += cvt.ToInt( str );
            }
            return num;
        }

        public Boolean CheckHasVote( int userId ) {

            String typeName = this.GetType().FullName + "Result";
            Type t = ObjectContext.Instance.TypeList[typeName];

            return ndb.find( t, "MemberId>0 and MemberId=" + userId + " and PollId=" + this.Id ).first() != null;
        }


    }
}

