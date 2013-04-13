/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using wojilu.Web.Mvc;
using wojilu.Common.AppBase;
using wojilu.Members.Users.Domain;
using wojilu.Members.Sites.Domain;
using wojilu.Data;
using wojilu.Members.Users.Enum;
using wojilu.Common.Resource;
using wojilu.ORM;
using wojilu.OAuth;


namespace wojilu.Web.Controller.Admin.Members {

    public partial class UserController : ControllerBase {


        private void bindUserList( DataPage<User> list ) {
            IBlock block = getBlock( "list" );

            List<User> users = list.Results;
            List<UserConnect> connects = getUserConnects( users );

            foreach (User u in users) {
                block.Set( "user.Name", u.Name );
                block.Set( "user.RoleName", u.Role.Name );
                block.Set( "user.RealName", strUtil.SubString( u.RealName, 8 ) );

                String realNameInfo = strUtil.HasText( u.RealName ) ? "(" + strUtil.SubString( u.RealName, 8 ) + ")" : "";
                block.Set( "user.RealNameInfo", realNameInfo );

                String isEmailConfirm = getEmailConfirmStatus( u, connects );
                block.Set( "user.IsEmailConfirm", isEmailConfirm );
                String email = getUserEmail( u, connects );
                block.Set( "user.Email", email );

                block.Set( "user.CreateTime", u.Created.GetDateTimeFormats( 'g' )[0] );
                block.Set( "user.LastLoginTime", u.LastLoginTime );
                block.Set( "user.Id", u.Id );
                block.Set( "user.EditUrl", to( Edit, u.Id ) );
                block.Set( "user.Url", toUser( u ) );
                block.Set( "statusIcon", getStatusIcon( u ) );

                block.Set( "user.Ip", u.LastLoginIp );

                block.Next();
            }

            set( "page", list.PageBar );
        }

        private Boolean isBindConnects( User u, List<UserConnect> connects ) {
            foreach (UserConnect x in connects) {
                if (x.User != null && x.User.Id == u.Id) {
                    return true;
                }
            }
            return false;
        }

        private string getUserEmail( User u, List<UserConnect> connects ) {

            String email = u.Email;
            String bindAccounts = "";
            foreach (UserConnect x in connects) {
                if ( x.User != null && x.User.Id == u.Id) {
                    bindAccounts += getConnectName( x.ConnectType ) + ",";
                }
            }
            bindAccounts = bindAccounts.TrimEnd( ',' );

            if (strUtil.HasText( bindAccounts )) {
                email = string.Format( "<span><img src=\"{0}\" title=\"绑定帐号：{1}\" /></span> ", strUtil.Join( sys.Path.Img, "/s/external-link.png" ), bindAccounts ) + email;
            }

            return email;
        }

        private string getConnectName( string connectType ) {
            AuthConnectConfig x = AuthConnectConfig.GetByType( connectType );
            return x == null ? "" : x.Name;
        }

        private List<UserConnect> getUserConnects( List<User> users ) {

            if (users.Count == 0) return new List<UserConnect>();

            String ids = strUtil.GetIds( users );
            return UserConnect.find( "UserId in (" + ids + ")" ).list();
        }

        private string getEmailConfirmStatus( User user, List<UserConnect> connects ) {
            if (user.IsEmailConfirmed == EmailConfirm.Confirmed) return "√";
            if (user.IsEmailConfirmed == EmailConfirm.EmailError) {
                if (isBindConnects( user, connects )) {
                    return "--";
                }
                else {
                    return lang( "EmailError" );
                }
            }
            if (user.IsEmailConfirmed == EmailConfirm.UnConfirmed) return lang( "UnConfirmed" );
            if (user.IsEmailConfirmed == EmailConfirm.UnSendEmail) return lang( "UnSendEmail" );
            return "";
        }

        private String getStatusIcon( User user ) {

            if (user.Status == MemberStatus.Deleted) return "<img src=\"" + strUtil.Join( sys.Path.Img, "trash.gif" ) + "\"/>";
            if (user.Status == MemberStatus.Approving) return "--";
            if (user.Status == MemberStatus.Pick) return "★";
            return "";

        }

