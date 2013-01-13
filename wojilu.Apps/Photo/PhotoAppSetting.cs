/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using wojilu.Config;

namespace wojilu.Apps.Photo {

    /// <summary>
    /// 相册配置
    /// </summary>
    public class PhotoAppSetting : SettingBase<PhotoAppSetting> {

        private String _metaTitle;

        public String MetaTitle {
            get {
                if (strUtil.IsNullOrEmpty( _metaTitle )) return lang.get( "photo" );
                return _metaTitle;
            }
            set { _metaTitle = value; }
        }

        public String MetaKeywords { get; set; }
        public String MetaDescription { get; set; }


    }
}

