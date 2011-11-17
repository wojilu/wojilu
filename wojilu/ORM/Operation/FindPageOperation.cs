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
using System.Data;
using System.Web;
using wojilu;
using wojilu.Data;
using wojilu.Web;

namespace wojilu.ORM.Operation {

    internal class FindPageOperation {

        private static readonly ILog logger = LogManager.GetLogger( typeof( FindPageOperation ) );
                
        public static IList FindPage( ObjectInfo state ) {
            return FindPage( state, "" );
        }

        public static IList FindPage( ObjectInfo state, String queryString ) {

            // see: wojilu/wojilu/Web/Mvc/Routes/RouteTool.cs, line 211

            if (state.Pager.getCurrent() <= 0) {
                int page = CurrentRequest.getCurrentPage();
                state.Pager.setCurrent( page );
            }

            if ( queryString != null && queryString.ToLower().StartsWith( "order " )) {
                queryString = " " + queryString;
            }

            PageCondition pc = new PageCondition();
            pc.ConditionStr = queryString;
            pc.Property = state.Includer.SelectedProperty;
            pc.CurrentPage = state.Pager.getCurrent();
            pc.Size = state.Pager.getSize();
            pc.OrderStr = state.Order;
            pc.Pager = state.Pager;

            String sql = new SqlBuilder( state.EntityInfo ).GetPageSql( pc );
            return EntityPropertyUtil.FindList( state, sql );
        }




    }
}

