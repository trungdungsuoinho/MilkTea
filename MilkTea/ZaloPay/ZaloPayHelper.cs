using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using ZaloPayDemo.ZaloPay.Crypto;
using ZaloPayDemo.ZaloPay.Models;
using ZaloPayDemo.ZaloPay.Extension;
using Microsoft.Extensions.Configuration;

namespace ZaloPayDemo.ZaloPay
{
    public class ZaloPayHelper
    {
        private static long uid = Util.GetTimeStamp();

        static IConfiguration Configuration;

        public static bool VerifyCallback(string data, string requestMac)
        {
            try
            {
                string mac = HMACHelper.Compute(ZaloPayHMAC.HMACSHA256, Configuration["ZaloPay:Key2"], data);

                return requestMac.Equals(mac);
            } catch
            {
                return false;
            }
        }

        public static bool VerifyRedirect(Dictionary<string, object> data)
        {
            try
            {
                string reqChecksum = data["checksum"].ToString();
                string checksum = ZaloPayMacGenerator.Redirect(data);

                return reqChecksum.Equals(checksum);
            } catch
            {
                return false;
            }
        }

        public static string GenTransID(IConfiguration configuration)
        {
            Configuration = configuration;
            return DateTime.Now.ToString("yyMMdd") + "_" + Configuration["ZaloPay:Appid"] + "_" + (++uid); 
        }

        public static Task<Dictionary<string, object>> CreateOrder(Dictionary<string, string> orderData)
        {
            return HttpHelper.PostFormAsync(Configuration["ZaloPay:ZaloPayApiCreateOrder"], orderData);
        }

        public static Task<Dictionary<string, object>> CreateOrder(IConfiguration configuration, OrderData orderData)
        {
            Configuration = configuration;
            return CreateOrder(orderData.AsParams());
        }

        public static Task<Dictionary<string, object>> QuickPay(Dictionary<string, string> orderData)
        {
            return HttpHelper.PostFormAsync(Configuration["ZaloPay:ZaloPayApiQuickPay"], orderData);
        }

        public static Task<Dictionary<string, object>> QuickPay(QuickPayOrderData orderData)
        {
            return QuickPay(orderData.AsParams());
        }

        public static Task<Dictionary<string, object>> GetOrderStatus(string apptransid)
        {
            var data = new Dictionary<string, string>();
            data.Add("appid", Configuration["Zalo:Appid"]);
            data.Add("apptransid", apptransid);
            data.Add("mac", ZaloPayMacGenerator.GetOrderStatus(data));

            return HttpHelper.PostFormAsync(Configuration["ZaloPay:ZaloPayApiGetOrderStatus"], data);
        }

        public static Task<Dictionary<string, object>> Refund(Dictionary<string, string> refundData)
        {
            return HttpHelper.PostFormAsync(Configuration["ZaloPay:ZaloPayApiRefund"], refundData);
        }

        public static Task<Dictionary<string, object>> Refund(RefundData refundData)
        {
            return Refund(refundData.AsParams());
        }

        public static Task<Dictionary<string, object>> GetRefundStatus(string mrefundid)
        {
            var data = new Dictionary<string, string>();
            data.Add("appid", Configuration["Zalo:Appid"]);
            data.Add("mrefundid", mrefundid);
            data.Add("timestamp", Util.GetTimeStamp().ToString());
            data.Add("mac", ZaloPayMacGenerator.GetRefundStatus(data));

            return HttpHelper.PostFormAsync(Configuration["ZaloPay:ZaloPayApiGetRefundStatus"], data);
        }

        public static Task<Dictionary<string, object>> GetBankList()
        {
            var data = new Dictionary<string, string>();
            data.Add("appid", Configuration["Zalo:Appid"]);
            data.Add("reqtime", Util.GetTimeStamp().ToString());
            data.Add("mac", ZaloPayMacGenerator.GetBankList(data));

            return HttpHelper.PostFormAsync(Configuration["ZaloPay:ZaloPayApiGetBankList"], data);
        }

        public static List<BankDTO> ParseBankList(Dictionary<string, object> banklistResponse)
        {
            var banklist = new List<BankDTO>();
            var bankMap = (banklistResponse["banks"] as JObject);

            foreach (var bank in bankMap)
            {
                var bankDTOs = bank.Value.ToObject<List<BankDTO>>();
                foreach (var bankDTO in bankDTOs)
                {
                    banklist.Add(bankDTO);
                }
            }

            return banklist;
        }
    }
}