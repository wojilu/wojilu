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

using wojilu.DI;
using wojilu.Reflection;
using wojilu.SOA.Controls;

namespace wojilu.SOA {

    /// <summary>
    /// 获取服务的工具类
    /// </summary>
    public class ServiceContext {

        /// <summary>
        /// 根据 id 获取服务
        /// </summary>
        /// <param name="serviceId"></param>
        /// <returns></returns>
        public static Service Get( int serviceId ) {
            return (new Service().findById( serviceId ) as Service);
        }

        /// <summary>
        /// 获取所有服务
        /// </summary>
        /// <returns></returns>
        public static IList GetAll() {
            return new Service().findAll();
        }

        /// <summary>
        /// 获得某分类下的所有服务(tag当做分类法使用)
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="ownerType"></param>
        /// <returns></returns>
        public static IList GetByTag( String tag, String ownerType ) {
            IList list = new ArrayList();
            IList all = GetAll();
            foreach (Service service in all) {
                if (hasTag( service.Tags, tag ) && isOwner( ownerType, service.GetOwnerTypes() ) && service.Status != ServiceStatus.ListForbidden) {
                    list.Add( service );
                }
            }
            return list;
        }

        private static Boolean isOwner( String ownerType, String[] ownerTypes ) {
            foreach (String o in ownerTypes) {
                if (ownerType.Equals( o )) return true;
            }
            return false;
        }

        /// <summary>
        /// 根据服务 id 和参数，调用服务，返回服务运行的结果
        /// </summary>
        /// <param name="serviceId"></param>
        /// <param name="serviceParamValues">服务的参数</param>
        /// <param name="defaultValues">默认参数</param>
        /// <returns></returns>
        public static IList GetData( int serviceId, Dictionary<String, String> serviceParamValues, Dictionary<String, String> defaultValues ) {

            ArrayList argList = new ArrayList();

            Service service = Get( serviceId );
            IList parms = service.GetParams();
            for (int i = 0; i < parms.Count; i++) {

                String pkey = "param" + i;
                String val = serviceParamValues.ContainsKey( pkey ) ? serviceParamValues[pkey] : null;

                Object argValue = ((ParamControl)parms[i]).ChangeType( val );
                argList.Add( argValue );
            }

            foreach (KeyValuePair<String, String> pair in defaultValues) {
                Object argValue = getValue( service, pair );
                argList.Add( argValue );
            }

            object[] args = argList.ToArray();

            Object objService = ObjectContext.GetByType( service.Type );
            return (ReflectionUtil.CallMethod( objService, service.Method, args ) as IList);
        }

        private static Object getValue( Service service, KeyValuePair<String, String> pair ) {
            Dictionary<String, String> pd = service.GetParamDefault();
            String valueType = pd[pair.Key];
            if (valueType.Equals( "int" ))
                return cvt.ToInt( pair.Value );
            return pair.Value;
        }

        private static Boolean hasTag( String svcTag, String tag ) {
            if (strUtil.HasText( svcTag )) {
                tag = tag.Trim();
                String[] strArray = svcTag.Split( new[] { ',', '/', ' ', '|' } );
                foreach (String str in strArray) {
                    if (str.Trim() == tag) {
                        return true;
                    }
                }
            }
            return false;
        }


    }
}

