/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using wojilu;
using wojilu.Common.AppBase.Interface;

namespace wojilu.Common.Tags {


    public class AppTagService {


        public static IList GetTopTag( IApp app, int count ) {
            //ObjectBase appTag = getAppTagObject( app );
            return ndb.find( apt( app ), "AppId=" + app.Id + " order by PostCount desc, Id asc" ).list( count );
        }

        /// <summary>
        /// 保存某个 app 的 tag 信息，比如在 ForumTag 中保存
        /// </summary>
        /// <param name="app"></param>
        /// <param name="tagString"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static Boolean Insert( IApp app, String tagString, int userId ) {
            Result result = new Result();
            if (strUtil.IsNullOrEmpty( tagString )) {
                return false;
            }
            string[] arrTag = TagService.GetTags( tagString );
            foreach (String tag in arrTag) {
                join_Tag_App( getTag( tag, userId ), app );
            }
            return true;
        }

        private static Tag getTag( String strTag, int userId ) {
            Tag tag = TagService.GetTag( strTag );
            if (tag == null) {
                tag = new Tag();
                tag.Name = strTag;
                tag.CreatorId = userId;
                //tag.CreatorId = ctx.Viewer.obj.Id;
                db.insert( tag );
            }
            return tag;
        }

        private static void join_Tag_App( Tag t, IApp app ) {
            IAppTag tag;
            //ObjectBase tagTool = getAppTagObject( app );
            IEntity mytag = ndb.find( apt( app ), "AppId=:appId and Tag.Id=:tagId" ).set( "appId", app.Id ).set( "tagId", t.Id ).first();
            if (mytag == null) {
                tag = getAppTagObject( app ) as IAppTag;
                tag.AppId = app.Id;
                tag.Tag = t;
                tag.PostCount = 1;
                db.insert( tag );
            }
            else {
                tag = mytag as IAppTag;
                tag.PostCount++;
                db.update( tag );
            }
        }

        private static IEntity getAppTagObject( IApp app ) {
            return Entity.New( strUtil.TrimEnd( app.GetType().FullName, "App" ) + "Tag" );
        }
        private static Type apt( IApp app ) {
            return getAppTagObject( app ).GetType();
        }

    }
}

