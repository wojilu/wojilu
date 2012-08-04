/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using wojilu.Data;


namespace wojilu.Common.Money.Domain {

    [Serializable]
    public class UserAction : CacheObject {

        public static UserAction Forum_CreateTopic { get { return GetById( 1 ); } }
        public static UserAction Forum_ReplyTopic { get { return GetById( 2 ); } }
        public static UserAction Forum_Vote { get { return GetById( 3 ); } }
        public static UserAction Forum_TopicPicked { get { return GetById( 4 ); } }
        public static UserAction Forum_TopicSticky { get { return GetById( 5 ); } }

        public static UserAction Forum_TopicDeleted { get { return GetById( 6 ); } }
        public static UserAction Forum_PostDeleted { get { return GetById( 7 ); } }
        public static UserAction Forum_TopicLocked { get { return GetById( 8 ); } }
        public static UserAction Forum_PostBanned { get { return GetById( 9 ); } }

        public static UserAction Forum_AddAttachment { get { return GetById( 10 ); } }
        public static UserAction Forum_DownloadAttachment { get { return GetById( 11 ); } }
        public static UserAction Forum_Question { get { return GetById( 12 ); } } // TODO

        public static UserAction Blog_CreatePost { get { return GetById( 13 ); } }
        public static UserAction Blog_ReplyPost { get { return GetById( 14 ); } } // TODO

        public static UserAction Photo_CreatePost { get { return GetById( 15 ); } }
        public static UserAction Photo_ReplyPost { get { return GetById( 16 ); } } // TODO

        public static UserAction User_UpdateAvatar { get { return GetById( 17 ); } }
        public static UserAction User_ConfirmEmail { get { return GetById( 18 ); } }

        public static UserAction GetById( int id ) {
            return cdb.findById<UserAction>( id ) ;
        }

    }
}

