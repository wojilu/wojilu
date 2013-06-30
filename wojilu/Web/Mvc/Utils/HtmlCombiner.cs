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

namespace wojilu.Web.Mvc.Utils {

    /// <summary>
    /// 合并 action 和 layout 的内容
    /// </summary>
    public class HtmlCombiner {

        public static String combinePage( String layoutString, String pageString ) {
            if (strUtil.IsNullOrEmpty( layoutString )) {
                return pageString;
            }
            if (layoutString.IndexOf( "#{layout_content}" ) < 0) {
                return pageString;
            }
            return layoutString.Replace( "#{layout_content}", pageString );
        }

        //public static String combinePage( Template layoutTemplate, String actionContent ) {

        //    if (layoutTemplate == null) return actionContent;
        //    if (layoutTemplate.IsTemplateExist() == false) return actionContent;

        //    layoutTemplate.Set( "layout_content", actionContent );
        //    return layoutTemplate.ToString();
        //}

    }
}
