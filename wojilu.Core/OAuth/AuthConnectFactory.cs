/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using wojilu.DI;

namespace wojilu.OAuth {

    public class AuthConnectFactory {

        private static readonly ILog logger = LogManager.GetLogger( typeof( AuthConnectFactory ) );

        private static readonly Dictionary<String, AuthConnect> _allConnects = loadConnects();


        public static Dictionary<String, AuthConnect> GetAllConnects() {
            return _allConnects;
        }

        public static AuthConnect GetConnect( string typeFullName ) {

            if (strUtil.IsNullOrEmpty( typeFullName )) return null;

            foreach (KeyValuePair<String, AuthConnect> kv in GetAllConnects()) {
                if (kv.Value.GetType().FullName == typeFullName) return kv.Value;
            }

            return null;
        }


        private static Dictionary<String, AuthConnect> loadConnects() {

            Dictionary<String, AuthConnect> map = new Dictionary<String, AuthConnect>();

            foreach (KeyValuePair<String, Type> kv in ObjectContext.Instance.TypeList) {

                Type t = kv.Value;

                if( t.IsSubclassOf( typeof( AuthConnect ) ) ) {

                    AuthConnectConfig cfgConnect = AuthConnectConfig.GetByType( t.FullName );
                    if (cfgConnect == null) {
                        logger.Warn( String.Format( "config of '{0}' is empty", t.FullName ) );
                        continue;
                    }

                    AuthConnect obj = initConnect( t, cfgConnect );

                    map.Add( t.FullName, obj );


                }
            }

            return map;
        }

        private static AuthConnect initConnect( Type t, AuthConnectConfig x ) {

            AuthConnect obj = ObjectContext.Create( t ) as AuthConnect;

            if (strUtil.IsNullOrEmpty( x.ConsumerKey ))
                throw new Exception( String.Format( "请设置 Consumer Key: {0}", x.Name ) );
            if (strUtil.IsNullOrEmpty( x.ConsumerSecret ))
                throw new Exception( String.Format( "请设置 Consumer Secret: {0}", x.Name ) );

            obj.ConsumerKey = x.ConsumerKey;
            obj.ConsumerSecret = x.ConsumerSecret;

            return obj;
        }


    }
}
