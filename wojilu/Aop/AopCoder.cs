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
using wojilu.DI;

namespace wojilu.Aop {

    internal class AopCoder {

        private static readonly ILog logger = LogManager.GetLogger( typeof( AopCoder ) );

        public static String GetProxyClassCode( Dictionary<Type, ObservedType> observers ) {

            StringBuilder sb = new StringBuilder();
            append_using( sb );

            foreach (KeyValuePair<Type, ObservedType> kv in observers) {

                if (kv.Value.GetInterfaceType().Count > 0) {

                    foreach (Type interfaceType in kv.Value.GetInterfaceType()) {

                        createInterfaceProxy( sb, kv, interfaceType );

                    }
                }

                if (kv.Value.Type.IsSealed == false) {
                    createSubClassProxy( sb, kv );
                }

            }

            return sb.ToString();
        }

        private static void createInterfaceProxy( StringBuilder sb, KeyValuePair<Type, ObservedType> kv, Type interfaceType ) {
            Type type = kv.Key;

            AopCoderDialect dialect = new AopCoderDialectInerface();

            append_ns_begin( sb, type );
            append_interface_class_begin( sb, type, dialect, interfaceType.FullName );

            append_methods( sb, kv, dialect );
            append_interface_methods_other( sb, kv, dialect, interfaceType );

            append_class_end( sb );
            append_ns_end( sb );
        }

        // 不在监控中的、接口必须包含的方法
        private static void append_interface_methods_other( StringBuilder sb, KeyValuePair<Type, ObservedType> kv, AopCoderDialect dialect, Type interfaceType ) {

            MethodInfo[] methods = interfaceType.GetMethods();
            foreach (MethodInfo m in methods) {

                if (isMethodCoded( m, kv.Value.MethodList )) continue;

                if (rft.IsMethodProperty( m )) {
                    continue;
                }

                append_interface_methods_other_single( sb, m, dialect );
            }

            PropertyInfo[] properties = interfaceType.GetProperties();
            foreach (PropertyInfo p in properties) {
                append_interface_properties_other_single( sb, p, dialect );
            }
        }

        private static void append_interface_properties_other_single( StringBuilder sb, PropertyInfo p, AopCoderDialect dialect ) {

            sb.AppendLine( "\t\tpublic " + p.PropertyType.FullName + " " + p.Name + " {" );
            if (p.CanRead) {
                sb.AppendLine( "\t\t\tget { return " + dialect.GetInvokeTargetThis() + "." + p.Name + ";}" );
            }
            if (p.CanWrite) {
                sb.AppendLine( "\t\t\tset { " + dialect.GetInvokeTargetThis() + "." + p.Name + " = value;}" );
            }
            sb.AppendLine( "\t\t}" );
            sb.AppendLine();
        }

        private static void append_interface_methods_other_single( StringBuilder sb, MethodInfo m, AopCoderDialect dialect ) {

            String strReturn = getReturnString( m );
            String strArgs = getArgString( m );
            String strArgBody = getArgBody( m );

            sb.AppendFormat( "\t\tpublic {0} {1} ( {2} ) ", strReturn, m.Name, strArgs );
            sb.Append( "{" );
            sb.AppendLine();

            String strReturnLabel = getReturnString( m );
            strReturnLabel = (strReturnLabel == "void" ? "" : "return ");

            sb.AppendFormat( "\t\t\t{3}{0}.{1}({2});", dialect.GetInvokeTargetThis(), m.Name, strArgBody, strReturnLabel );
            sb.AppendLine();

            sb.Append( "\t\t}" );
            sb.AppendLine();
            sb.AppendLine();
        }

        private static bool isMethodCoded( MethodInfo m, List<ObservedMethod> list ) {
            foreach (ObservedMethod x in list) {
                if (isMethodEqual( x.Method, m )) return true;
            }
            return false;
        }

        private static bool isMethodEqual( MethodInfo x, MethodInfo m ) {
            if (x.Name != m.Name) return false;
            if (isMethodParamsEqual( x.GetParameters(), m.GetParameters() ) == false) return false;
            return true;
        }

        private static bool isMethodParamsEqual( ParameterInfo[] p1, ParameterInfo[] p2 ) {
            if (p1.Length != p2.Length) return false;
            for (int i = 0; i < p1.Length; i++) {
                if (p1[i].GetType() != p2[i].GetType()) return false;
            }
            return true;
        }

