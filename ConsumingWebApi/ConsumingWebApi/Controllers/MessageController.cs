using ConsumingWebApi.Models.Message;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace ConsumingWebApi.Controllers
{
    public class MessageController : Controller
    {
        string Baseurl = "https://localhost:44388/api/";
        //public IActionResult Index()
        //{
        //    return View();
        //}
        public async Task<ActionResult> GetMessages(int id)
        {
            List<GetMessages> MsgInfo = new List<GetMessages>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync("Message/GetUserMessage?RoomId=" + id);

                if (Res.IsSuccessStatusCode)
                {
                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;
                    MsgInfo = JsonConvert.DeserializeObject<List<GetMessages>>(EmpResponse)!;
                }
                return View(MsgInfo);
            }
        }
        public ActionResult AddMessageToRoom()
        {
            return View();
        }

        // POST: HomeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddMessageToRoom(CreateMessage create)
        {
            try
            {

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);
                    //HTTP POST
                    var postTask = client.PostAsJsonAsync<CreateMessage>($"Message/AddUserMessage/{create.RoomId}&Message=hard%20coded%20value", create);
                    postTask.Wait();
                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction("GetMessages");
                    }
                }

                ModelState.AddModelError(string.Empty, "Server Error. Please check Connection String.");

                return View(create);
            }
            catch
            {
                return View();
            }
        }
    }
}
