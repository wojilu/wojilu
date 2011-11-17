using System;
using System.Collections.Generic;
using System.Text;

namespace wojilu.Apps.Forum.Domain {

    [Serializable]
    public class LastUpdateInfo {

        public String CreatorName { get; set; }
        public String CreatorUrl { get; set; }

        public int PostId { get; set; }
        public String PostTitle { get; set; }
        public String PostType { get; set; }

        public DateTime UpdateTime { get; set; }

        private Boolean _isEmpty = true;

        public Boolean IsEmpty() {
            return _isEmpty;
        }

        public LastUpdateInfo() {
        }

        public LastUpdateInfo( String savedString ) {

            if (strUtil.IsNullOrEmpty( savedString )) return;


            string[] arr = savedString.Split( '|' );
            if (arr.Length != 6) return;

            PostId = cvt.ToInt( arr[0] );
            PostType = arr[1];
            PostTitle = arr[2];
            CreatorName = arr[3];
            CreatorUrl = arr[4];
            UpdateTime = cvt.ToTime( arr[5] );

            if (PostId <= 0) return;

            _isEmpty = false;

        }

        public String ToSavedString() {
            StringBuilder builder = new StringBuilder();
            builder.Append( this.PostId );
            builder.Append( "|" );
            builder.Append( this.PostType );
            builder.Append( "|" );
            builder.Append( this.PostTitle );
            builder.Append( "|" );
            builder.Append( this.CreatorName );
            builder.Append( "|" );
            builder.Append( this.CreatorUrl );
            builder.Append( "|" );
            builder.Append( this.UpdateTime );
            return builder.ToString();
        }


    }

}
