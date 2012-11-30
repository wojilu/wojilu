/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

namespace wojilu.Members.Groups {

    /// <summary>
    /// »∫◊È≈‰÷√
    /// </summary>
    public class GroupSetting {

        public int LogoHeight {
            get { return 200; }
        }

        public int LogoWidth {
            get { return 200; }
        }

        public int TemplateId {
            get { return 1; }
        }

        public static readonly GroupSetting Instance = loadSettings();

        private static GroupSetting loadSettings() {
            return cfgHelper.Read<GroupSetting>();
        }

        public static void Save() {
            cfgHelper.Write( GroupSetting.Instance );
        }

    }
}

