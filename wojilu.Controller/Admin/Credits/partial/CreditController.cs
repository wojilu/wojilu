/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Text;

using wojilu.Web.Mvc;
using wojilu.Common.Money.Domain;
using wojilu.Members.Users.Domain;

namespace wojilu.Web.Controller.Admin.Credits {

    public partial class CreditController : ControllerBase {


        private void bindIncomeRule( IList currencyList, IList actions ) {

            StringBuilder builder = new StringBuilder();
            builder.Append( "<table style='width:100%;' id='dataAdminList' cellspacing='0'>" );

            // header
            builder.Append( "<tr class='tableHeader'><td></td>" );
            builder.AppendFormat( "<td><strong>{0}</strong>(" + lang( "keyCurrency" ) + ")</td>", KeyCurrency.Instance.Name );


            foreach (ICurrency c in currencyList) {
                builder.AppendFormat( "<td>{0}</td>", c.Name );
            }
            builder.Append( "</tr>" );
            builder.Append( Environment.NewLine );

            // init value
            builder.Append( "<tr class='tableItems'>" );
            builder.Append( "<td>" + lang( "initValueWithReg" ) + "</td>" );
            builder.AppendFormat( "<td><a href='{0}' class='frmBox' title='" + lang( "editValue" ) + "'>{1}</a></td>", to( EditKeyInit ), KeyCurrency.Instance.InitValue );
            foreach (ICurrency c in currencyList) {
                builder.AppendFormat( "<td><a href='{0}' class='frmBox' title='" + lang( "editValue" ) + "'>{1}</a></td>", to( EditInit, c.Id ), c.InitValue );
            }

            // items
            foreach (UserAction a in actions) {
                builder.Append( "<tr class='tableItems'>" );
                builder.AppendFormat( "<td>{0}</td>", a.Name );

                // keyCurrency rule
                KeyIncomeRule keyrule = currencyService.GetKeyIncomeRulesByAction( a.Id );
                builder.AppendFormat( "<td><a href='{0}' class='frmBox' title='" + lang( "editValue" ) + "'>{1}</a></td>", to( EditKeyRule, keyrule.Id ), keyrule.Income );


                IList rules = currencyService.GetRulesByAction( a.Id );
                foreach (IncomeRule rule in rules) {
                    builder.AppendFormat( "<td><a href='{0}' class='frmBox' title='" + lang( "editValue" ) + "'>{1}</a></td>", to( EditRule, rule.Id ), rule.Income );
                }

                builder.Append( "</tr>" );
                builder.Append( Environment.NewLine );
            }

            set( "ruleTable", builder );
        }

        private void log( String msg, Type t ) {
            logService.Add( (User)ctx.viewer.obj, msg, "", t.FullName, ctx.Ip );
        }

    }
}

