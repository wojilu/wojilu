/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */
using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Common.Resource;

namespace wojilu.Members.Users.Domain {

    [Serializable]
    public class UserPrivacy {

        public static int GetDefaultValue() {
            return UserPrivacy.EveryOne;
        }

        public static readonly int Self = 0;
        public static readonly int Friend = 1;
        public static readonly int LoginUser = 2;
        public static readonly int EveryOne = 3;

        public static PropertyCollection DropOptions = getOptions();

        private static PropertyCollection getOptions() {

            PropertyCollection p = new PropertyCollection();
            p.Add( new PropertyItem( lang.get( "privacySelf" ), 0 ) );
            p.Add( new PropertyItem( lang.get( "privacyFriend" ), 1 ) );
            p.Add( new PropertyItem( lang.get( "privacyLoggedUser" ), 2 ) );
            p.Add( new PropertyItem( lang.get( "privacyEveryone" ), 3 ) );

            return p;
        }

    }

}
