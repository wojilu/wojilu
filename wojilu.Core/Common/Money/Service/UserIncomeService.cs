/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using wojilu.Members.Users.Domain;
using wojilu.Members.Sites.Service;
using wojilu.Common.MemberApp;
using wojilu.Common.Money.Domain;
using wojilu.Common.MemberApp.Interface;
using wojilu.Common.Money.Interface;

namespace wojilu.Common.Money.Service {

    public class UserIncomeService : IUserIncomeService {

        private IMemberAppService _appService;
        private ICurrencyService _currencyService;
        private SiteRoleService _roleService;

        public virtual IMemberAppService appService {
            get { return _appService; }
            set { _appService = value; }
        }

        public virtual ICurrencyService currencyService {
            get { return _currencyService; }
            set { _currencyService = value; }
        }

        public virtual SiteRoleService roleService {
            get { return _roleService; }
            set { _roleService = value; }
        }

        public UserIncomeService() {
            currencyService = new CurrencyService();
            roleService = new SiteRoleService();
        }

        //----------------------------------------------------------------------


        public virtual List<UserIncome> GetUserIncome( int userId ) {
            return db.find<UserIncome>( "UserId=" + userId ).list();
        }

        public virtual DataPage<UserIncomeLog> GetUserIncomeLog( int userId, int currencyId ) {
            // KeyCurrency: currencyId==0  
            if (currencyId >= 0) {
                return db.findPage<UserIncomeLog>( "UserId=" + userId + " and CurrencyId=" + currencyId );
            }
            else {
                return db.findPage<UserIncomeLog>( "UserId=" + userId );
            }
        }

        public virtual UserIncome GetUserIncome( int userId, int currencyId ) {
            UserIncome income = db.find<UserIncome>( "UserId=" + userId + " and CurrencyId=" + currencyId ).first();
            if (income == null) {
                income = new UserIncome();
                income.UserId = userId;
                income.CurrencyId = currencyId;
                db.insert( income );
            }
            return income;
        }


        //-------------------------------- 增加收入 --------------------------------------

        public virtual Boolean HasEnoughKeyIncome( int userId, int income ) {
            UserIncome keyIncome = GetUserIncome( userId, KeyCurrency.Instance.Id );
            return keyIncome.Income >= income;
        }

        public virtual void AddKeyIncome( int userId, int income, String msg ) {
            User user = User.findById( userId );
            AddKeyIncome( user, income, msg );
        }

        public virtual void AddKeyIncome( User user, int income, String msg ) {
            addKeyIncome( user, income, msg );
        }

        private void addKeyIncome( User user, int income, String msg ) {

            // 更新用户当前货币的收入
            int totalIncome = UpdateUserIncome( user, KeyCurrency.Instance.Id, income, msg );

            user.Credit = totalIncome; // cache income
            db.update( user, "Credit" );

            // 检查是否晋级
            int newRankId = roleService.GetRankByCredit( user.Credit ).Id;
            if (user.RankId != newRankId) {
                user.RankId = newRankId;
                db.update( user, "RankId" );
            }
        }

        //--------------------------

        public virtual void AddIncome( User user, int currencyId, int income, String msg ) {
            if (currencyId == KeyCurrency.Instance.Id) {
                addKeyIncome( user, income, msg );
            }
            else {
                UpdateUserIncome( user, currencyId, income, msg );
            }
        }

        public virtual void AddIncome( User user, int actionId, String msg ) {

            // 添加基础货币：积分
            KeyIncomeRule keyRule = KeyIncomeRule.GetByAction( actionId );
            if (keyRule != null) {
                addKeyIncome( user, keyRule.Income, msg );
            }

            // 其他货币计算
            IList rules = currencyService.GetRulesByAction( actionId );
            foreach (IncomeRule rule in rules) {
                if (rule.Income != 0) {
                    UpdateUserIncome( user, rule.CurrencyId, rule.Income, msg );
                }
            }
        }

        public virtual void AddIncomeReverse( User user, int actionId, String msg ) {
            // 添加基础货币：积分
            KeyIncomeRule keyRule = KeyIncomeRule.GetByAction( actionId );
            if (keyRule != null) {
                addKeyIncome( user, -keyRule.Income, msg );
            }

            // 其他货币计算
            IList rules = currencyService.GetRulesByAction( actionId );
            foreach (IncomeRule rule in rules) {
                if (rule.Income != 0) {
                    UpdateUserIncome( user, rule.CurrencyId, -rule.Income, msg );
                }
            }
        }

