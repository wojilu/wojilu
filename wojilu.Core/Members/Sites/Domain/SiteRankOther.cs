/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Text;
using wojilu.Data;
using wojilu.ORM;

namespace wojilu.Members.Sites.Domain {


    [Serializable]
    public class SiteRankOther : CacheObject {

        private String _ranks;

        public String GetRankAllString( String seperator ) {
            StringBuilder builder = new StringBuilder();
            string[] strArray = Ranks.Split( new char[] { '/' } );
            for (int i = 0; i < strArray.Length; i++) {
                builder.Append( strArray[i] );
                if (i < (strArray.Length - 1)) {
                    builder.Append( " " );
                    builder.Append( seperator );
                    builder.Append( " " );
                }
            }
            return builder.ToString();
        }

        public String GetName( int i ) {
            string[] strArray = Ranks.Split( new char[] { '/' } );
            if (i < strArray.Length) {
                return strArray[i];
            }
            return null;
        }

        [NotSave]
        public int RankCount {
            get { return Ranks.Split( new char[] { '/' } ).Length; }
        }

        public String Ranks {
            get { return _ranks; }
            set { _ranks = value; }
        }

    }
}

