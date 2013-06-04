using System;
using System.Collections.Generic;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Apps.Content.Domain;
using wojilu.Apps.Content.Service;
using wojilu.Apps.Content.Interface;
using wojilu.Common.AppBase;
using System.IO;

namespace wojilu.Web.Controller.Content.Admin {

    [App( typeof( ContentApp ) )]
    public class ExportController : ControllerBase {

        private static readonly ILog logger = LogManager.GetLogger( typeof( ExportController ) );

        public IContentSectionService sectionService { get; set; }
        public IContentPostService postService { get; set; }

        public ExportController() {
            sectionService = new ContentSectionService();
            postService = new ContentPostService();
        }

        public void Index() {

            target( Export );

        }

        [HttpPost]
        public void Export() {

            ContentTheme theme = new ContentTheme();
            theme.Name = ctx.Post( "Name" );
            theme.Description = strUtil.CutString( ctx.Post( "Description" ), 200 );

            if (strUtil.IsNullOrEmpty( theme.Name )) {
                echoError( "请填写主题名称" );
                return;
            }


            List<ContentSection> sectionList = sectionService.GetByApp( ctx.app.Id );

            // 1、获取app信息
            ContentApp app = ctx.app.obj as ContentApp;
            XApp xapp = exportApp( app );
            xapp.SectionList = new List<XSection>();

            // 2、循环导出section
            string[] rowList = app.RowList;

            // 循环行
            for (int iRow = 1; iRow < (rowList.Length + 1); iRow++) {

                int columnCount = cvt.ToInt( rowList[iRow - 1] );
                if (columnCount <= 0) continue;

                // 循环列
                for (int iColumn = 1; iColumn < (columnCount + 1); iColumn++) {

                    List<ContentSection> sections = sectionService.GetByRowColumn( sectionList, iRow, iColumn );

                    // 循环section
                    for (int iSection = 1; iSection < sections.Count + 1; iSection++) {

                        XSection xSection = getSection( sections[iSection - 1], iRow, iColumn, iSection );
                        xapp.SectionList.Add( xSection );
                    }
                }
            }

            // 处理 #sectionId 的css问题，==> 转化成 #portarContainer .row-iRow .col-iColumn .section-iSection
            processCssId( xapp, sectionList );


            // 3、保存为 json 格式
            exportToDisk( xapp, theme );

            echoToParentPart( lang( "opok" ) );
        }

        /// <summary>
        /// 将安装包保存到磁盘
        /// </summary>
        /// <param name="xapp"></param>
        private void exportToDisk( XApp xapp, ContentTheme theme ) {

            String jsonString = Json.ToString( xapp );
            theme.Insert( jsonString );
        }

        private void processCssId( XApp xapp, List<ContentSection> sectionList ) {

            foreach (ContentSection s in sectionList) {

                XSection xSection = getXSection( xapp.SectionList, s );

                replaceIdToClass( xapp, "#section" + s.Id, xSection.CssPath + " " );
                replaceIdToClass( xapp, "#sectionTitle" + s.Id, xSection.CssPath + " .sectionTitle " );
                replaceIdToClass( xapp, "#sectionContent" + s.Id, xSection.CssPath + " .sectionContent " );
                replaceIdToClass( xapp, "#sectionContentText" + s.Id, xSection.CssPath + " .sectionContentText " );
            }

        }

        private void replaceIdToClass( XApp xapp, String id, String cls ) {

            xapp.Style = xapp.Style.Replace( id, cls );
            xapp.SkinStyle = xapp.SkinStyle.Replace( id, cls );

        }

        private XSection getXSection( List<XSection> xlist, ContentSection s ) {
            foreach (XSection x in xlist) {
                if (x.Id == s.Id) return x;
            }
            return null;
        }

        private XSection getSection( ContentSection section, int iRow, int iColumn, int iSection ) {

            XSection x = new XSection();

            x.Id = section.Id;

            x.Name = section.Title;
            x.LayoutStr = string.Format( "{0}{1}", iRow, iColumn );
            x.CssClass = section.CssClass;
            x.ListCount = section.ListCount;
            x.TypeFullName = section.SectionType;
            x.TemplateId = section.TemplateId;
            x.TemplateCustom = getCustomTemplateBody( section.CustomTemplateId );

            x.ServiceId = section.ServiceId;
            x.ServiceParams = section.ServiceParams;

            x.PostList = getPostList( section );

            x.CssPath = string.Format( "#row{0} .column{1} .section-{2}", iRow, iColumn, iSection );

            return x;
        }

        private List<XPost> getPostList( ContentSection section ) {

            List<XPost> list = new List<XPost>();

            String adminSectionType = section.SectionType.Replace( "Content.Section", "Content.Admin.Section" );

            IPageAdminSection adminSection = ObjectContext.Create( adminSectionType ) as IPageAdminSection;
            if (adminSection == null) return list;

            ControllerBase controller = adminSection as ControllerBase;
            controller.ctx = ctx;

            List<ContentPost> posts = adminSection.GetSectionPosts( section.Id );

            foreach (ContentPost post in posts) {

                XPost x = new XPost();

                x.Title = post.Title;
                x.Content = post.Content;
                x.Pic = post.GetImgOriginal();
                x.Summary = post.Summary;

                x.TitleHome = post.TitleHome;
                x.CategoryId = post.CategoryId;
                x.SourceLink = post.SourceLink;
                x.PickStatus = post.PickStatus;

                x.Width = post.Width;
                x.Height = post.Height;

                list.Add( x );

            }
            return list;
        }

        private string getCustomTemplateBody( int id ) {

            ContentCustomTemplate x = ContentCustomTemplate.findById( id );
            return x == null ? "" : x.Content;
        }

        private XApp exportApp( ContentApp app ) {
            XApp x = new XApp();
            x.Style = app.Style;
            x.Layout = app.Layout;
            x.SkinStyle = app.SkinStyle;
            return x;
        }

    }

}
