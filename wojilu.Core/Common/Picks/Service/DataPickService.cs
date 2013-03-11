/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using wojilu.ORM;
using wojilu.Web.Mvc;

using wojilu.Common.AppBase.Interface;
using wojilu.Members.Users.Domain;

namespace wojilu.Common.Picks {


    public class DataPickService<TData, TPick> where TData : IAppData where TPick : DataPickBase, new() {

        // 返回所有经过“编辑、删除、固定”的帖子
        public List<MergedData> GetAll( IList allTopicList, int appId ) {

            List<TData> allTopics = allTopicList as List<TData>;

            List<TPick> customList = getCustomList( appId );

            // 获取尚未删除的帖子
            List<TData> topics = GetUnDeletedTopic( allTopics, customList );

            // 获取固定的帖子
            List<TPick> pinList = getPinList( customList );

            return getMergedDatas( topics, pinList, customList );
        }

        private List<MergedData> getMergedDatas( List<TData> topics, List<TPick> pinList, List<TPick> customList ) {

            List<MergedData> mergedList = new List<MergedData>();
            int iCount = 13;
            for (int i = 0; i < iCount; i++) {

                MergedData mpost = getPinedPost_Merged( pinList, i );
                if (mpost != null) {
                    mergedList.Add( mpost );
                    continue;
                }

                MergedData tpost = getNextTopic_Merged( topics, mergedList, customList );
                if (tpost != null) {
                    mergedList.Add( tpost );
                }

            }

            return mergedList;
        }

        private MergedData getPinedPost_Merged( List<TPick> pinList, int i ) {
            foreach (TPick x in pinList) {
                if (x.PinIndex == i + 1) {
                    MergedData mPost = new MergedData();
                    mPost.Title = x.Title;
                    mPost.Link = x.Link;
                    mPost.Summary = x.Summary;
                    return mPost;
                }
            }
            return null;
        }

        // 取出尚未添加的第一个帖子
        private MergedData getNextTopic_Merged( List<TData> topics, List<MergedData> mergedList, List<TPick> customList ) {
            foreach (TData x in topics) {
                if (hasAdded( mergedList, x ) == false) {

                    MergedData mPost = new MergedData();
                    mPost.Topic = x;

                    // 是否编辑过？
                    TPick editPost = getEditPost( x, customList );
                    if (editPost != null) {
                        mPost.Title = editPost.Title;
                        mPost.Link = editPost.Link;
                        mPost.Summary = editPost.Summary;
                        mPost.IsEdit = 1;
                    }
                    else {
                        mPost.Title = x.Title;
                        mPost.Link = alink.ToAppData( x );
                        mPost.Summary = getSummary( x );
                    }

                    return mPost;
                }
            }
            return null;
        }

        private string getSummary( TData x ) {
            IEntity obj = x as IEntity;
            String summary = getSummary( obj );
            return strUtil.ParseHtml( summary, 150 );
        }

        public string getSummary( IEntity obj ) {

            EntityInfo ei = Entity.GetInfo( obj );
            if (ei == null) return null;

            if (ei.GetProperty( "Content" ) != null) return obj.get( "Content" ).ToString();
            if (ei.GetProperty( "Summary" ) != null) return obj.get( "Summary" ).ToString();
            return null;
        }

        private bool hasAdded( List<MergedData> mergedList, TData topic ) {
            foreach (MergedData x in mergedList) {
                if (x.Topic != null && x.Topic.Id == topic.Id) return true;
            }
            return false;
        }

        //------------------------------固定-------------------------------------

        public String GetIndexIds( int appId ) {
            String condition = "IsPin=1";
            return db.find<TPick>( addAppId( condition, appId ) ).get( "PinIndex" );
        }

        public String GetIndexIds( int appId, int index ) {
            String condition = "IsPin=1 and PinIndex<>" + index;
            return db.find<TPick>( addAppId( condition, appId ) ).get( "PinIndex" );
        }

        public Result AddPinTopic( TData topic, int index ) {

            Result ret = new Result();
            
            if (hasPosition( topic.AppId, index ) == false) {
                ret.Add( "位置已被占用:" + index );
                return ret;
            }

            List<TPick> customList = getCustomList( topic.AppId );
            TPick editPost = getEditPost( topic, customList );

            // 先隐藏帖子
            this.DeleteTopic( topic.AppId, topic.Id );

            // 然后添加固定内容
            if (editPost != null) {
                return this.AddPinPost( topic.AppId, index, editPost.Title, editPost.Link, editPost.Summary, topic.Id );
            }
            else {
                return this.AddPinPost( topic.AppId, index, topic.Title, alink.ToAppData( topic ), getSummary( topic ), topic.Id );
            }
        }


