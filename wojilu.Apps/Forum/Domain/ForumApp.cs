/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;

using wojilu.ORM;

using wojilu.Common.Security;
using wojilu.Common.AppBase.Interface;

using wojilu.Apps.Forum.Domain.Security;
using wojilu.Common.MemberApp.Interface;
using wojilu.Members.Sites.Domain;

namespace wojilu.Apps.Forum.Domain {

    [Serializable]
    public class ForumApp : ObjectBase<ForumApp>, IApp, ISecurity {

        public ForumApp() {

            this.Security = ForumPermission.GetDefaultPermission();

        }


        public int OwnerId { get; set; }
        public String OwnerType { get; set; }
        public String OwnerUrl { get; set; }

        [LongText]
        public String Notice { get; set; }

        public int TopicCount { get; set; }
        public int PostCount { get; set; }
        public int DeleteCount { get; set; }
        public int PickCount { get; set; }

        public String RateCurrency { get; set; }

        public int Status { get; set; }
        public int AccessStatus { get; set; }
        public DateTime Created { get; set; }

        public String LastUpdateMemberName { get; set; }
        public String LastUpdateMemberUrl { get; set; }
        public String LastUpdatePostTitle { get; set; }
        public String LastUpdatePostUrl { get; set; }
        public DateTime LastUpdateTime { get; set; }

        public int TodayPostCount { get; set; }
        public DateTime TodayTime { get; set; }
        public int TodayTopicCount { get; set; }
        public int TodayVisitCount { get; set; }

        public int PeakPostCount { get; set; }
        public int MemberCount { get; set; }
        public int VisitCount { get; set; }
        public int YestodayPostCount { get; set; }
        public int OnlinePeakCount { get; set; }
        public DateTime OnlinePeakTime { get; set; }

        [LongText]
        public String Security { get; set; }

        [LongText]
        public String StickyTopic { get; set; }


        [NotSave]
        public int OnlineMemberCount { get; set; }

        [NotSave]
        public IList OnlineUser { get; set; }


        [NotSave]
        public int AllPostCount {
            get { return (this.PostCount + this.TopicCount); }
        }

        [NotSave]
        public int AllTodayPostCount {
            get { return (this.TodayTopicCount + this.TodayPostCount); }
        }

        [NotSave]
        public int MaxRateValue {
            get { return 20; }
        }

        public new Result update() {
            return db.update( this );
        }

        public int CountPost() {
            return ForumPost.find( "AppId=" + base.Id ).count();
        }

        public int CountTopic() {
            return ForumTopic.find( "AppId=" + base.Id ).count();
        }

        [LongText]
        public String Settings { get; set; }

        private ForumSetting _settings;
        public ForumSetting GetSettingsObj() {
            if (strUtil.IsNullOrEmpty( this.Settings )) return new ForumSetting();

            if (_settings != null) return _settings;

            ForumSetting s = Json.Deserialize<ForumSetting>( this.Settings );
            s.SetDefaultValue();
            _settings = s;

            return s;
        }


    }
}

