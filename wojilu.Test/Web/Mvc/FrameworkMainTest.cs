using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wojilu.Web;
using wojilu.Web.Mvc;
using System.Web;
using System.IO;
using NUnit.Framework;
using System.Collections;
using System.Collections.Specialized;
using wojilu.Web.Controller.Admin;

namespace wojilu.Test.Web.Mvc {

    [TestFixture]
    public class FrameworkMainTest {

        [Test]
        public void InitSite() {


            //StringWriter sw = new StringWriter();
            //IWebContext webContext = MockWebContext.New( 1, "http://localhost:5111/Admin/Members/User/Index.aspx", sw );

            //AdminSecurityUtils.SetSession( webContext ); // 授予后台访问许可

            //new CoreHandler().ProcessRequest( webContext );

            ////Console.WriteLine( sw.ToString() );

            //List<string> list = CurrentRequest.getItem( Template.loadedTemplates ) as List<string>;
            //if (list != null) {
            //    foreach (string tpl in list) Console.WriteLine( tpl );
            //}

            //CurrentRequest.setRequest( null );

            ////webContext = MockWebContext.New( "http://localhost/group.aspx", sw );
            ////new CoreHandler().ProcessRequest( webContext );

            ////CurrentRequest.setRequest( null );

        }




        [Test]
        public void testPath() {

            // 如果后一个path以反斜杠开头，则不能正确合并
            string path = Path.Combine( "D:\\Projects\\wojilu\\wojilu.TestCommon\\bin\\Debug", "\\framework\\data\\wojilu.DI.MapItem.config" );
            Assert.AreEqual( @"\framework\data\wojilu.DI.MapItem.config", path );


            string path2 = Path.Combine( "D:\\Projects\\wojilu\\wojilu.TestCommon\\bin\\Debug", "framework//data//wojilu.DI.MapItem.config" );
            Console.WriteLine( path2 );

            String path3 = PathHelper.CombineAbs( new string[] { AppDomain.CurrentDomain.BaseDirectory, "/framework/", "data", "wojilu.DI.MapItem.config" } );
            Console.WriteLine( path3 );

        }



    }

}
