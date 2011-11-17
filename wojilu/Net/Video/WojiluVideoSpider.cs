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
using wojilu.Net.Video;

namespace wojilu.Net.Video {

    /// <summary>
    /// 默认视频抓取器，可根据播放页网址，自动返回相应网站的视频信息
    /// </summary>
    public class WojiluVideoSpider : IVideoSpider {

        public VideoInfo GetInfo( String url ) {
            return getVideoSpider( url ).GetInfo( url );
        }

        private IVideoSpider getVideoSpider( String url ) {

            if (url.IndexOf( "youku.com" ) >= 0) return new YoukuSpider();
            if (url.IndexOf( "tudou.com" ) >= 0) return new TudouSpider();
            if (url.IndexOf( "ku6.com" ) >= 0) return new Ku6Spider();
            if (url.IndexOf( "56.com" ) >= 0) return new WuliuSpider();
            if (url.IndexOf( "video.sina.com.cn" ) >= 0) return new SinaSpider();

            if (url.IndexOf( "v.ifeng.com" ) >= 0) {
                if (url.IndexOf( "#" ) > 0) {
                    return new IFendVideoSpider();
                }
                else {
                    return new IFengNewsSpider();
                }
            }

            return new NullSpider();
        }

    }

}
