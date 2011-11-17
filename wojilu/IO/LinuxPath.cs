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
using System.Web;
using wojilu.Web;

namespace wojilu.IO {

    internal class LinuxPath : PathTool {

        public override String CombineAbs( String[] arrPath ) {

            if (arrPath.Length == 0) return "";

            String result = arrPath[0];
            for (int i = 1; i < arrPath.Length; i++) {
                if (strUtil.IsNullOrEmpty( arrPath[i] )) continue;
                result = strUtil.Join( result, arrPath[i].Replace( "\\", "/" ) );
            }
            return result;

        }

        public override String Map( String path ) {

            if (strUtil.IsNullOrEmpty( path )) return "";

            if (SystemInfo.IsWeb == false) {

                return strUtil.Join( AppDomain.CurrentDomain.BaseDirectory, path );
            }
            else {

                String str = path;
                if (path.ToLower().StartsWith( SystemInfo.ApplicationPath ) == false)
                    str = strUtil.Join( SystemInfo.ApplicationPath, path );

                return HttpContext.Current.Server.MapPath( path );
            }
        }
    }

}
