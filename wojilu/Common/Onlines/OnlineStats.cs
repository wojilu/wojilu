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
using System.Collections.Generic;
using System.Text;

namespace wojilu.Common.Onlines {

    /// <summary>
    /// 在线状态数据
    /// </summary>
    public class OnlineStats {

        public static readonly OnlineStats Instance = new OnlineStats();
        private object objLock = new object();

        private OnlineStats() { }

        public int Count {
            get { return cdb.findAll<OnlineUser>().Count; }
        }

        public int GuestCount {
            get { return this.Count - this.MemberCount; }
        }

        private int _memberCount;

        public int MemberCount {
            get { return _memberCount; }
        }

        // 1) 新用户登录的时候(+1)
        // 2) 用户(+1)->游客(注销会-1)->用户(登录会+1)
        public void AddMemberCount() {

            lock (objLock) {
                _memberCount = _memberCount + 1;
            }
        }

        // 1) 注销的时候-1
        // 2) 用其他客户端，同一账号登录-1
        // 3) 超时，由系统自动清除-1
        public void SubtractMemberCount() {

            lock (objLock) {
                _memberCount = _memberCount - 1;
                if (_memberCount < 0) _memberCount = 0;
            }
        }

        public void ReCount() {

            lock (objLock) {

                _memberCount = OnlineService.GetLoggerUser().Count;

            }

        }


        public int MaxCount {
            get { return config.Instance.Site.MaxOnline; }
        }
        public DateTime MaxTime {
            get { return config.Instance.Site.MaxOnlineTime; }
        }



    }

}
