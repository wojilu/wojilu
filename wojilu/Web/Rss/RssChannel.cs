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
using System.IO;
using System.Web;
using System.Xml;

using wojilu;
using System.Text;
using System.Collections.Generic;

namespace wojilu.Web {

    /// <summary>
    /// rss 数据抓取工具
    /// </summary>
    public class RssChannel {

        public RssChannel() {
            this.Generator = "http://www.wojilu.com/rss";
            this.RssItems = new RssItemList();
            this.Ttl = 60;
        }

        public String Title { get; set; }
        public String Link { get; set; }
        public String Description { get; set; }
        public String Generator { get; set; }
        public String Language { get; set; }
        public DateTime LastBuildDate { get; set; }
        public int Ttl { get; set; }
        public DateTime PubDate { get; set; }

        public RssImage RssImage { get; set; }
        public RssItemList RssItems { get; set; }


        public static RssChannel Create( String rssUrl ) {
            RssChannel feed = new RssChannel();
            RssImage img = new RssImage();
            XmlTextReader reader = new XmlTextReader( rssUrl );
            Boolean isNewItem = false;
            Boolean isImage = false;
            RssItem rssItem = new RssItem();
            reader.MoveToContent();
            while (reader.Read()) {
                if (!reader.Name.Equals( "channel" ) && ((reader.NodeType != XmlNodeType.Whitespace) && (reader.NodeType != XmlNodeType.Comment))) {
                    if (((reader.NodeType == XmlNodeType.EndElement) && reader.Name.Equals( "item" )) && strUtil.HasText( rssItem.Title )) {
                        feed.RssItems.Add( rssItem );
                        rssItem = new RssItem();
                        isNewItem = false;
                    }
                    else {
                        if ((reader.NodeType == XmlNodeType.EndElement) && reader.Name.Equals( "image" )) {
                            isImage = false;
                            continue;
                        }
                        if (equal( reader, "image" )) {
                            isImage = true;
                            isNewItem = false;
                            continue;
                        }
                        if (equal( reader, "item" )) {
                            isNewItem = true;
                            isImage = false;
                            continue;
                        }
                        if (reader.NodeType == XmlNodeType.Element) {
                            if (!(isNewItem || isImage)) {
                                readRssChannel( feed, reader );
                            }
                            else if (isNewItem) {
                                readRssItem( rssItem, reader );
                            }
                            else if (isImage) {
                                readRssImage( img, reader );
                            }
                        }
                    }
                }
            }
            feed.RssImage = img;
            reader.Close();
            return feed;
        }


        public String GetRenderContent() {
            RssChannel channel = this;

            StringBuilder sb = new StringBuilder();
            sb.Append( "<?xml version=\"1.0\" encoding=\"utf-8\" standalone=\"yes\"?>" );
            sb.Append( "<rss version=\"2.0\" xmlns:dc=\"http://purl.org/dc/elements/1.1/\">" );
            sb.Append( "<channel>" );
            sb.AppendFormat( "<title>{0}</title>", channel.Title );
            sb.AppendFormat( "<link>{0}</link>", channel.Link );
            sb.AppendFormat( "<description>{0}</description>", channel.Description );
            sb.AppendFormat( "<language>{0}</language>", channel.Language );
            sb.AppendFormat( "<pubDate>{0}</pubDate>", channel.PubDate.ToUniversalTime().ToString( "r" ) );
            sb.AppendFormat( "<lastBuildDate>{0}</lastBuildDate>", channel.LastBuildDate.ToUniversalTime().ToString( "r" ) );
            sb.AppendFormat( "<generator>{0}</generator>", channel.Generator );
            sb.AppendFormat( "<ttl>{0}</ttl>", channel.Ttl );
            for (int i = 0; i < this.RssItems.Count; i++) {
                RssItem item = this.RssItems[i];
                sb.Append( "<item>" );

                sb.AppendFormat( "<title>{0}</title>", item.Title );
                sb.AppendFormat( "<link>{0}</link>", item.Link );
                sb.AppendFormat( "<description>{0}</description>", item.DescriptionCDATA );
                sb.AppendFormat( "<author>{0}</author>", item.Author );
                sb.AppendFormat( "<pubDate>{0}</pubDate>", item.PubDate.ToUniversalTime().ToString( "r" ) );
                sb.AppendFormat( "<category>{0}</category>", item.Category );

                sb.Append( "</item>" );

            }

            sb.Append( "</channel>" );

            sb.Append( "</rss>" );
            return sb.ToString();
        }


        private static DateTime parseTime( String timeString ) {
            timeString = timeString.Replace( " GMT+8", "" );
            DateTime now = DateTime.Now;
            try {
                now = DateTime.Parse( timeString );
            }
            catch {
            }
            return now;
        }

        private static void readRssChannel( RssChannel feed, XmlReader reader ) {
            if (equal( reader, "title" )) {
                feed.Title = reader.ReadString();
            }
            else if (equal( reader, "link" )) {
                feed.Link = reader.ReadString();
            }
            else if (equal( reader, "description" )) {
                feed.Description = reader.ReadString();
            }
            else if (equal( reader, "pubdate" )) {
                feed.PubDate = parseTime( reader.ReadString() );
            }
            else if (equal( reader, "language" )) {
                feed.Language = reader.ReadString();
            }
            else if (equal( reader, "generator" )) {
                feed.Generator = reader.ReadString();
            }
            else if (equal( reader, "lastbuilddate" )) {
                feed.LastBuildDate = parseTime( reader.ReadString() );
            }
        }

        private static Boolean equal( XmlReader reader, String name ) {
            return reader.Name.ToLower().Equals( name );
        }

        private static void readRssImage( RssImage img, XmlReader reader ) {
            if (equal( reader, "title" )) {
                img.Title = reader.ReadString();
            }
            else if (equal( reader, "url" )) {
                img.Url = reader.ReadString();
            }
            else if (equal( reader, "link" )) {
                img.Link = reader.ReadString();
            }
            else if (equal( reader, "description" )) {
                img.Description = reader.ReadString();
            }
            else if (equal( reader, "width" )) {
                img.Width = cvt.ToInt( reader.ReadString() );
            }
            else if (equal( reader, "height" )) {
                img.Height = cvt.ToInt( reader.ReadString() );
            }
        }

        private static void readRssItem( RssItem item, XmlReader reader ) {
            if (equal( reader, "title" )) {
                item.Title = reader.ReadString();
            }
            else if (equal( reader, "link" )) {
                item.Link = reader.ReadString();
            }
            else if (equal( reader, "content:encoded" )) {
                item.Description = reader.ReadString();
            }
            else if (equal( reader, "description" ) && strUtil.IsNullOrEmpty( item.Description )) {
                item.Description = reader.ReadString();
            }
            else if (equal( reader, "pubdate" )) {
                item.PubDate = parseTime( reader.ReadString() );
            }
            else if (equal( reader, "author" )) {
                item.Author = reader.ReadString();
            }
            else if (equal( reader, "category" )) {
                item.Category = reader.ReadString();
            }
        }

        //public void RendFeed() {
        //    String content = this.GetRenderContent();
        //    webContext.RenderXml( content );
        //}

    }
}

