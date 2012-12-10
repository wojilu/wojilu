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

namespace wojilu.Data {

    /// <summary>
    /// 各种数据库的特殊语法处理接口
    /// </summary>
    public interface IDatabaseDialect {

        String GetConnectionItem( String connectionString, ConnectionItemType connectionItem );
        String GetLimit( String sql, int limit );
        String GetLimit( String sql );

        String GetParameter( String parameterName );
        String GetParameterAdder( String parameterName );

        String GetTimeQuote();

        String GetLeftQuote();
        String GetRightQuote();


        String Top { get; }

    }
}

