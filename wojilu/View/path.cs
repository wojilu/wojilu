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
using wojilu.Web.Context;

namespace wojilu.View {

    /// <summary>
    /// 常用数据的路径信息
    /// </summary>
    public class path {

        /// <summary>
        /// 获取上传图片的缩略图。缩略图类型参看 site.config 中的 PhotoThumb 配置项
        /// </summary>
        /// <param name="picPath"></param>
        /// <param name="thumbType">缩略图类型参看 site.config 中的 PhotoThumb 配置项</param>
        /// <returns></returns>
        public static String pic( String picPath, String thumbType ) { return null; }

        /// <summary>
        /// 获取上传头像的缩略图。缩略图类型参看 site.config 中的 AvatarThumb 配置项
        /// </summary>
        /// <param name="facePath"></param>
        /// <param name="thumbType">缩略图类型参看 site.config 中的 AvatarThumb 配置项</param>
        /// <returns></returns>
        public static String face( String facePath, String thumbType ) { return null; }

        /// <summary>
        /// 获取文件在服务器上的绝对路径
        /// </summary>
        /// <param name="relativePath"></param>
        /// <returns></returns>
        public static String map( String relativePath ) { return null; }
    }


}
