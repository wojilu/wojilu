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
    /// action 缓存对象的接口，包括缓存key，更新操作等
    /// </summary>
    public interface IActionCache {

        /// <summary>
        /// action缓存的key
        /// </summary>
        /// <param name="actionName">action名称(有时候并不是ctx.route.action)</param>
        /// <returns></returns>
        String GetCacheKey( MvcContext ctx, String actionName );

        /// <summary>
        /// 关联action。一旦关联action被操作，则缓存会失效
        /// </summary>
        /// <returns></returns>
        Dictionary<Type, String> GetRelatedActions();

        /// <summary>
        /// 关联action操作之后，需要清除缓存或者重建缓存的具体操作
        /// </summary>
        /// <param name="ctx"></param>
        void UpdateCache( MvcContext ctx );


    }

    public class ActionCache : IActionCache {

        protected Dictionary<Type, String> dic = new Dictionary<Type, String>();

        public virtual string GetCacheKey( MvcContext ctx, String actionName ) {
            return null;
        }

        public virtual Dictionary<Type, String> GetRelatedActions() {
            if (dic.Count == 0) {
                this.ObserveActions();
            }
            return this.dic;
        }

        public virtual void UpdateCache( MvcContext ctx ) {
        }

        /// <summary>
        /// 设置需要观察/监控的action。一旦被监控的action运行之后，则自动触发UpdateCache方法，以及时刷新缓存。
        /// </summary>
        public virtual void ObserveActions() {
        }

        protected void observe( aAction action ) {

            Type t = action.Target.GetType();

            String actions;
            dic.TryGetValue( t, out actions );

            dic[t] = strUtil.Join( actions, action.Method.Name );
        }

        protected void observe( aActionWithId action ) {

            Type t = action.Target.GetType();

            String actions;
            dic.TryGetValue( t, out actions );

            dic[t] = strUtil.Join( actions, action.Method.Name );
        }


    }

}
