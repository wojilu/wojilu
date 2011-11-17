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
using wojilu.Members.Interface;
using wojilu.Common;
using wojilu.Members;

namespace wojilu.Web.Mvc {

    /// <summary>
    /// 成员路径处理工具
    /// </summary>
    public class MemberPath {

        private static Dictionary<String, String> _ownerTypeUrlMaps = getOwnerTypeAndUrlMaps();
        private static Dictionary<String, String> getOwnerTypeAndUrlMaps() {

            Dictionary<String, String> ownerTypeUrlMaps = new Dictionary<String, String>();

            foreach (KeyValuePair<Type, String> kv in MemberHelper.GetMemberTypes()) {
                ownerTypeUrlMaps.Add( kv.Key.Name.ToLower(), kv.Value );
            }

            return ownerTypeUrlMaps;
        }

        public static String GetPath( String memberType ) {
            try {
                return _ownerTypeUrlMaps[memberType.ToLower()];
            }
            catch (Exception) {
                throw new Exception( "key not found in _ownerTypeUrlMaps:" + memberType );
            }
        }

        public static String GetTypeByPath( String ownerTypeUrl ) {
            foreach (String key in _ownerTypeUrlMaps.Keys) {
                if (strUtil.EqualsIgnoreCase( _ownerTypeUrlMaps[key], ownerTypeUrl )) return key;
            }
            return null;
        }

        public static Boolean IsValidOwnerType( String ownerType ) {
            foreach (String key in _ownerTypeUrlMaps.Keys) {
                if (ownerType.Equals( _ownerTypeUrlMaps[key] )) return true;
            }
            return false;
        }

        //------------------------------------------------------------------------------------------------

        internal static OwnerInfo getOwnerInfo( String[] arrPathRow ) {

            if (arrPathRow.Length < 2) return null;

            foreach (String key in _ownerTypeUrlMaps.Keys) {

                if (strUtil.EqualsIgnoreCase( arrPathRow[0], _ownerTypeUrlMaps[key] )) {

                    OwnerInfo ownerInfo = new OwnerInfo();
                    ownerInfo.Owner = arrPathRow[1];
                    ownerInfo.OwnerType = key;
                    return ownerInfo;
                }

            }

            return null;
        }



    }
}
