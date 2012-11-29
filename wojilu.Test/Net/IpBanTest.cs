using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using wojilu.Net;

namespace wojilu.Test.Net {

    [TestFixture]
    public class IpBanTest {











        private static readonly String unknowIp = "unknow";

        [Test]
        public void BandByUtil() {


            List<String> list = new List<String>();

            list.Add( "88.88.99.99" );
            list.Add( "192.168.3.2-192.168.3.250" );
            list.Add( "66.66.66.*" );
            list.Add( "55.55.*.*" );
            list.Add( "44.*.*.*" );
            list.Add( "33.168.10.*-33.168.90.*" );
            list.Add( "unknow" );

            Assert.IsFalse( IpUtil.IsAllowedIp( "88.88.99.99", list ) );
            Assert.IsTrue( IpUtil.IsAllowedIp( "88.88.99.100", list ) );

            Assert.IsFalse( IpUtil.IsAllowedIp( "192.168.3.2", list ) );
            Assert.IsFalse( IpUtil.IsAllowedIp( "192.168.3.250", list ) );
            Assert.IsTrue( IpUtil.IsAllowedIp( "192.168.3.1", list ) );
            Assert.IsTrue( IpUtil.IsAllowedIp( "192.168.3.251", list ) );

            Assert.IsFalse( IpUtil.IsAllowedIp( "66.66.66.1", list ) );
            Assert.IsFalse( IpUtil.IsAllowedIp( "66.66.66.2", list ) );
            Assert.IsFalse( IpUtil.IsAllowedIp( "66.66.66.254", list ) );
            Assert.IsFalse( IpUtil.IsAllowedIp( "66.66.66.255", list ) );

            Assert.IsFalse( IpUtil.IsAllowedIp( "55.55.33.33", list ) );
            Assert.IsFalse( IpUtil.IsAllowedIp( "55.55.99.99", list ) );
            Assert.IsTrue( IpUtil.IsAllowedIp( "55.56.33.33", list ) );

            Assert.IsFalse( IpUtil.IsAllowedIp( "44.55.33.33", list ) );
            Assert.IsFalse( IpUtil.IsAllowedIp( "44.99.33.33", list ) );
            Assert.IsTrue( IpUtil.IsAllowedIp( "45.99.33.33", list ) );

            Assert.IsFalse( IpUtil.IsAllowedIp( "33.168.10.1", list ) );
            Assert.IsFalse( IpUtil.IsAllowedIp( "33.168.10.2", list ) );
            Assert.IsFalse( IpUtil.IsAllowedIp( "33.168.10.255", list ) );
            Assert.IsFalse( IpUtil.IsAllowedIp( "33.168.90.1", list ) );
            Assert.IsFalse( IpUtil.IsAllowedIp( "33.168.90.2", list ) );
            Assert.IsFalse( IpUtil.IsAllowedIp( "33.168.90.255", list ) );

            Assert.IsFalse( IpUtil.IsAllowedIp( "", list ) );
            Assert.IsFalse( IpUtil.IsAllowedIp( "unknow", list ) );
            Assert.IsFalse( IpUtil.IsAllowedIp( "abc", list ) );
            Assert.IsFalse( IpUtil.IsAllowedIp( "-", list ) );

            Assert.IsTrue( IpUtil.IsAllowedIp( "68.81.101.87", list ) );
            Assert.IsTrue( IpUtil.IsAllowedIp( "71.85.125.152", list ) );
            Assert.IsTrue( IpUtil.IsAllowedIp( "85.114.137.152", list ) );
        }






        [Test]
        public void Ban() {

            List<String> list = new List<String>();

            list.Add( "88.88.99.99" );
            list.Add( "192.168.3.2-192.168.3.250" );
            list.Add( "66.66.66.*" );
            list.Add( "55.55.*.*" );
            list.Add( "44.*.*.*" );
            list.Add( "33.168.10.*-33.168.90.*" );
            list.Add( "unknow" );

            Assert.IsFalse( isAllowedIp( "88.88.99.99", list ) );
            Assert.IsTrue( isAllowedIp( "88.88.99.100", list ) );

            Assert.IsFalse( isAllowedIp( "192.168.3.2", list ) );
            Assert.IsFalse( isAllowedIp( "192.168.3.250", list ) );
            Assert.IsTrue( isAllowedIp( "192.168.3.1", list ) );
            Assert.IsTrue( isAllowedIp( "192.168.3.251", list ) );

            Assert.IsFalse( isAllowedIp( "66.66.66.1", list ) );
            Assert.IsFalse( isAllowedIp( "66.66.66.2", list ) );
            Assert.IsFalse( isAllowedIp( "66.66.66.254", list ) );
            Assert.IsFalse( isAllowedIp( "66.66.66.255", list ) );
            
            Assert.IsFalse( isAllowedIp( "55.55.33.33", list ) );
            Assert.IsFalse( isAllowedIp( "55.55.99.99", list ) );
            Assert.IsTrue( isAllowedIp( "55.56.33.33", list ) );

            Assert.IsFalse( isAllowedIp( "44.55.33.33", list ) );
            Assert.IsFalse( isAllowedIp( "44.99.33.33", list ) );
            Assert.IsTrue( isAllowedIp( "45.99.33.33", list ) );

            Assert.IsFalse( isAllowedIp( "33.168.10.1", list ) );
            Assert.IsFalse( isAllowedIp( "33.168.10.2", list ) );
            Assert.IsFalse( isAllowedIp( "33.168.10.255", list ) );
            Assert.IsFalse( isAllowedIp( "33.168.90.1", list ) );
            Assert.IsFalse( isAllowedIp( "33.168.90.2", list ) );
            Assert.IsFalse( isAllowedIp( "33.168.90.255", list ) );

            Assert.IsFalse( isAllowedIp( "", list ) );
            Assert.IsFalse( isAllowedIp( "unknow", list ) );
            Assert.IsFalse( isAllowedIp( "abc", list ) );
            Assert.IsFalse( isAllowedIp( "-", list ) );

            Assert.IsTrue( isAllowedIp( "68.81.101.87", list ) );
            Assert.IsTrue( isAllowedIp( "71.85.125.152", list ) );
            Assert.IsTrue( isAllowedIp( "85.114.137.152", list ) );

        }


