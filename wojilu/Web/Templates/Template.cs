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

namespace wojilu.Web {

    /// <summary>
    /// Ä£°åÒýÇæ
    /// </summary>
    public class Template : ContentBlock, ITemplate {

        private static readonly ILog logger = LogManager.GetLogger( typeof( Template ) );

        public static readonly String loadedTemplates = "loadedTemplates";

        private String _templateContent;

        public Boolean IsTemplateExist() {
            return _isTemplateExist;
        }


        public Template() {
        }

        public Template( String absPath ) {

            InitPath( absPath );
        }

        public Template( String absPath, StringBuilder builder ) {

            //base.resultBuilder = builder;

            InitPath( absPath );
        }


        private static Dictionary<String, String> templates = new Dictionary<String, String>();

        private object objLockReadFile = new object();

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

        public ITemplate InitContent( String content ) {
            _templateContent = content;

            TemplateParser parser = TemplateParser.GetParser( content );
            _thisToken = parser.getToken() as BlockToken;
            return this;
        }

        public ITemplate InitContent( String content, StringBuilder resultBuilder ) {
            //base.resultBuilder = resultBuilder;
            return InitContent( content );
        }

        public ITemplate InitContent( String absPath, String content ) {

            _templateContent = content;

            TemplateParser parser = TemplateParser.GetParser( absPath, content );
            _thisToken = parser.getToken() as BlockToken;

            return this;
        }



        public Boolean IsEmpty {
            get { return strUtil.IsNullOrEmpty( this._templateContent ); }
        }

        public void Replace( String lbl, String lblValue ) {
        }


        public override String ToString() {

            if (strUtil.IsNullOrEmpty( _templateContent )) return "";

            //logger.Info( "_templateContent=>" + _templateContent );
            return base.ToString();
        }

        public String getTemplateString() {
            return _templateContent;
        }

    }
}