        private static void createSubClassProxy( StringBuilder sb, KeyValuePair<Type, ObservedType> kv ) {

            Type type = kv.Key;
            AopCoderDialect dialect = new AopCoderDialectSub();

            append_ns_begin( sb, type );
            append_class_begin( sb, type, dialect );

            append_methods( sb, kv, dialect );
            append_methods_base( sb, kv, dialect );

            append_class_end( sb );
            append_ns_end( sb );
        }

        private static void append_using( StringBuilder sb ) {
            sb.AppendLine();
            sb.AppendLine( "using System;" );
            sb.AppendLine( "using System.Collections.Generic;" );
            sb.AppendLine( "using System.Reflection;" );
            sb.AppendLine( "using wojilu.Aop;" );

            sb.AppendLine();
        }

        private static void append_methods( StringBuilder sb, KeyValuePair<Type, ObservedType> kv, AopCoderDialect dialect ) {
            foreach (ObservedMethod x in kv.Value.MethodList) {

                if (x.Method.IsVirtual == false) continue;

                append_method_single( sb, x, dialect );
            }
        }

        private static void append_method_single( StringBuilder sb, ObservedMethod x, AopCoderDialect dialect ) {
            String strArgs = getArgString( x.Method );
            String strReturn = getReturnString( x.Method );

            append_method_begin( sb, x, strArgs, strReturn, dialect );
            append_method_before( sb, x, dialect );

            append_method_invoke( sb, x, dialect );

            append_method_after( sb, x, dialect );
            append_method_end( sb, x );
        }

        private static void append_methods_base( StringBuilder sb, KeyValuePair<Type, ObservedType> kv, AopCoderDialect dialect ) {
            foreach (ObservedMethod x in kv.Value.MethodList) {

                if (x.Method.IsVirtual == false) continue;

                append_method_base_single( sb, x, dialect );
            }
        }

        private static void append_interface_methods_base( StringBuilder sb, KeyValuePair<Type, ObservedType> kv, AopCoderDialect dialect ) {
            foreach (ObservedMethod x in kv.Value.MethodList) {
                append_method_base_single( sb, x, dialect );
            }
        }

        private static void append_method_base_single( StringBuilder sb, ObservedMethod x, AopCoderDialect dialect ) {
            String strReturn = getReturnString( x.Method );
            String strArg = getArgString( x.Method );
            String strArgBody = getArgBody( x.Method );

            sb.AppendFormat( "\t\tpublic {0} {1}{2}({3}) ", strReturn, dialect.GetMethodBasePrefix(), x.Method.Name, strArg );
            sb.Append( "{" );
            sb.AppendLine();

            String strReturnLable = strReturn == "void" ? "" : "return ";

            sb.AppendFormat( "\t\t\t{0}{1}.{2}( {3} );", strReturnLable, dialect.GetInvokeTargetBase(), x.Method.Name, strArgBody );
            sb.AppendLine();

            sb.Append( "\t\t}" );
            sb.AppendLine();
            sb.AppendLine();
        }

        //----------------------------------------------------------------------------------------

        private static List<MethodObserver> getMethodObserver( Type t, String methodName ) {
            return AopContext.GetMethodObservers( t, methodName );
        }

        private static void append_method_before( StringBuilder sb, ObservedMethod x, AopCoderDialect dialect ) {

            if (x.Method.GetParameters().Length > 0) {
                sb.AppendFormat( "\t\t\tMethodInfo m = {0}.GetType().GetMethod( \"{1}\", {2} );", dialect.GetInvokeTargetThis(), x.Method.Name, "new Type[] {" + getArgType( x.Method ) + "}" );
            }
            else {

                sb.AppendFormat( "\t\t\tMethodInfo m = {0}.GetType().GetMethod( \"{1}\", {2} );", dialect.GetInvokeTargetThis(), x.Method.Name, "new Type[]{}" );
            }
            sb.AppendLine();

            sb.AppendFormat( "\t\t\tObject[] args = {0};", getArgArray( x.Method ) );
            sb.AppendLine();

            List<MethodObserver> osList = getMethodObserver( x.ObservedType.Type, x.Method.Name );
            int i = 1;
            foreach (MethodObserver os in osList) {
                sb.AppendFormat( "\t\t\t{0} observer{1} = {2};", os.GetType().FullName, i, getObserverCreator( os ) );
                sb.AppendLine();
                sb.AppendFormat( "\t\t\tobserver{0}.Before( m, args, {1} );", i, dialect.GetInvokeTargetThis() );
                sb.AppendLine();
                i++;
            }
        }

