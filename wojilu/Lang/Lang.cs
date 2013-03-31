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
using System.IO;
using wojilu.Web;

namespace wojilu {

    /// <summary>
    /// 语言包工具，用于加载多国语言。
    /// </summary>
    /// <remarks>
    /// 默认语言包文件存放在 /framework/lang/ 中，比如 /framework/lang/zh-cn/ 。只要在 /framework/lang/  中新增一个语言包文件夹，则系统将其作为语言包列表自动加载。可添加的语言包名称包括：en-us,en-gb,zh-cn,zh-tw,ja,ko,fr,de,it
    /// </remarks>
    public class lang {

        private static readonly ILog logger = LogManager.GetLogger( typeof( lang ) );

        private static Dictionary<String, Dictionary<String, LanguageSetting>> langLocaleAll = getLangLocale();

        public static readonly String CoreLangPrefix = "_!";
        public static readonly String AppLangPrefix = "_@";

        //--------------------------------------------------------------------

        /// <summary>
        /// 获取当前语言字符(比如 zh-cn，或 en-us)
        /// </summary>
        /// <returns></returns>
        public static String getLangString() {

            Object ret = CurrentRequest.getItem( "__lang_name" );
            if (ret == null) {
                String langCookie = CurrentRequest.getLangCookie();
                logger.Info( "lang cookie: " + ret );
                ret = getLangNamePrivate( langCookie );
                CurrentRequest.setItem( "__lang_name", ret );
            }

            return ret.ToString();
        }

        private static String getLangNamePrivate( String langCookie ) {

            String defaultLang = "zh-cn";

            if (strUtil.HasText( langCookie ) && langLocaleAll.ContainsKey( langCookie )) {
                return langCookie;
            }

            if (CurrentRequest.getUserLanguages() == null) return defaultLang;
            String[] reqLangs = CurrentRequest.getUserLanguages();
            if (reqLangs.Length == 0) return defaultLang;
            if (langLocaleAll.ContainsKey( reqLangs[0] )) return reqLangs[0];

            return defaultLang;
        }


        /// <summary>
        /// 获取某 key 的语言值(从 core.config 中获取)
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static String get( String key ) {
            return getCoreLang().get( key );
        }

        /// <summary>
        /// 获取在 core.config 中定义的核心语言包
        /// </summary>
        /// <returns></returns>
        public static LanguageSetting getCoreLang() {
            return getCoreLang( getLangString() );
        }

        private static LanguageSetting getCoreLang( String langStr ) {
            Dictionary<String, LanguageSetting> langlist;
            langLocaleAll.TryGetValue( langStr, out langlist );

            if (langlist == null) {
                langlist = new Dictionary<String, LanguageSetting>();
                logger.Error( "no language: " + langStr );
            }

            String configName = "core";
            LanguageSetting ret;
            langlist.TryGetValue( configName, out ret );
            if (ret == null) {
                ret = LanguageSetting.NewNull();
                logger.Error( "no language config: " + langStr + " => " + configName );
            }
            return ret;
        }

        /// <summary>
        /// 根据类型 t 获取语言列表
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static LanguageSetting getByApp( Type t ) {

            String langStr = getLangString();
            Dictionary<String, LanguageSetting> langlist;

            langLocaleAll.TryGetValue( langStr, out langlist );

            if (langlist == null) {
                langlist = new Dictionary<String, LanguageSetting>();
                logger.Error( "no language: " + langStr );
            }

            LanguageSetting result;
            langlist.TryGetValue( t.FullName, out result );

            if (result == null) {
                result = LanguageSetting.NewNull();
                logger.Error( "no language config: " + langStr + " => " + t.FullName );
            }

            return result;
        }

        //--------------------------------------------------------------------

        private static Dictionary<String, Dictionary<String, LanguageSetting>> getLangLocale() {

            String dirRoot = PathHelper.Map( getLangRootPath() );

            if (Directory.Exists( dirRoot ) == false) {
                logger.Error( "no language directory: " + dirRoot );
                return new Dictionary<String, Dictionary<String, LanguageSetting>>();
            }

            String[] dirPaths = Directory.GetDirectories( dirRoot );

            Dictionary<String, Dictionary<String, LanguageSetting>> results = new Dictionary<String, Dictionary<String, LanguageSetting>>();
            foreach (String path in dirPaths) {
                String langName = Path.GetFileName( path );
                results.Add( langName.ToLower(), getLangList( path ) );
            }

            if (results.Count == 0) {
                logger.Error( "no language sub directory: " + dirRoot );
            }

            return results;
        }

        private static Dictionary<String, LanguageSetting> getLangList( String path ) {

            String[] files = Directory.GetFiles( path );

            Dictionary<String, LanguageSetting> results = new Dictionary<String, LanguageSetting>();

            foreach (String file in files) {

                if (Path.GetExtension( file ) != ".config") continue;

                String fileName = Path.GetFileNameWithoutExtension( file );

                Dictionary<String, String> _lang = cfgHelper.Read( file, '=' );
                LanguageSetting lbl = new LanguageSetting( fileName, _lang );

                results.Add( fileName, lbl );
            }

            return results;
        }

        //--------------------------------------------------------------------

        private static String getLangRootPath() {
            return cfgHelper.FrameworkRoot + "lang/";
        }

        //------------------------------------------------------------------

        /// <summary>
        /// 获取所有支持的语言包
        /// </summary>
        /// <returns></returns>
        public static List<Dictionary<String, String>> GetSupportedLang() {

            List<Dictionary<String, String>> list = new List<Dictionary<String, String>>();
            foreach (String key in langLocaleAll.Keys) {
                Dictionary<String, String> pair = new Dictionary<String, String>();
                pair.Add( "Name", GetLangInfo( key ) );
                pair.Add( "Value", key );
                list.Add( pair );
            }
            return list;
        }

        private static String GetLangInfo( String langStr ) {
            if (langMap.ContainsKey( langStr )) return langMap[langStr];
            return langStr;
        }

        private static Dictionary<String, String> langMap = getLangInfo();

        private static Dictionary<String, String> getLangInfo() {
            Dictionary<String, String> map = new Dictionary<String, String>();
            map.Add( "en-us", "English (US)" );
            map.Add( "en-gb", "English (British)" );
            map.Add( "zh-cn", "中文(简体)" ); // skipLang
            map.Add( "zh-tw", "正體中文(繁體)" ); // skipLang
            map.Add( "ja", "日本語" );// skipLang
            map.Add( "ko", "한국어" );
            map.Add( "fr", "Français" );
            map.Add( "de", "Deutsch" );
            map.Add( "it", "Italiano" );

            return map;
        }

    }



}
