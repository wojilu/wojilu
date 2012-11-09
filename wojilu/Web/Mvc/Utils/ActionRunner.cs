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

using wojilu.Web.Context;
using wojilu.Web.Mvc.Interface;
using wojilu.Web.Mvc.Attr;

namespace wojilu.Web.Mvc.Utils {

    internal class ActionRunner {

        internal static MethodInfo getActionMethod( ControllerBase controller, String actionName ) {

            MethodInfo x = controller.GetType().GetMethod( actionName );
            if (x != null) return x;
            if (MvcConfig.Instance.IsUrlToLower == false) return x;

            Dictionary<String, ControllerAction> actionMap = ControllerMeta.GetController( controller.GetType().FullName ).ActionMaps;
            foreach (KeyValuePair<String, ControllerAction> kv in actionMap) {
                if (strUtil.EqualsIgnoreCase( kv.Key, actionName )) return kv.Value.MethodInfo;
            }
            return null;
        }

        public static void runLayoutAction( MvcContext ctx, ControllerBase layoutController, aAction action ) {
            runAction( ctx, layoutController, action.Method, action, true );
        }

        public static void runAction( MvcContext ctx, ControllerBase controller, MethodInfo actionMethod, aAction run ) {
            runAction( ctx, controller, actionMethod, run, false );
        }

        public static void runAction( MvcContext ctx, ControllerBase controller, MethodInfo actionMethod, aAction run, Boolean isLayout ) {

            if (isLayout) {
                run();
                return;
            }

            List<IActionFilter> filters = controller.utils.getActionFilters( actionMethod );

            if (filters.Count == 0) {
                run();
                return;
            }

            for (int i = 0; i < filters.Count; i++) {

                filters[i].BeforeAction( controller );

                if (ctx.utils.isEnd()) return;

            }

            try {
                if (controller.utils.IsRunAction()) {
                    run();
                }
            }
            catch (Exception ex) {
                ctx.utils.setException( ex );
            }
            finally {

                for (int i = filters.Count - 1; i >= 0; i--) {

                    filters[i].AfterAction( controller );

                    if (ctx.utils.isEnd()) break;
                }

                Exception ex = ctx.utils.getException();
                if (ex != null) throw ctx.utils.getException();
            }

        }


    }

}
