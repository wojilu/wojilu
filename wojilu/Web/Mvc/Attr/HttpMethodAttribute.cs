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

namespace wojilu.Web.Mvc.Attr {

    /// <summary>
    /// http 方法
    /// </summary>
    public interface IHttpMethod {
        String GetString();
    }

    /// <summary>
    /// 当前 action 只能接受 GET 方法
    /// </summary>
    [Serializable, AttributeUsage( AttributeTargets.Method )]
    public class HttpGetAttribute : Attribute, IHttpMethod {
        public String GetString() {
            return HttpMethod.Get;
        }
    }

    /// <summary>
    /// 当前 action 只能接受 POST 方法
    /// </summary>
    [Serializable, AttributeUsage( AttributeTargets.Method )]
    public class HttpPostAttribute : Attribute, IHttpMethod {
        public String GetString() {
            return HttpMethod.Post;
        }
    }

    /// <summary>
    /// 当前 action 只能接受 PUT 方法
    /// </summary>
    [Serializable, AttributeUsage( AttributeTargets.Method )]
    public class HttpPutAttribute : Attribute, IHttpMethod {
        public String GetString() {
            return HttpMethod.Put;
        }
    }

    /// <summary>
    /// 当前 action 只能接受 DELETE 方法
    /// </summary>
    [Serializable, AttributeUsage( AttributeTargets.Method )]
    public class HttpDeleteAttribute : Attribute, IHttpMethod {
        public String GetString() {
            return HttpMethod.Delete;
        }
    }



}