        private String getCondition() {

            String condition = "";
            String rolePrefix = "role_";
            String rankPrefix = "rank_";

            String filter = ctx.Get( "filter" );
            if (strUtil.HasText( filter )) {

                EntityInfo ei = Entity.GetInfo( typeof( User ) );

                String t = ei.Dialect.GetTimeQuote();
                String fs = "and Created between " + t + "{0}" + t + " and " + t + "{1}" + t + " ";
                String fsl = "and LastLoginTime between " + t + "{0}" + t + " and " + t + "{1}" + t + " order by LastLoginTime desc, Id desc";
                DateTime now = DateTime.Now;

                if (filter == "today")
                    condition += string.Format( fs, now.ToShortDateString(), now.AddDays( 1 ).ToShortDateString() );
                else if (filter == "week")
                    condition += string.Format( fs, now.AddDays( -7 ).ToShortDateString(), now.AddDays( 1 ).ToShortDateString() );
                else if (filter == "month")
                    condition += string.Format( fs, now.AddMonths( -1 ).ToShortDateString(), now.AddDays( 1 ).ToShortDateString() );
                else if (filter == "month3")
                    condition += string.Format( fs, now.AddMonths( -3 ).ToShortDateString(), now.AddDays( 1 ).ToShortDateString() );

                else if (filter == "todayl")
                    condition += string.Format( fsl, now.ToShortDateString(), now.AddDays( 1 ).ToShortDateString() );
                else if (filter == "weekl")
                    condition += string.Format( fsl, now.AddDays( -7 ).ToShortDateString(), now.AddDays( 1 ).ToShortDateString() );
                else if (filter == "monthl")
                    condition += string.Format( fsl, now.AddMonths( -1 ).ToShortDateString(), now.AddDays( 1 ).ToShortDateString() );
                else if (filter == "month3l")
                    condition += string.Format( fsl, now.AddMonths( -3 ).ToShortDateString(), now.AddDays( 1 ).ToShortDateString() );

                else if (filter == "UnSendEmail")
                    condition += "and IsEmailConfirmed=" + (int)EmailConfirm.UnSendEmail;
                else if (filter == "Confirmed")
                    condition += "and IsEmailConfirmed=" + (int)EmailConfirm.Confirmed;
                else if (filter == "UnConfirmed")
                    condition += "and IsEmailConfirmed=" + (int)EmailConfirm.UnConfirmed;
                else if (filter == "EmailError")
                    condition += "and IsEmailConfirmed=" + (int)EmailConfirm.EmailError;
                else if (filter == "bind")
                    condition += "and IsBind=1";


                else if (filter == "male")
                    condition += "and Gender=1 ";
                else if (filter == "female")
                    condition += "and Gender=2 ";

                else if (filter == "picked")
                    condition += "and Status=" + MemberStatus.Pick;

                else if (filter == "deleted")
                    condition += "and Status=" + MemberStatus.Deleted;

                else if (filter == "administrator")
                    condition += "and RoleId=" + SiteRole.Administrator.Id + " ";

                else if (filter.StartsWith( rolePrefix )) {
                    int roleId = cvt.ToInt( strUtil.TrimStart( filter, rolePrefix ) );
                    if (roleId > 0) {
                        condition += "and RoleId=" + roleId + " ";
                    }
                }
                else if (filter.StartsWith( rankPrefix )) {
                    int rankId = cvt.ToInt( strUtil.TrimStart( filter, rankPrefix ) );
                    if (rankId > 0) {
                        condition += "and RankId=" + rankId + " ";
                    }
                }

                set( "s.Name", "" );
                set( "s.RealName", "" );
                set( "s.Email", "" );

                return condition + " ";
            }

            String name = ctx.Get( "name" );
            set( "s.Name", name );
            if (strUtil.HasText( name )) {
                name = strUtil.SqlClean( name, 15 );
                condition += string.Format( "and Name like '%{0}%' ", name );
            }

            String rname = ctx.Get( "rname" );
            set( "s.RealName", rname );
            if (strUtil.HasText( rname )) {
                rname = strUtil.SqlClean( rname, 15 );
                condition += string.Format( "and RealName like '%{0}%' ", rname );
            }

            String email = ctx.Get( "email" );
            set( "s.Email", email );
            if (strUtil.HasText( email )) {
                name = strUtil.SqlClean( email, 30 );
                condition += string.Format( "and Email='{0}' ", name );
            }

            return strUtil.TrimStart( condition.Trim(), "and" );
        }

        private void bindReceiverList( List<User> users, String idsStr ) {

            String userStr = "";
            foreach (User user in users) userStr += user.Name + ", ";
            userStr = userStr.Trim().TrimEnd( ',' );
            set( "userList", userStr );
            set( "userIds", idsStr );
        }

        private void bindReceiverEmailList( List<User> users, String idsStr ) {

            String userStr = "";
            foreach (User user in users) {
                if (isEmailValid( user ) == false) continue;
                userStr += user.Name + "<span class='note'>&lt;" + user.Email + "&gt;</span>" + ", ";
            }

            userStr = userStr.Trim().TrimEnd( ',' );

            if (strUtil.IsNullOrEmpty( userStr )) {
                echoRedirectPart( lang( "exUserEmailInvalid" ) );
                return;
            }

            set( "userList", userStr );
            set( "userIds", idsStr );
        }

