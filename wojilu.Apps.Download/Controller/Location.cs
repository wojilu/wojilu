using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Context;
using wojilu.Apps.Download.Domain;

namespace wojilu.Web.Controller.Download {

    public class Location {

        public static String GetFile( MvcContext ctx, FileItem f ) {
            String catLocation = GetCategory( ctx, f.CategoryId );
            String fileLink = string.Format( "<a href=\"{0}\">{1}</a>", ctx.link.To( new FileController().Show, f.Id ), f.Title );
            return catLocation + " " + separator + " " + fileLink;
        }

        public static String GetCategory( MvcContext ctx, int categoryId ) {

            FileCategory cat = FileCategory.GetById( categoryId );
            if (cat.ParentId == 0) {
                return getCatLink( ctx, cat );
            }
            else {
                FileCategory parent = FileCategory.GetById( cat.ParentId );
                return getCatLink( ctx, parent ) + " " + separator + " " + getCatLink( ctx, cat );
            }
        }

        private static String getCatLink( MvcContext ctx, FileCategory cat ) {
            String catLink = string.Format( "<a href=\"{0}\">{1}</a>", ctx.link.To( new CategoryController().Show, cat.Id ), cat.Name );
            return catLink;
        }

        public static String GetSubCategories( MvcContext ctx, FileCategory c ) {

            int rootId = c.Id;
            if (c.ParentId > 0) rootId = c.ParentId;

            StringBuilder sb = new StringBuilder();

            List<FileCategory> subs = FileCategory.GetByParentId( rootId );
            foreach (FileCategory sub in subs) {

                sb.AppendFormat( "<a href=\"{0}\">{1}</a> ", ctx.link.To( new CategoryController().Show, sub.Id ), sub.Name );

            }

            return sb.ToString();
        }

        public static readonly String separator = "&rsaquo;&rsaquo;";


    }

}
