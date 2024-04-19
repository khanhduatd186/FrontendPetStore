using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using WebBanThu.Areas.Admin.Models;
using WebBanThu.Models;

namespace WebBanThu.Areas.Admin.Controllers
{
    //[Authorize]
    [Area("Admin")]
    [Authorize(Roles = "ADMIN")]
    public class AdminServiceCartController : Controller
    {
        string domain = "https://localhost:7253/";
        HttpClient client = new HttpClient();

        public async Task<IActionResult> Index()
        {
            client.BaseAddress = new Uri(domain);

            var services = new List<Service_CartModel>();
            var responseMessage = client.GetAsync("api/Service_Cart/GetAllService_Cart").Result;
            
            if (responseMessage.IsSuccessStatusCode)
            {
                var data = responseMessage.Content.ReadAsStringAsync().Result;
                services = JsonConvert.DeserializeObject<List<Service_CartModel>>(data);
                foreach (var item in services)
                {
                    var jsonUser = client.GetAsync("api/Account/GetUserById/" + item.IdUser).Result;
                    var userString = jsonUser.Content.ReadAsStringAsync().Result;
                    var user = JsonConvert.DeserializeObject<SignUpModel>(userString);
                    item.Name = user?.Name ?? "";
                }
            }
            
            return View(services);
        }
    }
}