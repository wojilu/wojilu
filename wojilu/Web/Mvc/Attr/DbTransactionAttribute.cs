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
using System.Data;

using wojilu.Web.Context;
using wojilu.Web.Mvc.Interface;
using wojilu.Data;

namespace wojilu.Web.Mvc.Attr {

    /// <summary>
    /// 在当前 action 上启用数据库事务，支持多数据库事务
    /// </summary>
    [Serializable, AttributeUsage( AttributeTargets.Method )]
    public class DbTransactionAttribute : Attribute, IActionFilter {

        private static readonly ILog logger = LogManager.GetLogger( typeof( DbTransactionAttribute ) );

        public void BeforeAction( ControllerBase controller ) {

            DbContext.beginAndMarkTransactionAll();
        }

        public void AfterAction( ControllerBase controller ) {

            if (controller.ctx.utils.getException() == null) {
                DbContext.commitAll();
            }
            else {
                try {
                    DbContext.rollbackAll();
                }
                catch (Exception ex) {
                    logger.Error( "data operation ( rollbackAll ):" + ex.StackTrace );
                    throw ex;
                }
                finally {
                    logger.Info( "DbTransaction : rollbackAll" );
                    DbContext.closeConnectionAll();
                }
            }

        }

        public int Order { get; set; }

    }

}
