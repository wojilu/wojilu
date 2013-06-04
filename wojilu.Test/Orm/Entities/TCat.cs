using System;
using wojilu;
using wojilu.ORM;
using System.Collections;

namespace wojilu.Test.Orm.Entities {

    [Serializable]
    public class TCat : ObjectBase<TCat> {
        private string _name;

        private int _articleCount;
        private int _todayTopicCount;
        private DateTime _lasttime;
        private string _lastdata;
        private int _articleReplyCount;

        [Column]
        public string Name {
            get { return _name; }
            set { _name = value; }
        }

        // =====统计缓存=====
        [Column]
        public int ArticleCount {
            get { return _articleCount; }
            set { _articleCount = value; }
        }

        [Column]
        public int TodayTopicCount {
            get { return _todayTopicCount; }
            set { _todayTopicCount = value; }
        }

        [Column]
        public DateTime ArticleLastUpdateTime {
            get { return _lasttime; }
            set { _lasttime = value; }
        }

        [Column]
        public string ArticleLastUpdateData {
            get { return _lastdata; }
            set { _lastdata = value; }
        }

        [Column]
        public int ArticleReplyCount {
            get { return _articleReplyCount; }
            set { _articleReplyCount = value; }
        }
    }
}
