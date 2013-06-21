using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Common.Categories;
using wojilu.Serialization;
using wojilu.Data;
using wojilu.Common.AppBase.Interface;
using wojilu.ORM;
using wojilu.Common.AppBase;

namespace wojilu.Apps.Download.Domain {

    public class ViewCat {
        public int Id { get; set; }
        public String Name { get; set; }
        public int ParentId { get; set; }
    }



    public class FileCategory : CacheObject, ISort, IComparable {

        public int ParentId { get; set; }

        public int OrderId { get; set; }

        public int IsThumbView { get; set; } // 前台是否按缩略图模式浏览
        
        public string Description{get; set;}
        [NotSerialize]
        public String ThumbIcon {
            get { return IsThumbView == 1 ? "<img src=\"" + strUtil.Join( sys.Path.Img, "img.gif" ) + "\" />" : ""; }
        }

        public int DataCount { get; set; }

        public void updateOrderId() {
            this.update();
        }

        public int CompareTo( object obj ) {
            FileCategory t = obj as FileCategory;
            if (this.OrderId > t.OrderId) return -1;
            if (this.OrderId < t.OrderId) return 1;
            if (this.Id > t.Id) return 1;
            if (this.Id < t.Id) return -1;
            return 0;
        }

        //-----------------------------------------------------------------------------------

        public static FileCategory GetById( int id ) {
            return cdb.findById<FileCategory>( id );
        }

        private static List<FileCategory> GetAll() {
            List<FileCategory> list = cdb.findAll<FileCategory>();
            list.Sort();
            return list;
        }

        public static List<FileCategory> GetRootList() {

            List<FileCategory> list = GetAll();

            List<FileCategory> results = new List<FileCategory>();
            foreach (FileCategory f in list) {
                if (f.ParentId == 0) results.Add( f );
            }

            return results;
        }

        public static List<FileCategory> GetSubCategories() {
            List<FileCategory> list = GetAll();

            List<FileCategory> results = new List<FileCategory>();
            foreach (FileCategory f in list) {
                if (f.ParentId > 0) results.Add( f );
            }

            return results;
        }

        public static List<ViewCat> GetSubCatsForSelect() {

            List<ViewCat> results = new List<ViewCat>();
            List<FileCategory> list = GetSubCategories();
            foreach (FileCategory f in list) {
                ViewCat c = new ViewCat();
                c.Id = f.Id;
                c.Name = f.Name;
                c.ParentId = f.ParentId;
                results.Add( c );
            }

            return results;
        }

        public static String GetSubCatsJson() {

            List<ViewCat> subcats = GetSubCatsForSelect();
            String jsons = Json.ToStringList( subcats );
            return jsons;
        }

        public static List<FileCategory> GetByParentId( int parentId ) {
            List<FileCategory> list = GetAll();

            List<FileCategory> results = new List<FileCategory>();
            foreach (FileCategory f in list) {
                if (f.ParentId == parentId) results.Add( f );
            }

            return results;
        }


        public static string GetName( int id ) {
            return cdb.findById<FileCategory>( id ).Name;
        }

        public static string GetParentName( int id ) {
            FileCategory c = cdb.findById<FileCategory>( id );
            return cdb.findById<FileCategory>( c.ParentId ).Name;
        }

        public static int GetParentId( int id ) {
            FileCategory c = cdb.findById<FileCategory>( id );
            return c.ParentId;
        }


    }

}
