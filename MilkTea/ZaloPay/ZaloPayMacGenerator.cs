﻿using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using ZaloPayDemo.ZaloPay.Crypto;

namespace ZaloPayDemo.ZaloPay
{
    public class ZaloPayMacGenerator
    {
        static readonly IConfiguration Configuration;

        public static string Compute(string data, string key = "")
        {
            if (string.IsNullOrEmpty(key))
            {
                key = Configuration["ZaloPay:Key1"];
            }

            return HMACHelper.Compute(ZaloPayHMAC.HMACSHA256, key, data);
        }

        private static string CreateOrderMacData(Dictionary<string, string> order)
        {
            return order["appid"] + "|" + order["apptransid"] + "|" + order["appuser"] + "|" + order["amount"]
              + "|" + order["apptime"] + "|" + order["embeddata"] + "|" + order["item"];
        }

        public static string CreateOrder(Dictionary<string, string> order)
        {
            return Compute(CreateOrderMacData(order));
        }

        public static string QuickPay(Dictionary<string, string> order, string paymentcodeRaw)
        {
            return Compute(CreateOrderMacData(order) + "|" + paymentcodeRaw);
        }

        public static string Refund(Dictionary<string, string> data)
        {
            return Compute(data["appid"] + "|" + data["zptransid"] + "|" + data["amount"] + "|" + data["description"] + "|" + data["timestamp"]);
        }

        public static string GetOrderStatus(Dictionary<string, string> data)
        {
            return Compute(data["appid"] + "|" + data["apptransid"] + "|" + Configuration["Zalo:Key1"]);
        }

        public static string GetRefundStatus(Dictionary<string, string> data)
        {
            return Compute(data["appid"] + "|" + data["mrefundid"] + "|" + data["timestamp"]);
        }

        public static string GetBankList(Dictionary<string, string> data)
        {
            return Compute(data["appid"] + "|" + data["reqtime"]);
        }

        public static string Redirect(Dictionary<string, object> data)
        {
            return Compute(data["appid"] + "|" + data["apptransid"] + "|" + data["pmcid"] + "|" + data["bankcode"]
                + "|" + data["amount"] + "|" + data["discountamount"] + "|" + data["status"]);
        }
    }
}