/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using wojilu.Data;

namespace wojilu.Common.Money.Domain {


    [Serializable]
    public class KeyCurrency : CacheObject, ICurrency {

        public static KeyCurrency Instance {
            get {
                return cdb.findAll<KeyCurrency>()[0];
            }
        }

        public String Guid { get; set; }

        public int InitValue { get; set; }

        public int IsShow { get; set; }

        public String Unit { get; set; }

        public int CanDeal { get; set; }

        public int CanDelete { get; set; }

        public int CanRate { get; set; }

    }
}

