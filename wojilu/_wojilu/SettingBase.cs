using System;
using System.Collections.Generic;
using System.Text;

namespace wojilu {

    public class SettingBase<T> {

        public static T Instance {
            get { return _instance; }
        }

        private static T _instance = loadSettings();

        private static T loadSettings() {
            return cfgHelper.Read<T>();
        }

        public static void Save( T s ) {
            _instance = s;
            cfgHelper.Write( _instance );
        }

    }

}
