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
            return (this.index >= this.charList.Length - 1);
        }

    }

}
