/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Text;

using wojilu.Apps.Forum.Domain;
using wojilu.Members.Users.Domain;
using wojilu.Common.Money.Service;
using wojilu.Common.Money.Domain;

namespace wojilu.Web.Controller.Forum {


    public class UserIncomeUtil {

        private static CurrencyService currencyService = new CurrencyService();
        private static UserIncomeService incomeService = new UserIncomeService();

        public static void AddIncomeListToUser( IList posts ) {
            IList users = new ArrayList();
            String userIds = getUserIds( posts, users );
            IList incomeList = incomeService.GetIncomeList( userIds );
            addIncomeToUsers( users, incomeList );
        }

        private static void addIncomeToUsers( IList users, IList incomeList ) {
            foreach (User member in users) {
                StringBuilder builder = new StringBuilder();
                foreach (UserIncome income in incomeList) {
                    if (income.UserId == member.Id) {
                        builder.Append( "<div>" );
                        builder.Append( currencyService.GetICurrencyById( income.CurrencyId ).Name );
                        builder.Append( ":" );
                        builder.Append( income.Income );
                        builder.Append( "</div>" );
                    }
                }
                member.IncomeInfo = builder.ToString();
            }
        }

        private static String getUserIds( IList posts, IList users ) {
            StringBuilder builder = new StringBuilder();
            ArrayList list = new ArrayList();
            foreach (ForumPost post in posts) {
                if (post.Creator == null) continue;
                users.Add( post.Creator );
                if (!list.Contains( post.Creator.Id )) {
                    builder.Append( post.Creator.Id );
                    builder.Append( "," );
                    list.Add( post.Creator.Id );
                }
            }
            return builder.ToString().TrimEnd( new char[] { ',' } );
        }
    }
}

