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


using System.Collections.Generic;
using wojilu.Config;
using System;

namespace wojilu {

    /// <summary>
    /// 网站的配置信息
    /// </summary>
    public class config {


        private SiteSetting _siteSetting;
        private GroupSetting _groupSetting;

        /// <summary>
        /// 网站的配置信息
        /// </summary>
        public SiteSetting Site {
            get { return _siteSetting; }
        }

        /// <summary>
        /// 群组的配置信息
        /// </summary>
        public GroupSetting Group {
            get { return _groupSetting; }
        }

        //------------------------------------------------------------

        private config() { loadAll(); }

        private static volatile config _instance;
        private static Object _syncRoot = new object();
        public static config Instance {
            get {
                if (_instance == null) {
                    lock (_syncRoot) {
                        if (_instance == null) _instance = new config();
                    }
                }
                return _instance;
            }
        }

        private void Reload() {
            loadAll();
        }


        private void loadAll() {
            initSiteSettings();
            initGroupSettings();
        }


        private void initGroupSettings() {
            _groupSetting = new GroupSetting();
        }

        // ------------------------- site settings -------------------------

        private void initSiteSettings() {

            _siteSetting = new SiteSetting();

            Dictionary<String, String> dic = cfgHelper.Read( siteconfigAbsPath );
            if (dic.Count <= 0) return;


            _siteSetting.SiteName = getVal( dic, "SiteName" );
            _siteSetting.SiteUrl = getVal( dic, "SiteUrl" );
            _siteSetting.SiteLogo = getVal( dic, "SiteLogo" );

            _siteSetting.Webmaster = getVal( dic, "Webmaster" );
            _siteSetting.Email = getVal( dic, "Email" );
            _siteSetting.Copyright = getVal( dic, "Copyright" );
            _siteSetting.Keywords = getVal( dic, "Keywords" );
            _siteSetting.Description = getVal( dic, "Description" );
            _siteSetting.PageDefaultTitle = getVal( dic, "PageDefaultTitle" );
            _siteSetting.IsClose = cvt.ToBool( getVal( dic, "IsClose" ) );
            _siteSetting.CloseReason = getVal( dic, "CloseReason" );
            _siteSetting.IsInstall = cvt.ToBool( getVal( dic, "IsInstall" ) );

            _siteSetting.RegisterType = cvt.ToInt( getVal( dic, "RegisterType" ) );
            _siteSetting.LoginType = cvt.ToInt( getVal( dic, "LoginType" ) );
            _siteSetting.TopNavDisplay = cvt.ToInt( getVal( dic, "TopNavDisplay" ) );

            _siteSetting.NeedLogin = cvt.ToBool( getVal( dic, "NeedLogin" ) );
            _siteSetting.UserNeedApprove = cvt.ToBool( getVal( dic, "UserNeedApprove" ) );

            _siteSetting.AlertActivation = cvt.ToBool( getVal( dic, "AlertActivation" ) );
            _siteSetting.AlertUserPic = cvt.ToBool( getVal( dic, "AlertUserPic" ) );


            _siteSetting.UserSendConfirmEmailInterval = cvt.ToInt( getVal( dic, "UserSendConfirmEmailInterval" ) );

            _siteSetting.UserInitApp = getVal( dic, "UserInitApp" );

            //
            _siteSetting.TagLength = cvt.ToInt( getVal( dic, "TagLength" ) );

            _siteSetting.ValidationType = cvt.ToInt( getVal( dic, "ValidationType" ) );
            _siteSetting.ValidationLength = cvt.ToInt( getVal( dic, "ValidationLength" ) );
            _siteSetting.ValidationChineseLength = cvt.ToInt( getVal( dic, "ValidationChineseLength" ) );


            _siteSetting.StatsEnabled = cvt.ToBool( getVal( dic, "StatsEnabled" ) );
            _siteSetting.StatsJs = getVal( dic, "StatsJs" );

            _siteSetting.MicroblogContentMax = cvt.ToInt( getVal( dic, "MicroblogContentMax" ) );
            _siteSetting.MicroblogPageSize = cvt.ToInt( getVal( dic, "MicroblogPageSize" ) );


            _siteSetting.SkinId = cvt.ToInt( getVal( dic, "SkinId" ) );
            _siteSetting.Md5Is16 = cvt.ToBool( getVal( dic, "Md5Is16" ) );

            _siteSetting.SystemMsgTitle = getVal( dic, "SystemMsgTitle" );
            _siteSetting.SystemMsgContent = getVal( dic, "SystemMsgContent" );
            _siteSetting.UserNameLengthMax = cvt.ToInt( getVal( dic, "UserNameLengthMax" ) );
            _siteSetting.UserNameLengthMin = cvt.ToInt( getVal( dic, "UserNameLengthMin" ) );
            _siteSetting.LoginNeedImgValidation = cvt.ToBool( getVal( dic, "LoginNeedImgValidation" ) );
            _siteSetting.RegisterNeedImgValidateion = cvt.ToBool( getVal( dic, "RegisterNeedImgValidateion" ) );
            _siteSetting.ShowSexyInfoInProfile = cvt.ToBool( getVal( dic, "ShowSexyInfoInProfile" ) );

            _siteSetting.ReservedUserName = _siteSetting.getArrayValue( dic, "ReservedUserName" );
            _siteSetting.ReservedUserUrl = _siteSetting.getArrayValue( dic, "ReservedUserUrl" );
            _siteSetting.ReservedKey = _siteSetting.getArrayValue( dic, "ReservedKey" );

            _siteSetting.PhotoThumbHeight = cvt.ToInt( getVal( dic, "PhotoThumbHeight" ) );
            _siteSetting.PhotoThumbWidth = cvt.ToInt( getVal( dic, "PhotoThumbWidth" ) );
            _siteSetting.PhotoThumbHeightMedium = cvt.ToInt( getVal( dic, "PhotoThumbHeightMedium" ) );
            _siteSetting.PhotoThumbWidthMedium = cvt.ToInt( getVal( dic, "PhotoThumbWidthMedium" ) );
            _siteSetting.PhotoThumbHeightBig = cvt.ToInt( getVal( dic, "PhotoThumbHeightBig" ) );
            _siteSetting.PhotoThumbWidthBig = cvt.ToInt( getVal( dic, "PhotoThumbWidthBig" ) );

            _siteSetting.AvatarThumbHeight = cvt.ToInt( getVal( dic, "AvatarThumbHeight" ) );
            _siteSetting.AvatarThumbWidth = cvt.ToInt( getVal( dic, "AvatarThumbWidth" ) );
            _siteSetting.AvatarThumbHeightMedium = cvt.ToInt( getVal( dic, "AvatarThumbHeightMedium" ) );
            _siteSetting.AvatarThumbWidthMedium = cvt.ToInt( getVal( dic, "AvatarThumbWidthMedium" ) );
            _siteSetting.AvatarThumbHeightBig = cvt.ToInt( getVal( dic, "AvatarThumbHeightBig" ) );
            _siteSetting.AvatarThumbWidthBig = cvt.ToInt( getVal( dic, "AvatarThumbWidthBig" ) );

            _siteSetting.UploadFileTypes = _siteSetting.getArrayValue( dic, "UploadFileTypes" );
            _siteSetting.UploadPicTypes = _siteSetting.getArrayValue( dic, "UploadPicTypes" );

            _siteSetting.UploadPicMaxMB = cvt.ToInt( getVal( dic, "UploadPicMaxMB" ) );
            _siteSetting.UploadFileMaxMB = cvt.ToInt( getVal( dic, "UploadFileMaxMB" ) );


            _siteSetting.IsSaveAvatarMedium = cvt.ToBool( getVal( dic, "IsSaveAvatarMedium" ) );
            _siteSetting.IsSaveAvatarBig = cvt.ToBool( getVal( dic, "IsSaveAvatarBig" ) );

            _siteSetting.BadWords = _siteSetting.getArrayValue( dic, "BadWords" );
            _siteSetting.BadWordsReplacement = getVal( dic, "BadWordsReplacement" );

            _siteSetting.BannedIp = _siteSetting.getArrayValue( dic, "BannedIp" );
            _siteSetting.BannedIpInfo = getVal( dic, "BannedIpInfo" );


            _siteSetting.Spider = _siteSetting.getArrayValue( dic, "Spider" );
            if (_siteSetting.Spider.Length == 0) _siteSetting.Spider = new String[] { "spider", "robot", "Slurp", "sogou", "youdao", "google" };


            _siteSetting.MaxOnline = cvt.ToInt( getVal( dic, "MaxOnline" ) );
            _siteSetting.MaxOnlineTime = cvt.ToTime( getVal( dic, "MaxOnlineTime" ) );
            _siteSetting.UserTemplateId = cvt.ToInt( getVal( dic, "UserTemplateId" ) );

            _siteSetting.FeedKeepDay = cvt.ToInt( getVal( dic, "FeedKeepDay" ) );
            _siteSetting.LastFeedClearTime = cvt.ToTime( getVal( dic, "LastFeedClearTime" ) );

            _siteSetting.NoRefreshSecond = cvt.ToInt( getVal( dic, "NoRefreshSecond" ) );

            _siteSetting.SmtpUrl = getVal( dic, "SmtpUrl" );
            _siteSetting.SmtpUser = getVal( dic, "SmtpUser" );
            _siteSetting.SmtpPwd = getVal( dic, "SmtpPwd" );
            _siteSetting.SmtpEnableSsl = cvt.ToBool( getVal( dic, "SmtpEnableSsl" ) );
            _siteSetting.EnableEmail = cvt.ToBool( getVal( dic, "EnableEmail" ) );

            _siteSetting.CloseComment = cvt.ToBool( getVal( dic, "CloseComment" ) );

            _siteSetting.setValueAll( dic );
        }

        private String getVal( Dictionary<String, String> dic, String key ) {
            String val;
            dic.TryGetValue( key, out val );
            return val;
        }


        internal static readonly String siteconfigAbsPath = getSiteConfigAbsPath();

        private static String getSiteConfigAbsPath() {
            return PathHelper.Map( strUtil.Join( cfgHelper.ConfigRoot, "site.config" ) );
        }


    }
}
