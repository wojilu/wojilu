using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;

using wojilu.Apps.Forum.Domain;
using wojilu.Apps.Forum.Interface;

namespace wojilu.Apps.Forum.Service {

    public class ForumPickService : IForumPickService {

        // 返回所有经过“编辑、删除、固定”的帖子
        public List<MergedPost> GetAll( List<ForumTopic> allTopics, int appId ) {

            List<ForumPick> customList = getCustomList( appId );

            // 获取尚未删除的帖子
            List<ForumTopic> topics = GetUnDeletedTopic( allTopics, customList );

            // 获取固定的帖子
            List<ForumPick> pinList = getPinList( customList );

            return getMergedPosts( topics, pinList, customList );
        }

        private List<MergedPost> getMergedPosts( List<ForumTopic> topics, List<ForumPick> pinList, List<ForumPick> customList ) {

            List<MergedPost> mergedList = new List<MergedPost>();
            int iCount = 13;
            for (int i = 0; i < iCount; i++) {

                MergedPost mpost = getPinedPost_Merged( pinList, i );
                if (mpost != null) {
                    mergedList.Add( mpost );
                    continue;
                }

                MergedPost tpost = getNextTopic_Merged( topics, mergedList, customList );
                if (tpost != null) {
                    mergedList.Add( tpost );
                }

            }

            return mergedList;
        }

        private MergedPost getPinedPost_Merged( List<ForumPick> pinList, int i ) {
            foreach (ForumPick x in pinList) {
                if (x.PinIndex == i + 1) {
                    MergedPost mPost = new MergedPost();
                    mPost.Title = x.Title;
                    mPost.Link = x.Link;
                    mPost.Summary = x.Summary;
                    return mPost;
                }
            }
            return null;
        }

