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

    public class TagHelper {


        public static Dictionary<String, String> getAttributes( String tag ) {

            tag = tag + " ";

            Dictionary<String, String> dic = new Dictionary<String, String>();

            TagAttrState st = new TagAttrState();

            for (int i = 0; i < tag.Length; i++) {
                st.OneStr += tag[i];

                if (tag[i] == '=') {
                    st.EqualFound = true;
                    continue;
                }

                if (st.EqualFound && st.ValueBegin == false) {

                    if (tag[i] == '"') {
                        st.ValueBegin = true;
                        st.IsQuoteBegin = true;
                        continue;
                    }

                    if (tag[i] != ' ') {
                        st.ValueBegin = true;
                        st.IsQuoteBegin = false;
                        continue;
                    }
                }

                if (st.IsQuoteBegin && tag[i] == '"') {
                    st.IsQuoteEnd = true;
                    continue;
                }

                if (i == tag.Length - 1) {
                    st.OneEnd = true;
                }
                else {

                    if (tag[i] == ' ' && st.EqualFound && st.ValueBegin) {

                        if (st.IsQuoteBegin == false) {
                            st.OneEnd = true;
                        }
                        else {
                            if (st.IsQuoteEnd) st.OneEnd = true;
                        }

                    }

                }


                if (st.OneEnd) {

                    String[] kvPair = st.GetPair();
                    if (kvPair != null) {
                        dic[kvPair[0]] = trimSingleQuote( kvPair[1] );
                    }

                    st = new TagAttrState();
                }

            }
            return dic;
        }

        private static String trimSingleQuote( String str ) {
            if (str == null) return null;
            return str.Trim().TrimStart( '\'' ).TrimEnd( '\'' ).Trim();
        }

    }


}
