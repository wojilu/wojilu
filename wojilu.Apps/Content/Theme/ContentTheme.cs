using System;
using System.Collections.Generic;
using System.IO;
using wojilu.ORM;
using wojilu.Common.Themes;

namespace wojilu.Apps.Content.Domain {

    /// <summary>
    /// Content安装主题。所谓主题，就是一个设计好的界面，包括样式、布局、初始化数据。
    /// </summary>
    public class ContentTheme : ITheme, IComparable {

        private static readonly ILog logger = LogManager.GetLogger( typeof( ContentTheme ) );

        [NotSave]
        public String Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// 简介
        /// </summary>
        public String Description { get; set; }

        /// <summary>
        /// 图片路径
        /// </summary>
        public String Pic { get; set; }

        /// <summary>
        /// 排序ID
        /// </summary>
        public int OrderId { get; set; }

        public int CompareTo( object obj ) {
            ContentTheme t = obj as ContentTheme;
            if (this.OrderId > t.OrderId) return -1;
            if (this.OrderId < t.OrderId) return 1;
            return 0;
        }


        [NotSave]
        public String PicShow {
            get {
                if (strUtil.IsNullOrEmpty( this.Pic )) return "";
                if (this.Pic.StartsWith( "http:" )) return this.Pic;
                if (this.Pic.StartsWith( "/" )) return this.Pic;
                return strUtil.Join( sys.Path.Static, "/theme/wojilu.Apps.Content/" ) + this.Pic;
            }
        }

        /// <summary>
        /// 获取文件所在路径
        /// </summary>
        /// <returns></returns>
        public static String GetFileAbsDir() {
            return PathHelper.Map( strUtil.Join( sys.Path.DiskStatic, "/theme/wojilu.Apps.Content/" ) );
        }

        /// <summary>
        /// 获取文件绝对路径的完整名称
        /// </summary>
        /// <returns></returns>
        public String GetFileAbsPath() {
            return Path.Combine( GetFileAbsDir(), this.Id+".data.config" );
        }

        //---------------------------------------------------------------

        public List<ITheme> GetAll() {
            return cvt.ToList<ITheme>( findAll() );
        }

        public ITheme GetById( String id ) {
            return findById( id );
        }

        public void Delete() {
            delete( this.Id );
        }

        //---------------------------------------------------------------

        public void Insert( String jsonString ) {

            this.Id = Guid.NewGuid().ToString();

            // 1) 
            saveDataPrivate( jsonString, ".data" );

            // 2)
            saveMetaInfo();
        }


        public static XApp GetByThemeId( String themeId ) {

            ContentTheme x = findById( themeId );
            if (x == null) return null;

            String jsonStr = file.Read( x.GetFileAbsPath() );
            return Json.Deserialize<XApp>( jsonStr );
        }

        //---------------------------------------------------------------

        private void saveMetaInfo() {

            this.Pic = "";
            String jsonString = Json.ToString( this );

            saveDataPrivate( jsonString, "" );
        }

        private void saveDataPrivate( String jsonString, String ext ) {

            String fileName = this.Id + ext + ".config";
            String savedPath = ContentTheme.GetFileAbsDir();

            if (Directory.Exists( savedPath ) == false) {
                Directory.CreateDirectory( savedPath );
            }

            file.Write( Path.Combine( savedPath, fileName ), jsonString );
        }

        private static void delete( String guid ) {
            deletePrivate( guid, "" );
            deletePrivate( guid, ".data" );
        }

        private static void deletePrivate( String guid, String ext ) {
            String fileName = guid + ext + ".config";
            String savedPath = ContentTheme.GetFileAbsDir();
            String filePath = Path.Combine( savedPath, fileName );

            if (file.Exists( filePath )) file.Delete( filePath );
        }

        private static ContentTheme findById( String guid ) {
            List<ContentTheme> list = findAll();
            foreach (ContentTheme x in list) {
                if (x.Id == guid) return x;
            }
            return null;
        }


        private static List<ContentTheme> findAll() {

            List<String> nameList = new List<String>();

            List<ContentTheme> list = new List<ContentTheme>();

            String themeDir = GetFileAbsDir();
            if (Directory.Exists( themeDir ) == false) {
                logger.Error( "theme dir not exist=" + themeDir );
                return list;
            }

            String[] files = Directory.GetFiles( themeDir );
            foreach (String x in files) {

                if (x.EndsWith( ".data.config" )) continue;
                if (x.EndsWith( ".config" ) == false) continue;

                String name = Path.GetFileName( x );
                if (nameList.Contains( name )) continue;

                String jsonString = file.Read( x );
                try {
                    ContentTheme theme = Json.Deserialize<ContentTheme>( jsonString );

                    if (theme == null) {
                        logger.Error( "Deserialize ContentTheme=null" );
                    } else if (strUtil.IsNullOrEmpty( theme.Name )) {
                        logger.Error( "Deserialize ContentTheme=theme name is empty" );
                    } else {
                        theme.Id = strUtil.TrimEnd( name, ".config" );
                        list.Add( theme );
                    }
                }
                catch (Exception ex) {
                    logger.Error( "Deserialize ContentTheme=" + ex );
                }

                nameList.Add( name );
            }

            list.Sort();

            return list;
        }

        public static Boolean IdHasError( string tid ) {
            if (strUtil.IsNullOrEmpty( tid )) return true;
            if (tid.Length > 40) return true;
            String[] arr = tid.Split( '-' );
            if (arr.Length != 5) return true;
            return false;
        }

    }
}
