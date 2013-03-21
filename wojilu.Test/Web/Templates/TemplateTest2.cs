using System;
using wojilu.Web;
using NUnit.Framework;
using System.Collections;
using wojilu.Test.Orm.Utils;

namespace wojilu.Test.Web.Templates {

    [TestFixture]
    public class TemplateTest2 {






		public void Test() {
			//TReg.Init();
			Console.WriteLine( "模板处理……" );
			SpeedUtil.Start();
            loopThird();
			SpeedUtil.Stop();
		}


        [Test]
        public void loopThird() {
            ConsoleTitleUtil.ShowTestTitle( "loopThird" );

			string looppage = @"
<table>
<!-- BEGIN newphotos -->
	<tr><td>#{Title}</td><td>#{CreateTime}</td></tr>
	<tr><td colspan=2><!-- BEGIN item --> #{Name} 
<!-- BEGIN suitem -->
#{SubName}<br/><!-- END suitem -->
<!-- END item --></td></tr>
<!-- END newphotos -->
</table>
";

			Template t = new Template();
			t.InitContent( looppage );

            IBlock block = t.GetBlock( "newphotos" );
			for( int i=0;i<3;i++ ) {
				block.Set( "Title", "mytitle" + i );
				block.Set( "CreateTime", "190"+i );

                IBlock block2 = block.GetBlock( "item" );
				for( int k=0;k<3;k++ ) {
					block2.Set( "Name", "nameis"+k);
                    IBlock subblock = block2.GetBlock( "suitem" );
					for( int j=0;j<3;j++ ) {
						subblock.Set( "SubName", "xxsub"+j );
						subblock.Next();
					}

					block2.Next();
				}

				block.Next();
			}

			Console.WriteLine( t );


		}

        [Test]
        public void loopAgain() {
            ConsoleTitleUtil.ShowTestTitle( "loopAgain" );

			string looppage = @"
<table>
<!-- BEGIN newphotos -->
	<tr><td>#{Title}</td><td>#{CreateTime}</td></tr>
	<tr><td colspan=2><!-- BEGIN item --> #{Name} <!-- END item --></td></tr>
<!-- END newphotos -->
</table>
";
			//TReg.Init();

			Template t = new Template();
			t.InitContent( looppage );

            IBlock block = t.GetBlock( "newphotos" );
			for( int i=0;i<5;i++ ) {
				block.Set( "Title", "mytitle" + i );
				block.Set( "CreateTime", "190"+i );

                IBlock block2 = block.GetBlock( "item" );
				for( int k=0;k<5;k++ ) {
					block2.Set( "Name", "nameis"+k);
					block2.Next();
				}

				block.Next();
			}

			Console.WriteLine( t );


		}

        [Test]
        public void loop() {
            ConsoleTitleUtil.ShowTestTitle( "loop" );


			string looppage = @"
<table><!-- BEGIN newphotos -->
	<tr><td>#{Title}</td><td>#{CreateTime}</td></tr><!-- END newphotos -->
</table>
";

			Template loopTemplate = new Template();
			loopTemplate.InitContent( looppage );

			// 给循环块设值
            IBlock loopBlock = loopTemplate.GetBlock( "newphotos" );
			for( int i=0;i<5;i++ ) {
				loopBlock.Set( "Title", "一百航的诗篇"+i );
				loopBlock.Set( "CreateTime", DateTime.Now.ToString() );
				loopBlock.Next();
			}

			Console.WriteLine( loopTemplate );

		}

//        [Test]
//        public void loopNew() {
//            TestUtil.ShowTestTitle( "loop" );


//            string looppage = @"
//<table><!-- BEGIN newphotos -->
//	<tr><td>#{Title}</td><td>#{CreateTime}</td></tr><!-- END newphotos -->
//</table>
//";

//            SyyTemplate loopTemplate = new SyyTemplate();

//            loopTemplate.InitContent( looppage );

//            SyyLoopBlock loopBlock = loopTemplate.GetBlock( "newphotos" );

//            // 给循环块设值
//            //LoopBlock loopBlock = loopTemplate.GetBlock( "newphotos" );
//            for (int i = 0 ; i < 5 ; i++) {
//                loopBlock.Set( "Title", "一百航的诗篇" + i );
//                loopBlock.Set( "CreateTime", DateTime.Now.ToString() );
//                loopBlock.Next();
//            }

//            Console.WriteLine( loopTemplate );

//        }

        [Test]
        public void single() {
            ConsoleTitleUtil.ShowTestTitle( "single" );

			string a = @"  
<h4>
#{Title}</h4> 
<b>#{CreateTime}</b><br />
	";
			SpeedUtil.Start();

			Template t = new Template();
			t.InitContent( a );

			t.Set( "Title", "一百航的诗篇" );
			t.Set( "CreateTime", "今年的时间 " + DateTime.Now );

			Console.WriteLine( t );
			SpeedUtil.Stop();

		}

//        [Test]
//        public void singleNew() {
//            string a = @"  
//<h4>
//#{Title}</h4> 
//<b>#{CreateTime}</b><br />
//	";
//            timer.Start();

//            SyyTemplate t = new SyyTemplate();
//            t.InitContent( a );

//            t.Set( "Title", "一百航的诗篇" );
//            t.Set( "CreateTime", "今年的时间 " + DateTime.Now );

//            Console.WriteLine( t );
//            timer.Stop();
//        }


	}


    class ViewData {
        private string _Name;

        public string Name {
            get { return _Name; }
            set { _Name = value; }
        }

        private string _Title;

        public string Title {
            get { return _Title; }
            set { _Title = value; }
        }

        private DateTime _Created;

        public DateTime Created {
            get { return _Created; }
            set { _Created = value; }
        }
    }
}
