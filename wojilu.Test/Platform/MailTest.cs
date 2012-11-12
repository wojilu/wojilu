using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using wojilu.Config;

namespace wojilu.Test.Platform {

    [TestFixture]
    public class MailTest {

        [Test]
        public void testGetDomain() {

            String defaultName = string.Concat( 'w', 'o', 'j', 'i', 'l', 'u', '.', 'c', 'o', 'm' );
            Assert.AreEqual( defaultName, getDomainName( null ) );
            Assert.AreEqual( defaultName, getDomainName( "       " ) );

            Assert.AreEqual( "126.com", getDomainName( "kkkk@126.com" ) );
            Assert.AreEqual( "sina.com.cn", getDomainName( "kkkk@sina.com.cn" ) );

            Assert.AreEqual( "126.com", getDomainName( "           kkkk@126.com    " ) );
            Assert.AreEqual( "sina.com.cn", getDomainName( "    kkkk@sina.com.cn      " ) );
        }

        private String getDomainName( string smtpUser ) {
            return new SiteSetting().GetSmtpUserDomain( smtpUser );
        }

    }
}
