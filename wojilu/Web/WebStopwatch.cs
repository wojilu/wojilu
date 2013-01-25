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
using System.Web;

namespace wojilu.Web {

    /// <summary>
    /// web 计时秒表
    /// </summary>
    public class WebStopwatch {

        // 默认的秒表名称
        private const String stopwatchstring = "stopwatch";

        public static void Start() {
            Start( stopwatchstring );
        }

        //public static Stopwatch Stop() {
        //    return Stop( stopwatchstring );
        //}

        //// 在一个页面流程内部，要使用多个秒表时，用名称以区分
        //public static void Start( String stopwatchName ) {
        //    Stopwatch stopwatch = new Stopwatch();
        //    stopwatch.Start();
        //    stopwatch.Stop();
        //    stopwatch.Start();
        //    WebContext.Current.Items[stopwatchName] = stopwatch;
        //}

        //public static Stopwatch Stop( String stopwatchName ) {
        //    Stopwatch stopwatch = WebContext.Current.Items[stopwatchName] as Stopwatch;
        //    if (stopwatch == null) {
        //        Start();
        //    }
        //    stopwatch.Stop();
        //    return stopwatch;
        //}

        public static System.Diagnostics.Stopwatch Stop() {
            return Stop( stopwatchstring );
        }

        // 在一个页面流程内部，要使用多个秒表时，用名称以区分
        public static void Start( String stopwatchName ) {
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Reset();
            stopwatch.Start();
            CurrentRequest.setItem( stopwatchName, stopwatch );
        }

        public static System.Diagnostics.Stopwatch Stop( String stopwatchName ) {
            System.Diagnostics.Stopwatch stopwatch = CurrentRequest.getItem( stopwatchName ) as System.Diagnostics.Stopwatch;
            if (stopwatch == null) {
                Start();
            }
            stopwatch.Stop();
            return stopwatch;
        }

    }
}

