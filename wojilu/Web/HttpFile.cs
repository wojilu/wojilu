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
using System.IO;
using System.Web;

namespace wojilu.Web {

    /// <summary>
    /// 对上传文件的封装
    /// </summary>
    public class HttpFile {

        private string _contentType;
        private string _filePath;
        private byte[] _stream;

        public HttpFile() {
        }

        public HttpFile( string absFilePath ) {
            _contentType = getFileType( absFilePath );
            _filePath = absFilePath;
            _stream = File.ReadAllBytes( absFilePath );
        }

        public HttpFile( HttpPostedFile postedFile ) {
            this.PostedFile = postedFile;

            _contentType = postedFile.ContentType;
            _filePath = postedFile.FileName;
            _stream = new byte[postedFile.ContentLength];
            postedFile.InputStream.Read( _stream, 0, postedFile.ContentLength );
        }

        /// <summary>
        /// 上传文件的原始数据(用在服务端)
        /// </summary>
        public HttpPostedFile PostedFile { get; set; }

        /// <summary>
        /// 文件长度（以字节为单位）
        /// </summary>
        public int ContentLength {
            get { return PostedFile.ContentLength; }
        }

        /// <summary>
        /// 上载文件的 MIME 内容类型。
        /// </summary>
        public String ContentType {
            get {
                if (this.PostedFile != null) return PostedFile.ContentType;
                return _contentType;
            }
        }

        /// <summary>
        /// 客户端的文件的名称，包含目录路径。
        /// </summary>
        public String FilePath {
            get {
                if (this.PostedFile != null) return PostedFile.FileName;
                return _filePath;
            }
        }

        public String FileName {
            get {
                if (this.PostedFile != null) return Path.GetFileName( PostedFile.FileName );
                return Path.GetFileName( _filePath );
            }
        }

        // 异常:
        //   System.Web.HttpException:
        //     System.Web.Configuration.HttpRuntimeSection 对象的 System.Web.Configuration.HttpRuntimeSection.RequireRootedSaveAsPath
        //     属性设置为 true，但 filename 不是绝对路径。
        /// <summary>
        /// 保存上载文件的内容(用在服务端)
        /// </summary>
        /// <param name="filename">保存的文件的名称。</param>
        public void SaveAs( String filename ) {
            PostedFile.SaveAs( filename );
        }

        //---------------------------------

        public byte[] FileStream {
            get { return _stream; }
        }


        private string getFileType( string fileName ) {
            String ext = Path.GetExtension( fileName ).TrimStart( '.' );
            if (ext.Equals( "gif" )) return "image/gif";
            if (ext.Equals( "png" )) return "image/png";
            if (ext.Equals( "bmp" )) return "image/bmp";
            if (ext.Equals( "jpg" ) || ext.Equals( "jpeg" )) return "image/jpg";
            return "application/octet-stream";
        }

    }

}
