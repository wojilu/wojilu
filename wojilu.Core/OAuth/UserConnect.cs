/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Members.Users.Domain;
using wojilu.ORM;

namespace wojilu.OAuth {

    public class UserConnect : ObjectBase<UserConnect> {

        public User User { get; set; }
        public String ConnectType { get; set; }

        public String Uid { get; set; } // 用户在第三方平台的唯一ID标识
        public String Name { get; set; } // 用户在第三方平台的名称

        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public int ExpiresIn { get; set; }

        public String Scope { get; set; }

        public DateTime Created { get; set; }

        // AccessToken 更新时间
        public DateTime Updated { get; set; }

        // 禁止同步，默认值0表示不禁止(=允许同步)
        public int NoSync { get; set; }

        [NotSave]
        public bool IsExpired {
            get {
                if (this.Updated == null) return true;
                return (DateTime.Now - this.Updated).TotalSeconds > ExpiresIn;
            }
        }


    }

}
