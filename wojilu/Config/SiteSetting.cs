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
using System.Text;
using System.Collections.Generic;
using wojilu.Web.Handler;
using wojilu.Web.Mvc;

namespace wojilu.Config {

    /// <summary>
    /// 注册类型
    /// </summary>
    public class RegisterType {

        /// <summary>
        /// 开放注册
        /// </summary>
        public static readonly int Open = 0;

        /// <summary>
        /// 关闭注册
        /// </summary>
        public static readonly int Close = 1;

        /// <summary>
        /// 关闭注册，但受邀请的除外
        /// </summary>
        public static readonly int CloseUnlessInvite = 2;
    }

    public class LoginType {

        /// <summary>
        /// 注册之后自动登录
        /// </summary>
        public static readonly int Open = 0;

        /// <summary>
        /// 注册之后并不自动登录；必须激活才能登录
        /// </summary>
        public static readonly int ActivationEmail = 1;

    }

    /// <summary>
    /// (网站页面顶部的)用户导航栏的显示状态
    /// </summary>
    public class TopNavDisplay {

        /// <summary>
        /// 显示
        /// </summary>
        public static readonly int Show = 0;

        /// <summary>
        /// 隐藏
        /// </summary>
        public static readonly int Hide = 1;

        /// <summary>
        /// 在关闭注册之后隐藏
        /// </summary>
        public static readonly int NoRegHide = 2;

    }

    /// <summary>
    /// 网站配置
    /// </summary>
    public class SiteSetting {

        //-------------------------------base---------------------------------

        /// <summary>
        /// 网站名称
        /// </summary>
        public String SiteName { get; set; }

        /// <summary>
        /// 网站的网址
        /// </summary>
        public String SiteUrl { get; set; }

        /// <summary>
        /// logo的图片网址
        /// </summary>
        public String SiteLogo { get; set; }

        /// <summary>
        /// 站长名字
        /// </summary>
        public String Webmaster { get; set; }

        /// <summary>
        /// 网站的email
        /// </summary>
        public String Email { get; set; }

        /// <summary>
        /// 网页默认关键词
        /// </summary>
        public String Keywords { get; set; }

        /// <summary>
        /// 网页默认的描述
        /// </summary>
        public String Description { get; set; }

        /// <summary>
        /// 网页默认的标题
        /// </summary>
        public String PageDefaultTitle { get; set; }

        /// <summary>
        /// 网站是否需要登录才能访问，默认不需要登录
        /// </summary>
        public Boolean NeedLogin { get; set; }

        /// <summary>
        /// 用户注册之后必须经过人工审核
        /// </summary>
        public Boolean UserNeedApprove { get; set; }

        /// <summary>
        /// 对尚未激活的用户，是否提醒他激活
        /// </summary>
        public Boolean AlertActivation { get; set; }

        /// <summary>
        /// 用户自己重发激活邮件的间隔
        /// </summary>
        public int UserSendConfirmEmailInterval { get; set; }

        /// <summary>
        /// 对尚未上传头像的用户，是否提醒他上传头像
        /// </summary>
        public Boolean AlertUserPic { get; set; }

        /// <summary>
        /// 注册的三种类型 (1)开放注册 (2)关闭注册 (3)只有受邀请用户才可以注册
        /// </summary>
        public int RegisterType { get; set; }

        /// <summary>
        /// 登录限制(1)注册之后自动登录 (2)必须激活才能登录)
        /// </summary>
        public int LoginType { get; set; }

        /// <summary>
        /// 顶部用户栏状态(1)显示 (2)隐藏 (3)在关闭注册之后隐藏
        /// </summary>
        public int TopNavDisplay { get; set; }

        private String _initApp;

        /// <summary>
        /// 用户注册之后默认安装的app。
        /// </summary>
        public String UserInitApp {
            get {
                //if (strUtil.IsNullOrEmpty( _initApp )) return "home, blog, photo";
                if (strUtil.IsNullOrEmpty( _initApp )) return "";
                return _initApp;
            }
            set { _initApp = value; }
        }

        private int _ValidationType;

        /// <summary>
        /// 验证码类型
        /// </summary>
        public int ValidationType {
            get {
                if (_ValidationType == 0) return ValidationDefault.Type;
                return _ValidationType;
            }
            set {
                _ValidationType = value;
            }
        }

