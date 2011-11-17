/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

namespace wojilu.Common.Money.Domain {

    public interface ICurrency {

        int Id { get; set; }
        String Guid { get; set; }

        String Name { get; set; }
        int InitValue { get; set; }
        String Unit { get; set; }
        int IsShow { get; set; }

        int CanDeal { get; set; }
        int CanDelete { get; set; }
        int CanRate { get; set; }

    }
}

