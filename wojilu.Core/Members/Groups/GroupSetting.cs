/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using wojilu.Config;

namespace wojilu.Members.Groups {

    /// <summary>
    /// 群组配置
    /// </summary>
    public class GroupSetting : SettingBase<GroupSetting> {

        private String _metaTitle;

        public String MetaTitle {
            get {
                if (strUtil.IsNullOrEmpty( _metaTitle )) return lang.get( "group" );
                return _metaTitle;
            }
            set { _metaTitle = value; }
        }

        public String MetaKeywords { get; set; }
        public String MetaDescription { get; set; }

        private int _logoWidth;
        private int _logoHeight;
        private int _templateId;

        public int LogoWidth {
            get {
                if (_logoWidth <= 0) return 200;
                return _logoWidth;
            }
            set { _logoWidth = value; }
        }

        public int LogoHeight {
            get {
                if (_logoHeight <= 0) return 200;
                return _logoHeight;
            }
            set { _logoHeight = value; }
        }

        public int TemplateId {
            get {
                if (_templateId <= 0) return 1;
                return _templateId;
            }
            set { _templateId = value; }
        }


    }
}