        private int _ValidationLength;

        /// <summary>
        /// 英文或数字的验证没长度
        /// </summary>
        public int ValidationLength {
            get {
                if (_ValidationLength == 0) return ValidationDefault.Length;
                return _ValidationLength;
            }
            set {
                _ValidationLength = value;
            }

        }

        private int _ValidationChineseLength;

        /// <summary>
        /// 中文验证码长度
        /// </summary>
        public int ValidationChineseLength {
            get {
                if (_ValidationChineseLength == 0) return ValidationDefault.ChineseLength;
                return _ValidationChineseLength;
            }
            set {
                _ValidationChineseLength = value;
            }

        }

        public Boolean IsInstall { get; set; }


        /// <summary>
        /// 网站是否关闭
        /// </summary>
        public Boolean IsClose { get; set; }

        /// <summary>
        /// 如果关闭，关闭的原因
        /// </summary>
        public String CloseReason { get; set; }

        /// <summary>
        /// 是否开启统计，需要结合StatsJs
        /// </summary>
        public Boolean StatsEnabled { get; set; }

        /// <summary>
        /// 统计js
        /// </summary>
        public String StatsJs { get; set; }

        /// <summary>
        /// 版权信息
        /// </summary>
        public String Copyright { get; set; }

        /// <summary>
        /// logo宽度
        /// </summary>
        public int LogoWidth { get { return 250; } }

        /// <summary>
        /// logo高度
        /// </summary>
        public int LogoHeight { get { return 80; } }

        /// <summary>
        /// 网站当前皮肤的ID
        /// </summary>
        public int SkinId { get; set; }

        /// <summary>
        /// 用户密码是否采用16位的md5加密方式(默认为否，用于兼容旧的系统)
        /// </summary>
        public Boolean Md5Is16 { get; set; }

        /// <summary>
        /// 评论内容的长度
        /// </summary>
        public int CommentLength { get { return 500; } }

        private int _TagLength;

        /// <summary>
        /// tag的长度
        /// </summary>
        public int TagLength {
            get {
                if (_TagLength == 0) return 20;
                return _TagLength;
            }
            set {
                _TagLength = value;
            }

        }

        /// <summary>
        /// 网站logo图片的完整网址
        /// </summary>
        public String SiteLogoFull {
            get { return strUtil.Join( sys.Path.Photo, this.SiteLogo ); }
        }

        /// <summary>
        /// 获取统计的js内容
        /// </summary>
        /// <returns></returns>
        public String GetStatsJs() {
            if (this.StatsEnabled == false) return "";
            return this.StatsJs;
        }

        /// <summary>
        /// 如果设置了logo图片，返回img；否则返回siteName
        /// </summary>
        /// <returns></returns>
        public String GetLogoHtml() {

            String logo;
            if (strUtil.HasText( this.SiteLogo )) {
                logo = "<img src=\"" + SiteLogoFull + "?v=" + MvcConfig.Instance.CssVersion + "\"/>";
            }
            else {
                logo = "<div class=\"nologo\">" + this.SiteName + "</div>";
            }

            return logo;
        }

        //----------------------------- user -----------------------------------

        public Boolean ShowSexyInfoInProfile { get; set; }

        /// <summary>
        /// 注册用户名允许的长度
        /// </summary>
        public int UserNameLengthMax { get; set; }

        /// <summary>
        /// 注册用户名至少要达到的长度
        /// </summary>
        public int UserNameLengthMin { get; set; }

        /// <summary>
        /// 登录是否启用验证码
        /// </summary>
        public Boolean LoginNeedImgValidation { get; set; }

        /// <summary>
        /// 注册是否启用验证码
        /// </summary>
        public Boolean RegisterNeedImgValidateion { get; set; }

        /// <summary>
        /// 给新注册用户发送的欢迎信息的标题
        /// </summary>
        public String SystemMsgTitle { get; set; }

        /// <summary>
        /// 给新注册用户发送的欢迎信息的内容(支持html)
        /// </summary>
        public String SystemMsgContent { get; set; }

