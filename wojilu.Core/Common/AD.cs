using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Data;
using wojilu.ORM;
using wojilu.Members.Users.Domain;

namespace wojilu.Common {


    /// <summary>
    /// 广告范围
    /// </summary>
    public class AdScope : CacheObject {

        public static String GetName( int id ) {
            AdScope c = cdb.findById<AdScope>( id );
            return c == null ? "" : c.Name;
        }
    }


    /// <summary>
    /// 广告类型
    /// </summary>
    public class AdCategory : CacheObject {

        public static String GetName( int id ) {
            AdCategory c = cdb.findById<AdCategory>( id );
            return c == null ? "" : c.Name;
        }

        public static readonly int Banner = 1;
        public static readonly int NavBottom = 2;
        public static readonly int Footer = 3;


        public static readonly int ForumBoards = 4;
        public static readonly int ForumPosts = 5;
        public static readonly int ForumTopicInner = 6;

        public static readonly int ArticleSidebarTop = 7;
        public static readonly int ArticleSidebarBottom = 8;
        public static readonly int ArticleInner = 9;
    }

    public class AdItem : ObjectBase<AdItem> {

        public static String GetAdByName( String catName ) {

            if ("Banner".Equals( catName )) return GetAdById( AdCategory.Banner );
            if ("NavBottom".Equals( catName )) return GetAdById( AdCategory.NavBottom );
            if ("Footer".Equals( catName )) return GetAdById( AdCategory.Footer );
            //if ("Float".Equals( catName )) return GetAdById( AdCategory.Float );
            //if ("Gate".Equals( catName )) return GetAdById( AdCategory.Gate );

            if ("ForumBoards".Equals( catName )) return GetAdById( AdCategory.ForumBoards );
            if ("ForumPosts".Equals( catName )) return GetAdById( AdCategory.ForumPosts );
            if ("ForumTopicInner".Equals( catName )) return GetAdById( AdCategory.ForumTopicInner );

            if ("ArticleInner".Equals( catName )) return GetAdById( AdCategory.ArticleInner );
            if ("ArticleSidebarTop".Equals( catName )) return GetAdById( AdCategory.ArticleSidebarTop );
            if ("ArticleSidebarBottom".Equals( catName )) return GetAdById( AdCategory.ArticleSidebarBottom );

            return "";
        }


        public User Creator { get; set; }

        public String Name { get; set; }

        public int CategoryId { get; set; }

        [NotSave]
        public String CategoryName {
            get {
                return AdCategory.GetName( this.CategoryId );            
            }
        }

        public int ScopeId { get; set; }

        [NotSave]
        public String ScopeName {
            get {
                return AdScope.GetName( this.ScopeId );
            }
        }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        [LongText]
        public String AdCode { get; set; }

        [LongText]
        public String Description { get; set; }

        public DateTime Created { get; set; }

        [NotSave]
        public String CreatorName {
            get {
                return this.Creator == null ? "" : this.Creator.Name;
            }
        }

        public int IsStopped { get; set; }

        [NotSave]
        public String StartStr {
            get {
                if (this.StartTime < new DateTime( 1901, 1, 1 )) return "";
                return this.StartTime.ToShortDateString();
            }
        }

        [NotSave]
        public String EndStr {
            get {
                if (this.EndTime > new DateTime( 2900, 1, 1 )) return "";
                return this.EndTime.ToShortDateString();
            }
        }

        [NotSave]
        public String StatusStr {
            get { return this.IsStopped == 1 ? "已禁用" : ""; }
        }

        //------------------------------------------------------------------------------------

        private static readonly Random rd = new Random();

        public static String GetAdById( int categoryId ) {


            List<AdItem> list = AdItem.find( "CategoryId=" + categoryId + " and IsStopped=0" ).list();
            if (list.Count == 0) return "";
            int iIndex = rd.Next( 0, list.Count );

            return list[iIndex].AdCode;
        }




    }

}
