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

namespace wojilu.Data {

    public class DbUtil {

        public static Object ProcessDefaultTime( Object objValue ) {

            if (objValue == null) return null;

            if (!(objValue is DateTime)) return objValue;


            DateTime time = (DateTime)objValue;
            if ((time < new DateTime( 2, 1, 1 )) || (time > new DateTime( 9000, 1, 1 ))) {
                return DateTime.Now;
            }

            return objValue;

        }

    }
}
