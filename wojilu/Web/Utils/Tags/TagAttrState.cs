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

namespace wojilu.Web.Utils.Tags {

    internal class TagAttrState {

        public String OneStr = "";
        public Boolean EqualFound = false;
        public Boolean OneEnd = false;

        public Boolean ValueBegin = false;
        public Boolean IsQuoteBegin = false;
        public Boolean IsQuoteEnd = false;

        public String[] GetPair() {
            String[] arr = this.OneStr.Split( new char[] { '=' }, 2  );
            if (arr.Length != 2) return null;

            String key = arr[0].Trim();
            String val = arr[1].Trim().TrimStart( '"' ).TrimEnd( '"' ).Trim();

            String[] result = new string[2];
            result[0] = key;
            result[1] = val;
            return result;
        }

    }

}
