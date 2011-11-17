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
using wojilu.Serialization;
using System.Text.RegularExpressions;
using System.Xml;

namespace wojilu.Net.Video {

    /// <summary>
    /// 凤凰视频
    /// </summary>
    public class IFengNewsSpider : IVideoSpider {

        private static readonly ILog logger = LogManager.GetLogger( typeof( IFengNewsSpider ) );

        public VideoInfo GetInfo( string playUrl ) {

            String[] arrItem = strUtil.TrimEnd( playUrl, ".shtml" ).Split( '/' );
            String flashId = arrItem[arrItem.Length - 1];

            VideoInfo vi = new VideoInfo();
            vi.PlayUrl = playUrl;
            vi.FlashId = flashId;
            vi.FlashUrl = string.Format( "http://v.ifeng.com/include/exterior.swf?guid={0}&AutoPlay=false", flashId );


            try {
                String pageBody = PageLoader.Download( playUrl, "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1)", "utf-8" );

                Match mt = Regex.Match( pageBody, "var videoinfo=({.+?});" );
                String strJson = mt.Groups[1].Value;

                Dictionary<String, Object> dic = JSON.ToDictionary( strJson );

                vi.PicUrl = dic.ContainsKey( "img" ) ? dic["img"].ToString() : "";
                vi.Title = dic.ContainsKey( "name" ) ? dic["name"].ToString() : "";


                return vi;
            }
            catch (Exception ex) {
                logger.Error( "getUrl=" + playUrl );
                logger.Error( ex.Message );
                return vi;
            }


        }

    }

    /// <summary>
    /// 凤凰视频
    /// </summary>
    public class IFendVideoSpider : IVideoSpider {

        private static readonly ILog logger = LogManager.GetLogger( typeof( IFendVideoSpider ) );

        public VideoInfo GetInfo( string playUrl ) {


            String[] arrItem = playUrl.Split( '#' );
            String flashId = arrItem[arrItem.Length - 1];

            VideoInfo vi = new VideoInfo();
            vi.PlayUrl = playUrl;
            vi.FlashId = flashId;
            vi.FlashUrl = string.Format( "http://v.ifeng.com/include/exterior.swf?guid={0}&AutoPlay=false", flashId );


            try {
                String pageBody = PageLoader.Download( playUrl, "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1)", "utf-8" );

                Match mt = Regex.Match( pageBody, "<li name=\"" + flashId + "\">(.+?)</li>", RegexOptions.Singleline );
                String strHtml = mt.Value.Replace( "'", "\"" );

                System.IO.StringReader strRd = new System.IO.StringReader( strHtml );
                XmlTextReader reader = new XmlTextReader( strRd );

                reader.MoveToContent();
                while (reader.Read()) {

                    if (equal( reader, "img" )) {
                        vi.PicUrl = reader.GetAttribute( "src" );
                    }
                    else if (equal( reader, "h4" )) {
                        vi.Title = reader.ReadString();
                    }

                }


                return vi;
            }
            catch (Exception ex) {
                logger.Error( "getUrl=" + playUrl );
                logger.Error( ex.Message );
                return vi;
            }


        }

        private static Boolean equal( XmlReader reader, String name ) {
            return reader.Name.ToLower().Equals( name );
        }

    }


}
