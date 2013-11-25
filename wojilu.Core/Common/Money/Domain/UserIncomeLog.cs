/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using wojilu.ORM;

namespace wojilu.Common.Money.Domain {


    [Serializable]
    public class UserIncomeLog : ObjectBase<UserIncomeLog> {

        public long AppId { get; set; }

        public long ActionId { get; set; }

        public long UserId { get; set; }

        public long DataId { get; set; }

        public long CurrencyId { get; set; }

        public int Income { get; set; }

        [Column( Name = "Description" )]
        public String Note { get; set; }


        public long OperatorId { get; set; }

        public String OperatorName { get; set; }

        public DateTime Created { get; set; }


        [NotSave]
        public String CurrencyName {
            get {
                if (this.CurrencyId == 0) return KeyCurrency.Instance.Name;
                Currency c = cdb.findById<Currency>( this.CurrencyId );
                return c == null ? "" : c.Name;
            }
        }

    }
}

