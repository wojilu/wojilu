/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using wojilu.Data;
using wojilu.ORM;

namespace wojilu.Common.Money.Domain {


    [Serializable]
    public class IncomeRule : CacheObject {

        public int ActionId { get; set; }

        public int Income { get; set; }


        private String _actionName;

        [NotSave, NotSerialize]
        public String ActionName {
            get {
                if (_actionName == null) {
                    UserAction action = UserAction.GetById( ActionId );
                    _actionName = action.Name;
                }
                return _actionName;
            }
        }

        public int CurrencyId { get; set; }


        [NotSave, NotSerialize]
        public String CurrencyName {
            get { return getCurrency().Name; }
        }

        [NotSave, NotSerialize]
        public String CurrencyUnit {
            get { return getCurrency().Unit; }
        }

        private ICurrency getCurrency() {
            return cdb.findById<Currency>( this.CurrencyId );
        }

    }
}