        //----------------------------------------------------------------------------------------------

        private bool isAllowedIp( String ip, List<String> list ) {

            if (isIpError( ip )) {
                return !list.Contains( unknowIp );
            }


            foreach (String setting in list) {

                if (setting.Equals( unknowIp )) continue;

                if (isAllowedSingle( ip, setting ) == false) return false;
            }

            return true;
        }

        private bool isAllowedSingle( String ip, String settingItem ) {

            if (settingItem.IndexOf( "-" ) > 0) return isAllowedScaleIp( settingItem, ip );
            if (settingItem.IndexOf( "*" ) >= 0) return isAllowedWildcards( settingItem, ip );

            if (isIpError( settingItem )) return true;// 配置无效

            return !ip.Equals( settingItem );

        }

        //----------------------------------------------------------------------------------------------

        [Test]
        public void TestWildcards() {

            Assert.IsTrue( isAllowedWildcards( "129.105.15.*", "88.88.88.88" ) );
            Assert.IsTrue( isAllowedWildcards( "129.105.15.*", "888.105.15.88" ) );

            Assert.IsFalse( isAllowedWildcards( "129.105.15.*", "129.105.15.1" ) );
            Assert.IsFalse( isAllowedWildcards( "129.105.15.*", "129.105.15.2" ) );
            Assert.IsFalse( isAllowedWildcards( "129.105.15.*", "129.105.15.3" ) );
            Assert.IsFalse( isAllowedWildcards( "129.105.15.*", "129.105.15.254" ) );

        }

        // 192.168.35.*
        private bool isAllowedWildcards( String settingItem, String ip ) {

            if (isIpError( settingItem )) return true; // 配置无效

            String[] arr = settingItem.Split( '.' );
            String[] arrIp = ip.Split( '.' );
            for (int i = 0; i < arr.Length; i++) {
                if (arr[i].Equals( "*" )) continue;
                if (arr[i].Equals( arrIp[i] ) == false) return true;
            }

            return false;
        }

        //----------------------------------------------------------------------------------------------


        // 192.168.3.12-192.168.3.250
        private bool isAllowedScaleIp( String scaleSetting, String ip ) {

            String[] settingItems = scaleSetting.Split( '-' );

            String itemA = settingItems[0];
            String itemB = settingItems[1];

            if (isIpError( itemA )) return true; // 配置无效
            if (isIpError( itemB )) return true; // 配置无效

            Boolean inScale = ipGreaterOrEqual( ip, itemA ) && ipLessOrEqual( ip, itemB );
            return !inScale;
        }

        [Test]
        public void TestScaleAllowIp() {

            Assert.IsFalse( isAllowedScaleIp( "192.1.1.12-192.1.1.250", "192.1.1.12" ) );
            Assert.IsFalse( isAllowedScaleIp( "192.1.1.12-192.1.1.250", "192.1.1.13" ) );
            Assert.IsFalse( isAllowedScaleIp( "192.1.1.12-192.1.1.250", "192.1.1.250" ) );
            
            Assert.IsTrue( isAllowedScaleIp( "192.1.1.12-192.1.1.250", "192.1.2.13" ) );

            Assert.IsTrue( isAllowedScaleIp( "192.1.1.12-192.1.1.250", "97.65.164.215" ) );


            Assert.IsFalse( isAllowedScaleIp( "192.1.10.*-192.1.11.*", "192.1.10.1" ) );
            Assert.IsFalse( isAllowedScaleIp( "192.1.10.*-192.1.11.*", "192.1.10.254" ) );
            Assert.IsFalse( isAllowedScaleIp( "192.1.10.*-192.1.11.*", "192.1.11.1" ) );
            Assert.IsFalse( isAllowedScaleIp( "192.1.10.*-192.1.11.*", "192.1.11.254" ) );

        }


