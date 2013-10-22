/*
 * Copyright 2010 www.wojilu.com
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *      http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace wojilu.Web.Templates {

    internal class CharSource {

        public char[] charList { get; set; }

        private int index = 0;

        public int getIndex() {
            return this.index;
        }

        public CharSource( String str ) {

            if (strUtil.IsNullOrEmpty( str )) return;

            this.charList = str.ToCharArray();
        }

        public void move() {
            this.index++;
        }

        public void move( int step ) {
            this.index = this.index + step;
        }

        public bool isCode() {
            if (this.charList.Length < this.index + 4) return false;
            return this.charList[index] == '<' &&
                this.charList[index + 1] == '%';
        }

        public bool isCodeEnd() {
            if (this.charList.Length < this.index + 2) return false;
            return this.charList[index] == '%' &&
                this.charList[index + 1] == '>';
        }

        public Boolean isFunction() {
            //<script runat=""server"">
            if (this.charList.Length < this.index + 22) return false;
            //<script
            return this.charList[index] == '<' &&
                this.charList[index + 1] == 's' &&
                this.charList[index + 2] == 'c' &&
                this.charList[index + 3] == 'r' &&
                this.charList[index + 4] == 'i' &&
                this.charList[index + 5] == 'p' &&
                this.charList[index + 6] == 't'&&
            this.charList[index + 7] == ' ' &&
            this.charList[index + 8] == 'r' &&
            this.charList[index + 9] == 'u' &&
            this.charList[index + 10] == 'n'&&
            this.charList[index + 11] == 'a' &&
            this.charList[index + 12] == 't' &&
            this.charList[index + 13] == '=' &&
            this.charList[index + 14] == '"' &&
            this.charList[index + 15] == 's' &&
            this.charList[index + 16] == 'e' &&
            this.charList[index + 17] == 'r' &&
            this.charList[index + 18] == 'v' &&
            this.charList[index + 19] == 'e' &&
            this.charList[index + 20] == 'r' &&
            this.charList[index + 21] == '"';
        }

        public bool isFunctionEnd() {
            // </script>
            if (this.charList.Length < this.index + 9) return false;
            return this.charList[index] == '<' &&
                this.charList[index + 1] == '/' &&
                this.charList[index + 2] == 's' &&
                this.charList[index + 3] == 'c' &&
                this.charList[index + 4] == 'r' &&
                this.charList[index + 5] == 'i' &&
                this.charList[index + 6] == 'p' &&
                this.charList[index + 7] == 't' &&
                this.charList[index + 8] == '>';
        }

        public Boolean isBlock() {

            // 20个字符=begin+end所有
            if (this.charList.Length < this.index + 20) return false;

            return this.charList[index] == '<' &&
                this.charList[index + 1] == '!' &&
                this.charList[index + 2] == '-' &&
                this.charList[index + 3] == '-' &&
                this.charList[index + 4] == ' ' &&
                this.charList[index + 5] == 'B' &&
                this.charList[index + 6] == 'E' &&
                this.charList[index + 7] == 'G' &&
                this.charList[index + 8] == 'I' &&
                this.charList[index + 9] == 'N' &&
                this.charList[index + 10] == ' ';
        }

        public Boolean isBlockEnd() {
            return this.charList[index] == '<' &&
              this.charList[index + 1] == '!' &&
              this.charList[index + 2] == '-' &&
              this.charList[index + 3] == '-' &&
              this.charList[index + 4] == ' ' &&
              this.charList[index + 5] == 'E' &&
              this.charList[index + 6] == 'N' &&
              this.charList[index + 7] == 'D';
        }

        public Boolean moveBegin() {

            String lblBegin = "<!-- BEGIN ";

            StringBuilder sb = new StringBuilder();
            for (int i = 1; i <= lblBegin.Length; i++) {
                sb.Append( this.current() );
                this.move();
            }

            String movStr = sb.ToString();

            if (movStr == lblBegin)
                return true;
            else {
                this.back( lblBegin.Length );
                return false;
            }
        }

        public void back() {
            this.index--;
        }

        public void back( int step ) {
            this.index = this.index - step;
        }

        public char current() {
            return this.charList[index];
        }

        public Boolean isEnd() {
            return (this.index > this.charList.Length - 1);
        }


    }

}
