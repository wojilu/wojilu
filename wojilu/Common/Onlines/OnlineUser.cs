/*
 * Copyright 2010 www.wojilu.com
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *      http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Text;

using wojilu.Data;
using wojilu.ORM;
using wojilu.Net;

namespace wojilu.Common.Onlines {

    /// <summary>
    /// 在线用户信息封装
    /// </summary>
    [NotSave]
    public class OnlineUser : CacheObject, IComparable {

        public int UserId { get; set; }
        public String UserName { get; set; }
        public String UserUrl { get; set; }
        public String UserPicUrl { get; set; }

        public String Role { get; set; }

        public int IsHidden { get; set; }

        public String Location { get; set; }
        public String Referrer { get; set; }
        public String Agent { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime LastActive { get; set; }

        public String Target { get; set; }

        public String Ip { get; set; }
        public String TrueIp { get; set; }

        public String GetIp( int hideLength ) {
            return IpUtil.GetIpWild( this.Ip, hideLength );
        }

        public int CompareTo( Object obj ) {
            OnlineUser target = obj as OnlineUser;
            if (target.LastActive > this.LastActive) return 1;
            return -1;
        }

    }
}

