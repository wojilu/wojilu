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

namespace wojilu.Aop {

    internal class AopCoderDialectSub : AopCoderDialect {

        public override String GetClassPrefix() {
            return "__";
        }

        public override String GetInvokeTargetBase() {
            return "base";
        }
        public override String GetMethodBasePrefix() {
            return "__base_";
        }
        public override String GetMethodOverride() {
            return " override ";
        }
        public override String GetInvokeTargetThis() {
            return "this";
        }

        public override string IsSubClassStr() {
            return "true";
        }
        public override String GetClassFullName( Type t, String interfaceFullName ) {
            return this.GetClassPrefix() + t.Name;
        }
    }


}
