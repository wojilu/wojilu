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
using System.Data;
using wojilu.Data;

namespace wojilu.ORM.Operation {

    internal class PageCondition {

        private static readonly ILog logger = LogManager.GetLogger( typeof( PageCondition ) );

        public String ConditionStr { get; set; }
        public String Property { get; set; }

        public int CurrentPage { get; set; }
        public int Size { get; set; }
        public String OrderStr { get; set; }
        public PageHelper Pager { get; set; }

        public int beginCount( String countSql, PageHelper pager, EntityInfo entityInfo ) {

            String commandText = countSql;
            logger.Info( "[Page Record Count] " + commandText );
            IDbCommand command = DataFactory.GetCommand( commandText, DbContext.getConnection( entityInfo ) );
            pager.RecordCount = cvt.ToInt( command.ExecuteScalar() );
            pager.computePageCount();
            pager.resetCurrent();

            return pager.getCurrent();
        }

    }

}
