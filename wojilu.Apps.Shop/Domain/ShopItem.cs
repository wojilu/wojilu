/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

using wojilu.Drawing;
using wojilu.ORM;
using wojilu.Members.Users.Domain;
using wojilu.Common.Tags;
using wojilu.Common.AppBase.Interface;
using wojilu.Common;

namespace wojilu.Apps.Shop.Domain {

    [Serializable]
    public class ShopItem : ObjectBase<ShopItem>, IAppData, IHits, IComparable, IShareData {

        [NotSerialize]
        public User Creator { get; set; }
        [Column( Length = 20 )]
        public String CreatorUrl { get; set; }

        public int OwnerId { get; set; }
        public String OwnerType { get; set; }
        [Column( Length = 50 )]
        public String OwnerUrl { get; set; }

        public int AppId { get; set; }
        public int MethodId { get; set; } // wojilu.Apps.Shop.Enum.ItemMethod
        public int CategoryId { get; set; }
        [NotSave, NotSerialize]
        public ShopCategory Category
        {
            get { return CategoryId > 0 ? cdb.findById<ShopCategory>(this.CategoryId) : null; }
        }
        
        public int OrderId { get; set; }
        [Column(Name = "SectionId"), NotSerialize]
        public ShopSection PageSection { get; set; }

        public String TypeName { get; set; }

        public String Title { get; set; }
        public String TitleHome { get; set; } // 显示在频道首页的标题，为了样式整齐供编辑额外调整

        public String Style { get; set; }
        public String Author { get; set; }

        private int _width;
        private int _height;

        public int Width {
            //get { if (_width <= 0) return 300; return _width; }
            get { return _width; }
            set { _width = value; }

        }
        public int Height {
            //get { if (_height <= 0) return 255; return _height; }
            get { return _height; }
            set { _height = value; }
        }

        [LongText]
        public String Content { get; set; }

        [LongText]
        public String Summary { get; set; }

        public String ItemSKU { get; set; }

        [LongText, HtmlText]
        public String ShortDescription { get; set; }

        [Money]
        public Decimal SalePrice { get; set; }//销售单价

        [Money]
        public Decimal CostPrice { get; set; }//成本单价

        [Money]
        public Decimal RetaPrice { get; set; }//市场单价

        public int MinOrderQty { get; set; }//单次最小购买数量

        public int MaxOrderQty { get; set; }//单次最大购买数量
         /// <summary> 
        /// 数量
        /// </summary>
        public int ItemAmount { get; set; }
        public String UnitString { get; set; }//单位

        public int Weight { get; set; }//重量
        [NotSerialize]
        public ShopSupplier Provider { get; set; } // 提供商
        [NotSerialize]
        public ShopBrand Brand { get; set; } // 品牌

        private String _imglink;
        // 存储的是相对网址 2009-11-21/1572640972943524.jpg
        public String ImgLink
        {
            get { return _imglink; }
            set { _imglink = value; }
        }

        private String _adlink;
        // 存储的是相对网址 2009-11-21/1572640972943524.jpg
        public String AdImgLink //广告图
        {
            get { return _adlink; }
            set { _adlink = value; }
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

        public int PickStatus { get; set; }


        public DateTime Created { get; set; }
        /// <summary> 
        /// 过期时间
        /// </summary>
        public DateTime Expiration { get; set; }

        [TinyInt]
        public int SaveStatus { get; set; }
        public int AccessStatus { get; set; }

        [Column( Length = 40 )]
        public String Ip { get; set; }

        [TinyInt]
        public int IsSale { get; set; }//是否上架销售

        [TinyInt]
        public int IsGift { get; set; } // 是否礼品
        /// <summary> 
        /// 折扣
        /// </summary>
        public int Discount { get; set; }
        public int OrderTimes { get; set; } // 订购次数

        public int HasImgList { get; set; }

        public int Attachments { get; set; }

        [TinyInt]
        public int IsAttachmentLogin { get; set; }


        public int DiggUp { get; set; }
        public int DiggDown { get; set; }

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

        [NotSave,NotSerialize]
        public TagTool Tag {
            get {
                if (_tag == null) {
                    _tag = new TagTool( this );
                }
                return _tag;
            }
        }

        public String GetImgThumb()
        {

            if (strUtil.IsNullOrEmpty(this.ImgLink)) return null;
            if (this.ImgLink.ToLower().StartsWith("http://")) return this.ImgLink;
            return sys.Path.GetPhotoThumb(this.ImgLink);
        }

        public String GetImgMedium()
        {
            if (strUtil.IsNullOrEmpty(this.ImgLink)) return null;
            if (this.ImgLink.ToLower().StartsWith("http://")) return this.ImgLink;
            return sys.Path.GetPhotoThumb(this.ImgLink, ThumbnailType.Medium);
        }

        public String GetImgUrl()
        {
            if (strUtil.IsNullOrEmpty(this.ImgLink)) return null;
            if (this.ImgLink.ToLower().StartsWith("http://")) return this.ImgLink;
            return sys.Path.GetPhotoOriginal(this.ImgLink);
        }

        public String GetAdImgThumb()
        {

            if (strUtil.IsNullOrEmpty(this.AdImgLink)) return null;
            if (this.AdImgLink.ToLower().StartsWith("http://")) return this.AdImgLink;
            return sys.Path.GetPhotoThumb(this.AdImgLink);
        }

        public String GetAdImgMedium()
        {
            if (strUtil.IsNullOrEmpty(this.AdImgLink)) return null;
            if (this.AdImgLink.ToLower().StartsWith("http://")) return this.AdImgLink;
            return sys.Path.GetPhotoThumb(this.AdImgLink, ThumbnailType.Medium);
        }

        public String GetAdImgUrl()
        {
            if (strUtil.IsNullOrEmpty(this.AdImgLink)) return null;
            if (this.AdImgLink.ToLower().StartsWith("http://")) return this.AdImgLink;
            return sys.Path.GetPhotoOriginal(this.AdImgLink);
        }

        public String GetTitle() {
            if (strUtil.HasText( this.Title )) return this.Title;
            if (this.PageSection == null) return alang.get( typeof( ShopApp ), "noTitle" );

            return this.PageSection.Title + " " + this.Created.ToShortDateString();
        }

        public String GetSummary( int length ) {
            if (strUtil.HasText( this.Summary )) return strUtil.CutString( this.Summary, length );
            return strUtil.ParseHtml( this.Content, length );
        }

        public bool HasImg()
        {
            return strUtil.HasText(this.ImgLink);
        }

        public bool HasAdImg()
        {
            return strUtil.HasText(this.AdImgLink);
        }

        public override int CompareTo( object obj ) {

            ShopItem target = (ShopItem)obj;
            return target.OrderId > this.OrderId ? 1 : -1;
        }


        public IShareInfo GetShareInfo() {
            return new ShopShare( this );
        }


        [NotSave]
        public String PickStatusStr {
            get {
                String str = wojilu.Apps.Shop.Enum.PickStatus.GetPickStatusStr( this.PickStatus );
                if (str == "普通") return "";
                return str;
            }
        }



    }




}

