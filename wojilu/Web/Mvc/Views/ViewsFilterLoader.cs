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
using wojilu.Web.Context;

namespace wojilu.Web.Mvc {



    public class ViewsFilterLoader {

        private static List<IViewsFilter> filterList = loadFilterList();

        public static List<IViewsFilter> GetFilterList() {
            return filterList;
        }

        private static List<IViewsFilter> loadFilterList() {

            List<IViewsFilter> list = new List<IViewsFilter>();

            foreach (String filterType in MvcConfig.Instance.ViewsFilter) {

                if (strUtil.IsNullOrEmpty( filterType )) continue;

                IViewsFilter v = ObjectContext.GetByType( filterType ) as IViewsFilter;
                if (v == null) {
                    throw new NullReferenceException( "views filter:" + filterType );
                }

                list.Add( v );
            }

            return list;
        }

    }





}