        //-------------------------------------- 初始化 -----------------------------------------

        public virtual void InitUserIncome( User user ) {
            if (KeyCurrency.Instance.InitValue > 0) {
                insertInitIncome( user, KeyCurrency.Instance );
            }
            IList currencyAll = currencyService.GetCurrencyAll();
            foreach (Currency currency in currencyAll) {
                if (currency.InitValue > 0) {
                    insertInitIncome( user, currency );
                }
            }
        }

        private void insertInitIncome( User user, ICurrency c ) {
            UserIncome income = new UserIncome();
            income.UserId = user.Id;
            income.CurrencyId = c.Id;
            income.Income = c.InitValue;
            InsertIncome( income );

            // save log
            UserIncomeLog incomeLog = new UserIncomeLog();
            incomeLog.UserId = user.Id;
            incomeLog.CurrencyId = c.Id;
            incomeLog.Income = c.InitValue;
            incomeLog.Note = "注册奖励";
            incomeLog.insert();
        }

        //-------------------------------------- 头像下面显示部分 -----------------------------------------


        public virtual IList GetIncomeList( String userIds ) {
            IList incomeList = db.find<UserIncome>( " UserId in (" + userIds + ") and CurrencyId in (" + this.getShowIds() + ")" ).list();
            IList results = new ArrayList();
            string[] arrUserId = userIds.Split( new char[] { ',' } );
            foreach (String oneId in arrUserId) {
                int userId = cvt.ToInt( oneId );
                IList incomeByUser = this.getUserShowIncomeList( userId, incomeList );
                foreach (UserIncome income in incomeByUser) {
                    results.Add( income );
                }
            }
            return results;
        }

        private String getShowIds() {
            StringBuilder builder = new StringBuilder();
            IList showCurrencyList = this.getShowCurrencyList();
            foreach (ICurrency currency in showCurrencyList) {
                if (currency.IsShow == 1) {
                    builder.Append( currency.Id );
                    builder.Append( "," );
                }
            }
            return builder.ToString().TrimEnd( new char[] { ',' } );
        }

        private IList getShowCurrencyList() {
            IList list = new ArrayList();
            if (KeyCurrency.Instance.IsShow == 1) {
                list.Add( KeyCurrency.Instance );
            }
            IList currencyAll = currencyService.GetCurrencyAll();
            foreach (Currency currency in currencyAll) {
                if (currency.IsShow == 1) {
                    list.Add( currency );
                }
            }
            return list;
        }

        private IList getUserShowIncomeList( int userId, IList incomeList ) {
            IList showCurrencyList = this.getShowCurrencyList();
            IList results = new ArrayList();

            // 每种要显示的货币都要有收入，即时数据库中没有
            foreach (ICurrency currency in showCurrencyList) {
                results.Add( this.getIncomeFromList( userId, currency, incomeList ) );
            }
            return results;
        }

        private UserIncome getIncomeFromList( int userId, ICurrency c, IList incomeList ) {
            foreach (UserIncome income in incomeList) {
                if ((income.UserId == userId) && (income.CurrencyId == c.Id)) {
                    return income;
                }
            }
            UserIncome results = new UserIncome();
            results.UserId = userId;
            results.CurrencyId = c.Id;
            results.Income = 0;
            return results;
        }



        //-------------------------------------- tools -----------------------------------------

        public virtual void UpdateUserIncome( UserIncome userIncome ) {
            db.update( userIncome, "Income" );
        }

        public virtual void InsertIncome( UserIncome income ) {
            db.insert( income );
        }

        public virtual int UpdateUserIncome( User user, int currencyId, int income, String msg ) {
            UserIncome userIncome = this.GetUserIncome( user.Id, currencyId );
            userIncome.Income += income;
            this.UpdateUserIncome( userIncome );

            // save log
            UserIncomeLog incomeLog = new UserIncomeLog();
            incomeLog.UserId = user.Id;
            incomeLog.CurrencyId = currencyId;
            incomeLog.Income = income;
            incomeLog.Note = msg;
            incomeLog.insert();

            return userIncome.Income; // return total income
        }

    }

}
