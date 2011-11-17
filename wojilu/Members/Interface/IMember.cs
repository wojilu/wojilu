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
using wojilu.Common.Security;


namespace wojilu.Members.Interface {

    /// <summary>
    /// IMember 接口，可以是网站、用户或群组等
    /// </summary>
    public interface IMember {

        int Id { get; set; }
        String Name { get; set; }
        String Url { get; set; }
        int TemplateId { get; set; }
        int Status { get; set; }
        DateTime Created { get; set; }

        IList GetRoles();
        IRole GetAdminRole();

        IRole GetUserRole( IMember user );

        String GetUrl();


    }
}

