/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Members.Users.Domain;
using wojilu.Apps.Blog.Domain;
using wojilu.Apps.Photo.Domain;
using wojilu.Apps.Forum.Domain;
using wojilu.Common.Feeds.Domain;

namespace wojilu.Web.Controller.Users.Admin {

    public class FeedType {

        private static Dictionary<int, string> typeTable = loadTable();

        private static Dictionary<int, string> loadTable() {
            Dictionary<int, string> tbl = new Dictionary<int, string>();
            tbl.Add( 2, typeof( BlogPost ).FullName );
            tbl.Add( 3, typeof( PhotoPost ).FullName );
            tbl.Add( 4, typeof( ForumPost ).FullName );
            tbl.Add( 5, typeof( Share ).FullName );
            tbl.Add( 6, typeof( FriendShip ).FullName );
            return tbl;
        }

        public static String GetShareType() {
            return GetByInt( 5 );
        }

        public static String GetByInt( int id ) {
            if (typeTable.ContainsKey( id ))
                return typeTable[id];
            return null;
        }

        public static int Get( Type t ) {
            if (t == null) return -1;
            return GetIntByName( t.FullName );
        }

        public static int GetIntByName( String name ) {
            foreach (KeyValuePair<int, string> pair in typeTable) {
                if (pair.Value.Equals( name )) return pair.Key;
            }
            return -1;
        }

    }

}
