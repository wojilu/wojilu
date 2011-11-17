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
using wojilu.Web;
using System.Web;

namespace wojilu.IO {

    internal abstract class PathTool {

        public abstract String CombineAbs( String[] arrPath );
        public abstract String Map( String path );

        public static PathTool getInstance() {
            if (SystemInfo.IsWindows) return new WindowsPath();
            return new LinuxPath();
        }

        /// <summary>
        /// bin 的绝对路径
        /// </summary>
        /// <returns></returns>
        public static String GetBinDirectory() {
            if (SystemInfo.IsWeb) {
                return HttpRuntime.BinDirectory;
            }

            return AppDomain.CurrentDomain.BaseDirectory;
        }

    }

}
