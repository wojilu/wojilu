using System;
using System.Collections;

using wojilu;
using wojilu.ORM;
using System.Xml.Serialization;


namespace wojilu.Test.Orm.Entities {

    public class TMoney : ObjectBase<TMoney> {

        [Money]
        public decimal MyMoney { get; set; }

        [Decimal( Precision=10, Scale=5)]
        public decimal DecimalNum { get; set; }

        public double DubleNum { get; set; }
        public float SingleNum { get; set; }

    }


    public class TPostLong : ObjectBase<TPostLong> {

        public String Name { get; set; }

        public long Width { get; set; }

    }

    public class TMember : ObjectBase<TMember> {
        private string _Name;
        private int _ArticleCount;


        public string Name {
            get {
                return _Name;
            }
            set {
                this._Name = value;
            }
        }

        public int ArticleCount {
            get {
                return _ArticleCount;
            }
            set {
                this._ArticleCount = value;
            }
        }
    }

    public class TBoard : ObjectBase<TBoard> {


        private string _name;

        public string Name {
            get {
                return _name;
            }
            set {
                this._name = value;
            }
        }
    }


    [Table( "Syy_Article" )]
    [Serializable]
    public class TArticle : ObjectBase<TArticle> {

        private TMember _member;
        private TCat _cat;
        private TBoard _board;
        private string _author;
        private string _title;
        private DateTime _createtime;
        private int _channelid;
        private int _isdelete;
        private string _body;

        private int _orderId;

        public int OrderId {
            get { return _orderId; }
            set { _orderId = value; }
        }


        [XmlIgnore]
        [Column, CacheCount( "ArticleCount" )]
        public TMember Member {
            get { return _member; }
            set { _member = value; }
        }

        [XmlIgnore]
        [Column, CacheCount( "ArticleCount" )]
        public TCat Cat {
            get { return _cat; }
            set { _cat = value; }
        }

        [Column, XmlIgnore]
        public TBoard Board {
            get { return _board; }
            set { _board = value; }
        }

        public string Author {
            get { return _author; }
            set { _author = value; }
        }

        public string Title {
            get { return _title; }
            set { _title = value; }
        }

        public string Body {
            get { return _body; }
            set { _body = value; }
        }

        public DateTime CreateTime {
            get { return _createtime; }
            set { _createtime = value; }
        }

        public int ChannelId {
            get { return _channelid; }
            set { _channelid = value; }
        }

        public int IsDelete {
            get { return _isdelete; }
            set { _isdelete = value; }
        }

    }
}
