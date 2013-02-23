using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using wojilu.OAuth;
using wojilu.OAuth.Connects;

namespace wojilu.Test.OpenAuth {

    [TestFixture]
    public class ConnectConfigTest {




        // 测试之前，请先确保 wojilu.Test/bin/Debug/framework/data/wojilu.OpenAuth.AuthConnectConfig.config 中已经添加 QQConnect
        [Test]
        public void testLoadConfig() {

            List<AuthConnectConfig> cfgList = cdb.findAll<AuthConnectConfig>();
            Assert.IsTrue( cfgList.Count > 0 );

            Dictionary<string, AuthConnect> connects = AuthConnectFactory.GetAllConnects();
            Assert.IsNotNull( connects );
            Assert.IsTrue( connects.Count > 0 );

            String qqConnect = typeof( QQConnect ).FullName;
            AuthConnect authConnect = null;
            connects.TryGetValue( qqConnect, out authConnect );

            Assert.IsNotNull( authConnect );

            AuthConnect objConnect = AuthConnectFactory.GetConnect( qqConnect );
            Assert.IsNotNull( objConnect );
        }





    }
}
