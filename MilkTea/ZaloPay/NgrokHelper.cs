using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZaloPayDemo.ZaloPay;
using System.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace ZaloPayDemo.ZaloPay
{
    public class NgrokHelper
    {
        public static string PublicUrl { get; private set; }
        
        static IConfiguration Configuration;
        public static async Task<string> Init()
        {
            //try
            //{
                var response = await HttpHelper.GetJson(Configuration["ZaloPay:NgrokTunnels"]);
                var tunnels = response["tunnels"] as JArray;
                var tunnel = tunnels[0].ToObject<Dictionary<string, object>>();
                return tunnel["public_url"].ToString();
            //}
            //catch
            //{
            //    PublicUrl = "";
            //}
        }


        public static Dictionary<string, object> CreateEmbeddataWithPublicUrl(Dictionary<string, object> embeddata)
        {
            PublicUrl = Init().Result;
            if (!string.IsNullOrEmpty(PublicUrl))
            {
                embeddata["callbackurl"] = PublicUrl + "/Callback";
            }
            return embeddata;
        }

        public static Dictionary<string, object> CreateEmbeddataWithPublicUrl(IConfiguration configuration)
        {
            Configuration = configuration;
            return CreateEmbeddataWithPublicUrl(new Dictionary<string, object>());
        }
    }
}