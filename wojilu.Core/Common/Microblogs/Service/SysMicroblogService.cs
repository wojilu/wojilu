/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */
using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Common.Microblogs.Domain;
using wojilu.Web.Mvc;

namespace wojilu.Common.Microblogs.Service {

    public class SysMicroblogService {

        public List<Microblog> GetRecent( int count ) {
            if (count <= 0) count = 10;
            return Microblog.find( "order by Id desc" ).list( count );                 
        }

        public List<Microblog> GetByReplies( int count ) {
            if (count <= 0) count = 10;
            return Microblog.find( "order by Replies desc, Id desc" ).list( count );
        }

        public List<IBinderValue> GetRecentMicroblog( int count ) {
            return populatePost( GetRecent( count ) );
        }

        public List<IBinderValue> GetMicroblogByReplies( int count ) {
            return populatePost( GetByReplies( count ) );
        }

        internal static List<IBinderValue> populatePost( List<Microblog> list ) {

            List<IBinderValue> results = new List<IBinderValue>();

            foreach (Microblog post in list) {

                IBinderValue vo = new ItemValue();

                vo.CreatorName = post.User.Name;
                vo.CreatorLink = alink.ToUserMicroblog( post.User );
                vo.CreatorPic = post.User.PicSmall;

                vo.Title = post.Content;
                vo.Link = alink.ToUserMicroblog( post.User );
                vo.Content = post.Content;
                vo.Created = post.Created;
                vo.Replies = post.Replies;

                results.Add( vo );
            }

            return results;
        }

    }

}
