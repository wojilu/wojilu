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

        public HttpFile() {
        }

        public HttpFile( HttpPostedFile postedFile ) {
            this.PostedFile = postedFile;
        }

        /// <summary>
        /// 上传文件的原始数据
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
            get { return PostedFile.ContentType; }
        }

        /// <summary>
        /// 客户端的文件的名称，包含目录路径。
        /// </summary>
        public String FileName {
            get { return PostedFile.FileName; }
        }

        /// <summary>
        /// 指向文件的 System.IO.Stream。
        /// </summary>   
        public Stream InputStream {
            get { return PostedFile.InputStream; }
        }


        // 异常:
        //   System.Web.HttpException:
        //     System.Web.Configuration.HttpRuntimeSection 对象的 System.Web.Configuration.HttpRuntimeSection.RequireRootedSaveAsPath
        //     属性设置为 true，但 filename 不是绝对路径。
        /// <summary>
        /// 保存上载文件的内容。
        /// </summary>
        /// <param name="filename">保存的文件的名称。</param>
        public void SaveAs( String filename ) {
            PostedFile.SaveAs( filename );
        }

    }

}
