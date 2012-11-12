using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;

using wojilu.Apps.Blog.Domain;
using wojilu.Apps.Blog.Interface;

namespace wojilu.Apps.Blog.Service {

    public class BlogPickService : IBlogPickService {

        // 返回所有经过“编辑、删除、固定”的帖子
        public List<MergedPost> GetAll( List<BlogPost> allTopics ) {

            List<BlogPick> customList = getCustomList();

            // 获取尚未删除的帖子
            List<BlogPost> topics = GetUnDeletedTopic( allTopics, customList );

            // 获取固定的帖子
            List<BlogPick> pinList = getPinList( customList );

            return getMergedPosts( topics, pinList, customList );
        }

        private List<MergedPost> getMergedPosts( List<BlogPost> topics, List<BlogPick> pinList, List<BlogPick> customList ) {

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

        private MergedPost getPinedPost_Merged( List<BlogPick> pinList, int i ) {
            foreach (BlogPick x in pinList) {
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
        private MergedPost getNextTopic_Merged( List<BlogPost> topics, List<MergedPost> mergedList, List<BlogPick> customList ) {
            foreach (BlogPost x in topics) {
                if (hasAdded( mergedList, x ) == false) {

                    MergedPost mPost = new MergedPost();
                    mPost.Topic = x;

                    // 是否编辑过？
                    BlogPick editPost = getEditPost( x, customList );
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

        private static string getSummary( BlogPost x ) {
            return strUtil.ParseHtml( x.Content, 80 );
        }

        private bool hasAdded( List<MergedPost> mergedList, BlogPost topic ) {
            foreach (MergedPost x in mergedList) {
                if (x.Topic != null && x.Topic.Id == topic.Id) return true;
            }
            return false;
        }

        //------------------------------固定-------------------------------------

        public String GetIndexIds() {

            return BlogPick
                .find( " IsPin=1" ).get( "PinIndex" );
        }


        public String GetIndexIds( int index ) {

            return BlogPick
                .find( " IsPin=1 and PinIndex<>" + index ).get( "PinIndex" );
        }

        public void AddPinTopic( BlogPost topic, int index ) {

            if (hasPosition( index ) == false) throw new Exception( "位置已被占用:" + index );

            List<BlogPick> customList = getCustomList();
            BlogPick editPost = getEditPost( topic, customList );

            // 先隐藏帖子
            this.DeleteTopic( topic.AppId );

            // 然后添加固定内容
            if (editPost != null) {
                this.AddPinPost( index, editPost.Title, editPost.Link, editPost.Summary, topic.Id );
            }
            else {
                this.AddPinPost( index, topic.Title, alink.ToAppData( topic ), getSummary( topic ), topic.Id );
            }
        }


        public void AddPinPost( int index, String title, String link, String summary ) {
            this.AddPinPost( index, title, link, summary, 0 );

        }

        public void AddPinPost( int index, String title, String link, String summary, int topicId ) {

            if (hasPosition( index ) == false) throw new Exception( "位置已被占用:" + index );

            BlogPick x = new BlogPick();
            x.IsPin = 1;
            x.PinIndex = index;
            x.PinTopicId = topicId;

            x.Title = title;
            x.Link = link;
            x.Summary = summary;

            x.insert();
        }

        private bool hasPosition( int index ) {

            return GetPinPostByIndex( index ) == null;
        }

        private List<BlogPick> getPinList( List<BlogPick> customList ) {

            List<BlogPick> pins = new List<BlogPick>();
            foreach (BlogPick x in customList) {
                if (x.IsPin == 1) pins.Add( x );
            }

            return pins;
        }

        //------------------------------编辑-------------------------------------

        public void EditTopic( int topicId, String title, String link, String summary ) {

            BlogPick x = GetEditTopic( topicId );

            if (x == null) {
                x = new BlogPick();

                x.Title = title;
                x.Link = link;
                x.Summary = summary;

                x.IsEdit = 1;
                x.EditId = topicId;
                x.insert();

            }
            else {

                x.Title = title;
                x.Link = link;
                x.Summary = summary;

                x.update();
            }
        }


        public void EditPinPost( BlogPick x, int index, String title, String link, String summary ) {

            if (x == null) throw new NullReferenceException( "EditPinPost" );

            x.PinIndex = index;

            x.Title = title;
            x.Link = link;
            x.Summary = summary;

            x.update();
        }

        public BlogPick GetPinPostByIndex( int index ) {

            return BlogPick
                .find( " IsPin=1 and PinIndex=" + index )
                .first();
        }


        public BlogPick GetEditTopic( int topicId ) {
            return BlogPick
                .find( " IsEdit=1 and EditId=" + topicId )
                .first();
        }


        private BlogPick getEditPost( BlogPost topic, List<BlogPick> customList ) {

            foreach (BlogPick x in customList) {
                if (x.IsEdit == 1 && x.EditId == topic.Id) return x;
            }

            return null;
        }

        //------------------------------删除-------------------------------------

        public DataPage<BlogPick> GetDeleteList() {
            return BlogPick
                .findPage( " IsDelete=1", 10 );
        }

        public void RestoreTopic( int topicId ) {

            BlogPick x = BlogPick
                .find( " IsDelete=1 and DeleteId=" + topicId )
                .first();

            if (x != null) x.delete();
        }

        public void DeleteTopic( int topicId ) {

            BlogPost topic = BlogPost.findById( topicId );

            BlogPick x = new BlogPick();

            x.Title = topic.Title;

            x.IsDelete = 1;
            x.DeleteId = topicId;
            x.insert();
        }


        public void DeletePinPost( int index ) {

            BlogPick x = GetPinPostByIndex( index );
            if (x == null) throw new NullReferenceException( "DeletePinedPost" );

            // 直接从主题隐藏的，还要恢复隐藏
            if (x.PinTopicId > 0) {
                this.RestoreTopic( x.PinTopicId );
            }

            x.delete();
        }

        public List<BlogPost> GetUnDeletedTopic( List<BlogPost> topics, List<BlogPick> customList ) {

            List<BlogPost> list = new List<BlogPost>();
            foreach (BlogPost x in topics) {
                if (isTopicDeleted( x, customList ) == false) list.Add( x );
            }
            return list;
        }

        private bool isTopicDeleted( BlogPost topic, List<BlogPick> customList ) {
            foreach (BlogPick x in customList) {
                if (x.IsDelete == 1 && x.DeleteId == topic.Id) return true;
            }
            return false;
        }

        //-------------------------------------------------------------------

        private List<BlogPick> getCustomList() {
            return BlogPick.find( "" ).list();
        }

    }

}
