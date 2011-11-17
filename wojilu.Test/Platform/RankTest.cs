using System;
using System.Text;

using NUnit.Framework;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Routes;
using System.Collections;
using wojilu.Members.Sites.Domain;



namespace wojilu.Test.Platform {

    [TestFixture]
    public class RankTest {





        [Test]
        public void testGetRank() {


            Assert.AreEqual( getRank( 2 ).Name, "游民" );
            Assert.AreEqual( getRank( 5 ).Name, "初学弟子" );
            Assert.AreEqual( getRank( 32 ).Name, "江湖新秀" );
            Assert.AreEqual( getRank( 60 ).Name, "江湖少侠" );
            Assert.AreEqual( getRank( 50 ).Name, "江湖少侠" );
            Assert.AreEqual( getRank( 150 ).Name, "江湖大侠" );
            Assert.AreEqual( getRank( 220 ).Name, "江湖大侠" );
            Assert.AreEqual( getRank( 260 ).Name, "江湖豪侠" );
            Assert.AreEqual( getRank( 499 ).Name, "江湖豪侠" );

            Assert.AreEqual( getRank( 500 ).Name, "一派掌门" );
            Assert.AreEqual( getRank( 9999 ).Name, "一派掌门" );

        }

        private SiteRank getRank( int credit ) {

            IList ranks = getRanks();


            SiteRank lastRank = new SiteRank { Credit = Int32.MaxValue };

            for (int i = 0; i < ranks.Count; i++) {

                SiteRank rank = ranks[i] as SiteRank;

                SiteRank nextRank = null;
                if (i < ranks.Count - 1) {
                    nextRank = ranks[i + 1] as SiteRank;
                }
                else
                    nextRank = lastRank;

                if (credit >= rank.Credit && credit < nextRank.Credit) return rank;
            }

            return lastRank;

        }

        private IList getRanks() {
            IList ranks = new ArrayList();
            ranks.Add( new SiteRank { Name = "游民", Credit = 0 } );
            ranks.Add( new SiteRank { Name = "初学弟子", Credit = 5 } );
            ranks.Add( new SiteRank { Name = "初入江湖", Credit = 20 } );
            ranks.Add( new SiteRank { Name = "江湖新秀", Credit = 30 } );
            ranks.Add( new SiteRank { Name = "江湖少侠", Credit = 50 } );
            ranks.Add( new SiteRank { Name = "江湖大侠", Credit = 100 } );
            ranks.Add( new SiteRank { Name = "江湖豪侠", Credit = 250 } );
            ranks.Add( new SiteRank { Name = "一派掌门", Credit = 500 } );
            return ranks;
        }


    }
}
