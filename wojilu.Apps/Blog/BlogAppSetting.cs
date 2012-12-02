/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using wojilu.Config;

namespace wojilu.Apps.Blog {

    /// <summary>
    /// ≤©øÕ≈‰÷√
    /// </summary>
    public class BlogAppSetting : SettingBase<BlogAppSetting> {

        private String _metaTitle;

        public String MetaTitle {
            get {
                if (strUtil.IsNullOrEmpty( _metaTitle )) return lang.get( "blog" );
                return _metaTitle;
            }
            set { _metaTitle = value; }
        }

        public String MetaKeywords { get; set; }
        public String MetaDescription { get; set; }

        private int _pickDataCount;
        private int _pickImgCount;

        public int PickDataCount {
            get {
                if (_pickDataCount <= 0) return 6;
                return _pickDataCount;
            }
            set { _pickDataCount = value; }
        }

        public int PickImgCount {
            get {
                if (_pickImgCount <= 0) return 6;
                return _pickImgCount;
            }
            set { _pickImgCount = value; }
        }


        private String _blogStarColumnName;

        public String BlogStarColumnName {
            get {
                if (strUtil.IsNullOrEmpty( _blogStarColumnName )) return "≤©øÕ÷Æ–«";
                return _blogStarColumnName;
            }
            set { _blogStarColumnName = value; }
        }

        public int BlogStarUserId { get; set; }
        public String BlogStarUserTitle{ get; set; }
        public String BlogStarUserDescription { get; set; }


    }
}

