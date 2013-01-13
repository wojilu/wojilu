/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using wojilu.Config;

namespace wojilu.Common.Microblogs {

    /// <summary>
    /// 微博配置
    /// </summary>
    public class MicroblogAppSetting : SettingBase<MicroblogAppSetting> {

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


        private String _metaTitle;

        public String MetaTitle {
            get {
                if (strUtil.IsNullOrEmpty( _metaTitle )) return lang.get( "microblog" );
                return _metaTitle;
            }
            set { _metaTitle = value; }
        }

        public String MetaKeywords { get; set; }
        public String MetaDescription { get; set; }



    }
}

