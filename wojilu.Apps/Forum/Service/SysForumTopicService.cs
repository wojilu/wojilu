using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Apps.Forum.Domain;
using wojilu.Common;
using wojilu.Web.Mvc;

namespace wojilu.Apps.Forum.Service {

    public class SysForumTopicService {


        public List<ForumTopic> GetRankByReplies( int appId, int count ) {
            if (count <= 0) count = 10;
            String strApp = appId > 0 ? "AppId=" + appId + " and " : "";
            return ForumTopic.find( strApp + getNonDelCondition() + " order by Replies desc, Id desc" ).list( count );
        }

        public List<ForumTopic> GetRankByHits( int appId, int count ) {
            if (count <= 0) count = 10;
            String strApp = appId > 0 ? "AppId=" + appId + " and " : "";
            return ForumTopic.find( strApp + getNonDelCondition() + " order by Hits desc, Replies desc, Id desc" ).list( count );
        }

        public List<IBinderValue> GetRankTopicByReplies( int appId, int count ) {
            return populateBinderValue( GetRankByReplies( appId, count ) );
        }

        public List<IBinderValue> GetRankTopicByHits( int appId, int count ) {
            return populateBinderValue( GetRankByHits( appId, count ) );
        }

        private String getNonDelCondition() {
            return string.Format( " (Status={0} or Status={1})", (int)TopicStatus.Normal, TopicStatus.Sticky );
        }

        internal static List<IBinderValue> populateBinderValue( List<ForumTopic> list ) {
            List<IBinderValue> results = new List<IBinderValue>();
            foreach (ForumTopic topic in list) {
                IBinderValue vo = new ItemValue();

                vo.Title = topic.Title;
                vo.Category = topic.ForumBoard.Name;
                vo.Link = alink.ToAppData( topic );
                vo.Replies = topic.Replies;
                vo.Content = topic.Content;

                vo.CreatorName = topic.Creator.Name;
                vo.CreatorLink = Link.ToMember( topic.Creator );
                vo.CreatorPic = topic.Creator.PicSmall;


                vo.Created = topic.Created;

                results.Add( vo );
            }

            return results;
        }

    }

}