        [Test]
        public void TestGreater() {

            Assert.IsTrue( ipGreaterOrEqual( "128.111.52.63", "128.111.52.62" ) );
            Assert.IsTrue( ipGreaterOrEqual( "128.111.52.88", "128.111.52.62" ) );
            Assert.IsTrue( ipGreaterOrEqual( "128.111.52.88", "128.111.52.*" ) );

            Assert.IsTrue( ipGreaterOrEqual( "128.111.53.88", "128.111.52.88" ) );

            Assert.IsFalse( ipGreaterOrEqual( "128.111.52.22", "128.111.52.88" ) );


        }

        // ip>=itemA
        private bool ipGreaterOrEqual( String ip, String itemA ) {

            String[] arrIp = ip.Split( '.' );
            String[] arrSetting = itemA.Split( '.' );

            for (int i = 0; i < arrSetting.Length; i++) {
                if (arrSetting[i].Equals( "*" )) continue;
                if (cvt.ToInt( arrIp[i] ) < cvt.ToInt( arrSetting[i] )) return false;
            }

            return true;
        }

        // ip<=itemB
        private bool ipLessOrEqual( String ip, String itemA ) {


            String[] arrIp = ip.Split( '.' );
            String[] arrSetting = itemA.Split( '.' );

            for (int i = 0; i < arrSetting.Length; i++) {
                if (arrSetting[i].Equals( "*" )) continue;
                if (cvt.ToInt( arrIp[i] ) > cvt.ToInt( arrSetting[i] )) return false;
            }

            return true;
        }


        //----------------------------------------------------------------------------------------------


        // 配置格式是否正确
        private bool isIpError( string ip ) {

            Boolean error = true;

            if (strUtil.IsNullOrEmpty( ip )) return error;
            String[] arrItem = ip.Split( '.' );
            if (arrItem.Length != 4) return error;

            foreach (String item in arrItem) {
                if (item.Equals( "*" )) continue;
                if (cvt.IsInt( item ) == false) return error;
                int itemValue = cvt.ToInt( item );
                if (itemValue < 0 || itemValue > 255) return error;
            }

            return false;
        }

        [Test]
        public void TestIpFormat() {

            // 空白字符无效
            Assert.IsTrue( isIpError( "" ) );
            Assert.IsTrue( isIpError( " " ) );

            // 少于4位无效
            Assert.IsTrue( isIpError( "33" ) );
            Assert.IsTrue( isIpError( "33.55" ) );
            Assert.IsTrue( isIpError( "33.55.00" ) );

            // 超过4位无效
            Assert.IsTrue( isIpError( "33.55.00.99.22" ) );

            // 非法字符无效
            Assert.IsTrue( isIpError( "k" ) );
            Assert.IsTrue( isIpError( "abc" ) );
            Assert.IsTrue( isIpError( "-" ) );
            Assert.IsTrue( isIpError( "33.55.00.a33" ) );

            // 数值超过255无效
            Assert.IsTrue( isIpError( "33.55.00.256" ) );

            //----------------------有效的格式----------------------------------------

            Assert.IsFalse( isIpError( "33.55.00.86" ) );
            Assert.IsFalse( isIpError( "127.0.0.1" ) );
            Assert.IsFalse( isIpError( "255.255.255.255" ) );
            Assert.IsFalse( isIpError( "97.65.164.215" ) );
            Assert.IsFalse( isIpError( "130.245.191.59" ) );
            Assert.IsFalse( isIpError( "142.150.238.13" ) );
            Assert.IsFalse( isIpError( "128.227.56.8" ) );

            // 允许通配符
            Assert.IsFalse( isIpError( "129.107.35.*" ) );
            Assert.IsFalse( isIpError( "129.107.*.131" ) );
            Assert.IsFalse( isIpError( "129.*.35.131" ) );
            Assert.IsFalse( isIpError( "*.107.35.131" ) );
            Assert.IsFalse( isIpError( "*.*.*.*" ) );

        }

        public void testIpWildResult() {

            Assert.AreEqual( IpUtil.GetIpWild( "97.65.164.215", 0 ), "97.65.164.215" );
            Assert.AreEqual( IpUtil.GetIpWild( "97.65.164.215", 5 ), "97.65.164.215" );
            Assert.AreEqual( IpUtil.GetIpWild( "97.65.164.215", 1 ), "97.65.164.*" );
            Assert.AreEqual( IpUtil.GetIpWild( "97.65.164.215", 2 ), "97.65.*.*" );
            Assert.AreEqual( IpUtil.GetIpWild( "97.65.164.215", 3 ), "97.*.*.*" );
            Assert.AreEqual( IpUtil.GetIpWild( "97.65.164.215", 4 ), "*.*.*.*" );

        }


    }
}
