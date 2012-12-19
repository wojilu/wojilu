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

namespace wojilu.Aop {

    /// <summary>
    /// 被监控的类型
    /// </summary>
    public class ObservedType {

        /// <summary>
        /// 被监控的 Type
        /// </summary>
        public Type Type;

        /// <summary>
        /// 被监控的方法列表
        /// </summary>
        public List<ObservedMethod> MethodList;

        /// <summary>
        /// 是否可以创建子代理
        /// </summary>
        public Boolean CanCreateSubProxy() {
            return this.GetNotVirtualMethodList().Count == 0;
        }

        public String GetNotVirtualMethodString() {
            String str = "";
            foreach (ObservedMethod x in this.GetNotVirtualMethodList()) {
                str += x.Method.Name + ",";
            }
            return str.Trim().TrimEnd( ',' );
        }

        private List<ObservedMethod> _notVirtualMethodList;

        public List<ObservedMethod> GetNotVirtualMethodList() {

            if (_notVirtualMethodList == null) {

                _notVirtualMethodList = new List<ObservedMethod>();
                foreach (ObservedMethod x in this.MethodList) {
                    if (x.Method.IsVirtual == false) _notVirtualMethodList.Add( x );
                }

            }
            return _notVirtualMethodList;
        }

        /// <summary>
        /// 是否可以根据接口创建代理类
        /// </summary>
        /// <param name="interfaceType"></param>
        /// <returns></returns>
        public Boolean CanCreateInterfaceProxy( Type interfaceType ) {
            foreach (Type t in this.GetInterfaceType()) {
                if (t == interfaceType) return true;
            }
            return false;
        }

        private List<Type> _interfaceTypeList;

        /// <summary>
        /// 被监控的所有方法隶属的接口
        /// </summary>
        /// <returns></returns>
        public List<Type> GetInterfaceType() {

            if (_interfaceTypeList == null) {
                _interfaceTypeList = loadInterfaceType();
            }

            return _interfaceTypeList;
        }


        public String GetInterfaceTypeString() {
            String str = "";
            for (int i = 0; i < this._interfaceTypeList.Count; i++) {

                str += _interfaceTypeList[i].FullName;
                if (i < _interfaceTypeList.Count - 1) str += ",";

            }
            return str;
        }

        private List<Type> loadInterfaceType() {

            List<Type> results = new List<Type>();

            // 检索所有的接口
            Type[] interfaceList = this.Type.GetInterfaces();

            foreach (Type interfaceOne in interfaceList) {
                if (isObservedInferface( interfaceOne, this.MethodList )) {
                    results.Add( interfaceOne );
                }
            }

            return results;
        }

        // 检查此 interface 是否匹配所有的被监控方法
        private Boolean isObservedInferface( System.Type interfaceOne, List<ObservedMethod> list ) {

            foreach (ObservedMethod x in list) {

                if (methodMatchInterface( x, interfaceOne ) == false) return false;

            }

            return true;
        }

        private Boolean methodMatchInterface( ObservedMethod x, System.Type interfaceOne ) {
            return rft.IsMethodInInterface( x.Method, x.ObservedType.Type, interfaceOne );
        }


    }



}
