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
using wojilu.Data;
using wojilu.SOA.Controls;

namespace wojilu.SOA {

    /// <summary>
    /// 服务的状态
    /// </summary>
    public class ServiceStatus {

        /// <summary>
        /// 禁止在服务列表中查看
        /// </summary>
        public static readonly int ListForbidden = 1;
    }

    /// <summary>
    /// 系统中各类服务
    /// </summary>
    public class Service : CacheObject {

        /// <summary>
        /// 服务的全名，比如 wojilu.Apps.Forum.Service.ForumTopicService
        /// </summary>
        public String Type { get; set; }

        /// <summary>
        /// 服务公开的方法名称，比如 GetRecentTopic
        /// </summary>
        public String Method { get; set; }

        /// <summary>
        /// 参数类型，包括在web页面中显示的名称和控件类型(比如：显示数量=IntTextbox;性别=StringDroplist(男/女/保密);名称=StringTextBox;)
        /// </summary>
        public String Params { get; set; }

        /// <summary>
        /// 参数的默认值(可选)
        /// </summary>
        public String ParamsDefault { get; set; }

        /// <summary>
        /// 获取参数的默认值
        /// </summary>
        /// <returns></returns>
        public Dictionary<String, String> GetParamDefault() {
            Dictionary<String, String> result = new Dictionary<String, String>();
            if (strUtil.IsNullOrEmpty( this.ParamsDefault )) return result;
            String[] arrPair = this.ParamsDefault.Split( ';' );
            foreach (String pair in arrPair) {
                if (strUtil.IsNullOrEmpty( pair )) continue;
                String[] arrItem = pair.Split( '=' );
                if (arrItem.Length != 2) continue;
                result.Add( arrItem[0].Trim(), arrItem[1].Trim() );
            }
            return result;
        }

        /// <summary>
        /// 分类：可以通过 GetBy(String tag) 获得某类别的所有服务
        /// </summary>
        public String Tags { get; set; }

        /// <summary>
        /// 预留的自定义的筛选条件(通常指出对应的模板名称)
        /// </summary>
        public String Note { get; set; }

        /// <summary>
        /// 预留的服务状态：启用、停用、禁止等
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 只有允许的 owner 才可以使用本服务。留空表示不限制。多个owner可以用英文分号隔开
        /// </summary>
        public String Owner { get; set; }

        /// <summary>
        /// 服务简介。用于可视化界面中
        /// </summary>
        public String Description { get; set; }

        /// <summary>
        /// 此服务对应的链接
        /// </summary>
        public String Link { get; set; }

        private ArrayList _controls;

        /// <summary>
        /// 获得所有参数所对应的控件
        /// </summary>
        /// <returns></returns>
        public ArrayList GetParams() {
            if (_controls == null) {
                _controls = getParamsList( Params );
            }
            return _controls;
        }

        private ArrayList getParamsList( String paramString ) {
            ArrayList list = new ArrayList();
            if (strUtil.IsNullOrEmpty( paramString )) return list;

            String[] arrItem = paramString.Split( ';' );
            for (int i = 0; i < arrItem.Length; i++) {
                String item = arrItem[i];
                if (strUtil.HasText( item )) {
                    list.Add( ParamControl.GetControl( item, i ) );
                }
            }
            return list;
        }

        /// <summary>
        /// 获得所有允许的 Owner 数组
        /// </summary>
        /// <returns></returns>
        public String[] GetOwnerTypes() {
            return this.Owner.Split( ';' );
        }



    }
}

