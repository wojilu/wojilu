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
using wojilu.Web.Context;

namespace wojilu.Web.Mvc {

    /// <summary>
    /// action 监控器：监控action，并提供action前置和后置事件
    /// </summary>
    public class ActionObserver {

        protected Dictionary<Type, String> dic = new Dictionary<Type, String>();

        /// <summary>
        /// 被监控的关联 action
        /// </summary>
        /// <returns></returns>
        public virtual Dictionary<Type, String> GetRelatedActions() {
            if (dic.Count == 0) {
                this.ObserveActions();
            }
            return this.dic;
        }

        /// <summary>
        /// action 前置动作
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns>是否继续执行当前action(ctx.route.action)，默认是 true 继续执行</returns>
        public virtual Boolean BeforeAction( MvcContext ctx ) {
            return true;
        }

        /// <summary>
        /// action 后置动作
        /// </summary>
        /// <param name="ctx"></param>
        public virtual void AfterAction( MvcContext ctx ) {
        }

        /// <summary>
        /// 设置需要观察/监控的action。一旦被监控的action运行之后，则自动触发UpdateCache方法，以及时刷新缓存。
        /// </summary>
        public virtual void ObserveActions() {
        }

        /// <summary>
        /// 监控其他 action
        /// </summary>
        /// <param name="action"></param>
        protected void observe( aAction action ) {

            Type t = action.Target.GetType();

            String actions;
            dic.TryGetValue( t, out actions );

            dic[t] = strUtil.Join( actions, action.Method.Name );
        }

        /// <summary>
        /// 监控其他 action
        /// </summary>
        /// <param name="action"></param>
        protected void observe( aActionWithId action ) {

            Type t = action.Target.GetType();

            String actions;
            dic.TryGetValue( t, out actions );

            dic[t] = strUtil.Join( actions, action.Method.Name );
        }

        /// <summary>
        /// 监控其他 action
        /// </summary>
        /// <param name="t"></param>
        /// <param name="actions"></param>
        protected void observe( Type t, String actions ) {

            String exitActions;
            dic.TryGetValue( t, out exitActions );

            dic[t] = strUtil.Join( exitActions, actions );
        }


    }


}