        // 取出尚未添加的第一个帖子
        private MergedPost getNextTopic_Merged( List<ForumTopic> topics, List<MergedPost> mergedList, List<ForumPick> customList ) {
            foreach (ForumTopic x in topics) {
                if (hasAdded( mergedList, x ) == false) {

                    MergedPost mPost = new MergedPost();
                    mPost.Topic = x;

                    // 是否编辑过？
                    ForumPick editPost = getEditPost( x, customList );
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

        private static string getSummary( ForumTopic x ) {
            return strUtil.ParseHtml( x.Content, 150 );
        }

        private bool hasAdded( List<MergedPost> mergedList, ForumTopic topic ) {
            foreach (MergedPost x in mergedList) {
                if (x.Topic != null && x.Topic.Id == topic.Id) return true;
            }
            return false;
        }

        //------------------------------固定-------------------------------------

        public String GetIndexIds( int appId ) {

            return ForumPick
                .find( "AppId=" + appId + " and IsPin=1" ).get( "PinIndex" );
        }


        public String GetIndexIds( int appId, int index ) {

            return ForumPick
                .find( "AppId=" + appId + " and IsPin=1 and PinIndex<>" + index ).get( "PinIndex" );
        }

        public void AddPinTopic( ForumTopic topic, int index ) {

            if (hasPosition( topic.AppId, index ) == false) throw new Exception( "位置已被占用:" + index );

            List<ForumPick> customList = getCustomList( topic.AppId );
            ForumPick editPost = getEditPost( topic, customList );

            // 先隐藏帖子
            this.DeleteTopic( topic.AppId, topic.Id );

            // 然后添加固定内容
            if (editPost != null) {
                this.AddPinPost( topic.AppId, index, editPost.Title, editPost.Link, editPost.Summary, topic.Id );
            }
            else {
                this.AddPinPost( topic.AppId, index, topic.Title, alink.ToAppData( topic ), getSummary( topic ), topic.Id );
            }
        }


        public void AddPinPost( int appId, int index, String title, String link, String summary ) {
            this.AddPinPost( appId, index, title, link, summary, 0 );

        }

        public void AddPinPost( int appId, int index, String title, String link, String summary, int topicId ) {

            if (hasPosition( appId, index ) == false) throw new Exception( "位置已被占用:" + index );

            ForumPick x = new ForumPick();
            x.AppId = appId;
            x.IsPin = 1;
            x.PinIndex = index;
            x.PinTopicId = topicId;

            x.Title = title;
            x.Link = link;
            x.Summary = summary;

            x.insert();
        }

        private bool hasPosition( int appId, int index ) {

            return GetPinPostByIndex( appId, index ) == null;
        }

        private List<ForumPick> getPinList( List<ForumPick> customList ) {

            List<ForumPick> pins = new List<ForumPick>();
            foreach (ForumPick x in customList) {
                if (x.IsPin == 1) pins.Add( x );
            }

            return pins;
        }

        //------------------------------编辑-------------------------------------

        public void EditTopic( int appId, int topicId, String title, String link, String summary ) {

            ForumPick x = GetEditTopic( appId, topicId );

            if (x == null) {
                x = new ForumPick();
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


        public void EditPinPost( ForumPick x, int index, String title, String link, String summary ) {

            if (x == null) throw new NullReferenceException( "EditPinPost" );

            x.PinIndex = index;

            x.Title = title;
            x.Link = link;
            x.Summary = summary;

            x.update();
        }

        public ForumPick GetPinPostByIndex( int appId, int index ) {

            return ForumPick
                .find( "AppId=" + appId + " and IsPin=1 and PinIndex=" + index )
                .first();
        }


        public ForumPick GetEditTopic( int appId, int topicId ) {
            return ForumPick
                .find( "AppId=" + appId + " and IsEdit=1 and EditId=" + topicId )
                .first();
        }


        private ForumPick getEditPost( ForumTopic topic, List<ForumPick> customList ) {

            foreach (ForumPick x in customList) {
                if (x.IsEdit == 1 && x.EditId == topic.Id) return x;
            }

            return null;
        }

        //------------------------------删除-------------------------------------

        public DataPage<ForumPick> GetDeleteList( int appId ) {
            return ForumPick
                .findPage( "AppId=" + appId + " and IsDelete=1", 10 );
        }

        public void RestoreTopic( int appId, int topicId ) {

            ForumPick x = ForumPick
                .find( "AppId=" + appId + " and IsDelete=1 and DeleteId=" + topicId )
                .first();

            if (x != null) x.delete();
        }

        public void DeleteTopic( int appId, int topicId ) {

            ForumTopic topic = ForumTopic.findById( topicId );

            ForumPick x = new ForumPick();

            x.AppId = appId;

            x.Title = topic.Title;

            x.IsDelete = 1;
            x.DeleteId = topicId;
            x.insert();
        }


        public void DeletePinPost( int appId, int index ) {

            ForumPick x = GetPinPostByIndex( appId, index );
            if (x == null) throw new NullReferenceException( "DeletePinedPost" );

            // 直接从主题隐藏的，还要恢复隐藏
            if (x.PinTopicId > 0) {
                this.RestoreTopic( appId, x.PinTopicId );
            }

            x.delete();
        }

        public List<ForumTopic> GetUnDeletedTopic( List<ForumTopic> topics, List<ForumPick> customList ) {

            List<ForumTopic> list = new List<ForumTopic>();
            foreach (ForumTopic x in topics) {
                if (isTopicDeleted( x, customList ) == false) list.Add( x );
            }
            return list;
        }

        private bool isTopicDeleted( ForumTopic topic, List<ForumPick> customList ) {
            foreach (ForumPick x in customList) {
                if (x.IsDelete == 1 && x.DeleteId == topic.Id) return true;
            }
            return false;
        }

        //-------------------------------------------------------------------

        private List<ForumPick> getCustomList( int appId ) {
            return ForumPick.find( "AppId=" + appId ).list();
        }

    }

}
