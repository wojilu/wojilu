/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web;
using wojilu.Web.Mvc;

namespace wojilu.Common.AppBase {


    public class AccessStatusUtil {

        private String _name;
        private int _value;

        public String Name {
            get { return _name; }
            set { _name = value; }
        }

        public int Value {
            get { return _value; }
            set { _value = value; }
        }

        public AccessStatusUtil( int val, String name ) {
            _value = val;
            _name = name;
        }

        public static AccessStatus GetPostValue( int accessStatus ) {
            return (AccessStatus)Enum.Parse( typeof( AccessStatus ), accessStatus.ToString() );
        }

        public static String GetRadioList( AccessStatus status ) {
            return GetRadioList( (int)status );
        }

        public static String GetRadioList( int statusId ) {
            return Html.RadioList( getAll(), "AccessStatus", "Name", "Value", statusId );
        }

        private static List<AccessStatusUtil> getAll() {

            List<AccessStatusUtil> list = new List<AccessStatusUtil>();
            list.Add( new AccessStatusUtil( 0, lang.get( "statusPublic" ) ) );
            list.Add( new AccessStatusUtil( 1, lang.get( "statusFriend" ) ) );
            list.Add( new AccessStatusUtil( 2, lang.get( "statusPrivate" ) ) );
            return list;

        }


    }
}

