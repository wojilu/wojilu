/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using wojilu.ORM;

namespace wojilu.Members.Users.Domain {

    /// <summary>
    /// 自定义注册用户需要填写的项目
    /// </summary>
    [Serializable]
    public class ProfileItem : ObjectBase<ProfileItem> {

        private int _categoryId;
        private int _orderId;
        private String _name;

        //数据类型，比如“Int,String”等
        private String _type; 
        private String _defaultValue;
        private int _length;

        //值的最小长度，比如必须5个字符
        private int _minlen; 
        private int _maxlen;

        //正则表达式
        private String _regValidation;

        //是否在注册的时候显示
        private int _isShowOnReg;

        //对此项的描述
        private String _description; 

        public int CategoryId {
            get { return _categoryId; }
            set { _categoryId = value; }
        }

        public int OrderId {
            get { return _orderId; }
            set { _orderId = value; }
        }

        [Column( Length = 50 )]
        public String Name {
            get { return _name; }
            set { _name = value; }
        }

        [Column( Length = 50 )]
        public String Type {
            get { return _type; }
            set { _type = value; }
        }

        public String DefaultValue {
            get { return _defaultValue; }
            set { _defaultValue = value; }
        }

        public int Length {
            get { return _length; }
            set { _length = value; }
        }

        public int Minlen {
            get { return _minlen; }
            set { _minlen = value; }
        }

        public int Maxlen {
            get { return _maxlen; }
            set { _maxlen = value; }
        }

        public String RegValidation {
            get { return _regValidation; }
            set { _regValidation = value; }
        }

        public int IsShowOnReg {
            get { return _isShowOnReg; }
            set { _isShowOnReg = value; }
        }

        public String Description {
            get { return _description; }
            set { _description = value; }
        }
    }
}
