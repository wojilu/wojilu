/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

using wojilu.Drawing;
using wojilu.ORM;
using wojilu.Members.Users.Domain;
using wojilu.Common.Tags;
using wojilu.Common.Jobs;
using wojilu.Common.AppBase.Interface;
using wojilu.Common.Feeds.Interface;
using wojilu.Common;
using wojilu.Common.Comments;

namespace wojilu.Apps.Content.Domain {

    [Serializable]
    public class ContentPost : ObjectBase<ContentPost>, IAppData, IHits, IComparable, IShareData, ICommentTarget {

        public User Creator { get; set; }
        [Column( Length = 20 )]
        public String CreatorUrl { get; set; }

        public int OwnerId { get; set; }
        public String OwnerType { get; set; }
        [Column( Length = 50 )]
        public String OwnerUrl { get; set; }

        public int AppId { get; set; }
        public int CategoryId { get; set; } // wojilu.Apps.Content.Enum.PostCategory
        public int OrderId { get; set; }

        // 缓存第一个section信息
        [Column( Name = "SectionId" )]
        public ContentSection PageSection { get; set; }

        public String TypeName { get; set; }

        public String Title { get; set; }
        public String TitleHome { get; set; } // 显示在频道首页的标题，为了样式整齐供编辑额外调整

        public String Style { get; set; }
        public String Author { get; set; }

        private int _width;
        private int _height;

        public int Width {
            get { return _width; }
            set { _width = value; }

        }

        public int Height {
            get { return _height; }
            set { _height = value; }
        }

        [LongText]
        public String Content { get; set; }

        [LongText]
        public String Summary { get; set; }


        private String _imglink;
        // 存储的是相对网址 2009-11-21/1572640972943524.jpg
        public String ImgLink {
            get { return _imglink; }
            set { _imglink = value; }
        }

        [Column( Name = "OutUrl", Length = 150 )]
        public String SourceLink { get; set; }

        // 0表示允许评论；1表示关闭评论。见 wojilu.Common.AppBase.CommentCondition
        [Column( Name = "AllowComment" )]
        public int CommentCondition { get; set; }

        public int Hits { get; set; }
        public int Replies { get; set; }

        public String MetaKeywords { get; set; }
        public String MetaDescription { get; set; }
        public String RedirectUrl { get; set; }

        /// <summary>
        /// 普通Normal = 0; 精选Picked = 1; 头条Focus = 2;
        /// </summary>
        public int PickStatus { get; set; }


        public DateTime Created { get; set; }

        [TinyInt]
        public int SaveStatus { get; set; }
        public int AccessStatus { get; set; }

        [Column( Length = 40 )]
        public String Ip { get; set; }

        public int HasImgList { get; set; }

        public int Attachments { get; set; }

        [TinyInt]
        public int IsAttachmentLogin { get; set; }


        public int DiggUp { get; set; }
        public int DiggDown { get; set; }

        //-----------------------------用于界面绑定显示----------------------------------------------------------------------------

        [NotSave]
        public String TitleShow {
            get {
                if (strUtil.HasText( this.TitleHome )) return this.TitleHome;
                return this.Title;
            }
        }

        [NotSave]
        public String SummaryShow {
            get {
                if (strUtil.HasText( this.Summary )) return this.Summary;
                return strUtil.ParseHtml( this.Content, 220 );
            }
        }

        [NotSave]
        public String PicS {
            get { return this.GetImgThumb(); }
        }

        [NotSave]
        public String PicSHtml {
            get {
                if (this.HasImg() == false) return "";
                return string.Format( "<img src=\"{0}\" />", this.GetImgThumb() );
            }
        }

        [NotSave]
        public String PicM {
            get { return this.GetImgMedium(); }
        }

        [NotSave]
        public String HitsShow {
            get {
                if (this.Hits <= 0) return "";
                return string.Format( "点击:{0}", this.Hits );
            }
        }

        [NotSave]
        public String RepliesShow {
            get {
                if (this.Replies <= 0) return "";
                return string.Format( "评论:{0}", this.Replies );
            }
        }

        [NotSave]
        public String TagShow {
            get {
                if (this.Tag.List.Count == 0) return "";
                return string.Format( "<span class=\"tag-label\">标签:</span> {0}", this.Tag.HtmlString );
            }
        }

        [NotSave]
        public String AuthorShow {
            get {
                if (strUtil.IsNullOrEmpty( this.Author )) return "";
                return string.Format( "<span class=\"author-label\">作者:</span> {0}", this.Author );
            }
        }

        //---------------------------------------------------------------------------------------------------------


        [NotSave]
        public int SectionId {
            get {
                if (this.PageSection == null) return 0;
                return this.PageSection.Id;
            }
        }

        [NotSave]
        public String SectionName {
            get {
                if (this.PageSection == null) return "";
                return this.PageSection.Title;
            }
        }

        [NotSave]
        public String DiggUpPercent {
            get {
                decimal d = (this.DiggUp + this.DiggDown == 0 ? 0 : (decimal)this.DiggUp / (this.DiggUp + this.DiggDown)) * 100;
                return string.Format( "{0:N2}", d );
            }
        }

        [NotSave]
        public String DiggDownPercent {
            get {
                decimal d = (this.DiggUp + this.DiggDown == 0 ? 0 : (decimal)this.DiggDown / (this.DiggUp + this.DiggDown)) * 100;
                return string.Format( "{0:N2}", d );
            }
        }


        private TagTool _tag;

        [NotSave]
        public TagTool Tag {
            get {
                if (_tag == null) {
                    _tag = new TagTool( this );
                }
                return _tag;
            }
        }

        public String GetImgThumb() {
            return sys.Path.GetPhotoThumb( this.ImgLink );
        }

        public String GetImgMedium() {
            return sys.Path.GetPhotoThumb( this.ImgLink, ThumbnailType.Medium );
        }

        public String GetImgOriginal() {
            return sys.Path.GetPhotoOriginal( this.ImgLink );
        }

        public String GetTitle() {
            if (strUtil.HasText( this.Title )) return this.Title;
            return "Post-"+ this.Id + "-"+this.Created.ToShortDateString();
        }

        public String GetSummary( int length ) {
            if (strUtil.HasText( this.Summary )) return strUtil.CutString( this.Summary, length );
            return strUtil.ParseHtml( this.Content, length );
        }


        public bool HasImg() {
            return strUtil.HasText( this.ImgLink );
        }

        public override int CompareTo( object obj ) {

            ContentPost target = (ContentPost)obj;
            return target.OrderId > this.OrderId ? 1 : -1;
        }


        public IShareInfo GetShareInfo() {
            return new ContentShare( this );
        }


        [NotSave]
        public String PickStatusStr {
            get {
                String str = wojilu.Apps.Content.Enum.PickStatus.GetPickStatusStr( this.PickStatus );
                if (str == "普通") return "";
                return str;
            }
        }

        public Type GetAppType() {
            return typeof( ContentApp );
        }

    }




}

