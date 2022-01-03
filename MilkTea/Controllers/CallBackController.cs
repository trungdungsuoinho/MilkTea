using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MilkTea.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZaloPayDemo.ZaloPay;
using ZaloPayDemo.ZaloPay.Models;

namespace MilkTea.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CallBackController : ControllerBase
    {
        private readonly MilkTeaContext _context;
        private readonly IConfiguration Configuration;

        public CallBackController(MilkTeaContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }

        // POST: api/CallBack
        [HttpPost]
        public IActionResult PostCallBack(CallbackRequest cbdata)
        {
            var result = new Dictionary<string, object>();

            try
            {
                var dataStr = Convert.ToString(cbdata.Data);
                var requestMac = Convert.ToString(cbdata.Mac);

                var isValidCallback = ZaloPayHelper.VerifyCallback(dataStr, requestMac);

                // kiểm tra callback hợp lệ (đến từ zalopay server)
                if (!isValidCallback)
                {
                    // callback không hợp lệ
                    result["returncode"] = -1;
                    result["returnmessage"] = "mac not equal";
                }
                else
                {
                    // thanh toán thành công
                    // merchant xử lý đơn hàng cho user
                    var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(dataStr);
                    var apptransid = data["apptransid"].ToString();

                    var orderDTO = _context.Transactions.SingleOrDefault(o => o.Apptransid.Equals(apptransid));
                    if (orderDTO != null)
                    {
                        orderDTO.Zptransid = data["zptransid"].ToString();
                        orderDTO.Channel = int.Parse(data["channel"].ToString());
                        orderDTO.Status = 1;
                        _context.SaveChanges();
                    }

                    result["returncode"] = 1;
                    result["returnmessage"] = "success";
                }
            }
            catch (Exception ex)
            {
                result["returncode"] = 0; // ZaloPay sẽ callback lại tối đa 3 lần
                result["returnmessage"] = ex.Message;                
            }
            // thông báo kết quả cho zalopay server
            return Ok(result);
        }
    }
}
