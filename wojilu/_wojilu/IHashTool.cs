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
namespace wojilu {

    /// <summary>
    /// 封装了常用 hash 算法
    /// </summary>
    public interface IHashTool {

        /// <summary>
        /// 根据指定的 hash 算法，加密字符串(比如密码)
        /// </summary>
        /// <param name="pwd">需要 hash 的字符串</param>
        /// <param name="ht">hash 算法类型</param>
        /// <returns></returns>
        String Get( String pwd, HashType ht );


        /// <summary>
        /// 根据 hash 算法和指定的 salt，加密字符串
        /// </summary>
        /// <param name="pwd"></param>
        /// <param name="salt">指定的 salt</param>
        /// <param name="ht">hash 算法类型</param>
        /// <returns></returns>
        String GetBySalt( String pwd, String salt, HashType ht );

        /// <summary>
        /// 获取随机密码(由英文字母和数字构成)
        /// </summary>
        /// <param name="passwordLength">密码长度</param>
        /// <returns></returns>
        String GetRandomPassword( int passwordLength );


        /// <summary>
        /// 获取随机密码(由英文字母和数字构成)
        /// </summary>
        /// <param name="passwordLength">密码长度</param>
        /// <param name="isLower">结果是否小写</param>
        /// <returns></returns>
        String GetRandomPassword( int passwordLength, Boolean isLower );

        /// <summary>
        /// 根据指定长度获取salt
        /// </summary>
        /// <param name="size">salt的长度</param>
        /// <returns></returns>
        String GetSalt( int size );

    }

}
