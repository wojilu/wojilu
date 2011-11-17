using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

using wojilu.DI;
using wojilu.Web.Mvc;
using wojilu.Reflection;

namespace wojilu.Web.Mvc {

    internal class ControllerFactoryCompiler {

        internal static Dictionary<String, IControllerFactory> GetCompiledFactory() {            

            Assembly asm = CodeDomPropertyAccessor.CompileCode( getControllerFactoryCode(), ObjectContext.Instance.AssemblyList );

            Dictionary<String, IControllerFactory> factories = new Dictionary<String, IControllerFactory>();

            List<Type> types = getControllerTypes();

            foreach (Type t in types) {
                factories.Add( t.FullName, (IControllerFactory)asm.CreateInstance( getFactoryNameWithNs( t.FullName ) ) );
            }

            return factories;
        }

        //public static String getCompiledFactoryString() {
        //    Dictionary<String, IControllerFactory> factories = compileFactory();
        //    StringBuilder sb = new StringBuilder();
        //    foreach (KeyValuePair<String, IControllerFactory> kv in factories) {
        //        sb.AppendLine( kv.Key );
        //    }
        //    return sb.ToString();
        //}

        private static String getControllerFactoryCode() {

            StringBuilder builder = new StringBuilder();
            builder.Append( "\n\nusing System;\n" );
            builder.Append( "using System.Collections;\n" );
            builder.Append( "using System.Collections.Generic;\n" );
            builder.Append( "using wojilu.Web.Mvc;\n" );
            builder.Append( "namespace " + getFactoryNs() + " {\n" );

            StringBuilder clsBuilder = new StringBuilder();

            List<Type> types = getControllerTypes();

            foreach (Type t in types) {


                if (t.IsSubclassOf( typeof( ControllerBase ) ) == false) continue;
                if (t.IsGenericType) continue;

                clsBuilder.Append( "\t[Serializable]\n" );
                clsBuilder.Append( "\tpublic class " );

                clsBuilder.Append( getFactoryName( t.FullName ) );
                clsBuilder.Append( " : IControllerFactory {\n" );

                clsBuilder.Append( "\t\tpublic ControllerBase New() {\n" );

                if ((t.GetConstructors().Length < 0) || (t.GetConstructor( Type.EmptyTypes ) == null)) {
                    clsBuilder.Append( "\t\t\treturn null;\n" );
                }
                else {
                    clsBuilder.AppendFormat( "\t\t\treturn new {0}();\n", t.FullName );
                }
                clsBuilder.Append( "\t\t}\n" );
                clsBuilder.Append( "\t}\n" );

            }

            builder.Append( clsBuilder );
            builder.AppendLine();
            builder.Append( "}" );


            return builder.ToString();

        }

        private static List<Type> getControllerTypes() {

            List<Type> types = new List<Type>();
            foreach (KeyValuePair<String, Type> kv in ObjectContext.Instance.TypeList) {

                if (kv.Value.IsSubclassOf( typeof( ControllerBase ) ) == false) continue;
                if (kv.Value.IsGenericType) continue;

                types.Add( kv.Value );

            }

            return types;
        }

        private static String getFactoryNameWithNs( String typeFullName ) {
            return getFactoryNs() + "." + getFactoryName( typeFullName );
        }

        private static String getFactoryName( String typeFullName ) {
            return typeFullName.Replace( ".", "" ) + "Factory";
        }

        private static String getFactoryNs() {
            return "wojilu.Reflection";
        }


    }

}
