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

namespace wojilu.Web.Handler {

    /// <summary>
    /// 验证码类型
    /// </summary>
    public class ValidationType {

        /// <summary>
        /// 数字
        /// </summary>
        public static readonly int Digit = 0;

        /// <summary>
        /// 英文字母
        /// </summary>
        public static readonly int Letter = 1;

        /// <summary>
        /// 数字和英文字母
        /// </summary>
        public static readonly int DigitAndLetter = 2;

        /// <summary>
        /// 中文
        /// </summary>
        public static readonly int Chinese = 3;

        
    }

    /// <summary>
    /// 验证码默认值
    /// </summary>
    public class ValidationDefault {

        /// <summary>
        /// 英文或字母验证码的默认长度
        /// </summary>
        public static readonly int Length = 6;

        /// <summary>
        /// 中文验证码的默认长度
        /// </summary>
        public static readonly int ChineseLength = 2;

        /// <summary>
        /// 默认的验证码类型
        /// </summary>
        public static readonly int Type = ValidationType.Digit;

    }

}
