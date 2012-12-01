/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

namespace wojilu.Members.Groups {

    /// <summary>
    /// »∫◊È≈‰÷√
    /// </summary>
    public class GroupSetting {

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

        public String MetaTitle { get; set; }
        public String MetaKeywords { get; set; }
        public String MetaDescription { get; set; }

        //--------------------------------------------------------

        public static GroupSetting Instance {
            get { return _instance; }
        }

        private static GroupSetting _instance = loadSettings();

        private static GroupSetting loadSettings() {
            return cfgHelper.Read<GroupSetting>();
        }

        public static void Save( GroupSetting s ) {
            _instance = s;
            cfgHelper.Write( _instance );
        }

    }
}

