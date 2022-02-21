using ZaloPayDemo.ZaloPay.Crypto;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using System;

namespace ZaloPayDemo.ZaloPay.Models
{
    public class OrderData
    {
        public int Appid { get; set; }
        public string Appuser { get; set; }
        public long Apptime { get; set; }
        public long Amount { get; set; }
        public string Apptransid { get; set; }
        public string Embeddata { get; set; }
        public string Item { get; set; }
        public string Mac { get; set; }
        public string Bankcode { get; set; }
        public string Description { get; set; }

        protected readonly IConfiguration Configuration;

        public OrderData(IConfiguration configuration, string appuser, long amount, object embeddata = null, object item = null, string bankcode = "")
        {
            Configuration = configuration;
            Appid = Convert.ToInt32(Configuration["ZaloPay:Appid"]);
            Appuser = appuser;
            Apptime = Util.GetTimeStamp();
            Amount = amount;
            Apptransid = ZaloPayHelper.GenTransID(Configuration);
            Embeddata = JsonConvert.SerializeObject(embeddata);
            Item = JsonConvert.SerializeObject(item);
            Mac = ComputeMac();
            Bankcode = bankcode;
            Description = "MilkTea - Thanh toán đơn hàng #" + Apptransid;
        }

        public virtual string GetMacData()
        {
            return Appid + "|" + Apptransid + "|" + Appuser + "|" + Amount + "|" + Apptime + "|" + Embeddata + "|" + Item;
        }

        public string ComputeMac()
        {
            return HMACHelper.Compute(ZaloPayHMAC.HMACSHA256, Configuration["ZaloPay:Key1"], GetMacData());
        }
    }

    public class QuickPayOrderData : OrderData
    {
        public string Paymentcode { get; set; }

        public QuickPayOrderData(IConfiguration configuration, string appuser, long amount, string paymentcodeRaw, object embeddata = null, object item = null)
            : base(configuration, appuser, amount, embeddata, item, "")
        {
            Paymentcode = RSAHelper.Encrypt(paymentcodeRaw, Configuration["ZaloPay:RSAPublicKey"]);
            Mac = ComputeMac();
        }

        public override string GetMacData()
        {
            return base.GetMacData() + "|" + Paymentcode;
        }
    }
}