        private static String getObserverCreator( MethodObserver os ) {
            return string.Format( "{0}.Create<{1}>()", typeof( ObjectContext ).FullName, os.GetType().FullName );
        }

        private static void append_method_invoke( StringBuilder sb, ObservedMethod x, AopCoderDialect dialect ) {

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
                append_invoke_object( sb, x, dialect );
                sb.AppendFormat( "\t\t\t{0}observer{1}.Invoke( invocation );", strReturn, invokeObserverIndex );
                sb.AppendLine();
            }
            else {
                sb.AppendFormat( "\t\t\t{0}{1}.{2}({3});", strReturn, dialect.GetInvokeTargetBase(), x.Method.Name, getArgBodyFromArray( x.Method ) );
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

        private static void append_method_after( StringBuilder sb, ObservedMethod x, AopCoderDialect dialect ) {

            List<MethodObserver> osList = getMethodObserver( x.ObservedType.Type, x.Method.Name );
            int i = 1;
            foreach (MethodObserver os in osList) {
                sb.AppendFormat( "\t\t\tobserver{0}.After( returnValue, m, args, {1} );", i, dialect.GetInvokeTargetThis() );
                sb.AppendLine();
                i++;
            }
        }

        private static void append_invoke_object( StringBuilder sb, ObservedMethod x, AopCoderDialect dialect ) {
            sb.Append( "\t\t\tIMethodInvocation invocation = new MethodInvocation();" );
            sb.AppendLine();

            sb.Append( "\t\t\tinvocation.Method = m;" );
            sb.AppendLine();

            sb.AppendFormat( "\t\t\tinvocation.Args = args;" );
            sb.AppendLine();

            sb.AppendFormat( "\t\t\tinvocation.Target = {0};", dialect.GetInvokeTargetThis() );
            sb.AppendLine();

            sb.AppendFormat( "\t\t\tinvocation.IsSubClass = {0};", dialect.IsSubClassStr() );
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

        private static void append_class_begin( StringBuilder sb, Type type, AopCoderDialect dialect ) {
            sb.AppendFormat( "\tpublic class {0} : {1} ", dialect.GetClassFullName( type, "" ), type.Name );
            sb.Append( "{" );
            sb.AppendLine();
            sb.AppendLine();
        }

        private static void append_interface_class_begin( StringBuilder sb, Type type, AopCoderDialect dialect, String interfaceFullName ) {


            sb.AppendFormat( "\tpublic class {0} : {1} ", dialect.GetClassFullName( type, interfaceFullName ), interfaceFullName );
            sb.Append( "{" );
            sb.AppendLine();
            sb.AppendLine();

            sb.AppendFormat( "\t\tprivate {0} _{1};", interfaceFullName, dialect.GetInvokeTargetBase() );
            sb.AppendLine();

            sb.AppendFormat( "\t\tpublic {0} {1} ", interfaceFullName, dialect.GetInvokeTargetBase() );
            sb.Append( "{" );
            sb.AppendLine();

            sb.Append( "\t\t\tget { return _" + dialect.GetInvokeTargetBase() + "; }" );
            sb.AppendLine();

            sb.Append( "\t\t\tset { _" + dialect.GetInvokeTargetBase() + " = value; }" );
            sb.AppendLine();

            sb.Append( "\t\t}" );
            sb.AppendLine();

            sb.AppendLine();
        }

        private static void append_class_end( StringBuilder sb ) {
            sb.Append( "\t}" );
            sb.AppendLine();
        }

        //----------------------------------------------------------------------------------------

        private static void append_method_begin( StringBuilder sb, ObservedMethod x, String strArgs, String strReturn, AopCoderDialect dialect ) {
            sb.AppendFormat( "\t\tpublic {0} {1} {2}( {3} ) ", dialect.GetMethodOverride(), strReturn, x.Method.Name, strArgs );
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

        // 获取参数类型，比如 BlogPost x1, String x2, int x3
        // 转成 BlogPost , String , int 
        // new Type[] {}
        private static string getArgType( MethodInfo methodInfo ) {
            String str = "";
            ParameterInfo[] args = methodInfo.GetParameters();
            int i = 1;
            foreach (ParameterInfo x in args) {
                str += string.Format( "typeof({0}),", x.ParameterType.FullName );
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
