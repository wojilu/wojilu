using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Common.Categories;
using wojilu.Serialization;
using wojilu.Data;
using wojilu.Common.AppBase.Interface;
using wojilu.ORM;
using wojilu.Common.AppBase;

namespace wojilu.Apps.Shop.Domain
{
    public class ViewCategory
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public int ParentId { get; set; }
    }

    [Serializable]
    public class ShopCategory : CacheObject, ISort, IComparable
    {

        public int ParentId { get; set; }
        public int OrderId { get; set; }
        public int IsThumbView { get; set; } // 前台是否按缩略图模式浏览

        [NotSerialize]
        public String ThumbIcon
        {
            get { return IsThumbView == 1 ? "<img src=\"" + strUtil.Join(sys.Path.Img, "img.gif") + "\" />" : ""; }
        }

        public int DataCount { get; set; }

        public void updateOrderId()
        {
            this.update();
        }

        public int CompareTo(object obj)
        {
            ShopCategory t = obj as ShopCategory;
            if (this.OrderId > t.OrderId) return -1;
            if (this.OrderId < t.OrderId) return 1;
            if (this.Id > t.Id) return 1;
            if (this.Id < t.Id) return -1;
            return 0;
        }

    }

}
