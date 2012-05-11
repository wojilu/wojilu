using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Common.AppBase;
using wojilu.ORM;
using wojilu.Common.Money.Domain;
using wojilu.Data;
using wojilu.Common.Money.Interface;
using wojilu.Common.Money.Service;

namespace wojilu.Apps.Task.Domain
{

    public class TaskInfo : CacheObject, ISort, IComparable
    {
        private static Dictionary<string, string> _dayDict = new Dictionary<string, string> {
              {"永远","forever"},
              {"每天","today"},
              {"每星期","week"},
              {"每月","month"},
              {"每三月","month3"}
        };

        /// <summary>
        /// 得到任务周期
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetDayType()
        {
            return _dayDict;
        }

        public int CatgoryId {
            get;
            set;
        }

        [Column(Length=100)]
        /// <summary>
        /// 简介
        /// </summary>
        public string ShortDesc {
            get;
            set;
        }

        [Column(Length=200)]
        /// <summary>
        /// 任务目的
        /// </summary>
        public string Goal {
            get;
            set;
        }

        private string _middleIcon=string.Empty;
        private string _smallIcon = string.Empty;
        private string _largeIcon = string.Empty;

        public string MiddleIcon {
            get {
                return SysPath.Instance.Img+"mission/m_"+Id+".gif";
            }
            set { _middleIcon = value; }
        }

        public string LargeIcon {
            get {
                return SysPath.Instance.Img + "mission/l_" + Id + ".gif";
            }
            set { _largeIcon = value; }
        }

        public string SmallIcon {
            get {
                return SysPath.Instance.Img + "mission/s_" + Id + ".gif";
            }
            set { _smallIcon = value; }
        }

        /// <summary>
        /// 任务跳转地址
        /// </summary>
        public string Url {
            get;
            set;
        }

        public int CurrencyId1 { get; set; }

        public int CurrencyId2 { get; set; }

        public int Reward1 {
            get;
            set;
        }

        public int Reward2 {
            get;
            set;
        }

        public int OrderId {
            get;
            set;
        }

        /// <summary>
        /// 任务是否启用
        /// </summary>
        public int Enable {
            get;
            set;
        }

        public void updateOrderId() {
            this.update();
        }

        public string Answer { get; set; }

        /// <summary>
        /// 处理该任务的类全名
        /// </summary>
        public string TypeName { get; set; }

        private string _day = _dayDict["永远"];

        /// <summary>
        /// 任务周期类型 forever,today,week,month,month3
        /// </summary>
        public string Day
        {
            get
            {
                return _day;
            }
            set
            {
                foreach(var item in _dayDict.Values)
                {
                    if(item == value)
                    {
                        _day = value;
                        break;
                    }
                }
            }
        }

        public int CompareTo(object obj) {
            TaskInfo t = obj as TaskInfo;
            if (this.OrderId > t.OrderId) return -1;
            if (this.OrderId < t.OrderId) return 1;
            if (this.Id > t.Id) return 1;
            if (this.Id < t.Id) return -1;
            return 0;
        }

        public string GetRewardHtml() {
            string _reward = string.Empty;
            ICurrencyService service = new CurrencyService();
            Currency c1=null,c2=null;
            if (this.CurrencyId1 > 0 && Reward1>0)
                c1 = service.GetCurrencyById(this.CurrencyId1);
            if (this.CurrencyId2 > 0 && Reward2>0)
                c2 = service.GetCurrencyById(this.CurrencyId2);
            if (c1 != null) {
                _reward += string.Format("<span class='{0}'>{1}+<strong>{2}</strong></span>", c1.Name, c1.Name, this.Reward1);
            }
            if (c2 != null) {
                _reward += string.Format("<span class='{0}'>{1}+<strong>{2}</strong></span>", c2.Name, c2.Name, this.Reward2);
            }
            return _reward;
        }
    }
}
