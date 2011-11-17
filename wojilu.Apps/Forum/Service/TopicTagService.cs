using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Apps.Forum.Domain;
using wojilu.Members.Users.Domain;

namespace wojilu.Apps.Forum.Service {

    public class TopicTagService {

        public List<UserTopicTag> GetByUser( int userId ) {
            return UserTopicTag.find( "User.Id=" + userId + " order by OrderId desc, Id asc" ).list();
        }

        public DataPage<ForumTopic> GetByTag( int tagId ) {
            DataPage<TopicTagShip> list = TopicTagShip.findPage( "TagId=" + tagId + " order by TopicId desc" );
            DataPage<ForumTopic> results = new DataPage<ForumTopic>();

            results.CopyStats( list );
            results.Results = new List<ForumTopic>();
            foreach (TopicTagShip tt in list.Results) {
                results.Results.Add( tt.Topic );
            }

            return results;
        }

        public Result CreateTag( int userId, String tagName ) {

            Result result = new Result();

            //--------------------
            List<UserTopicTag> ulist = UserTopicTag.find( "User.Id=" + userId + " and Name=:name" )
                .set( "name", tagName )
                .list();
            if (ulist.Count > 0) {
                result.Add( "分类已经存在" );
                return result;
            }

            //--------------------
            UserTopicTag ut = new UserTopicTag();
            ut.Name = tagName;
            ut.User = new User( userId );
            return ut.insert();

        }

        public void DeleteTag( int tagId ) {
            UserTopicTag ut = UserTopicTag.findById( tagId );
            if (ut == null) return;
            ut.delete();
        }

        public Result UpdateTagName( int tagId, String newTagName ) {

            Result result = new Result();

            //--------------------
            UserTopicTag ut = UserTopicTag.findById( tagId );
            if (ut == null) {
                result.Add( "分类不存在" );
                return result;
            }

            //--------------------
            List<UserTopicTag> ulist = UserTopicTag.find( "User.Id=" + ut.User.Id + " and Name=:name" )
                .set( "name", newTagName )
                .list();
            if (ulist.Count > 0) {
                result.Add( "同名分类已经存在" );
                return result;
            }

            //------------------------
            ut.Name = newTagName;
            return ut.update();
        }


        public UserTopicTag GetTagById( int id ) {
            return UserTopicTag.findById( id );
        }


    }

}
