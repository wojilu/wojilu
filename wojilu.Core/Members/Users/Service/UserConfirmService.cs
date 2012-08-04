/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using wojilu.Members.Users.Domain;
using wojilu.Members.Users.Enum;
using wojilu.Members.Users.Interface;
using wojilu.Common.Money.Service;
using wojilu.Common.Money.Interface;
using wojilu.Common.Money.Domain;
using wojilu.Common.Msg.Service;
using wojilu.Common.Msg.Interface;

namespace wojilu.Members.Users.Service {

    public class UserConfirmService : IUserConfirmService {

        public IUserIncomeService userIncomeService { get; set; }
        public ICurrencyService currencyService { get; set; }
        public IMessageService msgService { get; set; }

        public UserConfirmService() {
            userIncomeService = new UserIncomeService();
            currencyService = new CurrencyService();
            msgService = new MessageService();
        }

        public virtual Result CanSend( User user ) {

            int maxMinutes = config.Instance.Site.UserSendConfirmEmailInterval;

            Result result = new Result();
            UserConfirm ac = db.find<UserConfirm>( "User.Id=" + user.Id+" order by Id desc" ).first();
            if (ac == null) return result;

            if (DateTime.Now.Subtract( ac.Created ).Minutes < maxMinutes) {

                result.Add( string.Format( "{0} 分钟之内，最多只能发送一次", maxMinutes ) );

                return result;

            }

            return result;
        }

        public virtual User Valid( String code ) {


            string[] arrItem = code.Split( '_' );
            if (arrItem.Length != 2) {
                return null;
            }

            int userId = cvt.ToInt( arrItem[0] );
            if (userId <= 0) {
                return null;
            }

            User user = db.findById<User>( userId );
            if (user == null) {
                return null;
            }

            String guid = arrItem[1];

            UserConfirm ac = db.find<UserConfirm>( "User.Id=:userId and Code=:code" )
                .set( "userId", userId )
                .set( "code", guid )
                .first();

            if (ac == null) return null;

            user.IsEmailConfirmed = EmailConfirm.Confirmed;

            db.update( user, "IsEmailConfirmed" );
            db.delete( ac );

            addIncomeAndMsg( user );

            return user;
        }

        private void addIncomeAndMsg( User user ) {

            int actionId = UserAction.User_ConfirmEmail.Id;

            String msgTitle = "感谢您邮件激活";
            userIncomeService.AddIncome( user, actionId, msgTitle ); // 给用户增加收入

            String msgBody = getMsgBody( user, actionId );
            msgService.SiteSend( msgTitle, msgBody, user ); // 给用户发送站内私信
        }

        private String getMsgBody( User user, int actionId ) {

            KeyIncomeRule rule = currencyService.GetKeyIncomeRulesByAction( actionId ); // 获取当前操作action收入规则。这里获取的是中心货币，你也可以使用 GetRulesByAction(actionId) 获取其他所有货币的收入规则
            int creditValue = rule.Income; // 收入的值
            String creditName = rule.CurrencyName; // 货币的名称。这里是获取的中心货币。

            String msgBody = string.Format( "{0}：<br/>您好！<br/>感谢您激活邮件，您因此获得{1}奖励，共{2}。<br/>欢迎继续参与，谢谢。", user.Name, creditName, creditValue );
            return msgBody;
        }

        public virtual void AddConfirm( UserConfirm uc ) {
            db.insert( uc );
        }
    }

}
