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
using System.CodeDom.Compiler;
using System.Reflection;
using System.Text;
using Microsoft.CSharp;
using Microsoft.VisualBasic;

using wojilu.IO;

namespace wojilu.Reflection {

    /// <summary>
    /// 代码执行器，可以直接运行代码，输出结果
    /// </summary>
    public class CodeRunner {

        public static Object RunFromFile( String filePath ) {
            return RunFullCode( File.Read( filePath ) );
        }

        public static Object RunSnippetFromFile( String filePath ) {
            return Run( File.Read( filePath ) );
        }

        public static Object RunFullCode( String code ) {
            String outputName = @"c:\temp\_wojilu_runTempClass_" + Guid.NewGuid().ToString() + ".exe";
            CodeDomProvider codeDomProvider = GetCodeDomProvider( "c#" );
            CompilerParameters options = new CompilerParameters( new String[] { "System.dll" }, outputName, false );
            options.ReferencedAssemblies.Add( "System.Data.dll" );
            options.ReferencedAssemblies.Add( "System.Drawing.dll" );
            options.ReferencedAssemblies.Add( "System.Windows.Forms.dll" );
            options.GenerateExecutable = true;
            CompilerResults results = codeDomProvider.CompileAssemblyFromSource( options, new String[] { code } );
            String message = "error : ";
            if (results.Errors.Count > 0) {
                foreach (CompilerError error in results.Errors) {
                    message = message + error.ToString() + "\r\n";
                }
                throw new Exception( message );
            }
            Assembly compiledAssembly = results.CompiledAssembly;
            MethodInfo entryPoint = compiledAssembly.EntryPoint;
            String fullName = entryPoint.DeclaringType.FullName;
            return ReflectionUtil.CallMethod( compiledAssembly.CreateInstance( fullName ), entryPoint.Name );
        }

        public static Assembly CompileCodeToDll( String code ) {

            String outputName = @"c:\temp\_wojilu_runTempClass_" + Guid.NewGuid().ToString() + ".dll";
            CodeDomProvider codeDomProvider = GetCodeDomProvider( "c#" );
            CompilerParameters options = new CompilerParameters( new String[] { "System.dll" }, outputName, false );
            options.ReferencedAssemblies.Add( "System.Data.dll" );
            options.ReferencedAssemblies.Add( "System.Drawing.dll" );
            options.ReferencedAssemblies.Add( "System.Windows.Forms.dll" );
            options.ReferencedAssemblies.Add( "wojilu.dll" );
            options.GenerateExecutable = false;
            CompilerResults results = codeDomProvider.CompileAssemblyFromSource( options, new String[] { code } );
            String message = "";
            if (results.Errors.Count > 0) {
                foreach (CompilerError error in results.Errors) {
                    message = message + error.ToString() + "\r\n";
                }
                throw new Exception( message );
            }
            return results.CompiledAssembly;
        }

        private static CodeDomProvider GetCodeDomProvider( String language ) {
            switch (language) {
                case "c#":
                    return new CSharpCodeProvider();

                case "vb":
                    return new VBCodeProvider();
            }
            return new CSharpCodeProvider();
        }

        public static Object Run( String expression ) {
            String str = "";
            expression = expression.Trim().TrimEnd( new char[] { ';' } );
            String[] strArray = expression.Split( new char[] { ';' } );
            for (int i = 0; i < strArray.Length; i++) {
                if (i != (strArray.Length - 1)) {
                    str = str + strArray[i] + ";\r\n";
                }
                else {
                    str = str + "return (" + strArray[i] + ");\r\n";
                }
            }
            CodeDomProvider codeDomProvider = GetCodeDomProvider( "c#" );
            CompilerParameters options = new CompilerParameters();
            options.ReferencedAssemblies.Add( "system.dll" );
            options.GenerateExecutable = false;
            options.GenerateInMemory = true;
            StringBuilder builder = new StringBuilder();
            builder.Append( "using System;\n" );
            builder.Append( "namespace wojilu.Reflection {\n" );
            builder.Append( "\tpublic class __codeRunner {\n" );
            builder.Append( "\t\tpublic Object run() {\n" );
            builder.Append( str );
            builder.Append( "\t\t}\n" );
            builder.Append( "\t}\n" );
            builder.Append( "}\n" );
            CompilerResults results = codeDomProvider.CompileAssemblyFromSource( options, new String[] { builder.ToString() } );
            String message = "";
            if (results.Errors.Count > 0) {
                foreach (CompilerError error in results.Errors) {
                    message = message + error.ToString() + "\r\n";
                }
                throw new Exception( message );
            }
            return ReflectionUtil.CallMethod( results.CompiledAssembly.CreateInstance( "wojilu.Reflection.__codeRunner" ), "run" );
        }


    }
}

