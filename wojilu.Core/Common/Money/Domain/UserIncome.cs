/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;

using wojilu.ORM;
using wojilu.Common.Money.Service;

namespace wojilu.Common.Money.Domain {


    [Serializable]
    public class UserIncome : ObjectBase<UserIncome>, IComparable {


        public int UserId { get; set; }
        public int CurrencyId { get; set; }
        public int Income { get; set; }

        [NotSave]
        public String CurrencyName {
            get {
                return new CurrencyService().GetICurrencyById( this.CurrencyId ).Name;
            }
        }

        public static int ByCurrency( UserIncome x, UserIncome y ) {

            if (x.CurrencyId > y.CurrencyId)
                return 1;
            else if (x.CurrencyId == y.CurrencyId)
                return 0;
            else
                return -1;

        }



    }
}

