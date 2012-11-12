/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Common.AppBase.Interface;

using wojilu.Apps.Forum.Domain;
using wojilu.Apps.Forum.Interface;
using wojilu.Members.Users.Domain;
using wojilu.Members.Groups.Domain;
using wojilu.ORM;

namespace wojilu.Web.Controller.Admin.Groups {

    public partial class GroupController : ControllerBase {


        private DataPage<ForumPost> getPosts() {

            String name = ctx.Get( "name" );
            set( "s.Name", name );

            //// 排序(只检索主题)
            //String sorter = ctx.Get( "sorter" );
            //if (strUtil.HasText( sorter )) {

            //    String condition = "";

            //    if (sorter.Equals( "id" ))
            //        condition = "order by Id desc";
            //    else if (sorter.Equals( "replies" ))
            //        condition = "order by Replies desc, Id desc";
            //    else if (sorter.Equals( "hits" ))
            //        condition = "order by Hits desc, Id desc";

            //    return postService.GetTopicAll( condition );
            //}

            // 过滤
            String filter = ctx.Get( "filter" );

            if (strUtil.HasText( filter )) {

                EntityInfo ei = Entity.GetInfo( typeof( ForumPost ) );

                String t = ei.Dialect.GetTimeQuote();
                String fs = " and Created between " + t + "{0}" + t + " and " + t + "{1}" + t + " order by Id desc";
                DateTime now = DateTime.Now;

                String condition = "";

                if (filter == "today")
                    condition = string.Format( fs, now.ToShortDateString(), now.AddDays( 1 ).ToShortDateString() );
                else if (filter == "week")
                    condition = string.Format( fs, now.AddDays( -7 ).ToShortDateString(), now.AddDays( 1 ).ToShortDateString() );
                else if (filter == "month")
                    condition = string.Format( fs, now.AddMonths( -1 ).ToShortDateString(), now.AddDays( 1 ).ToShortDateString() );

                return postService.GetPostAll( condition );
            }

            // 关键词搜索
            if (strUtil.HasText( name )) {

                name = strUtil.SqlClean( name, 20 );

                String condition = "";
                String queryType = ctx.Get( "queryType" );

                if (queryType.Equals( "title" )) {
                    condition = string.Format( " and Title like '%{0}%' ", name );
                }
                else if (queryType.Equals( "creator" )) {

                    String creatorIds = getCreators( name );
                    if (strUtil.IsNullOrEmpty( creatorIds )) return DataPage<ForumPost>.GetEmpty();
                    condition = " and CreatorId in (" + creatorIds + ")";
                }

                return postService.GetPostAll( condition );

            }

            return postService.GetPostAll( "" );
        }


        private void bindGroupFilter( int id, List<GroupCategory> categories ) {
            //dropList( "categoryId", categories, "Name=Id", id );

            IBlock block = getBlock( "cfilterList" );
            foreach (GroupCategory c in categories) {
                block.Set( "c.Name", c.Name );
                block.Set( "c.Link", to( GroupAdmin, c.Id ) );
                block.Next();
            }

            set( "lnkAll", to( GroupAdmin, id ) + "?t=-1" );
            set( "lnkOpen", to( GroupAdmin, id ) + "?t=99" );
            set( "lnkClosed", to( GroupAdmin, id ) + "?t=" + GroupAccessStatus.Closed );
            set( "lnkHide", to( GroupAdmin, id ) + "?t=" + GroupAccessStatus.Secret );
            set( "lnkSystemLocked", to( GroupAdmin, id ) + "?t=100" );
            set( "lnkSystemHidden", to( GroupAdmin, id ) + "?t=101" );
        }

        // 让0也可以有意义
        private int getTypeId() {

            int qValue = ctx.GetInt( "t" );

            int typeId = -1;

            if (qValue == 99)
                typeId = 0;
            else if (qValue > 0)
                typeId = qValue;

            return typeId;
        }

        private DataPage<Group> getGroups( int categoryId, int typeId ) {

            String condition = "";
            if (categoryId > 0) condition = "CategoryId=" + categoryId;

            if (typeId > -1) {
                if (typeId < 100)
                    condition += " and AccessStatus=" + typeId;
                else if (typeId == 100)
                    condition += " and IsLock=1";
                else if (typeId == 101) {
                    condition += " and IsSystemHide=1";
                }
            }

            String name = ctx.Get( "name" );
            set( "s.Name", name );

            if (strUtil.HasText( name )) {

                name = strUtil.SqlClean( name, 20 );

                String queryType = ctx.Get( "queryType" );

                if ("name".Equals( queryType )) {
                    condition += string.Format( " and Name like '%{0}%' ", name );
                }
                else if ("creator".Equals( queryType )) {

                    String creatorIds = getCreators( name );
                    if (strUtil.IsNullOrEmpty( creatorIds )) return DataPage<Group>.GetEmpty();
                    condition += " and CreatorId in (" + creatorIds + ")";
                }
                else if ("description".Equals( queryType )) {
                    condition += string.Format( " and Description like '%{0}%' ", name );
                }

            }

            condition = condition.Trim();
            condition = strUtil.TrimStart( condition, "and" );

            return groupService.AdminSearchByCondition( condition );
        }


