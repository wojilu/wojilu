/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web;

using wojilu.Web.Mvc;
using wojilu.Common.AppBase.Interface;
using wojilu.Members.Users.Domain;
using wojilu.Members.Sites.Domain;
using wojilu.Web;

namespace wojilu.Common.Tags {

    public class TagService {

        public static IList FindByTags( String tagList, Type dataType, int count ) {
            List<Tag> tags = getTagListByStr( tagList );
            if (tags.Count == 0) return new ArrayList();
            String tagIds = "";
            foreach (Tag t in tags) tagIds += t.Id + ",";
            tagIds = tagIds.TrimEnd( ',' );

            List<DataTagShip> list = db.find<DataTagShip>( "Tag.Id in (" + tagIds + ") and TypeFullName=:dtype" )
    .set( "dtype", dataType.FullName )
    .list( count );

            StringBuilder builder = new StringBuilder();
            foreach (DataTagShip ship in list) {
                builder.Append( ship.DataId );
                builder.Append( "," );
            }
            String str = builder.ToString().TrimEnd( ',' );
            return ndb.find( dataType, "Id in (" + str + ")" ).list();

        }

        public static IList Find( String tagName, Type dataType, int count ) {

            Tag tag = GetTag( tagName );
            List<DataTagShip> list = db.find<DataTagShip>( "Tag.Id=" + tag.Id + " and TypeFullName=:dtype" )
                .set( "dtype", dataType.FullName )
                .list( count );

            StringBuilder builder = new StringBuilder();
            foreach (DataTagShip ship in list) {
                builder.Append( ship.DataId );
                builder.Append( "," );
            }
            String str = builder.ToString().TrimEnd( ',' );
            return ndb.find( dataType, "Id in (" + str + ")" ).list();
        }

        public static IList FindPage( String tagName ) {
            return null;
        }

        private static Tag findTag( String tagName, IAppData data ) {
            Tag tag = GetTag( tagName );
            if (tag == null) {
                tag = insertTag( tagName, data.Creator.Id );
            }
            return tag;
        }

        private static String linkTag( String tagName ) {
            return strUtil.Join( SystemInfo.SiteRoot, "tag/" + tagName ) + MvcConfig.Instance.UrlExt;
        }

        public static String GetHtml( List<Tag> tagList ) {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < tagList.Count; i++) {

                Tag tag = tagList[i];

                builder.Append( "<a href=\"" );
                builder.Append( linkTag( tag.Name ) );
                builder.Append( "\">" );
                builder.Append( tag.Name );
                builder.Append( "</a>" );

                if (i < tagList.Count - 1) builder.Append( ", " );

            }
            return builder.ToString().Trim();
        }

        public static Tag GetTag( String tagName ) {
            return db.find<Tag>( "Tag.Name=:name" ).set( "name", tagName ).first();
        }

        /// <summary>
        /// 将tag列表的原始字符串中不标准的分隔符去掉，替换成标准的英文逗号
        /// </summary>
        /// <param name="rawTagStr"></param>
        /// <returns></returns>
        public static String ResetRawTagString( String rawTagStr ) {

            String[] arrTag = GetTags( rawTagStr );
            String str = "";
            for (int i = 0; i < arrTag.Length; i++) {
                str += arrTag[i];
                if (i < arrTag.Length - 1) str += ",";
            }
            return str;
        }

        /// <summary>
        /// tag原始字符分割成若干个tag，tag之间用空格或逗号分开。如果tag中要使用空格，请用逗号分隔；或者使用引号括起来，比如 "win 7"
        /// </summary>
        /// <param name="tagString"></param>
        /// <returns></returns>
        public static String[] GetTags( String tagString ) {
            try {
                return splitTags( tagString );
            }
            catch {
                return new String[] { };
            }
        }

        private static String[] splitTags( String tagString ) {

            if (strUtil.IsNullOrEmpty( tagString )) return new String[] { };

            int current = 0;
            List<String> list = new List<String>();
            String word = "";
            char separator = getSeparator( tagString );

            char charQuote1 = '"';
            char charQuote2 = '\'';
            Boolean quoteBegin = false;

            while (current < tagString.Length) {

                if (tagString[current] == charQuote1 || tagString[current] == charQuote2) {
                    if (quoteBegin) {
                        word = addWord( list, word );
                        quoteBegin = false;
                    }
                    else {
                        quoteBegin = true;
                    }
                }
                else if (quoteBegin) {
                    word += tagString[current];
                }
                else if (current == tagString.Length - 1) {
                    word += tagString[current];
                    word = addWord( list, word );
                }
                else if (tagString[current] == separator) {
                    word = addWord( list, word );
                }
                else {
                    word += tagString[current];
                }

                current = current + 1;
            }

            return list.ToArray();
        }

        private static char getSeparator( String tagString ) {
            if (tagString.IndexOf( ',' ) >= 0) return ',';
            if (tagString.IndexOf( '，' ) >= 0) return '，';
            if (tagString.IndexOf( '/' ) >= 0) return '/';
            if (tagString.IndexOf( '|' ) >= 0) return '|';
            if (tagString.IndexOf( '、' ) >= 0) return '、';
            return ' ';
        }

        private static String addWord( List<String> list, String word ) {
            if (word.Trim().Length > 0) {
                list.Add( word.Trim() );
                word = "";
            }
            return word;
        }

