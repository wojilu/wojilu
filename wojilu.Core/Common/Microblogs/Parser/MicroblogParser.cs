using System;
using System.Collections.Generic;
using System.Text;

namespace wojilu.Common.Microblogs.Parser {

    public class MicroblogParser {

        private static readonly char[] separators = new char[] { ' ', ':', '：', '@', ',', '，', '、', '/', '|' };

        private char[] charSrc;
        int i = 0;
        private IMicroblogBinder mb;
        private String _result;

        private int maxUserLength = 20; // 允许的最长用户名，超过则无效
        private int maxTagLength = 10;

        public MicroblogParser( String str, IMicroblogBinder mbinder ) {
            String content = str.Replace( Environment.NewLine, " " ).Replace( "\n", " " ).Replace( "\r", " " );
            this.charSrc = content.ToCharArray();
            this.mb = mbinder;
        }

        private void move() { i++; }
        private void back() { i--; }
        private char current() { return charSrc[i]; }
        private char next() { return charSrc[i + 1]; }
        private Boolean isEnd() { return i >= charSrc.Length; }
        private Boolean canNext() { return i < charSrc.Length-1; }

        private Boolean currentIsEmpty() {
            foreach (char ch in separators) {
                if (current() == ch) return true;
            }
            return false;
        }

        private Boolean currentNotEmpty() { return currentNotEmpty( current() ); }

        private Boolean currentNotEmpty( char mychar ) {
            foreach (char ch in separators) {
                if (mychar == ch) return false;
            }
            return true;
        }

        private StringBuilder sb = new StringBuilder();
        private List<String> userList = new List<String>();
        private List<String> tagList = new List<String>();

        private void processPrivate() {

            if (current() == '@') {
                processUserName();
            }
            else if (current() == '#') {
                processTag();
            }
            else if (isUrl()) {
                processUrl();
            }


            if (isEnd()) return;

            if (current() == '@' && canNext() && currentNotEmpty( next() )) {
            }
            else {
                sb.Append( current() );
                move();
            }

            if (i < charSrc.Length) {
                processPrivate();
            }

        }



        private void processUserName() {

            move();

            if (isEnd() || current() == '@') {
                back();
                return;
            }

            int beginIndex = i;
            moveToUserEnd( beginIndex );
            int endIndex = i;

            if (endIndex <= beginIndex) {
                return;
            }

            if (endIndex - beginIndex > maxUserLength) {
                sb.Append( "@" );
                i = beginIndex;
            }
            else {

                String user = getUserName( beginIndex, endIndex );
                sb.Append( mb.GetLink( user ) );

                if (userList.Contains( user ) == false) userList.Add( user );
            }
        }

        private void processUrl() {

            if (isEnd() || current() == ' ') {
                back();
                return;
            }

            int beginIndex = i;
            moveToUrlEnd( beginIndex );
            int endIndex = i;

            if (endIndex <= beginIndex) {
                return;
            }

            String url = getUserName( beginIndex, endIndex );
            sb.Append( mb.GetUrlLink( url ) );
            //if (isEnd() == false)
            //    sb.Append( " " );
            //move();
        }


        private bool isUrl() {
            if (i + 10 > charSrc.Length) return false; // 最短网址 http://a.cn
            if (charSrc[i] == 'h' &&
                charSrc[i + 1] == 't' &&
                charSrc[i + 2] == 't' &&
                charSrc[i + 3] == 'p' &&
                charSrc[i + 4] == ':' &&
                charSrc[i + 5] == '/' &&
                charSrc[i + 6] == '/') return true;

            return false;
        }





        private void processTag() {
            move();

            if (isEnd() || current() == '#') {
                back();
                return;
            }

            Boolean isErrorTag = false;
            int beginIndex = i;
            moveToTagEnd( beginIndex, ref isErrorTag );
            int endIndex = i;

            if (endIndex <= beginIndex) {
                return;
            }

            if (isErrorTag) {
                String tag = getUserName( beginIndex, endIndex );
                sb.Append( "#" );
                sb.Append( tag );
                sb.Append( "#" );
                move();

            }

            else if (endIndex - beginIndex > maxTagLength) {
                sb.Append( "#" );
                i = beginIndex;
            }
            else {

                String tag = getUserName( beginIndex, endIndex );
                sb.Append( mb.GetTagLink( tag ) );
                move();

                if (tagList.Contains( tag ) == false) tagList.Add( tag );
            }

        }



        private void moveToUrlEnd( int beginIndex ) {
            Boolean begin = false;
            while (true) {


                if (begin && current() == ' ') break;

                move();
                if (isEnd()) break;

                if (!begin && current() == ' ') {
                    i = beginIndex - 1;
                    return;
                }

                if (current() != ' ') begin = true;

            }
        }

        private void moveToTagEnd( int beginIndex, ref Boolean isErrorTag ) {
            Boolean begin = false;
            while (true) {


                if (begin && current() == '#') break;

                // 处理emotion和@
                if (current() == '[' || current()=='@') {
                    isErrorTag = true;
                }

                move();
                if (isEnd()) break;

                if (!begin && current() == '#') {
                    i = beginIndex - 1;
                    return;
                }

                if (current() != '#') begin = true;

            }
        }

        private void moveToUserEnd( int beginIndex ) {
            Boolean begin = false;
            while (true) {

                if (currentNotEmpty()) begin = true;

                if (begin && currentIsEmpty()) break;

                move();
                if (isEnd()) break;

                if (!begin && current() == '@') {
                    i = beginIndex - 1;
                    return;
                }


            }
        }




        private string getUserName( int beginIndex, int endIndex ) {
            String user = "";
            Boolean begin = false;
            for (int k = beginIndex; k < endIndex; k++) {
                if (charSrc[k] == ' ' && begin == false) continue;
                user += charSrc[k];
                begin = true;
            }
            return user;
        }


        public String Process() {
            processPrivate();
            _result = sb.ToString();
            return _result;
        }

        public override string ToString() {
            return _result;
        }

        public List<String> GetUserList() {
            return userList;
        }

        public List<String> GetTagList() {
            return tagList;
        }

    }

}
