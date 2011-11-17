/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using wojilu.Config;
using wojilu.ORM;

namespace wojilu.Members.Sites.Domain.Settings {


    [Serializable]
    public class Setting : ObjectBase<Setting>, ISetting, ISettingValue {

        private SettingCategory _category;
        private String _dataType;
        private String _description;
        private String _title;
        private String _value;

        public Setting() {
        }

        public Setting( String title, String value, SettingCategory category, SettingType dataType ) {
            _title = title;
            _value = value;
            _category = category;
            _dataType = dataType.ToString();
        }

        public Setting( String title, String value, String description, SettingCategory category, SettingType dataType ) {
            _title = title;
            _value = value;
            _description = description;
            _category = category;
            _dataType = dataType.ToString();
        }



        public IList FindByCategory( int categoryId ) {
            return db.find<Setting>( "SettingCategory.Id=" + categoryId ).list();
        }

        public void Insert() {
            db.insert(this);
        }

        public void InsertNotReload() {
            db.insert(this);
        }

        public void Update() {
            db.update(this);
        }

        public void Update( String propertyName ) {
            db.update( this, propertyName );
        }

        [Column( Length = 10 )]
        public String DataType {
            get { return _dataType; }
            set { _dataType = value; }
        }

        public String Description {
            get { return _description; }
            set { _description = value; }
        }

        public SettingCategory SettingCategory {
            get { return _category; }
            set { _category = value; }
        }

        public String SettingValue {
            get { return _value; }
            set { _value = value; }
        }

        public String Name {
            get { return _title; }
            set { _title = value; }
        }

        public String Options { get; set; }

        [NotSave]
        public Boolean ValueBool {
            get { return cvt.ToBool( _value ); }
        }

        [NotSave]
        public int ValueInt {
            get { return cvt.ToInt( _value ); }
        }

        [NotSave]
        public String ValueString {
            get { return _value; }
        }

        [NotSave]
        public DateTime ValueTime {
            get { return cvt.ToTime( _value ); }
        }
    }
}

