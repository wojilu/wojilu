/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

using wojilu.Aop;
using wojilu.Apps.Forum.Service;
using wojilu.Apps.Forum.Domain;
using wojilu.Common.Microblogs.Domain;

namespace wojilu.Apps.Forum.Sync {

    /// <summary>
    /// 当 ForumTopic 被删除的时候，同时删除 Microblog
    /// </summary>
    public class TopicObserver : MethodObserver {

        public override void ObserveMethods() {

            observe( typeof( ForumTopicService ), "DeleteToTrash" );
            observe( typeof( ForumTopicService ), "DeleteList" );
        }

        public override void After( object returnValue, MethodInfo method, object[] args, object target ) {

            if (method.Name == "DeleteToTrash") {

                ForumTopic topic = args[0] as ForumTopic;
                Microblog mblog = Microblog.find( "DataId=:id and DataType=:dtype" )
                    .set( "id", topic.Id )
                    .set( "dtype", typeof( ForumTopic ).FullName )
                    .first();

                if (mblog != null) {
                    mblog.delete();
                }

            }

            else if (method.Name == "DeleteList") {

                AdminValue av = args[0] as AdminValue;
                if (strUtil.HasText( av.Ids )) {
                    String str = "DataType='" + typeof( ForumTopic ).FullName + "' and DataId in (" + av.Ids + ")";
                    Microblog.deleteBatch( str );
                }

            }

        }

    }

}
