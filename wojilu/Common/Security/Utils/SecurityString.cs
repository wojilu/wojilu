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
using System.Collections;
using System.Text;
using wojilu.ORM;

namespace wojilu.Common.Security {

    /// <summary>
    /// 权限序列化后工具，格式：wojilu.Security.Domain.SiteRole:2:1,2,8,16,17/
    /// </summary>
    public class SecurityString {

        public static char roleSeperator = '/';
        public static char itemSeperator = ':';
        public static char actionIdSeperator = ',';

        public Type RoleType { get; set; }
        public String TypeFullName { get; set; }
        public int RoleId { get; set; }
        public int[] ActionIds { get; set; }

        public SecurityString( String typeFullName, int roleId, IList actions ) {

            Type roleType = Entity.GetType( typeFullName );
            if (roleType == null || !rft.IsInterface( roleType, typeof( IRole ) )) return;

            if (roleId < 0) return;
            if (actions == null) return;

            int[] aids = new int[actions.Count];
            for (int i = 0; i < actions.Count; i++) {
                aids[i] = ((ISecurityAction)actions[i]).Id;
            }

            this.RoleType = roleType;
            this.TypeFullName = typeFullName;
            this.RoleId = roleId;
            this.ActionIds = aids;

        }

        public SecurityString( String strOne ) {
            parse( strOne );
        }

        private void parse( String strOne ) {

            if (strUtil.IsNullOrEmpty( strOne )) return;
            strOne = strOne.TrimEnd( roleSeperator );
            String[] arrItem = strOne.Split( itemSeperator );
            if (arrItem.Length != 3) return;

            String typeFullName = arrItem[0].Trim();
            Type roleType = Entity.GetType( typeFullName );
            if (roleType == null || !rft.IsInterface( roleType, typeof( IRole ) )) return;

            int roleId = cvt.ToInt( arrItem[1].Trim() );
            if (roleId < 0) return;

            int[] Ids = cvt.ToIntArray( arrItem[2].Trim().Replace( '_', ',' ) );
            foreach (int intOne in Ids) {
                if (intOne <= 0) return;
            }

            this.RoleType = roleType;
            this.TypeFullName = typeFullName;
            this.RoleId = roleId;
            this.ActionIds = Ids;
        }

        public override String ToString() {
            if (strUtil.IsNullOrEmpty( this.TypeFullName )) return "";
            if (this.RoleId < 0) return "";
            if (this.ActionIds == null) return "";
            return String.Format( "{0}:{1}:{2}", this.TypeFullName, this.RoleId, cvt.ToString( ActionIds ).Replace( ',', '_' ) );
        }

        public String GetKey() {
            return GetRoleKey( this.TypeFullName, this.RoleId );
        }

        public static String GetRoleKey( String typeFullName, int roleId ) {
            return String.Format( "{0}_{1}", typeFullName, roleId );
        }

        public Boolean IsError() {
            return (this.RoleType == null);
        }

        public IList GetActions( IList actionAll ) {
            IList results = new ArrayList();
            foreach (ISecurityAction action in actionAll) {
                foreach (int actionId in ActionIds) {
                    if (action.Id == actionId) {
                        results.Add( action );
                        continue;
                    }
                }
            }
            return results;
        }

    }
}
