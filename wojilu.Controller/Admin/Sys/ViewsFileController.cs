using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Caching;

namespace wojilu.Web.Controller.Admin.Sys {

    public class ViewsFileController : SystemFileController {

        public ViewsFileController() { LayoutControllerType = typeof( SiteSkinController ); }
        public override String getRootPath() { return MvcConfig.Instance.ViewDir; }
        public override String getEditTip() { return lang( "editViewsTip" ); }

        public void Search() {

            target( Search );

            String addr = ctx.Post( "webAddress" );
            if (strUtil.IsNullOrEmpty( addr ) || addr.StartsWith( "http:" ) == false) {
                set( "dirAndFiles", "" );
                return;
            }

            IWebContext webContext = MockWebContext.New( ctx.viewer.Id, addr, new StringWriter() );
            AdminSecurityUtils.SetSession( webContext );
            try {
                new CoreHandler().ProcessRequest( webContext );
            }
            catch (Exception ex) {
                set( "dirAndFiles", "<div class=\"warning\">" + lang( "exSearchViews" ) + "</div><div style=\"border:1px #f2f2f2 solid; margin-top:10px; padding:10px;display:none;\">" + ex.ToString().Replace( Environment.NewLine, "<br/>" ) + "</div>" );
                return;
            }

            List<string> list = CurrentRequest.getItem( Template.loadedTemplates ) as List<string>;

            bindResults( list );
        }

        private void bindResults( List<string> list ) {

            if (list == null || list.Count <= 1) {
                set( "dirAndFiles", "" );
                return;
            }

            StringBuilder sb = new StringBuilder();
            List<string> added = new List<string>();
            String absRootDir = PathHelper.Map( MvcConfig.Instance.ViewDir );

            // 跳过第一个模板(当前搜索页的模板)
            for (int i = 1; i < list.Count; i++) {
                String tpl = list[i];
                if (added.Contains( tpl )) continue;

                sb.Append( "<div class=\"line file\">" );
                String fileName = getParamItem( tpl, absRootDir );
                sb.AppendFormat( "<a href=\"{1}\">{0}</a>", fileName, to( Edit ) + "?file=" + fileName );
                sb.Append( "</div>" );

                added.Add( tpl );
            }

            set( "dirAndFiles", sb.ToString() );
        }

        public override void afterUpdate() {
            sys.Clear.ClearTemplateCache();
            sys.Clear.ClearOrmCache();
        }

    }


}
