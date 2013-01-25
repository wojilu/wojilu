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
using System.Text;

namespace wojilu.Web {

    /// <summary>
    /// 模板引擎中的区块
    /// </summary>
    public interface IBlock {

        /// <summary>
        /// 进入下一个循环
        /// </summary>
        void Next();

        /// <summary>
        /// 给模板变量赋值
        /// </summary>
        /// <param name="lbl"></param>
        /// <param name="lblValue"></param>
        void Set( String lbl, String lblValue );

        /// <summary>
        /// 给模板变量赋值
        /// </summary>
        /// <param name="lbl"></param>
        /// <param name="val"></param>
        void Set( String lbl, Object val );

        /// <summary>
        /// 将对象绑定到模板中
        /// </summary>
        /// <param name="obj"></param>
        void Bind( Object obj );

        /// <summary>
        /// 将对象绑定到模板中，并指定对象在模板中的变量名
        /// </summary>
        /// <param name="lbl"></param>
        /// <param name="obj"></param>
        void Bind( String lbl, Object obj );

        /// <summary>
        /// 将对象列表绑定到模板中
        /// </summary>
        /// <param name="listName"></param>
        /// <param name="lbl"></param>
        /// <param name="objList"></param>
        void BindList( String listName, String lbl, IList objList );

        /// <summary>
        /// 获取模板中的区块，用于进一步的绑定
        /// </summary>
        /// <param name="blockName"></param>
        /// <returns></returns>
        IBlock GetBlock( String blockName );

        /// <summary>
        /// 绑定方法，通常用于自定义绑定
        /// </summary>
        bindFunction bindFunc { get; set; }

        /// <summary>
        /// 绑定方法，通常用于自定义绑定
        /// </summary>
        otherBindFunction bindOtherFunc { get; set; }



    }

}
