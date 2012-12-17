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

using System.CodeDom.Compiler;

using System.IO;
using System.Reflection;
using System.Text;

using Microsoft.CSharp;

namespace wojilu.Aop {

    internal class AopCoder {

        private static readonly ILog logger = LogManager.GetLogger( typeof( AopCoder ) );

        internal static readonly String proxyClassPrefix = "__";
        internal static readonly String baseMethodPrefix = "__base_";

        public static String GetProxyClassCode( Dictionary<Type, ObservedType> observers ) {

            StringBuilder sb = new StringBuilder();
            append_using( sb );

            foreach (KeyValuePair<Type, ObservedType> kv in observers) {

                Type type = kv.Key;

                append_ns_begin( sb, type );
                append_class_begin( sb, type );

                append_methods( sb, kv );
                append_methods_base( sb, kv );

                append_class_end( sb );
                append_ns_end( sb );

            }

            return sb.ToString();
        }

        private static void append_using( StringBuilder sb ) {
            sb.AppendLine();
            sb.AppendLine( "using System;" );
            sb.AppendLine( "using System.Collections.Generic;" );
            sb.AppendLine( "using System.Reflection;" );
            sb.AppendLine( "using wojilu.Aop;" );

            sb.AppendLine();
        }

        private static void append_methods( StringBuilder sb, KeyValuePair<Type, ObservedType> kv ) {
            foreach (ObservedMethod x in kv.Value.MethodList) {

                String strArgs = getArgString( x.Method );
                String strReturn = getReturnString( x.Method );

                append_method_begin( sb, x, strArgs, strReturn );
                append_method_before( sb, x );

                append_method_invoke( sb, x );

                append_method_after( sb, x );
                append_method_end( sb, x );
            }
        }

        private static void append_methods_base( StringBuilder sb, KeyValuePair<Type, ObservedType> kv ) {
            foreach (ObservedMethod x in kv.Value.MethodList) {

                String strReturn = getReturnString( x.Method );
                String strArg = getArgString( x.Method );
                String strArgBody = getArgBody( x.Method );

                sb.AppendFormat( "\t\tpublic {0} {1}{2}({3}) ", strReturn, baseMethodPrefix, x.Method.Name, strArg );
                sb.Append( "{" );
                sb.AppendLine();

                String strReturnLable = strReturn == "void" ? "" : "return ";

                sb.AppendFormat( "\t\t\t{0}base.{1}( {2} );", strReturnLable, x.Method.Name, strArgBody );
                sb.AppendLine();

                sb.Append( "\t\t}" );
                sb.AppendLine();
                sb.AppendLine();
            }
        }

        //----------------------------------------------------------------------------------------

        private static void append_method_invoke( StringBuilder sb, ObservedMethod x ) {

            sb.Append( "\t\t\tObject returnValue = null;" );
            sb.AppendLine();

            sb.AppendFormat( "\t\t\tMethodObserver invokeObserver = AopContext.GetInvokeObserver( typeof( {0} ), \"{1}\" );", x.ObservedType.Type.FullName, x.Method.Name );
            sb.AppendLine();

            sb.Append( "\t\t\tif (invokeObserver != null) {" );
            sb.AppendLine();

            String strInvokeArgs = getInovkeArgs( x.Method );
            sb.AppendFormat( "\t\t\t\treturnValue = invokeObserver.Invoke( {0} );", strInvokeArgs );
            sb.AppendLine();

            sb.Append( "\t\t\t}" );
            sb.AppendLine();

            sb.Append( "\t\t\telse {" );
            sb.AppendLine();

            String strArgBody = getArgBody( x.Method );
            String strReturn = getReturnString( x.Method );
            if (strReturn == "void") {
                strReturn = "";
            }
            else {
                strReturn = "returnValue = ";
            }

            sb.AppendFormat( "\t\t\t\t{0}base.{1}({2});", strReturn, x.Method.Name, strArgBody );
            sb.AppendLine();

            sb.Append( "\t\t\t}" );
            sb.AppendLine();

        }


        private static void append_method_before( StringBuilder sb, ObservedMethod x ) {

            sb.AppendFormat( "\t\t\tMethodInfo m = this.GetType().GetMethod( \"{0}\" );", x.Method.Name );
            sb.AppendLine();

            sb.AppendFormat( "\t\t\tList<MethodObserver> mlist= getMethodObserver( \"{0}\" );", x.Method.Name );
            sb.AppendLine();

            sb.Append( "\t\t\tforeach( MethodObserver x in mlist ) {" );
            sb.AppendLine();

            String strInvokeArgs = getInovkeArgs( x.Method );

            sb.AppendFormat( "\t\t\t\tx.Before( {0} );", strInvokeArgs );
            sb.AppendLine();

            sb.Append( "\t\t\t}" );
            sb.AppendLine();

        }

        private static String getInovkeArgs( MethodInfo x ) {

            String strArgBody = getArgBody( x );

            StringBuilder sb = new StringBuilder();
            sb.Append( " m,  new Object[] {" );
            sb.Append( strArgBody );
            sb.Append( "}, this " );

            return sb.ToString();
        }


        private static void append_method_after( StringBuilder sb, ObservedMethod x ) {

            sb.Append( "\t\t\tforeach( MethodObserver x in mlist ) {" );
            sb.AppendLine();

            String strInvokeArgs = getInovkeArgs( x.Method );

            sb.AppendFormat( "\t\t\t\tx.After( returnValue, {0} );", strInvokeArgs );
            sb.AppendLine();

            sb.Append( "\t\t\t}" );
            sb.AppendLine();
        }

