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
using wojilu.Web.Context;
using wojilu.Web.Mvc.Utils;

namespace wojilu.Web.Mvc.Processors {

    internal class PermissionChecker : ProcessorBase {

        public override void Process( ProcessContext context ) {

            MvcEventPublisher.Instance.BeginCheckPermission( context.ctx );
            if (context.ctx.utils.isSkipCurrentProcessor()) return;

            MvcContext ctx = context.ctx;

            IList paths = ctx.utils.getLayoutPath();
            for (int i = paths.Count - 1; i >= 0; i--) {

                ControllerBase controller = ControllerFactory.FindSecurityController( paths[i].ToString(), ctx );
                if (controller == null) continue;

                if (ctx.controller.utils.isCheckPermission( controller.GetType() )) {
                    controller.CheckPermission();
                }

                if (ctx.utils.isEnd()) return;
            }

            ctx.controller.CheckPermission();
        }

    }

}
