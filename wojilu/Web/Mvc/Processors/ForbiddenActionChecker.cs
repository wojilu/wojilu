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
using System.Reflection;
using wojilu.Web.Mvc.Attr;

namespace wojilu.Web.Mvc.Processors {

    internal class ForbiddenActionChecker : ProcessorBase {

        public override void Process( ProcessContext context ) {

            MvcEventPublisher.Instance.BeginCheckForbiddenAction( context.ctx );
            if (context.ctx.utils.isSkipCurrentProcessor()) return;

            MethodInfo actionMethod = context.ctx.ActionMethodInfo; // context.getActionMethod();

            Attribute forbiddenAttr = context.getController().utils.getAttribute( actionMethod, typeof( NonVisitAttribute ) );
            if (forbiddenAttr != null) {
                context.endMsg( lang.get( "exVisitForbidden" ), HttpStatus.Forbidden_403 );
            }

        }

    }

}
