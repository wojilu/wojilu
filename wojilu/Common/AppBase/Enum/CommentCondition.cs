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
namespace wojilu.Common.AppBase {
    
    /// <summary>
    /// 关于是否可评论的枚举状态(允许、关闭、登录用户、好友)
    /// </summary>
    public class CommentCondition {

        public static readonly int AllowAll = 0;
        public static readonly int Close = 1;
        public static readonly int LoginUser = 2;
        public static readonly int Friend = 3;

    }

}

