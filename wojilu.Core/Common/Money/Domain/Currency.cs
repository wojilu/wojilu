/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using wojilu.Data;

namespace wojilu.Common.Money.Domain {

    [Serializable]
    public class Currency : CacheObject, ICurrency {

        public String Guid { get; set; }

        public String Unit { get; set; }
        public decimal ExchangeRate { get; set; }

        public int InitValue { get; set; }
        public int IsShow { get; set; }

        public int CanDeal { get; set; }
        public int CanDelete { get; set; }
        public int CanRate { get; set; }

        public static Currency GetForumCurrency() {
            return cdb.findById<Currency>( 1 );
        }

        public static Currency GetWeiWang() {
            return cdb.findById<Currency>( 2 );
        }

        public static Currency ReadPermission() {
            return cdb.findById<Currency>( 3 );
        }

        public static Currency DownloadCurrency() {
            return cdb.findById<Currency>( 4 );
        }

    }
}

