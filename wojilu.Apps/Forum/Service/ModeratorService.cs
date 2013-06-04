/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;

using wojilu.Web.Mvc;
using wojilu.Apps.Forum.Domain;
using wojilu.Members.Users.Service;
using wojilu.Members.Users.Domain;
using wojilu.Apps.Forum.Interface;
using wojilu.Members.Users.Interface;

namespace wojilu.Apps.Forum.Service {


    public class ModeratorService : IModeratorService {

        public virtual IUserService UserService { get; set; }

        public ModeratorService() {
            UserService = new UserService();
        }

        public virtual void AddModerator( ForumBoard fb, String moderatorName ) {
            String val = this.GetModeratorText( fb.Moderator ) + " " + moderatorName;
            this.setModerator( fb, val );
        }

        private void setModerator( ForumBoard fb, String val ) {
            if (!strUtil.IsNullOrEmpty( val )) {
                fb.Moderator = this.getModeratorPropertyAll( val );
                db.update( fb, "Moderator" );
            }
        }

        private String getModeratorPropertyAll( String val ) {
            string[] strArray = val.Split( new char[] { ' ', '/', '|', '，', '、' } );
            StringBuilder sb = new StringBuilder();
            foreach (String str in strArray) {
                if (strUtil.HasText( str )) {
                    this.appendModerator( sb, str );
                }
            }
            return sb.ToString().TrimEnd( '|' );
        }

        private void appendModerator( StringBuilder sb, String strMember ) {
            User user = UserService.GetByName( strMember.Trim() );
            if (user != null) {
                sb.AppendFormat( "{0}:{1}|", user.Name, user.Url );
            }
        }

        //--------------------------------------------------------------------------------------


        public virtual void DeleteModerator( ForumBoard fb, String moderatorName ) {
            string[] arrM = fb.Moderator.Split( '|' );
            String str = this.getNewModeratorProperty( moderatorName, arrM );
            fb.Moderator = str;
            db.update( fb, "Moderator" );
        }

        private String getNewModeratorProperty( String moderatorName, string[] arrM ) {
            StringBuilder sb = new StringBuilder();
            foreach (String str in arrM) {
                if (!this.isContinue( str )) {
                    this.addNewModerator_One( moderatorName, sb, str );
                }
            }
            return sb.ToString().TrimEnd( '|' );
        }

        private void addNewModerator_One( String moderatorName, StringBuilder sb, String m ) {
            string[] strArray = m.Trim().Split( new char[] { ':' } );
            if (!strArray[0].Trim().Equals( moderatorName )) {
                sb.AppendFormat( "{0}:{1}|", strArray[0], strArray[1] );
            }
        }

        //------------------------------------------------------------------------------------

        public virtual String GetModeratorHtml( ForumBoard fb ) {

            if (strUtil.IsNullOrEmpty( fb.Moderator )) {
                return string.Empty;
            }
            string[] arrM = fb.Moderator.Split( '|' );
            return this.getHtml( arrM );
        }

        public virtual String[] GetModeratorNames( ForumBoard fb ) {

            if (strUtil.IsNullOrEmpty( fb.Moderator )) {
                return new String[] { };
            }
            String[] arrNames = fb.Moderator.Split( '|' );
            String[] names = new String[arrNames.Length];
            for (int i = 0; i < arrNames.Length; i++) {

                String[] arrItem = arrNames[i].Split( ':' );
                names[i] = arrItem[0];

            }
            return names;
        }

        public virtual String GetModeratorJson( ForumBoard fb ) {
            String[] arrName = GetModeratorNames( fb );
            if (arrName.Length == 0) return "[]";
            String str = "[";
            for (int i = 0; i < arrName.Length; i++) {
                str += "'"+arrName[i]+"'";
                if (i < arrName.Length - 1) str += ",";
            }
            str += "]";
            return str;
        }

        private String getHtml( string[] arrM ) {
            StringBuilder sb = new StringBuilder( alang.get( typeof( ForumApp ), "moderator" ) + ": " );
            foreach (String user in arrM) {
                if (!this.isContinue( user )) {
                    this.getHtml_one( sb, user );
                }
            }
            return sb.ToString().Trim();
        }

        private void getHtml_one( StringBuilder sb, String m ) {
            User member = new User();
            string[] strArray = m.Trim().Split( new char[] { ':' } );
            member.Name = strArray[0];
            member.Url = strArray[1];
            sb.AppendFormat( "<a href=\"{0}\">{1}</a>", Link.ToMember( member ), member.Name );
            sb.Append( " " );
        }

        //--------------------------------------------------------

        public virtual List<User> GetModeratorList( ForumBoard fb ) {
            if (strUtil.IsNullOrEmpty( fb.Moderator )) {
                return new List<User>();
            }
            string[] arrM = fb.Moderator.Split( '|' );
            return this.getModeratorFromString( arrM );
        }

        private List<User> getModeratorFromString( string[] arrM ) {
            List<User> results = new List<User>();
            foreach (String str in arrM) {
                if (!this.isContinue( str )) {
                    this.loadOneModerator( results, this.getModeratorName( str ) );
                }
            }
            return results;
        }

        private void loadOneModerator( List<User> results, String userName ) {
            User user = UserService.GetByName( userName );
            if (user != null) {
                results.Add( user );
            }
        }

        private Boolean isContinue( String m ) {
            return (strUtil.IsNullOrEmpty( m ) || (m.Trim().Split( new char[] { ':' } ).Length != 2));
        }

        //-----------------------------------------------------------

        public virtual Boolean IsModerator( int appId, String userName ) {

            List<ForumBoard> boardList = ForumBoard.find( "AppId=" + appId ).list();

            foreach (ForumBoard bd in boardList) {
                Boolean isModerator = this.IsModerator( bd.Moderator, userName );
                if (isModerator) return true;
            }

            return false;
        }

        public virtual Boolean IsModerator( ForumBoard fb, User user ) {

            return this.IsModerator( fb.Moderator, user.Name );
        }

        public virtual Boolean IsModerator( ForumBoard fb, String memberName ) {
            return this.IsModerator( fb.Moderator, memberName );
        }

        private Boolean IsModerator( String fboardModeratorProperty, String memberName ) {
            if (strUtil.IsNullOrEmpty( fboardModeratorProperty )) {
                return false;
            }
            string[] arrModerator = this.GetModeratorText( fboardModeratorProperty ).Split( new char[] { ' ' } );
            return this.loopCheckIsExits( arrModerator, memberName );
        }

        private Boolean loopCheckIsExits( string[] arrModerator, String memberName ) {
            foreach (String str in arrModerator) {
                if (strUtil.HasText( str ) && str.Equals( memberName )) {
                    return true;
                }
            }
            return false;
        }

        //-----------------------------------------------------------

        public virtual String GetModeratorText( String moderatorRawPerperty ) {
            if (strUtil.IsNullOrEmpty( moderatorRawPerperty )) {
                return string.Empty;
            }
            string[] arrM = moderatorRawPerperty.Split( '|' );
            return this.getModeratorTextFromArray( arrM );
        }

        private String getModeratorTextFromArray( string[] arrM ) {
            StringBuilder sb = new StringBuilder();
            foreach (String str in arrM) {
                this.appendModeratorTextOne( sb, str );
            }
            return sb.ToString().Trim();
        }

        private void appendModeratorTextOne( StringBuilder sb, String m ) {
            if (!this.isContinue( m )) {
                sb.Append( this.getModeratorName( m ) );
                sb.Append( " " );
            }
        }

        private String getModeratorName( String rawString ) {
            return rawString.Trim().Split( new char[] { ':' } )[0].Trim();
        }

    }
}

