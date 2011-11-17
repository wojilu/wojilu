/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */


using System;
using System.Collections;
using System.Reflection;

using wojilu;


namespace wojilu.Members.Sites.Domain.Settings {

    public class SettingList : CollectionBase {

        public int Add( Setting setting ) {
            return base.List.Add( setting );
        }

        public Setting this[int index] {
            get {
                return (Setting)base.List[index];
            }
            set {
                base.List[index] = value;
            }
        }
    }
}

