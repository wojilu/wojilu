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
using wojilu.IO;
using wojilu.Web.Templates.Parser;
using wojilu.Web.Templates.Tokens;
using wojilu.Web.Templates;
using System.Reflection;
using wojilu.Reflection;

namespace wojilu.Web {

    /// <summary>
    /// 模板引擎
    /// </summary>
    public class Template : ContentBlock, ITemplate {

        private static readonly ILog logger = LogManager.GetLogger( typeof( Template ) );

        public static readonly String loadedTemplates = "loadedTemplates";

        private static Dictionary<String, String> templates = new Dictionary<String, String>();
        private static Dictionary<String, ITemplateResult> _compiledResults = new Dictionary<String, ITemplateResult>();
        private object objLockCode = new object();
        private object objLockReadFile = new object();

        private String _templateContent;

        public Boolean IsTemplateExist() {
            return _isTemplateExist;
        }

        public void NoTemplates( String templateList ) {
            _isTemplateExist = false;
            _templatePath = templateList;
        }

        public Template() {
        }

        /// <summary>
        /// 根据指定路径的模板文件初始化
        /// </summary>
        /// <param name="absPath">模板文件所在的绝对路径</param>
        public Template( String absPath ) {

            InitPath( absPath );
        }




        private void InitPath( String templatePath ) {

            base._templatePath = templatePath;

            Boolean isCache = Mvc.MvcConfig.Instance.IsCacheView;

            if (isCache) {
                String templateContent = getTemplateContent( templatePath );
                if (strUtil.HasText( templateContent )) {
                    logTemplate( templatePath );
                    InitContent( templatePath, templateContent );
                }
                else {
                    _isTemplateExist = false;
                }
            }
            else {
                if (File.Exists( templatePath )) {
                    logTemplate( templatePath );
                    InitContent( File.Read( templatePath ) );
                }
                else {
                    _isTemplateExist = false;
                }
            }
        }

        private void logTemplate( string templatePath ) {
            //logger.Info( "load template=>" + templatePath );
            List<String> tplList = CurrentRequest.getItem( loadedTemplates ) as List<String>;
            if (tplList == null) tplList = new List<string>();
            tplList.Add( templatePath );
            CurrentRequest.setItem( loadedTemplates, tplList );
        }

        public static Boolean ContainsCache( String templatePath ) {
            String tpl;
            templates.TryGetValue( templatePath, out tpl );
            return strUtil.HasText( tpl );
        }

        private String getTemplateContent( String templatePath ) {

            if (templates.ContainsKey( templatePath ) == false) {

                lock (objLockReadFile) {

                    if (templates.ContainsKey( templatePath ) == false) {

                        templates[templatePath] = readTemplateBodyToCache( templatePath );
                    }
                }
            }

            return templates[templatePath];
        }

        private String readTemplateBodyToCache( String templatePath ) {
            String templateContent = "";
            if (File.Exists( templatePath )) {
                templateContent = File.Read( templatePath );
            }
            if (strUtil.IsNullOrEmpty( templateContent )) _isTemplateExist = false;
            return templateContent;
        }

        /// <summary>
        /// 根据模板内容初始化
        /// </summary>
        /// <param name="content">需要使用的模板内容(模板字符串)</param>
        /// <returns></returns>
        public ITemplate InitContent( String content ) {
            _templateContent = content;

            TemplateParser parser = TemplateParser.GetParser( content );
            _thisToken = parser.getToken() as BlockToken;
            return this;
        }

        public ITemplate InitContent( String absPath, String content ) {

            _templateContent = content;

            TemplateParser parser = TemplateParser.GetParser( absPath, content );
            _thisToken = parser.getToken() as BlockToken;

            return this;
        }

        /// <summary>
        /// 判断模板内容是否为空
        /// </summary>
        public Boolean IsEmpty {
            get { return strUtil.IsNullOrEmpty( this._templateContent ); }
        }

        public void Replace( String lbl, String lblValue ) {
        }

        /// <summary>
        /// 获取模板绑定之后的最终结果
        /// </summary>
        /// <returns></returns>
        public override String ToString() {
            return ToStringBuilder().ToString();
        }

        public StringBuilder ToStringBuilder() {
            if (strUtil.IsNullOrEmpty( _templateContent )) return new StringBuilder();
            return this.Compile().GetResult();
        }

        /// <summary>
        /// 返回模板的原始内容，尚未给变量赋值
        /// </summary>
        /// <returns></returns>
        public String getTemplateString() {
            return _templateContent;
        }


        public ITemplateResult Compile() {

            String oContent = this.getTemplateString();

            if (_compiledResults.ContainsKey( oContent ) == false) {

                lock (objLockCode) {

                    if (_compiledResults.ContainsKey( oContent ) == false) {

                        String clsName = "__templatePage_" + Guid.NewGuid().ToString().Replace( "-", "" );

                        _compiledResults[oContent] = compileCode( clsName );

                    }

                }

            }

            ITemplateResult t = _compiledResults[oContent];
            t.SetData( new ViewData( this ) );
            return t;
        }

        private ITemplateResult compileCode( String clsName ) {
            String code = this.GetCode( clsName );

            Assembly asm;
            try {
                asm = CodeDomHelper.CompileCode( code, ObjectContext.Instance.AssemblyList, getTempDllPath2( clsName ) );
            }
            catch (Exception ex) {
                throw new TemplateCompileException( "模板编译错误", ex );
            }

            return asm.CreateInstance( "wojilu." + clsName ) as ITemplateResult;

        }

        // 在内存中编译，不生成实体dll
        private String getTempDllPath2( String clsName ) {
            return "";
        }

        // 在 wojilu.Web/__template/中生成dll
        private String getTempDllPath1( String clsName ) {

            String templatePath = "../__template/";

            String absPath = System.IO.Path.Combine( PathHelper.GetBinDirectory(), templatePath );

            if (System.IO.Directory.Exists( absPath ) == false) {
                System.IO.Directory.CreateDirectory( absPath );
            }

            return templatePath + clsName;
        }

        /// <summary>
        /// 清空所有缓存
        /// </summary>
        public static void Reset() {
            templates.Clear();
            TemplateParser.Reset();
            _compiledResults.Clear();
        }

    }
}
