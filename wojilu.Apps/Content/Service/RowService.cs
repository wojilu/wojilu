using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Apps.Content.Domain;
using wojilu.Web.UI;

namespace wojilu.Apps.Content.Service {

    public class MoveRowInfo {

        public String RowString { get; set; }
        public int Row1 { get; set; }
        public int Row2 { get; set; }

        public String RowStringOld { get; set; }
    }

    public class RowService {

        public void DeleteRow( ContentApp app,  int rowId ) {

            int lastRowId = moveRowToEnd( app, rowId );
            deleteRowPrivate( app, lastRowId );

        }

        private int moveRowToEnd( ContentApp app, int rowId ) {

            /*
next row int[]=4*,5,6,7
moveRow( 'up', 5 )=>4,5*,6,7
moveRow( 'up', 6 )=>4,5,6*,7
moveRow( 'up', 7 )=>4,5,6,7*
            */

            string[] rowList = app.RowList;
            if (rowId <= 0 || rowId > rowList.Length) return 0;

            for (int i = rowId + 1; i < rowList.Length + 1; i++) {

                MoveRowInfo mr = this.MoveRow( app.Layout, "up", i );
                this.UpdateRow( app, mr );

            }

            return rowList.Length;
        }

        private static void deleteRowPrivate( ContentApp app, int rowId ) {

            if (rowId <= 0) return;

            int appId = app.Id;
            List<string> list = new List<string>();
            string[] rowList = app.RowList;

            if (rowId <= rowList.Length) {

                if (rowId < rowList.Length) {
                    ContentSection.updateBatch( "RowId=RowId-1", "RowId>" + rowId + " and AppId=" + appId );
                }
                for (int i = 1; i <= rowList.Length; i++) {
                    if (rowId != i) {
                        list.Add( rowList[i - 1] );
                    }
                }
                String strLayout = string.Empty;
                foreach (String str in list) {
                    strLayout = strLayout + str + "/";
                }
                strLayout = strLayout.TrimEnd( '/' );
                app.Layout = strLayout;
                app.update( "Layout" );
            }
        }

        //---------------------------------------------------------------------------------------------

        public MoveRowInfo MoveRow( string strRow, string action, int rowId ) {

            string[] arrRow = strRow.Split( '/' );

            int rowId1 = 0;
            int rowId2 = 0;

            if (action == "up") {
                rowId1 = rowId - 1;
                rowId2 = rowId;

            }
            else if (action == "down") {
                rowId1 = rowId;
                rowId2 = rowId + 1;
            }

            if (rowActionError( arrRow.Length, action, rowId, rowId1, rowId2 )) return new MoveRowInfo();

            MoveRowInfo mr = new MoveRowInfo();
            mr.Row1 = rowId1;
            mr.Row2 = rowId2;
            mr.RowString = getNewRowString( arrRow, action, rowId );
            mr.RowStringOld = strRow;
            return mr;
        }

        private string getNewRowString( string[] arrRow, string action, int rowId ) {

            if (action == "up") {
                string currentRowId = arrRow[rowId - 1];
                arrRow[rowId - 1] = arrRow[rowId - 2];
                arrRow[rowId - 2] = currentRowId;

            }
            else if (action == "down") {

                string currentRowId = arrRow[rowId - 1];
                arrRow[rowId - 1] = arrRow[rowId];
                arrRow[rowId] = currentRowId;
            }

            String str = "";
            for (int i = 0; i < arrRow.Length; i++) {
                str += arrRow[i];
                if (i < arrRow.Length - 1) str += "/";
            }

            return str;
        }

        private bool rowActionError( int rowCount, string action, int rowId, int rowId1, int rowId2 ) {

            if (rowId1 > rowCount || rowId1 < 1) return true;
            if (rowId2 > rowCount || rowId2 < 1) return true;

            if (action != "up" && action != "down") return true;
            return false;
        }
        
        public void UpdateRow( ContentApp app, MoveRowInfo mr ) {

            app.Layout = mr.RowString;
            app.Style = updateStyle( app.Style, mr );
            app.SkinStyle = updateStyle( app.SkinStyle, mr );
            app.update();

            int tempId = 9999;
            ContentSection.updateBatch( "RowId=" + tempId, "AppId=" + app.Id + " and RowId=" + mr.Row1 );
            ContentSection.updateBatch( "RowId=" + mr.Row1, "AppId=" + app.Id + " and RowId=" + mr.Row2 );
            ContentSection.updateBatch( "RowId=" + mr.Row2, "AppId=" + app.Id + " and RowId=" + tempId );

            List<ContentSection> list = new ContentSectionService().GetByApp( app.Id );
        }

        private static String updateStyle( String style, MoveRowInfo mr ) {

            // 3/1/2  row1_column1=>row2_column1
            // 1/3/2  row2_column1=>row1_column1

            String[] arrRowStr = mr.RowStringOld.Split( '/' );

            int raw1 = mr.Row1 > mr.Row2 ? mr.Row2 : mr.Row1;
            int raw2 = mr.Row1 > mr.Row2 ? mr.Row1 : mr.Row2;

            int tempColumns = 0;
            for (int i = 0; i < arrRowStr.Length; i++) {

                int rowId = i + 1;
                int columns = cvt.ToInt( arrRowStr[i] );

                if (rowId == raw1) {
                    style = replaceColumnId( style, columns, rowId, raw2, true );
                    tempColumns = columns;
                }
                else if (rowId == raw2) {
                    style = replaceColumnId( style, columns, rowId, raw1, false );
                }
            }

            style = restoreTemp( style, tempColumns, raw2 );
            return style;
        }

        private static string restoreTemp( string style, int columns, int rowId ) {

            for (int k = 0; k < columns; k++) {

                String colId = string.Format( "_#_row{0}_column{1}", rowId, (k + 1) );
                String newColId = string.Format( "#row{0}_column{1}", rowId, (k + 1) );

                style = style.Replace( colId, newColId );
            }

            return style;
        }

        private static String replaceColumnId( String style, int columns, int rowId, int targetRowId, Boolean isTemp ) {

            for (int k = 0; k < columns; k++) {

                String colId = string.Format( "#row{0}_column{1}", rowId, (k + 1) );

                String newColId = "";
                if (isTemp) {
                    newColId = string.Format( "_#_row{0}_column{1}", targetRowId, (k + 1) );
                }
                else {
                    newColId = string.Format( "#row{0}_column{1}", targetRowId, (k + 1) );
                }

                style = style.Replace( colId, newColId );

            }

            return style;
        }

    }

}
