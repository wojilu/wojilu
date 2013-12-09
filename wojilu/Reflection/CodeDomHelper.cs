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
using System.Collections.Generic;
using System.Text;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.Collections;
using System.Reflection;
using System.IO;

namespace wojilu.Reflection {

    public class CodeDomHelper {

        private static readonly ILog logger = LogManager.GetLogger( typeof( CodeDomHelper ) );

        public static Assembly CompileCode( String code, Dictionary<String, Assembly> refrenceAsmList, String _tempDllName ) {

            Boolean isGenerateAssembly = strUtil.HasText( _tempDllName ) ? true : false;

            CompilerParameters options = new CompilerParameters();
            options.GenerateExecutable = false;
            options.CompilerOptions = "/optimize";
            if (isGenerateAssembly) {
                options.GenerateInMemory = false;
                // __wojilu.aop.dll
                if (_tempDllName.ToLower().EndsWith( ".dll" ) == false) _tempDllName = _tempDllName + ".dll";
                options.OutputAssembly = Path.Combine( PathHelper.GetBinDirectory(), _tempDllName );
            }
            else {
                options.GenerateInMemory = true;
            }
            Dictionary<String, String> allAsms = new Dictionary<String, String>();
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            allAsms[executingAssembly.FullName] = executingAssembly.Location;
            logger.Info( executingAssembly.FullName + "__" + executingAssembly.Location );
            addReferencedAsms( allAsms, executingAssembly.GetReferencedAssemblies() );

            foreach (KeyValuePair<String, Assembly> kv in refrenceAsmList) {
                allAsms[kv.Value.FullName] = kv.Value.Location;
                logger.Info( kv.Value.FullName + "__" + kv.Value.Location );
                addReferencedAsms( allAsms, kv.Value.GetReferencedAssemblies() );
            }

            foreach (KeyValuePair<String, String> kv in allAsms) {
                options.ReferencedAssemblies.Add( kv.Value );
            }

            CodeDomProvider provider = new CSharpCodeProvider();
            CompilerResults results = provider.CompileAssemblyFromSource( options, new String[] { code } );
            if (results.Errors.Count > 0) {
                StringBuilder builder = new StringBuilder();
                builder.AppendLine();
                foreach (CompilerError error in results.Errors) {
                    builder.AppendLine( "error.Line=" + error.Line );
                    builder.AppendLine( "error.ErrorText=" + error.ErrorText );
                    builder.AppendLine( "error.FileName=" + error.FileName );
                    builder.AppendLine();
                }
                logger.Fatal( code );
                throw new Exception( builder.ToString() );
            }
            return results.CompiledAssembly;
        }

        private static void addReferencedAsms( Dictionary<String, String> allAsms, AssemblyName[] assemblyName ) {
            foreach (AssemblyName name in assemblyName) {
                if (allAsms.ContainsKey( name.FullName ) == false) {
                    Assembly assembly = Assembly.Load( name );
                    allAsms[name.FullName] = assembly.Location;
                    logger.Info( name.FullName + "__" + assembly.Location );
                }
            }
        }

    }
}
