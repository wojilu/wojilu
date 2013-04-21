using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using wojilu.ORM;
using wojilu.Common.AppBase.Interface;
using wojilu.Members.Users.Domain;
using wojilu.Common.Tags;
using wojilu.Common.Comments;

namespace wojilu.Apps.Download.Domain {

    [Table( "DownloadItem" )]
    public class FileItem : ObjectBase<FileItem>, IAppData, ICommentTarget {

        public int OwnerId { get; set; }
        public String OwnerType { get; set; }
        [Column( Length = 50 )]
        public String OwnerUrl { get; set; }

        public int AppId { get; set; }
        public User Creator { get; set; }
        [Column( Length = 50 )]
        public String CreatorUrl { get; set; }

        public int AccessStatus { get; set; }

        [Column( Length = 40 )]
        public String Ip { get; set; }


        //-------------------------------------------------------------------------------------------

        [NotNull( "请填写标题" )]
        public String Title { get; set; }

        public String DemoUrl { get; set; }

        [NotSave]
        public String DemoLink {
            get {
                if (strUtil.IsNullOrEmpty( this.DemoUrl )) return "";
                return string.Format( "<a href=\"{0}\" target=\"_blank\">{0}</a>", this.DemoUrl );
            }
        }

        public String Url { get; set; } // 存储的地址
        public String Url2 { get; set; }
        public String Url3 { get; set; }

        public int CategoryId { get; set; }

        [NotSave]
        public int ParentCategoryId {
            get { return FileCategory.GetParentId( this.CategoryId ); }
        }

        [NotSave]
        public String CategoryName {
            get { return FileCategory.GetParentName( this.CategoryId ); }
        }

        [NotSave]
        public String SubCategoryName {
            get { return FileCategory.GetName( this.CategoryId ); }
        }

        // 完整的下载网址
        [NotSave]
        public String DownloadUrl {
            get { return sys.Path.GetPhotoOriginal( this.Url ); }
        }

        public Double SizeMB { get; set; }

        public String PreviewPic { get; set; } // 预览图片

        [NotSave]
        public String PreviewPicThumb {
            get { return sys.Path.GetPhotoThumb( this.PreviewPic ); }
        }

        [NotSave]
        public String PreviewPicMedium {
            get { return sys.Path.GetPhotoThumb( this.PreviewPic, wojilu.Drawing.ThumbnailType.Medium ); }
        }

        [NotSave]
        public String PreviewPicOriginal {
            get { return sys.Path.GetPhotoOriginal( this.PreviewPic ); }
        }

        public String Version { get; set; } // 版本

        public String Lang { get; set; } // 软件语言
        public String Provider { get; set; } // 软件提供商
        public String ProviderUrl { get; set; } // 软件提供商网址
        public String Email { get; set; }

        public int LicenseTypeId { get; set; } // 授权类型

        [NotSave]
        public String License {
            get { return LicenseType.GetName( this.LicenseTypeId ); }
        }

        public String PlatformIds { get; set; }

        public List<Platform> GetPlatforms() {
            List<Platform> list = new List<Platform>();
            if (strUtil.IsNullOrEmpty( this.PlatformIds )) return list;

            int[] ids = cvt.ToIntArray( this.PlatformIds );
            foreach (int id in ids) {
                list.Add( cdb.findById<Platform>( id ) );
            }

            return list;
        }

        [NotSave]
        public String PlatformStr {
            get {
                StringBuilder sb = new StringBuilder();
                List<Platform> list = GetPlatforms();
                foreach (Platform p in list) {
                    sb.Append( p.Name );
                    sb.Append( " " );
                }
                return sb.ToString();
            }
        }

        [TinyInt]
        public int Rank { get; set; } // 软件评级：10颗星

        [NotSave]
        public String RankStar {
            get {
                if (this.Rank <= 0) return "";
                String str = "";
                for (int i = 0; i < this.Rank; i++) {
                    str += string.Format( "<img src=\"{0}\" />", strUtil.Join( sys.Path.Img, "star/star.gif" ) );
                }
                return str;
            }
        }

        [TinyInt]
        public int IsFree { get; set; } // 是否免费

        public int RecommentPercent { get; set; } // 推荐度 82%
        public int Recomments { get; set; } // 推荐人数

        public int Hits { get; set; }
        public int Replies { get; set; }
        public int Downloads { get; set; } // 下载次数

        [LongText, HtmlText]
        public String Description { get; set; }
        public DateTime Created { get; set; }

        public DateTime Updated { get; set; } // 更新日期

        [NotSave]
        public String Summary {
            get { return strUtil.ParseHtml( this.Description, 200 ); }
        }

        private TagTool tag;
        [NotSave]
        public TagTool Tag {
            get {
                if (tag == null) tag = new TagTool( this );
                return tag;
            }
        }

        //-------------------------------------------------------------------------------------------

        public static DataPage<FileItem> GetPage( int appId ) {
            return FileItem.findPage( "AppId=" + appId );
        }

        public static DataPage<FileItem> GetPage( int appId, String key ) {
            if (strUtil.IsNullOrEmpty( key ))
                return FileItem.findPage( "AppId=" + appId );
            else
                return FileItem.findPage( "AppId=" + appId + " and Title like '%" + key + "%'" );
        }

        public bool HasPreviewPic() {
            return strUtil.HasText( this.PreviewPic );
        }

        public static DataPage<FileItem> GetPage( int appId, int categoryId ) {

            FileCategory c = FileCategory.GetById( categoryId );
            if (c.ParentId == 0) {

                List<FileCategory> list = FileCategory.GetByParentId( c.Id );
                String ids = "";
                for (int i = 0; i < list.Count; i++) {
                    ids += list[i].Id;
                    if (i < list.Count - 1) ids += ",";
                }

                return FileItem.findPage( "AppId=" + appId + " and CategoryId in (" + ids + ")" );

            }
            else {

                return FileItem.findPage( "AppId=" + appId + " and CategoryId=" + categoryId );
            }
        }

        public static void AddHits( FileItem f ) {
            f.Hits += 1;
            f.update( "Hits" );
        }


        public static void AddDownloads( FileItem f ) {
            f.Downloads += 1;
            f.update( "Downloads" );
        }

        public static void CreateFile( FileItem f ) {

            f.insert();

            int count = FileItem.count( "CategoryId=" + f.CategoryId );

            FileCategory c = FileCategory.GetById( f.CategoryId );
            c.DataCount = count;
            c.update();
        }

        public static void DeleteFile( FileItem f ) {

            int categoryId = f.CategoryId;
            f.delete();


            int count = FileItem.count( "CategoryId=" + categoryId );

            FileCategory c = FileCategory.GetById( categoryId );
            c.DataCount = count;
            c.update();


        }

        public static List<FileItem> GetTops( int categoryId ) {
            return FileItem.find( "CategoryId=" + categoryId + " order by Downloads desc, Hits desc, Replies desc, Id desc" ).list( 10 );
        }

        public static List<FileItem> GetTops() {
            return FileItem.find( "order by Downloads desc, Hits desc, Replies desc, Id desc" ).list( 10 );
        }


        public Type GetAppType() {
            return typeof( DownloadApp );
        }
    }

}
