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

namespace wojilu.Common.Security {

    /// <summary>
    /// 角色合并工具
    /// </summary>
    public class RoleMerger {

        private IList results = new ArrayList();
        private int iCount = 1;

        public RoleMerger() {
        }

        public RoleMerger( IList roles ) {
            this.Add( roles );
        }

        public RoleMerger Add( IList roles ) {

            foreach (IRole obj in roles) {

                RoleProxy rr = new RoleProxy();
                rr.Id = iCount;
                rr.Name = obj.Role.Name;
                rr.Role = obj.Role;

                results.Add( rr );
                iCount++;
            }

            return this;
        }

        public IList GetResults() {
            return results;
        }

    }

}