        public static List<Tag> getTagListByStr( String tagListStr ) {
            List<Tag> tags = new List<Tag>();
            if (strUtil.IsNullOrEmpty( tagListStr )) return tags;

            string[] strArray = GetTags( tagListStr );
            foreach (String str in strArray) {

                String allowedTag = strUtil.SubString( str, config.Instance.Site.TagLength );

                Tag tag = GetTag( allowedTag );
                if (tag != null)
                    tags.Add( tag );

            }

            return tags;
        }

        public static String GetText( List<Tag> tagList ) {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < tagList.Count; i++) {
                builder.Append( tagList[i].Name );
                if (i < tagList.Count - 1)
                    builder.Append( ", " );
            }
            return builder.ToString().Trim();
        }

        private static Tag insertTag( String tagName, int memberId ) {
            Tag tag = new Tag();
            tag.Name = tagName;
            tag.CreatorId = memberId;
            db.insert( tag );
            return tag;
        }

        public static Boolean SaveDataTag( IAppData data, List<String> tags ) {
            if (tags.Count == 0) return false;

            String strTags = "";
            for (int i = 0; i < tags.Count; i++) {
                strTags += tags[i];
                if (i < tags.Count - 1) strTags += ",";
            }

            return SaveDataTag( data, strTags );
        }

        /// <summary>
        /// 保存某个对象的所有tag
        /// </summary>
        /// <param name="data">必须是实现了 IAppData 接口的对象</param>
        /// <param name="tagString">以逗号分隔的tag原始字符串</param>
        /// <returns></returns>
        public static Boolean SaveDataTag( IAppData data, String tagString ) {
            if (strUtil.IsNullOrEmpty( tagString )) {
                return false;
            }
            clearDataTags( data );
            string[] strArray = GetTags( tagString );
            List<Tag> tags = new List<Tag>();
            foreach (String str in strArray) {

                String rawTag = strUtil.SubString( str, config.Instance.Site.TagLength );

                if (isAllowed( rawTag ) == false) continue;

                Tag tag = findTag( rawTag, data );
                tags.Add( tag );

                joinTag_Data( tag, data );
                joinTag_Member( tag, data.Creator );
            }

            recountTagDatas( tags );

            return true;
        }

        private static Boolean isAllowed( string rawTag ) {

            String[] arrKeys = config.Instance.Site.BadWords;
            foreach (String key in arrKeys) {
                if (strUtil.EqualsIgnoreCase( key, rawTag )) return false;
            }
            return true;
        }

        private static void recountTagDatas( List<Tag> tags ) {

            foreach (Tag tag in tags) {

                int count = DataTagShip.find( "Tag.Id=" + tag.Id ).count();
                tag.DataCount = count;
                tag.update( "DataCount" );

            }

        }

        private static void joinTag_Data( Tag tag, IAppData data ) {
            if (DataTagShip.find( "DataId=:dataId and Tag.Id=:tagId and TypeFullName=:dataType" )
                .set( "dataId", data.Id )
                .set( "tagId", tag.Id )
                .set( "dataType", data.GetType().FullName )
                .count() == 0) {

                DataTagShip ship = new DataTagShip();
                ship.DataId = data.Id;
                ship.TypeFullName = data.GetType().FullName;
                ship.Tag = tag;
                db.insert( ship );
            }
        }

        private static void joinTag_Member( Tag tag, User member ) {
            if (MemberTagShip.find( "Member.Id=:memberId and Tag.Id=:tagId" )
                .set( "memberId", member.Id )
                .set( "tagId", tag.Id )
                .count() == 0) {
                MemberTagShip ship = new MemberTagShip();
                ship.Member = member;
                ship.Tag = tag;
                db.insert( ship );
            }
        }

        private static void clearDataTags( IAppData data ) {
            List<DataTagShip> list = DataTagShip.find( "DataId=:dataId and TypeFullName=:dataType" )
                .set( "dataId", data.Id )
                .set( "dataType", data.GetType().FullName )
                .list();

            foreach (DataTagShip ship in list) {
                db.delete( ship );
            }
        }

        public static void DeleteTag( int tagId ) {

            Tag tag = Tag.findById( tagId );
            if (tag == null) return;

            DataTagShip.deleteBatch( "TagId=" + tagId );
            tag.delete();
        }

        public static void DeleteDataTag( int dataTagId ) {

            DataTagShip dt = DataTagShip.findById( dataTagId );
            if (dt == null) return;

            int tagId = dt.Tag == null ? 0 : dt.Tag.Id;
            dt.delete();

            if (tagId == 0) return;
            int count = DataTagShip.count( "TagId=" + tagId );

            Tag tag = Tag.findById( tagId );
            if (tag == null) return;
            tag.DataCount = count;
            tag.update();

        }

        public static Boolean tagEqual( String tag1, String tag2 ) {

            if (strUtil.IsNullOrEmpty( tag1 )) return strUtil.IsNullOrEmpty( tag2 );

            List<String> listTag1 = addList( tag1 );
            List<String> listTag2 = addList( tag2 );

            if (listTag1.Count != listTag2.Count) return false;

            foreach (String tag in listTag1) {
                if (listTag2.Contains( tag ) == false) return false;
            }

            return true;
        }

        private static List<String> addList( string tagStr ) {

            String[] arrTag = GetTags( tagStr );

            List<String> listTag = new List<String>();
            foreach (String tag in arrTag) {
                if (strUtil.IsNullOrEmpty( tag )) continue;
                listTag.Add( tag.Trim() );
            }

            return listTag;
        }


        public static List<Tag> GetHotTags( int count ) {
            return Tag.find( "order by DataCount desc, Id desc" ).list( count );
        }


    }
}

