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
using System.Text.RegularExpressions;
using wojilu.Serialization;

namespace wojilu.Net.Video {

    /// <summary>
    /// 新浪播客
    /// </summary>
    public class SinaSpider : IVideoSpider {

        private static readonly ILog logger = LogManager.GetLogger( typeof( SinaSpider ) );

        public VideoInfo GetInfo( string playUrl ) {

            VideoInfo vi = new VideoInfo();
            vi.PlayUrl = playUrl;

            try {

                String pageBody = PageLoader.Download( playUrl );

                Match mt = Regex.Match( pageBody, "video : {(" + "." + "+?)\\}[^,]", RegexOptions.Singleline );
                String strJson = "{" + mt.Groups[1].Value + "}";

                Dictionary<String, Object> dic = JSON.ToDictionary( strJson );

                vi.PicUrl = dic.ContainsKey( "pic" ) ? dic["pic"].ToString() : "";
                vi.FlashUrl = dic.ContainsKey( "swfOutsideUrl" ) ? dic["swfOutsideUrl"].ToString() : "";
                vi.Title = dic.ContainsKey( "title" ) ? dic["title"].ToString() : "";

                return vi;

            }
            catch (Exception ex) {

                logger.Error( "getUrl=" + playUrl );
                logger.Error( ex.Message );

                return vi;
            }




        }

    }

}
