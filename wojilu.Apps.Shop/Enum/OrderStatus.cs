using System;
using System.Collections.Generic;
using System.Text;

namespace wojilu.Apps.Shop.Enum
{
    /// <summary>
    /// 交易状态枚举
    /// </summary>
    public enum OrderStatus
    {
        /// <summary>
        /// 未生效的交易
        /// </summary>
        UnStart = 0,
        /// <summary>
        /// 等待买家付款
        /// </summary>
        WAIT_BUYER_PAY = 1,
        /// <summary>
        /// 交易已创建,等待卖家确认
        /// </summary>
        WAIT_SELLER_CONFIRM_TRADE = 2,
        /// <summary>
        /// 确认买家付款中，暂勿发货
        /// </summary>
        WAIT_SYS_CONFIRM_PAY = 3,
        /// <summary>
        /// 买家已付款(或支付宝收到买家付款),请卖家发货
        /// </summary>
        WAIT_SELLER_SEND_GOODS = 4,
        /// <summary>
        /// 卖家已发货，买家确认中
        /// </summary>
        WAIT_BUYER_CONFIRM_GOODS = 5,
        /// <summary>
        /// 买家确认收到货，等待支付宝打款给卖家
        /// </summary>
        WAIT_SYS_PAY_SELLER = 6,
        /// <summary>
        /// 交易成功结束
        /// </summary>
        TRADE_FINISHED = 7,
        /// <summary>
        /// 交易中途关闭(未完成)
        /// </summary>
        TRADE_CLOSED = 8,
        /// <summary>
        /// 等待卖家同意退款
        /// </summary>
        WAIT_SELLER_AGREE = 10,
        /// <summary>
        /// 卖家拒绝买家条件，等待买家修改条件
        /// </summary>
        SELLER_REFUSE_BUYER = 11,
        /// <summary>
        /// 卖家同意退款，等待买家退货
        /// </summary>
        WAIT_BUYER_RETURN_GOODS = 12,
        /// <summary>
        /// 等待卖家收货
        /// </summary>
        WAIT_SELLER_CONFIRM_GOODS = 13,
        /// <summary>
        /// 双方已经一致，等待支付宝退款
        /// </summary>
        WAIT_ALIPAY_REFUND = 14,
        /// <summary>
        /// 支付宝处理中
        /// </summary>
        ALIPAY_CHECK = 15,
        /// <summary>
        /// 结束的退款
        /// </summary>
        OVERED_REFUND = 16,
        /// <summary>
        /// 退款成功(卖家已收到退货)
        /// </summary>
        REFUND_SUCCESS = 17,
        /// <summary>
        /// 退款关闭
        /// </summary>
        REFUND_CLOSED = 18
    }

    /// <summary>
    /// 付款状态枚举
    /// </summary>
    public enum PaymentStatus
    {
        /// <summary>
        /// 买家未操作付款
        /// </summary>
        All = -1,
        /// <summary>
        /// 买家未付款
        /// </summary>
        NotYet = 0,
        /// <summary>
        /// 买家已付款
        /// </summary>
        Prepaid = 1
    }
    /// <summary>
    /// 订单状态枚举
    /// </summary>
    public enum OrderActStatus
    {
        /// <summary>
        /// 未知状态
        /// </summary>
        All = -1,
        /// <summary>
        /// 交易关闭
        /// </summary>
        Closed = 1,
        /// <summary>
        /// 正在交易中
        /// </summary>
        InProgress = 0,
        /// <summary>
        /// 成功交易结束
        /// </summary>
        Successed = 2
    }
    /// <summary>
    /// 订单状态筛选项
    /// </summary>
    public enum OrderFilterMethod { 
        /// <summary>
        /// 所有订单
        /// </summary>
        All=0,
        /// <summary>
        /// 未付款订单
        /// </summary>
        UnPay=1,
        /// <summary>
        /// 已付款订单
        /// </summary>
        Paid=2,
        /// <summary>
        /// 未发货订单
        /// </summary>
        UnDeliver=3,
        /// <summary>
        /// 已发货订单
        /// </summary>
        Delivered=4,
        /// <summary>
        /// 未完成订单
        /// </summary>
        UnDone=5,
        /// <summary>
        /// 已完成订单
        /// </summary>
        Done=6
    }
}
