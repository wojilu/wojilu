/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Data;
using wojilu.ORM;

namespace wojilu.Common.Money.Domain {


    [Serializable]
    public class KeyIncomeRule : CacheObject {


        public static KeyIncomeRule GetByAction( int actionId ) {
            List<KeyIncomeRule> list = cdb.findAll<KeyIncomeRule>();
            foreach (KeyIncomeRule rule in list) {
                if (rule.ActionId == actionId) {
                    return rule;
                }
            }
            return null;
        }

        public int ActionId { get; set; }

        private String _actionName;
        [NotSave]
        public String ActionName {
            get {
                if (_actionName == null) {
                    UserAction action = UserAction.GetById( ActionId );
                    _actionName = action.Name;
                }
                return _actionName;
            }
        }

        public int Income { get; set; }

        [NotSave, NotSerialize]
        public String CurrencyName {
            get { return getCurrency().Name; }
        }

        [NotSave, NotSerialize]
        public String CurrencyUnit {
            get { return getCurrency().Unit; }
        }

        private ICurrency getCurrency() {
            return KeyCurrency.Instance;
        }


    }
}