        /// <summary>
        /// 保留用户名(不可以注册的用户名)
        /// </summary>
        public String[] ReservedUserName { get; set; }

        /// <summary>
        /// 保留的用户个性网址
        /// </summary>
        public String[] ReservedUserUrl { get; set; }

        /// <summary>
        /// 保留的关键词(不可以在用户名和个性网址中使用)
        /// </summary>
        public String[] ReservedKey { get; set; }

        public Boolean IsReservedKeyContains( String inputName ) {

            string[] arr = this.ReservedKey;
            foreach (String key in arr) {
                if (strUtil.EqualsIgnoreCase( inputName, key ) || strUtil.EqualsIgnoreCase( inputName, key + "s" )) {
                    return true;
                }
            }

            return false;
        }

        //------------------------------filter----------------------------------

        /// <summary>
        /// 禁用的关键词
        /// </summary>
        public String[] BadWords { get; set; }

        /// <summary>
        /// 禁用词汇的替换词
        /// </summary>
        public String BadWordsReplacement { get; set; }

        /// <summary>
        /// 所有被禁的ip地址
        /// </summary>
        public String[] BannedIp { get; set; }

        private String _bannedIpInfo;

        /// <summary>
        /// 给被屏蔽访客的警告信息
        /// </summary>
        public String BannedIpInfo {
            get {
                if (_bannedIpInfo == null) return "对不起，你的 ip 地址已被屏蔽";
                return _bannedIpInfo;
            }
            set { _bannedIpInfo = value; }
        }


        //public Boolean IsWatermark { get; set; }


        private int _microblogContentMax;
        /// <summary>
        /// 微博内容字数最高限制
        /// </summary>
        public int MicroblogContentMax {
            get {
                if (_microblogContentMax <= 0) return 140;
                return _microblogContentMax;
            }
            set {
                _microblogContentMax = value;
            }
        }

        private int _microblogPageSize;
        /// <summary>
        /// 微博内容字数最高限制
        /// </summary>
        public int MicroblogPageSize {
            get {
                if (_microblogPageSize <= 0) return 20;
                return _microblogPageSize;
            }
            set {
                _microblogPageSize = value;
            }
        }

        //----------------------------------------------------------------

        public Boolean EnableEmail { get; set; } // 是否开启邮件服务，比如：邮件激活等
        public String SmtpUrl { get; set; }
        public String SmtpUser { get; set; }
        public String SmtpPwd { get; set; }
        public Boolean SmtpEnableSsl { get; set; }

        public Boolean CloseComment { get; set; }

        //-------------------------------other---------------------------------

        public int MaxOnline { get; set; }
        public DateTime MaxOnlineTime { get; set; }
        public int UserTemplateId { get; set; }

        public int NoRefreshSecond { get; set; }

        public int FeedKeepDay { get; set; }
        public DateTime LastFeedClearTime { get; set; }

        public String[] Spider { get; set; }

        private static string getArrayString( String[] arr ) {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < arr.Length; i++) {
                sb.Append( arr[i] );
                if (i < arr.Length - 1) sb.Append( "/" );
            }
            return sb.ToString();
        }


        //----------------------------------------------------------------

        public int PhotoThumbHeight { get; set; }
        public int PhotoThumbWidth { get; set; }

        public int PhotoThumbHeightMedium { get; set; }
        public int PhotoThumbWidthMedium { get; set; }

        public int PhotoThumbHeightBig { get; set; }
        public int PhotoThumbWidthBig { get; set; }


        public int AvatarThumbHeight { get; set; }
        public int AvatarThumbWidth { get; set; }

        public int AvatarThumbHeightMedium { get; set; }
        public int AvatarThumbWidthMedium { get; set; }

        public int AvatarThumbHeightBig { get; set; }
        public int AvatarThumbWidthBig { get; set; }

        public Boolean IsSaveAvatarMedium { get; set; }
        public Boolean IsSaveAvatarBig { get; set; }

        public String[] UploadFileTypes { get; set; }
        public String[] UploadPicTypes { get; set; }

        private int _uploadPicMaxMB;
        private int _uploadFileMaxMB;


        /// <summary>
        /// 图片最大上传的大小，单位MB
        /// </summary>
        public int UploadPicMaxMB {
            get {
                if (_uploadPicMaxMB == 0) return 5;
                return _uploadPicMaxMB;
            }
            set { _uploadPicMaxMB = value; }
        }

