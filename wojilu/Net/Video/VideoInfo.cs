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

namespace wojilu.Net.Video {

    /// <summary>
    /// 视频信息
    /// </summary>
    public class VideoInfo {

        /// <summary>
        /// 视频播放页面的网址
        /// </summary>
        public String PlayUrl { get; set; }

        /// <summary>
        /// 视频flash本身的网址
        /// </summary>
        public String FlashUrl { get; set; }

        /// <summary>
        /// 视频截图的网址
        /// </summary>
        public String PicUrl { get; set; }

        /// <summary>
        /// 视频的ID
        /// </summary>
        public String FlashId { get; set; }

        public String Title { get; set; }

    }

}
