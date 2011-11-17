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

namespace wojilu.Web.Mvc.Attr {

    /// <summary>
    /// 指明当前控制器所属的 app
    /// </summary>
    [Serializable, AttributeUsage( AttributeTargets.Class )]
    public class AppAttribute : Attribute {

        private Type _appType;

        public AppAttribute( Type appType ) {
            _appType = appType;
        }

        public Type AppType {
            get { return _appType; }
            set { _appType = value; }
        }

    }
}

