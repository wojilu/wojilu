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
using System.Collections.Generic;
using System.Reflection;
using System.Text;

using wojilu.ORM;

namespace wojilu.Reflection {

    internal class CodeDomPropertyAccessor {

        private static IDictionary _accessorList;
        private static Hashtable _concreteFactoryList = new Hashtable();
        private static readonly ILog logger = LogManager.GetLogger( typeof( CodeDomPropertyAccessor ) );

        public static void Init( MetaList metaList ) {
            GetAccessorList( metaList );
        }

        public static IPropertyAccessor GetAccessor( String typeFullName, String propertyName, MetaList metaList ) {

            return (GetAccessorList( metaList )[getAccessorName( typeFullName, propertyName )] as IPropertyAccessor);

        }

        public static IDictionary GetAccessorList( MetaList metaList ) {
            if (_accessorList == null) {

                Dictionary<String, EntityInfo> classList = metaList.ClassList;
                Dictionary<String, Assembly> asmList = metaList.AssemblyList;

                Assembly assembly = CodeDomHelper.CompileCode( GetAccessorCode( classList ), asmList, null );
                _accessorList = (assembly.CreateInstance( "wojilu.Reflection.CodeDomAccessorUtil" ) as IAccessorUtil).GetAccessorList();

                foreach (KeyValuePair<String, EntityInfo> kv in classList) {

                    String typeFullName = kv.Key;
                    IConcreteFactory factory = assembly.CreateInstance( getConcreteFactoryName( typeFullName ) ) as IConcreteFactory;
                    _concreteFactoryList[typeFullName] = factory;
                }
            }
            return _accessorList;
        }


        public static String GetAccessorCode( IDictionary classList ) {

            StringBuilder builder = new StringBuilder();
            builder.Append( "\n\nusing System;\n" );
            builder.Append( "using System.Collections;\n" );
            builder.Append( "using System.Collections.Generic;\n" );
            builder.Append( "namespace wojilu.Reflection {\n" );

            StringBuilder clsBuilder = new StringBuilder();
            StringBuilder utilBuilder = new StringBuilder();

            utilBuilder.Append( "[Serializable]\n" );
            utilBuilder.Append( "public class CodeDomAccessorUtil : IAccessorUtil {\n" );
            utilBuilder.Append( "\tprivate IDictionary _list;\n" );
            utilBuilder.Append( "\tpublic IDictionary GetAccessorList() {\n" );
            utilBuilder.Append( "\t\tif( _list == null ) {\n" );
            utilBuilder.Append( "\t\t_list = new Hashtable();\n" );

            foreach (DictionaryEntry entry in classList) {

                String str = entry.Key.ToString();
                EntityInfo info = entry.Value as EntityInfo;
                Type type = info.Type;
                clsBuilder.Append( "\t[Serializable]\n" );
                clsBuilder.Append( "\tpublic class " );
                clsBuilder.Append( str.Replace( ".", "" ) );
                clsBuilder.Append( "Factory : IConcreteFactory {\n" );
                //clsBuilder.Append( "\t\tpublic ObjectBase New() {\n" );
                clsBuilder.Append( "\t\tpublic IEntity New() {\n" );

                if ((type.GetConstructors().Length < 0) || (type.GetConstructor( Type.EmptyTypes ) == null)) {
                    clsBuilder.Append( "\t\t\treturn null;\n" );
                }
                else {
                    clsBuilder.AppendFormat( "\t\t\treturn new {0}();\n", str );
                }
                clsBuilder.Append( "\t\t}\n" );
                clsBuilder.Append( "\t}\n" );
                foreach (EntityPropertyInfo ep in info.PropertyListAll) {
                    if (ep.Name == "OID") {
                        continue;
                    }
                    String acname = getAccessorName( str, ep.Name );
                    builder.Append( "\n" );
                    builder.Append( "[Serializable]\n" );
                    builder.AppendFormat( "public class {0} : IPropertyAccessor ", acname );
                    builder.Append( "{\n" );
                    builder.Append( "\tpublic Object Get(Object target) {\n" );
                    if (ep.Property.CanRead) {
                        builder.AppendFormat( "\t\t{0} data = ({0})target;\n", str );
                        builder.AppendFormat( "\t\treturn data.{0};\n", ep.Name );
                    }
                    else {
                        builder.AppendFormat( "\t\tthrow new Exception( \"the current property {0}.{1} can't read. \" );\n", ep.ParentEntityInfo.FullName, ep.Name );
                    }
                    builder.Append( "\t}\n\n" );
                    builder.Append( "\tpublic void Set( Object target, Object val ) {\n" );
                    if (ep.Property.CanWrite) {
                        builder.AppendFormat( String.Format( "\t\t{0} data = ({0})target;\n", str ), new object[0] );

                        String ptname = ReflectionUtil.getPropertyTypeName( ep.Property );
                        builder.AppendFormat( String.Format( "\t\tdata.{0} = ({1})val;\n", ep.Name, ptname ), new object[0] );

                    }
                    builder.Append( "\t}\n" );
                    builder.Append( "}\n" );
                    utilBuilder.AppendFormat( "\t\t_list[\"{0}\"] = new {0}();\n", acname );
                }
            }
            utilBuilder.Append( "\t}\n" );
            utilBuilder.Append( "\treturn _list;\n" );
            utilBuilder.Append( "}\n" );
            utilBuilder.Append( "}\n" );
            builder.Append( utilBuilder );
            builder.Append( clsBuilder );
            builder.Append( "}" );
            return builder.ToString();
        }

        private static String getAccessorName( String typeFullName, String propertyName ) {
            return String.Format( "{0}_{1}", typeFullName.Replace( ".", "" ), propertyName );
        }

        private static String getConcreteFactoryName( String typeFullName ) {
            return String.Format( "wojilu.Reflection.{0}Factory", typeFullName.Replace( ".", "" ) );
        }

        public static Hashtable GetFactoryList() {
            return _concreteFactoryList;
        }


    }
}

