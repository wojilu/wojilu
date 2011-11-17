/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using wojilu.Common.Resource;

namespace wojilu.Members.Users.Domain {


    // 提供序列化和查询工具
    public class UserSecurity {

        // 查询
        public static int GetPrivacy( User user, String item ) {
            UserPermission up = (UserPermission)System.Enum.Parse( typeof( UserPermission ), item );
            return GetPrivacy( user, up );
        }

        // 查询
        private static int GetPrivacy( User user, UserPermission up ) {

            int defaultValue = UserPrivacy.GetDefaultValue();

            String securityStr = user.Security;

            if (strUtil.IsNullOrEmpty( securityStr )) return defaultValue; //默认值

            Dictionary<string, int> settings = getSettingValueByString( securityStr );

            int val = defaultValue;//默认值
            if (settings.ContainsKey( up.ToString() )) val = settings[up.ToString()];

            return val;
        }

        // 得到所有配置
        public static Dictionary<string, int> GetSettingsAll( User user ) {

            String securityStr = user.Security;

            Dictionary<string, int> settings = getSettingValueByString( securityStr );

            Dictionary<string, int> dic = new Dictionary<string, int>();
            FieldInfo[] fields = typeof( UserPermission ).GetFields();

            foreach (FieldInfo f in fields) {

                if (f.Name == "value__") continue;

                int val = UserPrivacy.GetDefaultValue();
                if (settings.ContainsKey( f.Name )) val = settings[f.Name];
                dic.Add( f.Name, val );
            }

            return dic;
        }

        // 序列化
        public static String Save( Dictionary<UserPermission, int> settings ) {

            Dictionary<String, int> dic = new Dictionary<string, int>();

            foreach (KeyValuePair<UserPermission, int> kv in settings) {
                dic.Add( kv.Key.ToString(), kv.Value );
            }

            return Save( dic );
        }

        //---------------------------------------------------------------------------------------------

        private static Dictionary<string, int> getSettingValueByString( String securityStr ) {

            String target = securityStr.Trim().TrimStart( '{' ).TrimEnd( '}' );
            string[] arrPair = target.Split( ',' );

            Dictionary<string, int> result = new Dictionary<string, int>();

            foreach (String pair in arrPair) {

                if (strUtil.IsNullOrEmpty( pair )) continue;

                string[] arrItem = pair.Split( ':' );
                if (arrItem.Length != 2) continue;

                String key = arrItem[0].Trim();
                String val = arrItem[1].Trim();
                if (strUtil.IsNullOrEmpty( key )) continue;

                result.Add( key, cvt.ToInt( val ) );

            }

            return result;
        }

        public static String Save( Dictionary<String, int> settings ) {
            StringBuilder sb = new StringBuilder();

            sb.Append( "{" );

            FieldInfo[] fields = typeof( UserPermission ).GetFields();

            foreach (FieldInfo f in fields) {

                if (f.Name == "value__") continue;

                int val = UserPrivacy.GetDefaultValue();
                if (settings.ContainsKey( f.Name )) val = settings[f.Name];

                sb.AppendFormat( "{0}:{1},", f.Name, val );
            }


            sb.Append( "}" );

            return sb.ToString();
        }


    }


}