        public Result AddPinPost( int appId, int index, String title, String link, String summary ) {
            return this.AddPinPost( appId, index, title, link, summary, 0 );
        }

        public Result AddPinPost( int appId, int index, String title, String link, String summary, int topicId ) {

            Result ret = new Result();

            if (hasPosition( appId, index ) == false) {
                ret.Add( "位置已被占用:" + index );
                return ret;
            }

            TPick x = new TPick();
            x.AppId = appId;
            x.IsPin = 1;
            x.PinIndex = index;
            x.PinTopicId = topicId;

            x.Title = title;
            x.Link = link;
            x.Summary = summary;

            return x.insert();
        }

        private bool hasPosition( int appId, int index ) {

            return GetPinPostByIndex( appId, index ) == null;
        }

        private List<TPick> getPinList( List<TPick> customList ) {

            List<TPick> pins = new List<TPick>();
            foreach (TPick x in customList) {
                if (x.IsPin == 1) pins.Add( x );
            }

            return pins;
        }

        //------------------------------编辑-------------------------------------

        public void EditTopic( int appId, int topicId, String title, String link, String summary ) {

            TPick x = GetEditTopic( appId, topicId );

            if (x == null) {
                x = new TPick();
                x.AppId = appId;

                x.Title = title;
                x.Link = link;
                x.Summary = summary;

                x.IsEdit = 1;
                x.EditId = topicId;
                x.insert();

            }
            else {
                x.AppId = appId;

                x.Title = title;
                x.Link = link;
                x.Summary = summary;

                x.update();
            }
        }


        public void EditPinPost( TPick x, int index, String title, String link, String summary ) {

            if (x == null) throw new NullReferenceException( "EditPinPost" );

            x.PinIndex = index;

            x.Title = title;
            x.Link = link;
            x.Summary = summary;

            x.update();
        }



        public TPick GetPinPostByIndex( int appId, int index ) {

            String condition = " IsPin=1 and PinIndex=" + index;

            return db.find<TPick>( addAppId( condition, appId ) ).first();
        }


        public TPick GetEditTopic( int appId, int topicId ) {

            String condition = "  IsEdit=1 and EditId=" + topicId;

            return db.find<TPick>( addAppId( condition, appId ) ).first();
        }


        private TPick getEditPost( TData topic, List<TPick> customList ) {

            foreach (TPick x in customList) {
                if (x.IsEdit == 1 && x.EditId == topic.Id) return x;
            }

            return null;
        }

        //------------------------------删除-------------------------------------

        public DataPage<TPick> GetDeleteList( int appId ) {

            String condition = "IsDelete=1";

            return db.findPage<TPick>( addAppId( condition, appId ), 10 );
        }

        public void RestoreTopic( int appId, int topicId ) {

            String condition = "IsDelete=1 and DeleteId=" + topicId;

            TPick x = db.find<TPick>( addAppId( condition, appId ) ).first();

            if (x != null) x.delete();
        }

        public void DeleteTopic( int appId, int topicId ) {

            TData topic = (TData)ndb.findById( typeof( TData ), topicId );

            TPick x = new TPick();

            x.AppId = appId;

            x.Title = topic.Title;

            x.IsDelete = 1;
            x.DeleteId = topicId;
            x.insert();
        }


        public void DeletePinPost( int appId, int index ) {

            TPick x = GetPinPostByIndex( appId, index );
            if (x == null) throw new NullReferenceException( "DeletePinedPost" );

            // 直接从主题隐藏的，还要恢复隐藏
            if (x.PinTopicId > 0) {
                this.RestoreTopic( appId, x.PinTopicId );
            }

            x.delete();
        }

        public List<TData> GetUnDeletedTopic( List<TData> topics, List<TPick> customList ) {

            List<TData> list = new List<TData>();
            foreach (TData x in topics) {
                if (isTopicDeleted( x, customList ) == false) list.Add( x );
            }
            return list;
        }

        private bool isTopicDeleted( TData topic, List<TPick> customList ) {
            foreach (TPick x in customList) {
                if (x.IsDelete == 1 && x.DeleteId == topic.Id) return true;
            }
            return false;
        }

        //-------------------------------------------------------------------

        private List<TPick> getCustomList( int appId ) {
            return db.find<TPick>( addAppId( "", appId ) ).list();
        }

        private String addAppId( String str, int appId ) {
            if (appId <= 0) return str;

            if (strUtil.HasText( str )) {
                return "AppId=" + appId + " and " + str;
            }

            return "AppId=" + appId;
        }

    }

}
