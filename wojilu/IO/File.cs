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
using System.IO;
using System.Text;

namespace wojilu.IO {

    /// <summary>
    /// 封装了文件常用操作方法
    /// </summary>
    public class File {

        /// <summary>
        /// 读取文件的内容(采用UTF8编码)
        /// </summary>
        /// <param name="absolutePath">文件的绝对路径</param>
        /// <returns>文件的内容</returns>
        public static String Read( String absolutePath ) {
            return Read( absolutePath, Encoding.UTF8 );
        }

        /// <summary>
        /// 以某种编码方式，读取文件的内容
        /// </summary>
        /// <param name="absolutePath">文件的绝对路径</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>文件的内容</returns>
        public static String Read( String absolutePath, Encoding encoding ) {
            using (StreamReader reader = new StreamReader( absolutePath, encoding )) {
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// 读取文件各行内容(采用UTF8编码)，以数组形式返回
        /// </summary>
        /// <param name="absolutePath">文件的绝对路径</param>
        /// <returns>文件各行内容</returns>
        public static String[] ReadAllLines( String absolutePath ) {
            return ReadAllLines( absolutePath, Encoding.UTF8 );
        }

        /// <summary>
        /// 以某种编码方式，读取文件各行内容(采用UTF8编码)，以数组形式返回
        /// </summary>
        /// <param name="absolutePath">文件的绝对路径</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>文件各行内容</returns>
        public static String[] ReadAllLines( String absolutePath, Encoding encoding ) {
            ArrayList list = new ArrayList();
            using (StreamReader reader = new StreamReader( absolutePath, encoding )) {
                String str;
                while ((str = reader.ReadLine()) != null) {
                    list.Add( str );
                }
            }
            return (String[])list.ToArray( typeof( String ) );
        }

        /// <summary>
        /// 将字符串写入某个文件中(采用UTF8编码)
        /// </summary>
        /// <param name="absolutePath">文件的绝对路径</param>
        /// <param name="fileContent">需要写入文件的字符串</param>
        public static void Write( String absolutePath, String fileContent ) {
            Write( absolutePath, fileContent, Encoding.UTF8 );
        }

        /// <summary>
        /// 将字符串写入某个文件中(需要指定文件编码方式)
        /// </summary>
        /// <param name="absolutePath">文件的绝对路径</param>
        /// <param name="fileContent">需要写入文件的字符串</param>
        /// <param name="encoding">编码方式</param>
        public static void Write( String absolutePath, String fileContent, Encoding encoding ) {
            using (StreamWriter writer = new StreamWriter( absolutePath, false, encoding )) {
                writer.Write( fileContent );
            }
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="absolutePath">文件的绝对路径</param>
        public static void Delete( String absolutePath ) {
            System.IO.File.Delete( absolutePath );
        }

        /// <summary>
        /// 判断文件是否存在
        /// </summary>
        /// <param name="absolutePath">文件的绝对路径</param>
        /// <returns></returns>
        public static Boolean Exists( String absolutePath ) {
            return System.IO.File.Exists( absolutePath );
        }

        /// <summary>
        /// 移动文件
        /// </summary>
        /// <param name="sourceFileName">原来的路径</param>
        /// <param name="destFileName">需要挪到的新路径</param>
        public static void Move( String sourceFileName, String destFileName ) {
            System.IO.File.Move( sourceFileName, destFileName );
        }

        /// <summary>
        /// 拷贝文件(如果目标存在，不覆盖)
        /// </summary>
        /// <param name="sourceFileName">原来的路径</param>
        /// <param name="destFileName">需要挪到的新路径</param>
        public static void Copy( String sourceFileName, String destFileName ) {
            System.IO.File.Copy( sourceFileName, destFileName, false );
        }

        /// <summary>
        /// 拷贝文件
        /// </summary>
        /// <param name="sourceFileName">原来的路径</param>
        /// <param name="destFileName">需要挪到的新路径</param>
        /// <param name="overwrite">如果目标存在，是否覆盖</param>
        public static void Copy( String sourceFileName, String destFileName, Boolean overwrite ) {
            System.IO.File.Copy( sourceFileName, destFileName, overwrite );
        }

        /// <summary>
        /// 拷贝文件夹
        /// <see cref="http://msdn.microsoft.com/en-us/library/bb762914.aspx"/>
        /// </summary>
        /// <param name="sourceDirName">源目录</param>
        /// <param name="destDirName">目标目标，如果不存在，则创建</param>
        /// <param name="copySubDirs">是否拷贝子目录</param>
        public static void CopyDirectory( string sourceDirName, string destDirName, bool copySubDirs ) {
            DirectoryInfo dir = new DirectoryInfo( sourceDirName );
            DirectoryInfo[] dirs = dir.GetDirectories();

            // If the source directory does not exist, throw an exception.
            if (!dir.Exists) {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName );
            }

            // If the destination directory does not exist, create it.
            if (!Directory.Exists( destDirName )) {
                Directory.CreateDirectory( destDirName );
            }

            // Get the file contents of the directory to copy.
            FileInfo[] files = dir.GetFiles();

            foreach (FileInfo file in files) {
                // Create the path to the new copy of the file.
                string temppath = Path.Combine( destDirName, file.Name );

                // Copy the file.
                file.CopyTo( temppath, false );
            }

            // If copySubDirs is true, copy the subdirectories.
            if (copySubDirs) {

                foreach (DirectoryInfo subdir in dirs) {
                    // Create the subdirectory.
                    string temppath = Path.Combine( destDirName, subdir.Name );

                    // Copy the subdirectories.
                    CopyDirectory( subdir.FullName, temppath, copySubDirs );
                }
            }
        }

        /// <summary>
        /// 将内容追加到文件中(采用UTF8编码)
        /// </summary>
        /// <param name="absolutePath">文件的绝对路径</param>
        /// <param name="fileContent">需要追加的内容</param>
        public static void Append( String absolutePath, String fileContent ) {
            Append( absolutePath, fileContent, Encoding.UTF8 );
        }

        /// <summary>
        /// 将内容追加到文件中
        /// </summary>
        /// <param name="absolutePath">文件的绝对路径</param>
        /// <param name="fileContent">需要追加的内容</param>
        /// <param name="encoding">编码方式</param>
        public static void Append( String absolutePath, String fileContent, Encoding encoding ) {
            using (StreamWriter writer = new StreamWriter( absolutePath, true, encoding )) {
                writer.Write( fileContent );
            }
        }

        //public static void Zip( String sourceFileName ) {
        //    Zip( sourceFileName, sourceFileName + ".zip" );
        //}

        //public static void Zip( String sourceFileName, String destFileName ) {
        //    throw new Exception( "Zip 方法未实现" );
        //}

        //public static void UnZip( String sourceFileName ) {
        //    throw new Exception( "UnZip 方法未实现" );
        //}

        //public static void UnZip( String sourceFileName, String destFilePath ) {
        //    throw new Exception( "UnZip 方法未实现" );
        //}

    }
}

