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

        private static List<MethodObserver> getMethodObserver( Type t, String methodName ) {
            return AopContext.GetMethodObservers( t, methodName );
        }

        private static void append_method_before( StringBuilder sb, ObservedMethod x ) {

            sb.AppendFormat( "\t\t\tMethodInfo m = this.GetType().GetMethod( \"{0}\" );", x.Method.Name );
            sb.AppendLine();

            sb.AppendFormat( "\t\t\tObject[] args = {0};", getArgArray( x.Method ) );
            sb.AppendLine();

            List<MethodObserver> osList = getMethodObserver( x.ObservedType.Type, x.Method.Name );
            int i = 1;
            foreach (MethodObserver os in osList) {
                sb.AppendFormat( "\t\t\t{0} observer{1} = new {0}();", os.GetType().FullName, i );
                sb.AppendLine();
                sb.AppendFormat( "\t\t\tobserver{0}.Before( m, args, this );", i );
                sb.AppendLine();
                i++;
            }
        }

        private static void append_method_invoke( StringBuilder sb, ObservedMethod x ) {

            sb.Append( "\t\t\tObject returnValue = null;" );
            sb.AppendLine();

            List<MethodObserver> osList = getMethodObserver( x.ObservedType.Type, x.Method.Name );

            String strReturn = getReturnString( x.Method );
            if (strReturn == "void") {
                strReturn = "";
            }
            else {
                strReturn = "returnValue = ";
            }

            int invokeObserverIndex = getInovkeObserverIndex( osList );

            if (invokeObserverIndex > -1) {
                append_invoke_object( sb, x );
                sb.AppendFormat( "\t\t\t{0}observer{1}.Invoke( invocation );", strReturn, invokeObserverIndex );
                sb.AppendLine();
            }
            else {
                sb.AppendFormat( "\t\t\t{0}base.{1}({2});", strReturn, x.Method.Name, getArgBodyFromArray( x.Method ) );
                sb.AppendLine();
            }
        }

        private static int getInovkeObserverIndex( List<MethodObserver> osList ) {
            int i = 1;
            foreach (MethodObserver os in osList) {

                MethodInfo m = os.GetType().GetMethod( "Invoke" );

                if (m.DeclaringType != typeof( MethodObserver )) {
                    return i;
                }

                i++;
            }

            return -1;
        }

        private static void append_method_after( StringBuilder sb, ObservedMethod x ) {

            List<MethodObserver> osList = getMethodObserver( x.ObservedType.Type, x.Method.Name );
            int i = 1;
            foreach (MethodObserver os in osList) {
                sb.AppendFormat( "\t\t\tobserver{0}.After( returnValue, m, args, this );", i );
                sb.AppendLine();
                i++;
            }
        }

        private static void append_invoke_object( StringBuilder sb, ObservedMethod x ) {
            sb.Append( "\t\t\tIMethodInvocation invocation = new MethodInvocation();" );
            sb.AppendLine();

            sb.Append( "\t\t\tinvocation.Method = m;" );
            sb.AppendLine();

            sb.AppendFormat( "\t\t\tinvocation.Args = args;" );
            sb.AppendLine();

            sb.Append( "\t\t\tinvocation.Target = this;" );
            sb.AppendLine();
        }


        //----------------------------------------------------------------------------------------

        private static void append_ns_begin( StringBuilder sb, Type type ) {
            sb.AppendFormat( "namespace {0} ", type.Namespace );
            sb.Append( "{" );
            sb.AppendLine();
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

                if (x.Method.ReturnType.IsValueType) {
                    sb.AppendFormat( "\t\t\tif( returnValue==null ||(returnValue is ValueType ==false )) return default({0});", getReturnType( x.Method.ReturnType ) );
                    sb.AppendLine();
                    sb.AppendFormat( "\t\t\treturn ({0})returnValue;", strReturn );
                    sb.AppendLine();
                }
                else {
                    sb.Append( "\t\t\tif( returnValue==null ) return null;" );
                    sb.AppendLine();
                    sb.AppendFormat( "\t\t\treturn returnValue as {0};", strReturn );
                    sb.AppendLine();
                }

            }

            sb.Append( "\t\t}" );
            sb.AppendLine();
            sb.AppendLine();
        }

        //----------------------------------------------------------------------------------------

        // 获取返回值，比如 void/int/string
        private static string getReturnString( MethodInfo methodInfo ) {

            if (methodInfo.ReturnType == typeof( void )) return "void";

            return getReturnType( methodInfo.ReturnType );
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

        private static String getArgBodyFromArray( MethodInfo methodInfo ) {
            String str = "";
            ParameterInfo[] args = methodInfo.GetParameters();
            int i = 0;
            foreach (ParameterInfo x in args) {
                str += "(" + x.ParameterType.FullName + ")args[" + i + "],";
                i++;
            }
            return str.Trim().TrimEnd( ',' );
        }

        private static String getArgArray( MethodInfo methodInfo ) {
            return "new Object[] {" + getArgBody( methodInfo ) + "}";
        }

        private static String getReturnType( Type t ) {
            return strUtil.GetGenericTypeWithArgs( t );
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
