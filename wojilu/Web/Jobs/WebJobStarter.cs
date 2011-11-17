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
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;

using wojilu.Data;
using wojilu.DI;

namespace wojilu.Web.Jobs {

    /// <summary>
    /// 计划任务启动器
    /// </summary>
    public class WebJobStarter {

        private static readonly ILog logger = LogManager.GetLogger( typeof( WebJobStarter ) );
        private static Dictionary<WebJob, Timer> timers = new Dictionary<WebJob, Timer>();

        private static readonly Object objlock = new object();

        private static Boolean isStart = false;

        public static void Init() {

            if (isStart == false) {

                lock (objlock) {

                    if( isStart == false ) {
                        startJob();
                        isStart = true;
                    }

                }
            }
        }

        private static void startJob() {

            IList jobs = new WebJob().findAll();
            foreach (WebJob job in jobs) {

                if (job.Interval <= 0) {
                    logger.Warn( "job is invalid, name:" + job.Name + ", type:" + job.Type + ", interval:" + job.Interval );
                    continue;
                }

                if (job.IsRunning == false) continue;

                timers.Add( job, new Timer( new TimerCallback( jobCallback ), job, job.Interval, job.Interval ) );
            }
        }

        public static void jobCallback( object state ) {

            WebJob job = (WebJob)state;
            Timer mytimer = timers[job];

            mytimer.Change( Timeout.Infinite, job.Interval );

            IWebJobItem obj = ObjectContext.GetByType( job.Type ) as IWebJobItem;

            if (obj == null) {
                logger.Info( "job[" + job.Name + "] type does not exists:" + job.Type );
                return;
            }

            try {
                obj.Execute();
            }
            catch (Exception ex) {
                logger.Error( ex.Message + Environment.NewLine + ex.StackTrace );
            }
            finally {
                obj.End();
                DbContext.closeConnectionAll();
                mytimer.Change( job.Interval, job.Interval );
            }

        }
    }


}