        private void bindPosts( List<ForumPost> posts, IBlock pblock ) {
            foreach (ForumPost post in posts) {

                if (post.Creator == null) continue;

                pblock.Set( "p.Title", post.Title );

                int hits = post.Hits;
                int replies = 0;
                if (post.ParentId == 0) {
                    ForumTopic topic = topicService.GetByPost( post.Id );
                    if (topic == null) continue;
                    hits = topic.Hits;
                    replies = topic.Replies;
                }

                pblock.Set( "p.Replies", replies + "/" );
                pblock.Set( "p.Hits", hits );

                pblock.Set( "p.Link", alink.ToAppData( post as IAppData ) );

                pblock.Set( "p.Created", cvt.ToTimeString( post.Created ) );

                pblock.Set( "p.UserName", post.Creator.Name );
                pblock.Set( "p.UserLink", toUser( post.Creator ) );

                Group g = getGroup( post );
                if (g == null) continue;


                pblock.Set( "p.GroupName", g.Name );
                pblock.Set( "p.GroupLink", Link.ToMember( g ) );

                pblock.Next();
            }
        }

        private void bindGroups( List<Group> groups ) {

            IBlock gblock = getBlock( "groups" );

            foreach (Group g in groups) {

                gblock.Set( "g.UserName", g.Creator.Name );
                gblock.Set( "g.UserLink", toUser( g.Creator ) );

                gblock.Set( "g.Name", g.Name );
                gblock.Set( "g.Link", toUser( g ) );
                gblock.Set( "g.Logo", g.LogoSmall );
                gblock.Set( "g.Created", cvt.ToTimeString( g.Created ) );
                gblock.Set( "g.Description", strUtil.CutString( g.Description, 50 ) );

                gblock.Set( "g.SendMsgLink", to( SendMsg, g.Id ) );
                gblock.Set( "g.LockLink", to( Lock, g.Id ) );
                gblock.Set( "g.HideLink", to( Hide, g.Id ) );
                gblock.Set( "g.DeleteLink", to( Delete, g.Id ) );

                String style = tbl[g.AccessStatus];
                if (g.IsLock == 1) style += " groupSystemLocked";
                if (g.IsSystemHide == 1) style += " groupSystemHidden";

                gblock.Set( "g.Class", style );

                String lockName = (g.IsLock == 1 ? lang( "unlock" ) : lang( "lock" ));
                String hideName = (g.IsSystemHide == 1 ? lang( "display" ) : lang( "hide" ));

                gblock.Set( "g.LockName", lockName );
                gblock.Set( "g.HideName", hideName );

                gblock.Next();
            }
        }

        private static Dictionary<int, string> tbl = getGroupClassMap();

        private static Dictionary<int, string> getGroupClassMap() {
            Dictionary<int, string> tbl = new Dictionary<int, string>();
            tbl.Add( 0, "" );
            tbl.Add( 1, "groupClosed" );
            tbl.Add( 2, "groupSecret" );
            return tbl;
        }

        //----------------------------------------------------------------------------------

        private Group getGroup( IPost post ) {
            return groupService.GetById( post.OwnerId );
        }

        private String getCreators( String name ) {

            List<User> users = userService.SearchByName( name );
            String ids = "";
            foreach (User user in users) ids += user.Id + ",";
            return ids.TrimEnd( ',' );
        }

        private void log( String msg ) {
            logService.Add( (User)ctx.viewer.obj, msg, "", typeof( Group ).FullName, ctx.Ip );
        }

        private void log( String msg, String dataInfo ) {
            logService.Add( (User)ctx.viewer.obj, msg, dataInfo, typeof( Group ).FullName, ctx.Ip );
        }

        private void log( String msg, Group g ) {
            String dataInfo = "{Id:" + g.Id + ", Name:'" + g.Name + "'}";
            logService.Add( (User)ctx.viewer.obj, msg, dataInfo, typeof( Group ).FullName, ctx.Ip );
        }

    }

}
