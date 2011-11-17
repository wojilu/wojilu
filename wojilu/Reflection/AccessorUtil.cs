/*
 * Copyright 2010 www.wojilu.com
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *      http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections;

using wojilu.Data;
using wojilu.ORM;

namespace wojilu.Reflection {


    internal class AccessorUtil {

        private static readonly ILog logger = LogManager.GetLogger( typeof( AccessorUtil ) );

        public static Hashtable Init( MetaList metaList ) {

            logger.Info( "cacheFactory..." );
            Hashtable factoryList = cacheFactory( metaList );

            logger.Info( "cacheAccessor..." );
            cacheAccessor( metaList );

            return factoryList;
        }



        private static Hashtable cacheFactory( MetaList metas ) {

            if( DbConfig.Instance.OptimizeMode == OptimizeMode.CodeDom ) {
                    CodeDomPropertyAccessor.Init( metas );
                    return CodeDomPropertyAccessor.GetFactoryList();
            }
            else if( DbConfig.Instance.OptimizeMode == OptimizeMode.IL ) {
                    ILPropertyAccessor.Init();
                    return null;
            }
            return null;
        }

        private static void cacheAccessor( MetaList metas ) {
            foreach (DictionaryEntry entry in metas.ClassList) {

                String str = entry.Key.ToString();
                EntityInfo info = entry.Value as EntityInfo;
                foreach (EntityPropertyInfo ep in info.PropertyListAll) {
                    ep.PropertyAccessor = GetAccessor( info.Type.FullName, ep.Name, metas );
                    if (ep.PropertyAccessor == null) {
                        throw new Exception( "PropertyAccessor can not be null : [type name]" + info.Type.FullName + ", [property name]" + ep.Name );
                    }
                }
            }
        }

        public static IPropertyAccessor GetAccessor( String typeFullName, String propertyName, MetaList mapping ) {
            switch (DbConfig.Instance.OptimizeMode) {
                case OptimizeMode.CodeDom:
                    return CodeDomPropertyAccessor.GetAccessor( typeFullName, propertyName, mapping );

                case OptimizeMode.IL:
                    return ILPropertyAccessor.GetAccessor( typeFullName, propertyName );
            }
            return null;
        }



    }
}

