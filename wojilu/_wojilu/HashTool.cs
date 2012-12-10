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
using System.Security.Cryptography;

namespace wojilu {

    /// <summary>
    /// 常用 hash 算法类型
    /// </summary>
    public enum HashType {
        MD5,
        MD5_16,
        SHA1,
        SHA256,
        SHA384,
        SHA512
    }

    /// <summary>
    /// 封装了常用 hash 算法
    /// </summary>
    public class HashTool : IHashTool {

        /// <summary>
        /// 根据指定的 hash 算法，加密字符串(比如密码)
        /// </summary>
        /// <param name="pwd">需要 hash 的字符串</param>
        /// <param name="ht">hash 算法类型</param>
        /// <returns></returns>
        public virtual String Get( String pwd, HashType ht ) {

            HashAlgorithm algorithm;

            if (ht == HashType.MD5 || ht == HashType.MD5_16)
                algorithm = MD5.Create();
            else if (ht == HashType.SHA1)
                algorithm = SHA1CryptoServiceProvider.Create();
            else if (ht == HashType.SHA256)
                algorithm = SHA256Managed.Create();
            else if (ht == HashType.SHA384)
                algorithm = SHA384Managed.Create();
            else if (ht == HashType.SHA512)
                algorithm = SHA512Managed.Create();
            else
                algorithm = MD5.Create();

            byte[] buffer = Encoding.UTF8.GetBytes( pwd );
            String result = BitConverter.ToString( algorithm.ComputeHash( buffer ) ).Replace( "-", "" );
            if (ht == HashType.MD5_16) return result.Substring( 8, 16 ).ToLower();
            return result;
        }

        /// <summary>
        /// 根据 hash 算法和指定的 salt，加密字符串
        /// </summary>
        /// <param name="pwd"></param>
        /// <param name="salt">指定的 salt</param>
        /// <param name="ht">hash 算法类型</param>
        /// <returns></returns>
        public virtual String GetBySalt( String pwd, String salt, HashType ht ) {
            return Get( pwd + salt, ht );
        }

        /// <summary>
        /// 获取随机密码(由英文字母和数字构成)
        /// </summary>
        /// <param name="passwordLength">密码长度</param>
        /// <returns></returns>
        public virtual String GetRandomPassword( int passwordLength ) {
            return GetRandomPassword( passwordLength, true );
        }

        /// <summary>
        /// 获取随机密码(由英文字母和数字构成)
        /// </summary>
        /// <param name="passwordLength">密码长度</param>
        /// <param name="isLower">结果是否小写</param>
        /// <returns></returns>
        public virtual String GetRandomPassword( int passwordLength, Boolean isLower ) {

            String charList = isLower ? "abcdefghijklmnopqrstuvwxyz0123456789" : "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            byte[] buffer = new byte[passwordLength];

            RNGCryptoServiceProvider.Create().GetBytes( buffer );

            char[] chars = new char[passwordLength];
            int charCount = charList.Length;
            for (int i = 0; i < passwordLength; i++) {
                chars[i] = charList[(int)buffer[i] % charCount];
            }

            return new string( chars );
        }

        /// <summary>
        /// 根据指定长度获取salt
        /// </summary>
        /// <param name="size">salt的长度</param>
        /// <returns></returns>
        public virtual String GetSalt( int size ) {
            byte[] buffer = new byte[size];
            RNGCryptoServiceProvider.Create().GetBytes( buffer );
            return BitConverter.ToString( buffer ).Replace( "-", "" );
        }

    }

}
