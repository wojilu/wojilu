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


namespace wojilu.Data {

    internal class DbConst {

        public static List<String> SqlKeyWords = getSqlKeyWords();

        private static List<String> getSqlKeyWords() {

            String[] strArray = "action/absolute/any/add/are/admindb/as/all/asc/alphanumeric/assertion/alter/authorization/alter/table/autoincrement/and/avg/as/begin/both/collation/between/column/binary/commit/bit/bit_length/comp/compression/connect/boolean/connection/constraint/constraints/by/container/byte/contains/cascade/convert/catalog/count/char/character/counter/char_length/create/character_length/currency/check/current_date/close/current_time/clustered/current_timestamp/coalesce/current_user/collate/cursor/database/disallow/date/disconnect/datetime/distinct/day/distinctrow/dec/decimal/domain/declare/double/delete/drop/desc/eqv/foreign/exclusiveconnect/from/exec/execute/exists/general/extract/grant/false/group/fetch/guid/first/having/float/float8/hour/float4/identity/input/ieeedouble/insensitive/ieeesingle/insert/ignore/insert/into/image/int/integer/integer4/imp/integer1/integer2/interval/index/indexcreatedb/is/inner/isolation/join/longtext/key/lower/language/match/last/max/left/memo/level/min/like/minute/logical/logical1/mod/long/money/longbinary/month/longchar/national/outer/nchar/output/nonclustered/owneraccess/not/pad/ntext/parameters/null/partial/number/password/numeric/percent/nvarchar/pivot/octet_length/position/oleobject/precision/on/prepare/open/primary/option/privileges/or/proc/procedure/order/public/smalldatetime/references/smallint/restrict/smallmoney/revoke/some/right/space/rollback/sql/schema/sqlcode/sqlerror/sqlstate/second/stdev/select/stdevp/selectschema/string/selectsecurity/substring/set/sum/short/sysname/single/system_user/size/table/updateowner/tableid/updatesecurity/temporary/upper/text/usage/time/user/timestamp/using/timezone_hour/value/timezone_minute/values/tinyint/var/to/varbinary/top/varchar/trailing/varp/transaction/varying/transform/view/translate/when/translation/whenever/trim/where/true/with/union/work/unique/xor/uniqueidentifier/year/unknown/yesno/update/zone/updateidentity".Split( new char[] { '/' } );

            return new List<String>( strArray );
        }
    }

}

