using Microsoft.Extensions.Configuration;
using ZaloPayDemo.ZaloPay.Crypto;

namespace ZaloPayDemo.ZaloPay.Models
{
    public class RefundData
    {
        public string Appid { get; set; }
        public string Zptransid { get; set; }
        public long Amount { get; set; }
        public string Description { get; set; }
        public long Timestamp { get; set; }
        public string Mrefundid { get; set; }
        public string Mac { get; set; }

        public IConfiguration Configuration;

        public RefundData(IConfiguration configuration, long amount, string zptransid, string description = "")
        {
            Configuration = configuration;
            Appid = Configuration["ZaloPay:Appid"];
            Zptransid = zptransid;
            Amount = amount;
            Description = description;
            Mrefundid = ZaloPayHelper.GenTransID(Configuration);
            Timestamp = Util.GetTimeStamp();
            Mac = ComputeMac();
        }

        public string GetMacData()
        {
            return Appid + "|" + Zptransid + "|" + Amount + "|" + Description + "|" + Timestamp;
        }

        public string ComputeMac()
        {
            return HMACHelper.Compute(ZaloPayHMAC.HMACSHA256, Configuration["ZaloPay:Key1"], GetMacData());
        }
    }
}