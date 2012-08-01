/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using wojilu.ORM;

namespace wojilu.Common.Money.Domain {


    [Serializable]
    public class UserIncomeLog : ObjectBase<UserIncomeLog> {

        public int AppId { get; set; }

        public int ActionId { get; set; }

        public int UserId { get; set; }

        public int DataId { get; set; }

        public int CurrencyId { get; set; }

        public int Income { get; set; }

        [Column( Name = "Description" )]
        public String Note { get; set; }


        public int OperatorId { get; set; }

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