        /// <summary>
        /// 图片最大上传的大小，单位MB
        /// </summary>
        public int UploadFileMaxMB {
            get {
                if (_uploadFileMaxMB == 0) return 20;
                return _uploadFileMaxMB;
            }
            set { _uploadFileMaxMB = value; }

        }


        // 扩展的值
        //-------------------------------------------------------------------------------------

        public int GetValueInt( String key ) {
            return cvt.ToInt( GetValue( key ) );
        }

        public Boolean GetValueBool( String key ) {
            return cvt.ToBool( GetValue( key ) );
        }

        public decimal GetValueDecimal( String key ) {
            return cvt.ToDecimal( GetValue( key ) );
        }

        public DateTime GetValueTime( String key ) {
            return cvt.ToTime( GetValue( key ) );
        }

        public String GetValue( String key ) {
            if (strUtil.IsNullOrEmpty( key )) return null;
            if (_valueAll == null) return null;
            if (_valueAll.ContainsKey( key ) == false) return null;
            return _valueAll[key];
        }

        //-------------------------

        public String[] GetArrayValue( String key ) {
            return getArrayValue( _valueAll, key );
        }

        internal String[] getArrayValue( Dictionary<String, String> dic, String key ) {

            if (dic == null) return new String[] { };

            if (dic.ContainsKey( key ) && strUtil.HasText( dic[key] )) {
                return GetArrayValueByString( dic[key] );
            }

            return new String[] { };
        }

        public static String[] GetArrayValueByString( String valOne ) {

            if (strUtil.IsNullOrEmpty( valOne )) return new String[] { };

            String[] arrValue = valOne.Split( new char[] { ',', '/', '|', '，', '、' } );

            // 剔除掉无效的字符串
            ArrayList results = new ArrayList();
            foreach (String val in arrValue) {
                if (strUtil.HasText( val )) results.Add( val.Trim() );
            }

            return (String[])results.ToArray( typeof( String ) );
        }

        private Dictionary<String, String> _valueAll;

        internal void setValueAll( Dictionary<String, String> valueAll ) {
            _valueAll = valueAll;
        }

        //----------------------------------------------------------------

        /// <summary>
        ///  将更新保存到磁盘
        /// </summary>
        /// <param name="item"></param>
        /// <param name="val"></param>
        public void Update( String item, Object val ) {

            String itemValue = "";
            if (val != null) itemValue = val.ToString();

            itemValue = strUtil.Text2Html( itemValue );

            saveConfig( item, itemValue );
        }

        public void UpdateHtml( String item, Object val ) {

            String itemValue = "";
            if (val != null) itemValue = val.ToString();
            itemValue = itemValue.Replace( "\n", "" ).Replace( "\r", "" );

            saveConfig( item, itemValue );
        }

        private void saveConfig( String item, String itemValue ) {
            String[] configString = file.ReadAllLines( config.siteconfigAbsPath );
            List<String> results = new List<string>();

            checkSingleLine( item, itemValue, configString, results );
            StringBuilder sb = new StringBuilder();
            foreach (String line in results) {
                sb.Append( line );
                sb.Append( Environment.NewLine );
            }
            lock (objLock) {
                file.Write( config.siteconfigAbsPath, sb.ToString() );
            }
        }

        private void checkSingleLine( String item, String itemValue, String[] configString, List<String> results ) {
            Boolean hasItem = false;
            foreach (String line in configString) {

                if (startsWith( line, item )) {
                    hasItem = true;
                    results.Add( item + " : " + itemValue );
                    _valueAll[item] = itemValue;
                }
                else {
                    results.Add( line );
                }
            }
            if (hasItem == false) {
                results.Add( item + " : " + itemValue );
                _valueAll[item] = itemValue;
            }
        }

        private static Boolean startsWith( String line, String item ) {

            if (line.StartsWith( item )) {
                String noPrefixLine = strUtil.TrimStart( line, item ).Trim();
                if (noPrefixLine.StartsWith( ":" )) {
                    return true;
                }
            }
            return false;
        }

        private static Object objLock = new object();


    }
}

