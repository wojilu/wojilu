using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Data;
using System.IO;

namespace wojilu.Apps.Content.Domain {

    /// <summary>
    /// Content安装主题。所谓主题，就是一个设计好的界面，包括样式、布局、初始化数据。
    /// </summary>
    public class ContentTheme : CacheObject {

        /// <summary>
        /// 名称
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// 简介
        /// </summary>
        public String Description { get; set; }

        /// <summary>
        /// 主题数据在磁盘上的文件名，默认保存在 /static/theme/ 目录下
        /// </summary>
        public String FileName { get; set; }

        /// <summary>
        /// 获取文件所在路径
        /// </summary>
        /// <returns></returns>
        public static String GetFileAbsDir() {
            return PathHelper.Map( strUtil.Join( sys.Path.DiskStatic, "/theme/" ) );
        }

        /// <summary>
        /// 获取文件绝对路径的完整名称
        /// </summary>
        /// <returns></returns>
        public String GetFileAbsPath() {
            return Path.Combine( GetFileAbsDir(), this.FileName );
        }

        public void DeleteTheme() {

            base.delete();

            String filePath = this.GetFileAbsPath();
            if (file.Exists( filePath )) {
                file.Delete( filePath );
            }
        }

    }
}
