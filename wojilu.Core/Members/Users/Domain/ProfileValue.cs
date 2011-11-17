/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */


using System;
using wojilu.ORM;

namespace wojilu.Members.Users.Domain {

    [Serializable]
    public class ProfileValue : ObjectBase<ProfileValue> {

        public int MemberId { get; set; }
        public int ProfileItemId { get; set; }

        public String SettingValue { get; set; }

    }
}

