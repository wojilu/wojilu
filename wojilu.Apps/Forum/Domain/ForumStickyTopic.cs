/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Common.AppBase;

namespace wojilu.Apps.Forum.Domain {

    [Serializable]
    public class StickyTopic : IComparable, ISort {

        public int Id { get; set; }
        public String Title { get; set; }
        public int OrderId { get; set; }

        public String CreatorName { get; set; }
        public String CreatorUrl { get; set; }
        public String Created { get; set; }

        public int Replies { get; set; }
        public int Views { get; set; }

        public String LastUserName { get; set; }
        public String LastUserUrl { get; set; }
        public String LastUpdated { get; set; }

        public int CompareTo( object obj ) {
            StickyTopic target = (StickyTopic)obj;
            return target.OrderId >= this.OrderId ? 1 : -1;
        }

        public void updateOrderId() {
        }

        //------------------------------ add 增加置顶的帖子-------------------------------------------------------------------

        public static String MergeData( String json, List<ForumTopic> newTopics ) {

            List<StickyTopic> stickyList = GetTopics( json );
            List<StickyTopic> results = mergeData( stickyList, newTopics );

            return Json.ToStringList( results );
        }

        private static List<StickyTopic> mergeData( List<StickyTopic> stickyList, List<ForumTopic> newTopics ) {
            // 已有的不加，没有的再加上

            List<StickyTopic> results = new List<StickyTopic>();
            results.AddRange( stickyList );

            foreach (ForumTopic t in newTopics) {
                if (!isTopicFound( stickyList, t )) {
                    StickyTopic topic = getStickyTopic( t );
                    results.Add( topic );
                }
            }

            return results;

        }


        private static Boolean isTopicFound( List<StickyTopic> stickyList, ForumTopic ft ) {

            foreach (StickyTopic t in stickyList) {
                if (t.Id == ft.Id) return true;
            }
            return false;
        }



        //---------------------------------------- 减少置顶的帖子 ---------------------------------------------------------

        public static void DeleteTopic( int appId, String ids ) {


            ForumApp app = ForumApp.findById( appId );
            if (app == null) return;
            List<StickyTopic> stickyList = GetTopics( app.StickyTopic );
            List<StickyTopic> list = new List<StickyTopic>();
            Boolean shouldDelete = false;

            int[] arrIds = cvt.ToIntArray( ids );

            foreach (StickyTopic x in stickyList) {
                if (containsTopicId( arrIds, x.Id )) {
                    shouldDelete = true;
                    continue;
                }
                list.Add( x );
            }

            if (shouldDelete) {
                String newJson = Json.ToStringList( list );
                app.StickyTopic = newJson;
                app.update();
            }
        }

        private static bool containsTopicId( int[] arrIds, int topicId ) {

            foreach (int id in arrIds) {
                if (id == topicId) return true;
            }

            return false;
        }

        public static void DeleteTopic( ForumTopic topic ) {

            ForumApp app = ForumApp.findById( topic.AppId );
            if (app == null) return;

            List<StickyTopic> stickyList = GetTopics( app.StickyTopic );
            List<StickyTopic> list = new List<StickyTopic>();
            Boolean shouldDelete = false;
            foreach (StickyTopic x in stickyList) {
                if (x.Id == topic.Id) {
                    shouldDelete = true;
                    continue;
                }
                list.Add( x );
            }

            if (shouldDelete) {
                String newJson = Json.ToStringList( list );
                app.StickyTopic = newJson;
                app.update();
            }
        }


        public static String SubtractData( String json, List<ForumTopic> newStickTopics ) {

            List<StickyTopic> stickyList = GetTopics( json );
            List<StickyTopic> results = subtractData( stickyList, newStickTopics );

            return Json.ToStringList( results );
        }

        private static List<StickyTopic> subtractData( List<StickyTopic> stickyList, List<ForumTopic> newStickTopics ) {

            List<StickyTopic> results = new List<StickyTopic>();

            foreach (StickyTopic st in stickyList) {
                if (needRemove( newStickTopics, st )) continue;
                results.Add( st );
            }

            return results;
        }

        private static Boolean needRemove( List<ForumTopic> newStickTopics, StickyTopic st ) {
            foreach (ForumTopic ft in newStickTopics) {
                if (ft.Id == st.Id) return true;
            }
            return false;
        }

        //-------------------------------------------------------------------------------------------------

        public static List<ForumTopic> GetForumTopic( String json, ForumApp app ) {

            List<StickyTopic> stickyList = GetTopics( json );

            List<ForumTopic> results = new List<ForumTopic>();
            foreach (StickyTopic st in stickyList) {
                ForumTopic topic = getTopicBySticky( st, app.OwnerId, app.OwnerType, app.OwnerUrl );
                results.Add( topic );
            }

            return results;
        }
        //-------------------------------------------------------------------------------------------------

        private static ForumTopic getTopicBySticky( StickyTopic st, int ownerId, String ownerType, String ownerUrl ) {

            ForumTopic topic = new ForumTopic();
            topic.Id = st.Id;
            topic.Title = st.Title;

            topic.Creator = new wojilu.Members.Users.Domain.User() { Name = st.CreatorName, Url = st.CreatorUrl };
            topic.CreatorUrl = st.CreatorUrl;
            topic.Created = cvt.ToTime( st.Created );

            topic.Replies = st.Replies;
            topic.Hits = st.Views;

            topic.Replied = cvt.ToTime( st.LastUpdated );
            topic.RepliedUserName = st.LastUserName;
            topic.RepliedUserFriendUrl = st.LastUserUrl;

            topic.OwnerId = ownerId;
            topic.OwnerType = ownerType;
            topic.OwnerUrl = ownerUrl;

            topic.IsGlobalSticky = true;

            return topic;
        }


        private static StickyTopic getStickyTopic( ForumTopic ft ) {

            StickyTopic t = new StickyTopic();

            t.Id = ft.Id;
            t.Title = ft.Title;
            t.CreatorName = ft.Creator.Name;
            t.CreatorUrl = ft.CreatorUrl;
            t.Created = ft.Created.ToString();
            t.Replies = ft.Replies;
            t.Views = ft.Hits;
            t.LastUserName = ft.RepliedUserName;
            t.LastUserUrl = ft.RepliedUserFriendUrl;
            t.LastUpdated = ft.Replied.ToString();

            return t;
        }

        private static List<StickyTopic> GetTopics( String json ) {

            List<StickyTopic> results = Json.DeserializeList<StickyTopic>( json );
            results.Sort();

            return results;

        }


        //-------------------------------------------------------------------------------------------------


        public static String MoveUp( String json, int topicId ) {

            List<StickyTopic> topics = GetTopics( json );
            StickyTopic t = getById( topics, topicId );
            SortUtil<StickyTopic> s = new SortUtil<StickyTopic>( t, topics );
            s.MoveUp();
            return Json.ToStringList( s.GetOrderedList() );

        }

        public static String MoveDown( String json, int topicId ) {
            List<StickyTopic> topics = GetTopics( json );
            StickyTopic t = getById( topics, topicId );
            SortUtil<StickyTopic> s = new SortUtil<StickyTopic>( t, topics );
            s.MoveDown();
            return Json.ToStringList( s.GetOrderedList() );
        }

        private static StickyTopic getById( List<StickyTopic> topics, int topicId ) {
            foreach (StickyTopic t in topics) {
                if (t.Id == topicId) return t;
            }
            return null;
        }






    }

}
