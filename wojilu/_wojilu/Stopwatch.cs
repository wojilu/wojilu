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
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace wojilu {

    /// <summary>
    /// 秒表，用于测试程序耗时
    /// </summary>
    public class Stopwatch {

        private Boolean _IsStop;
        private long frequency;
        private decimal multiplier = 1000000000M;
        private long start;
        private long stop;

        [DllImport( "KERNEL32" )]
        private static extern Boolean QueryPerformanceCounter( out long lpPerformanceCount );

        [DllImport( "Kernel32.dll" )]
        private static extern Boolean QueryPerformanceFrequency( out long lpFrequency );

        public Stopwatch() {
            if (!QueryPerformanceFrequency( out this.frequency )) {
                throw new Win32Exception();
            }
        }

        /// <summary>
        /// 开始启动
        /// </summary>
        public void Start() {
            this._IsStop = false;
            QueryPerformanceCounter( out this.start );
        }

        /// <summary>
        /// 停止
        /// </summary>
        public void Stop() {
            if (!this._IsStop) {
                QueryPerformanceCounter( out this.stop );
                this._IsStop = true;
            }
        }

        /// <summary>
        /// 总共耗时(毫秒)
        /// </summary>
        public double ElapsedMilliseconds {
            get {
                double result = this.getDuration() / 1000000.0;
                if (result < 0) result = 0;
                return result;
            }
        }

        /// <summary>
        /// 总共耗时(秒)
        /// </summary>
        public double ElapsedSeconds {
            get {
                double result = this.ElapsedMilliseconds / 1000.0;
                if (result < 0) result = 0;
                return result;
            }
        }

        private double getDuration() {
            return (((this.stop - this.start) * ((double)this.multiplier)) / ((double)this.frequency));
        }

    }
}

