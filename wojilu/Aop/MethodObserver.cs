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
using System.Reflection.Emit;
using wojilu.DI;

namespace wojilu.Aop {

    /// <summary>
    /// 监控器基类。所有 Aop 监控器，都要继承自本基类。
    /// </summary>
    public abstract class MethodObserver {

        protected Dictionary<Type, String> dic = new Dictionary<Type, String>();

        public Dictionary<Type, String> GetRelatedMethods() {
            if (dic.Count == 0) {
                this.ObserveMethods();
            }
            return this.dic;
        }

        /// <summary>
        /// 运行之前的拦截处理
        /// </summary>
        /// <param name="method">被运行的方法</param>
        /// <param name="args">方法的参数</param>
        /// <param name="target">运行的对象</param>
        public virtual void Before( MethodInfo method, Object[] args, Object target ) {
        }

        /// <summary>
        /// 运行之后的拦截处理
        /// </summary>
        /// <param name="returnValue">方法运行的结果</param>
        /// <param name="method">被运行的方法</param>
        /// <param name="args">方法的参数</param>
        /// <param name="target">被拦截的对象</param>
        public virtual void After( Object returnValue, MethodInfo method, Object[] args, Object target ) {
        }

        /// <summary>
        /// 将代码和被拦截对象的方法混合一起，并直接运行
        /// </summary>
        /// <param name="invocation"></param>
        /// <returns></returns>
        public virtual Object Invoke( IMethodInvocation invocation ) {
            return invocation.Proceed();
        }

        /// <summary>
        /// 设置需要监控的method。在method运行之前和之后，BeforeMethod和AfterMethod会被执行。
        /// </summary>
        public abstract void ObserveMethods();

        /// <summary>
        /// 监控其他 method
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="name">方法名称</param>
        protected void observe( Type t, String methodName ) {

            String methods;
            dic.TryGetValue( t, out methods );

            // save/update(int,string)/getById
            dic[t] = strUtil.Join( methods, methodName );
        }

        /// <summary>
        /// 监控其他 method
        /// </summary>
        /// <param name="t">类型</param>
        /// <param name="methodName">方法名称</param>
        /// <param name="args">参数类型</param>
        protected void observe( Type t, String methodName, Type[] args ) {

            String methods;
            dic.TryGetValue( t, out methods );

            if (args.Length > 0) {
                methodName = appendParams( methodName, args );
            }

            dic[t] = strUtil.Join( methods, methodName );
        }

        private String appendParams( String methodName, Type[] args ) {
            String strParams = "";
            for (int i = 0; i < args.Length; i++) {
                strParams += args[i].FullName;
                if (i < args.Length - 1) strParams += ",";
            }
            return string.Format( "{0}({1})", methodName, strParams );
        }

        /// <summary>
        /// 监控其他 method
        /// </summary>
        /// <param name="typeFullName">类型的完整名称</param>
        /// <param name="methodName">方法名称</param>
        protected void observe( String typeFullName, String methodName ) {
            Type t;
            ObjectContext.Instance.TypeList.TryGetValue( typeFullName, out t );
            if (t == null) throw new TypeNotFoundException( typeFullName );
            observe( t, methodName );
        }

        /// <summary>
        /// 监控其他 method
        /// </summary>
        /// <param name="typeFullName">类型的完整名称</param>
        /// <param name="methodName">方法名称</param>
        /// <param name="args">参数类型</param>
        protected void observe( String typeFullName, String methodName, Type[] args ) {
            Type t;
            ObjectContext.Instance.TypeList.TryGetValue( typeFullName, out t );
            if (t == null) throw new TypeNotFoundException( typeFullName );
            observe( t, methodName, args );
        }

    }


}
