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
using wojilu.DI;
using wojilu.Web.Mvc.Interface;

namespace wojilu.Web.Mvc {

    /// <summary>
    /// mvc ¹ýÂËÆ÷µÄ¼ÓÔØÆ÷
    /// </summary>
    public class MvcFilterLoader {

        public static void Init() {

            List<String> filter = MvcConfig.Instance.Filter;

            foreach (String t in filter) {

                IMvcFilter f = ObjectContext.GetByType( t ) as IMvcFilter;
                if (f == null) continue;
                f.Process( MvcEventPublisher.Instance );

            }

        }

    }

}
