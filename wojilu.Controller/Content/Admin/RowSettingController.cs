using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Apps.Content.Service;
using wojilu.Apps.Content.Domain;
using wojilu.Web.Mvc.Attr;
using wojilu.Apps.Content.Interface;
using wojilu.Web.UI;

namespace wojilu.Web.Controller.Content.Admin {

    [App( typeof( ContentApp ) )]
    public class RowController : ControllerBase {

        public RowService rowService { get; set; }
        public IContentSectionService sectionService { get; set; }

        public RowController() {
            rowService = new RowService();
            sectionService = new ContentSectionService();
        }

        public void ConfirmAddRow( int columnCount ) {
            target( AddRow, columnCount );
            if (columnCount <= 0 || columnCount > 6) {
                echo( "column count error" );
                return;
            }
            String row = alang( "addRow" + columnCount );
            set( "rowText", row );
        }

        [HttpPost, DbTransaction]
        public void AddRow( int columnCount ) {

            ContentApp app = ctx.app.obj as ContentApp;

            app.Layout = app.Layout + "/" + columnCount;

            int row = app.RowList.Length;
            String newStyle = getStyle( row, columnCount );
            String mergedStyle = CssFormUtil.mergeStyle( app.Style, newStyle );

            app.Style = mergedStyle;

            db.update( app, new string[] { "Layout", "Style" } );

            echoToParentPrivate();
        }


        [HttpPost]
        public void Move() {

            ContentApp app = ctx.app.obj as ContentApp;

            MoveRowInfo mr = rowService.MoveRow( app.Layout, ctx.Get( "action" ), ctx.GetInt( "rowId" ) );

            if (strUtil.IsNullOrEmpty( mr.RowString )) {

                errors.Add( "参数错误" );
                echoError();
            }
            else {
                rowService.UpdateRow( app, mr );
                echoAjaxOk();
            }
        }

        public void DeleteRow( int rowId ) {
            target( DeleteRowSave, rowId );
            ContentApp article = ctx.app.obj as ContentApp;
            if (sectionService.Count( ctx.app.Id, rowId ) > 0) {
                echo( alang( "exRemoveSectionFirst" ) );
            }
        }

        [HttpPost, DbTransaction]
        public void DeleteRowSave( int rowId ) {
            ContentApp app = ctx.app.obj as ContentApp;
            if (sectionService.Count( ctx.app.Id, rowId ) > 0) {
                echoToParentPart( alang( "exRemoveSectionFirst" ) );
                return;
            }

            rowService.DeleteRow( app, rowId );
            echoToParentPart( lang( "opok" ) );
        }

        private String getStyle( int row, int columnCount ) {

            String style = Environment.NewLine;
            int width = 100 / columnCount - 2;
            for (int i = 1; i < columnCount + 1; i++) {

                String margin = "margin-top:5px; margin-right:5px; margin-bottom:5px; margin-left:5px;";
                if (i == 1) margin = "margin-top:5px; margin-right:5px; margin-bottom:5px; margin-left:10px;";

                style += "#row" + row + "_column" + i + " { width:" + width + "%; " + margin + " }" + Environment.NewLine;
            }
            return style;
        }

        private void echoToParentPrivate() {
            echoToParentPart( lang( "opok" ), to( new ContentController().Home ), 0 );
        }

    }

}
