/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

namespace wojilu.Apps.Forum.Domain {

    [Serializable]
    public class ForumLogAction {

        public static readonly int Sticky = 1;
        public static readonly int UnSticky = 2;
        public static readonly int Pick = 3;
        public static readonly int UnPick = 4;
        public static readonly int Lock = 5;
        public static readonly int UnLock = 6;
        public static readonly int Highlight = 7;
        public static readonly int UnHighlight = 8;
        public static readonly int Ban = 9;
        public static readonly int UnBan = 10;
        public static readonly int Edit = 11;
        public static readonly int SetCategory = 12;
        public static readonly int Delete = 13;
        public static readonly int MoveTopic = 14;
        public static readonly int DeleteTrue = 15;
        public static readonly int GlobalSticky = 16;
        public static readonly int GlobalUnSticky = 17;

        public static String GetLable( int actionId ) {


            if (actionId == 1) return get( "logSticky" );
            if (actionId == 2) return get( "logUnSticky" );
            if (actionId == 3) return get( "logPick" );
            if (actionId == 4) return get( "logUnPick" );
            if (actionId == 5) return get( "logLock" );
            if (actionId == 6) return get( "logUnLock" );
            if (actionId == 7) return get( "logHighlight" );
            if (actionId == 8) return get( "logUnHighlight" );
            if (actionId == 9) return get( "logBan" );
            if (actionId == 10) return get( "logUnBan" );
            if (actionId == 11) return get( "logEdit" );
            if (actionId == 12) return get( "logSetCategory" );
            if (actionId == 13) return get( "logDelete" );
            if (actionId == 14) return get( "logMoveTopic" );
            if (actionId == 15) return get( "logDeleteTrue" );
            if (actionId == 16) return get( "logGlobalSticky" );
            if (actionId == 17) return get( "logGlobalUnSticky" );


            return "";
        }

        private static String get( String key ) {
            return alang.get( typeof( ForumApp ), key );
        }

    }

}

