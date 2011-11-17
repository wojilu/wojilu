using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace wojilu.Test.Platform {

    [TestFixture]
    public class MoveRowTest {







        [Test]
        public void testMoveRow() {

            String strRow = "3/1/2";
            MoveRowInfo mr = new MoveRowInfo();

            int rowId = 1;
            String action = "down";
            mr.RowString = "1/3/2";
            mr.Row1 = 1;
            mr.Row2 = 2;
            Assert.AreEqual( mr, moveRow( strRow, action, rowId ) );


            rowId = 2;
            action = "up";
            mr.RowString = "1/3/2";
            mr.Row1 = 1;
            mr.Row2 = 2;
            Assert.AreEqual( mr, moveRow( strRow, action, rowId ) );


            rowId = 2;
            action = "down";
            mr.RowString = "3/2/1";
            mr.Row1 = 2;
            mr.Row2 = 3;
            Assert.AreEqual( mr, moveRow( strRow, action, rowId ) );


            rowId = 3;
            action = "up";
            mr.RowString = "3/2/1";
            mr.Row1 = 2;
            mr.Row2 = 3;
            Assert.AreEqual( mr, moveRow( strRow, action, rowId ) );

            //---------------------------------------------

            //strRow = "3";
            //rowId = 1;
            //action = "down";
            //mr.RowString = "3";
            //mr.Row1 = 1;
            //mr.Row2 = 1;
            //Assert.AreEqual( mr, moveRow( strRow, action, rowId ) );



        }

        private MoveRowInfo moveRow( string strRow, string action, int rowId ) {

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

        [Test]
        public void testStyleUpdate() {

            String strRow = "3/1/2/1/2/1/2";
            int rowId = 1;
            String action = "down";

            MoveRowInfo mr = new MoveRowInfo();
            mr.RowString = "1/3/2/1/2/1/2";
            mr.Row1 = 1;
            mr.Row2 = 2;
            mr.RowStringOld = strRow;
            Assert.AreEqual( mr, moveRow( strRow, action, rowId ) );

            String style = @"
#row1_column1 {width:29%;margin-right:8px;margin-left:0px; }

#row1_column2 {width:43%; }

#row1_column3 {width:25%;margin-left:8px;margin-right:0px; }



#row2_column1 {width:100%; }

#row3_column1 {width:72%;margin-right:8px;margin-left:0px; }

#row3_column2 {width:25%; }



#row4_column1 {width:100%; }

#row5_column1 {width:72%;margin-right:8px;margin-left:0px; }

#row5_column2 {width:25%;}



#row6_column1 {width:100%; }

#row7_column1 {width:72%;margin-right:8px;margin-left:0px; }

#row7_column2 {width:25%; }

#portalContainer  {color:#333333;}

#portalContainer a {color:}

#portalContainer .sectionPanel {border:1px #e6e6e6 solid;}

.sectionTitle {background:url(/static/skin/apps/content/1/sectionTitleBg.jpg);}

#portalContainer .sectionTitleText a {color:#839300;}

#portalContainer .sectionMore a {color:#666;}

#row1_column3 li{font-size:12px;}

#row3_column2 li{font-size:12px;}

#row5_column2 li{font-size:12px;}

#row7_column2 li{font-size:12px;}

#section252.sectionPanel {border:0px;}

#sectionTitle252 {background:url(/static/img/big/rowBg1.jpg);}

#sectionTitle252 .sectionTitleText a{color:#fff;}

#section255.sectionPanel {border:0px;}

#sectionTitle255 {background:url(/static/img/big/rowBg2.jpg);}

#sectionTitle255 .sectionTitleText a{color:#fff;}

#section258.sectionPanel {border:0px;}

#sectionTitle258 {background:url(/static/img/big/rowBg3.jpg);}

#sectionTitle258 .sectionTitleText a{color:#fff;}

";
            String nstyle = updateStyle( style, mr );


            //foreach (KeyValuePair<string, string> kv in log) {
            //    Console.WriteLine( kv.Key + "=>" + kv.Value );
            //}

            Console.WriteLine( nstyle );

            String tstyle = nstyle.Replace( "#row1_column1", "#row2_column1" );

        }


        private String updateStyle( String style, MoveRowInfo mr ) {

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

        private string restoreTemp( string style, int columns, int rowId ) {

            for (int k = 0; k < columns; k++) {

                String colId = string.Format( "_#_row{0}_column{1}", rowId, (k + 1) );
                String newColId = string.Format( "#row{0}_column{1}", rowId, (k + 1) );

                style = style.Replace( colId, newColId );

            }

            return style;

        }

        private String replaceColumnId( String style, int columns, int rowId, int targetRowId, Boolean isTemp ) {

            for (int k = 0; k < columns; k++) {

                String colId = string.Format( "#row{0}_column{1}", rowId, (k + 1) );

                String newColId = "";
                if (isTemp) {
                    newColId = string.Format( "_#_row{0}_column{1}", targetRowId, (k + 1) );
                }
                else {
                    newColId = string.Format( "#row{0}_column{1}", targetRowId, (k + 1) );
                }

                //log.Add( colId, newColId );

                style = style.Replace( colId, newColId );

            }

            return style;
        }

        //private Dictionary<string, string> log = new Dictionary<string, string>();



    }




    public class MoveRowInfo {

        public String RowString { get; set; }
        public int Row1 { get; set; }
        public int Row2 { get; set; }

        public String RowStringOld { get; set; }

        public override bool Equals( object obj ) {

            MoveRowInfo t = obj as MoveRowInfo;

            return (
                    (t.Row1 == this.Row1 && t.Row2 == this.Row2) ||
                    (t.Row1 == this.Row2 && t.Row1 == this.Row1)
                ) &&
                t.RowString == this.RowString;
        }

        public override int GetHashCode() {
            return base.GetHashCode();
        }


    }
}
