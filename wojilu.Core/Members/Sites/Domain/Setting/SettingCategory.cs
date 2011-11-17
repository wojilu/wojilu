/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using wojilu;
using wojilu.ORM;
using System;
using wojilu.Web;

namespace wojilu.Members.Sites.Domain.Settings {

    [Serializable]
    public class SettingCategory : ObjectBase<SettingCategory> {

        private String _description;
        private String _name;

        public SettingCategory() {
        }

        public SettingCategory( String name ) {
            _name = name;
        }

        public SettingCategory( String name, String description ) {
            _name = name;
            _description = description;
        }

        [Column]
        public String Description {
            get {
                return _description;
            }
            set {
                _description = value;
            }
        }

        [Column( Length = 50 )]
        public String Name {
            get {
                return _name;
            }
            set {
                _name = value;
            }
        }

        //[NotSave]
        //public String Url {
        //    get {
        //        return Link.To("SystemCfg", "Edit", base.Id);
        //    }
        //}
    }
}

