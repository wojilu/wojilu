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
using wojilu.Web.Context;
using wojilu.Web;
using wojilu.Web.Mvc;

namespace wojilu.Common.Onlines {


    /// <summary>
    /// 提供在线状态的各类数据
    /// </summary>
    public class OnlineService {

        public static List<OnlineUser> GetAll() {

            List<OnlineUser> all = cdb.findAll<OnlineUser>();
            all.Sort();
            return all;
        }

        public static List<OnlineUser> GetGuest() {

            List<OnlineUser> all = cdb.findAll<OnlineUser>();

            List<OnlineUser> allUsers = new List<OnlineUser>();
            foreach (OnlineUser info in all) {
                if (info.UserId <= 0) allUsers.Add( info );
            }

            allUsers.Sort();

            return allUsers;
        }

        public static List<OnlineUser> GetLoggerUser() {

            List<OnlineUser> all = cdb.findAll<OnlineUser>();

            List<OnlineUser> allUsers = new List<OnlineUser>();
            foreach (OnlineUser info in all) {
                if (info.UserId > 0) allUsers.Add( info );
            }

            allUsers.Sort();

            return allUsers;
        }

        /// <summary>
        /// 最新登录用户
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public static List<OnlineUser> GetRecent( int count ) {

            List<OnlineUser> all = cdb.findAll<OnlineUser>();

            int icount = 1;
            List<OnlineUser> results = new List<OnlineUser>();
            foreach (OnlineUser info in all) {
                if (icount > count) break;
                if (info.UserId > 0) {
                    results.Add( info );
                    icount++;
                }
            }

            results.Sort();

            return results;
        }

        /// <summary>
        /// 最新所有用户
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public static List<OnlineUser> GetRecentAll( int count ) {

            List<OnlineUser> all = cdb.findAll<OnlineUser>();

            int icount = 1;
            List<OnlineUser> results = new List<OnlineUser>();
            foreach (OnlineUser info in all) {
                if (icount > count) break;

                results.Add( info );
                icount++;
            }

            results.Sort();

            return results;
        }



    }

}
