/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Text;
using wojilu.Web.Mvc;

namespace wojilu.Web.Controller.Admin.Credits {

    public class LayoutController : ControllerBase {

        public override void Layout() {


            set( "currencyLink", to( new CurrencyController().Index ) );
            set( "incomeRuleLink", to( new CreditController().IncomeRule ) );


        }
    }

}
