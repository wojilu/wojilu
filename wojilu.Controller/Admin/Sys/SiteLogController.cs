/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Members.Sites.Domain;
using wojilu.Members.Sites.Service;
using wojilu.Members.Interface;
using wojilu.Members.Users.Domain;
using wojilu.Members.Users.Interface;
using wojilu.Members.Users.Service;
using wojilu.Data;
using wojilu.ORM;

namespace wojilu.Web.Controller.Admin.Sys {

    public class SiteLogController : ControllerBase {

        public IAdminLogService<SiteLog> logService { get; set; }
        public IUserService userService { get; set; }

        public SiteLogController() {
            logService = new SiteLogService();
            userService = new UserService();
        }

        public override void Layout() {
        }

        public void Index() {

            set( "SearchAction", to( Index ) );

            String name = ctx.Get( "name" );
            set( "s.Name", name );

            String filter = ctx.Get( "filter" );

            DataPage<SiteLog> list;
            if (strUtil.HasText( filter )) {
                list = logService.GetPage( getCondition( filter ) );
            }
            else {

                String termCondition = getConditionByTerm( name );
                if (termCondition == null) {
                    list = DataPage<SiteLog>.GetEmpty();
                }
                else if (termCondition == "") {
                    list = logService.GetPage( 20 );
                }
                else {
                    list = logService.GetPage( termCondition );
                }

            }

            bindPage( list );
        }

        private String getConditionByTerm( String name ) {


            if (strUtil.IsNullOrEmpty( name )) return "";

            name = strUtil.SqlClean( name, 20 );

            String condition = "";
            String queryType = ctx.Get( "queryType" );

            if (queryType.Equals( "user" )) {
                String userIds = getUserIds( name );
                if (strUtil.IsNullOrEmpty( userIds )) return null;
                condition = "  UserId in (" + userIds + ")";
            }
            else if (queryType.Equals( "msg" )) {
                condition = string.Format( "  Message like '%{0}%' ", name );
            }
            else if (queryType.Equals( "ip" )) {
                condition = string.Format( "  Ip like '%{0}%' ", name );
            }

            return condition;
        }

        private String getUserIds( String name ) {

            List<User> users = userService.SearchByName( name );
            String ids = "";
            foreach (User user in users) ids += user.Id + ",";
            return ids.TrimEnd( ',' );
        }

        private static String getCondition( String filter ) {

            EntityInfo ei = Entity.GetInfo( typeof( SiteLog ) );

            String t = ei.Dialect.GetTimeQuote();

            String fs = " Created between " + t + "{0}" + t + " and " + t + "{1}" + t + " order by Id desc";

            DateTime now = DateTime.Now;

            String condition = "";

            if (filter == "today")
                condition = string.Format( fs, now.ToShortDateString(), now.AddDays( 1 ).ToShortDateString() );
            else if (filter == "week")
                condition = string.Format( fs, now.AddDays( -7 ).ToShortDateString(), now.AddDays( 1 ).ToShortDateString() );
            else if (filter == "month")
                condition = string.Format( fs, now.AddMonths( -1 ).ToShortDateString(), now.AddDays( 1 ).ToShortDateString() );
            return condition;
        }

        private void bindPage( DataPage<SiteLog> list ) {

            IBlock block = getBlock( "list" );
            foreach (SiteLog l in list.Results) {

                block.Set( "log.UserName", l.User == null ? "" : l.User.Name );
                block.Set( "log.UserLink", l.User == null ? "" : toUser( l.User ) );
                block.Set( "log.Message", l.Message );
                block.Set( "log.DataInfo", l.DataInfo );
                block.Set( "log.DataType", l.DataType );
                block.Set( "log.Crated", l.Created );
                block.Set( "log.Ip", l.Ip );

                block.Next();
            }

            set( "page", list.PageBar );
        }


    }
}