        //-----------------------------------------------------------------------------------------------------

        private Boolean isEmailValid( User user ) {

            if (user == null) return false;
            if (strUtil.IsNullOrEmpty( user.Email )) return false;

            Regex reg = new Regex( RegPattern.Email );
            return reg.IsMatch( user.Email );
        }

        private MsgInfo validateMsg() {
            return validateMsg( false );
        }

        private MsgInfo validateMsg( Boolean isHtml ) {

            String idsStr = ctx.PostIdList( "UserIds" );
            List<User> users = userService.GetByIds( idsStr );
            if (users.Count == 0) {
                errors.Add( lang( "exNoReceiver" ) );
                return null;
            }

            String title = ctx.Post( "Title" );
            String body = ctx.Post( "MsgBody" );
            if (isHtml) body = ctx.PostHtml( "MsgBody" );

            if (strUtil.IsNullOrEmpty( title )) errors.Add( lang( "exRequireTitle" ) );
            if (strUtil.IsNullOrEmpty( body )) errors.Add( lang( "exRequireContent" ) );

            MsgInfo msgInfo = new MsgInfo();
            msgInfo.Title = title;
            msgInfo.Body = body;
            msgInfo.Users = users;
            return msgInfo;
        }



        private void logUser( String msg, String ids ) {
            String dataInfo = "{Ids:[" + ids + "]}";
            logService.Add( (User)ctx.viewer.obj, msg, dataInfo, typeof( User ).FullName, ctx.Ip );
        }

        private void logUser( String msg, User user ) {
            String dataInfo = "{Id:" + user.Id + ", Name:'" + user.Name + "'}";
            logService.Add( (User)ctx.viewer.obj, msg, dataInfo, typeof( User ).FullName, ctx.Ip );
        }


        class MsgInfo {
            public String Title { get; set; }
            public String Body { get; set; }
            public List<User> Users { get; set; }
        }



        private void bindProfile( User m ) {

            set( "m.Id", m.Id );
            set( "m.Name", m.RealName );
            set( "m.NickName", m.Name );

            String kv = "Name=Value";
            dropList( "Year", AppResource.GetInts( 1910, 2009 ), kv, m.BirthYear );
            dropList( "Month", AppResource.GetInts( 1, 12 ), kv, m.BirthMonth );
            dropList( "Day", AppResource.GetInts( 1, 31 ), kv, m.BirthDay );

            dropList( "Gender", AppResource.Gender, kv, m.Gender );
            dropList( "Relationship", AppResource.Relationship, kv, m.Relationship );
            dropList( "Blood", AppResource.Blood, kv, m.Blood );
            dropList( "Degree", AppResource.Degree, kv, m.Degree );
            dropList( "Zodiac", AppResource.Zodiac, kv, m.Zodiac );

            dropList( "ProvinceId1", AppResource.Province, kv, m.ProvinceId1 );
            dropList( "ProvinceId2", AppResource.Province, kv, m.ProvinceId2 );

            set( "m.City1", m.City1 );
            set( "m.City2", m.City2 );

            set( "m.Birthday", string.Format( "{0}-{1}-{2}", m.BirthYear, m.BirthMonth, m.BirthDay ) );

            checkboxList( "Purpose", AppResource.Purpose, "Name=Name", m.Profile.Purpose );
            dropList( "ContactCondition", AppResource.ContactCondition, kv, m.Profile.ContactCondition );

            IBlock block = getBlock( "sexyInfo" );
            if (config.Instance.Site.ShowSexyInfoInProfile) {
                bindSexyInfo( block, m );
            }
            set( "m.Description", m.Profile.Description );
            set( "m.Signature", m.Signature );
        }

        private void bindSexyInfo( IBlock block, User m ) {
            String kv = "Name=Value";
            radioList( "Sexuality", AppResource.Sexuality, kv, m.Profile.Sexuality );
            dropList( "Smoking", AppResource.Smoking, kv, m.Profile.Smoking );
            dropList( "Sleeping", AppResource.Sleeping, kv, m.Profile.Sleeping );
            dropList( "Body", AppResource.Body, kv, m.Profile.Body );
            dropList( "Hair", AppResource.Hair, kv, m.Profile.Hair );
            dropList( "Height", AppResource.Height, kv, m.Profile.Height );
            dropList( "Weight", AppResource.Weight, kv, m.Profile.Weight );

            block.Set( "m.OtherInfo", m.Profile.OtherInfo );
            block.Next();
        }

    }
}