        //----------------------------------------------------------------------------------------

        private static void append_ns_begin( StringBuilder sb, Type type ) {
            sb.AppendFormat( "namespace {0} ", type.Namespace );
            sb.Append( "{" );
            sb.AppendLine();
        }

        private static void append_ns_end( StringBuilder sb ) {
            sb.Append( "}" );
            sb.AppendLine();
        }

        //----------------------------------------------------------------------------------------

        private static void append_class_begin( StringBuilder sb, Type type ) {
            sb.AppendFormat( "\tpublic class {0}{1} : {1} ", proxyClassPrefix, type.Name );
            sb.Append( "{" );
            sb.AppendLine();

            append_helper( sb, type );
        }

        private static void append_helper( StringBuilder sb, Type type ) {
            sb.Append( "\t\tprivate List<MethodObserver> getMethodObserver( String methodName ) {" );
            sb.AppendLine();

            sb.AppendFormat( "\t\t\treturn AopContext.GetMethodObservers( typeof({0}), methodName );", type.FullName );
            sb.AppendLine();

            sb.Append( "\t\t}" );
            sb.AppendLine();
        }

        private static void append_class_end( StringBuilder sb ) {
            sb.Append( "\t}" );
            sb.AppendLine();
        }

        //----------------------------------------------------------------------------------------

        private static void append_method_begin( StringBuilder sb, ObservedMethod x, String strArgs, String strReturn ) {
            sb.AppendFormat( "\t\tpublic override {0} {1}( {2} ) ", strReturn, x.Method.Name, strArgs );
            sb.Append( "{" );
            sb.AppendLine();
        }

        private static void append_method_end( StringBuilder sb, ObservedMethod x ) {

            String strReturn = getReturnString( x.Method );
            if (strReturn != "void") {
                sb.AppendFormat( "\t\t\treturn ({0})returnValue;", strReturn );
                sb.AppendLine();
            }

            sb.Append( "\t\t}" );
            sb.AppendLine();
        }

        //----------------------------------------------------------------------------------------

        // 获取返回值，比如 void/int/string
        private static string getReturnString( MethodInfo methodInfo ) {

            if (methodInfo.ReturnType == typeof( void )) return "void";

            return methodInfo.ReturnType.FullName;
        }

        // 获取参数签名等，比如 BlogPost x1, String x2, int x3
        private static string getArgString( MethodInfo methodInfo ) {
            String str = "";
            ParameterInfo[] args = methodInfo.GetParameters();
            int i = 1;
            foreach (ParameterInfo x in args) {
                str += string.Format( "{0} x{1},", x.ParameterType.FullName, i );
                i++;
            }
            return str.Trim().TrimEnd( ',' );
        }

        // 获取参数内容等，比如 BlogPost x1, String x2, int x3
        // 转成 x1,x2,x3
        private static string getArgBody( MethodInfo methodInfo ) {
            String str = "";
            ParameterInfo[] args = methodInfo.GetParameters();
            int i = 1;
            foreach (ParameterInfo x in args) {
                str += string.Format( "x{0},", i );
                i++;
            }
            return str.Trim().TrimEnd( ',' );
        }

        public static Assembly CompileCode( String code, IDictionary asmList ) {

            Boolean generateAopAssembly = false;

            CodeDomProvider provider = new CSharpCodeProvider();
            CompilerParameters options = new CompilerParameters();
            options.GenerateExecutable = false;
            options.CompilerOptions = "/optimize";
            if (generateAopAssembly) {
                options.GenerateInMemory = false;
                options.OutputAssembly = Path.Combine( PathHelper.GetBinDirectory(), "__wojilu.aop.dll" );
            }
            else {
                options.GenerateInMemory = true;
            }
            Hashtable tblReferencedAsms = new Hashtable();
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            tblReferencedAsms[executingAssembly.FullName] = executingAssembly.Location;
            logger.Info( executingAssembly.FullName + "__" + executingAssembly.Location );
            addReferencedAsms( tblReferencedAsms, executingAssembly.GetReferencedAssemblies() );
            foreach (DictionaryEntry entry in asmList) {
                Assembly asm = entry.Value as Assembly;
                tblReferencedAsms[asm.FullName] = asm.Location;
                logger.Info( asm.FullName + "__" + asm.Location );
                addReferencedAsms( tblReferencedAsms, asm.GetReferencedAssemblies() );
            }
            foreach (DictionaryEntry entry in tblReferencedAsms) {
                options.ReferencedAssemblies.Add( entry.Value.ToString() );
            }
            CompilerResults results = provider.CompileAssemblyFromSource( options, new String[] { code } );
            if (results.Errors.Count > 0) {
                StringBuilder builder = new StringBuilder();
                foreach (CompilerError error in results.Errors) {
                    builder.Append( error.ErrorText );
                    builder.Append( "\n" );
                }
                logger.Fatal( code );
                throw new Exception( builder.ToString() );
            }
            return results.CompiledAssembly;
        }

        private static void addReferencedAsms( Hashtable tblReferencedAsms, AssemblyName[] assemblyName ) {
            foreach (AssemblyName name in assemblyName) {
                if (tblReferencedAsms[name.FullName] == null) {
                    Assembly assembly = Assembly.Load( name );
                    tblReferencedAsms[name.FullName] = assembly.Location;
                    logger.Info( name.FullName + "__" + assembly.Location );
                }
            }
        }


    }

}
