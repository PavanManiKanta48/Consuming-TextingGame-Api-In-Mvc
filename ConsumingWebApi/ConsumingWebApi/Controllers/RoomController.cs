using ConsumingWebApi.Models.Model;
using ConsumingWebApi.Models.Room;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace ConsumingWebApi.Controllers
{
    public class RoomController : Controller
    {
        string Baseurl = "https://localhost:44388/api/";
        public async Task<ActionResult> RoomDetails(int id)
        {
            List<GetRoomResponse> EmpInfo = new List<GetRoomResponse>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync("Room?userId=" + id);

                if (Res.IsSuccessStatusCode)
                {
                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;
                    EmpInfo = JsonConvert.DeserializeObject<List<GetRoomResponse>>(EmpResponse)!;
                }
                return View(EmpInfo);
            }
        }


        public ActionResult CreateRoom()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateRoom(CreateRoom create)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                //HTTP POST
                var postTask = client.PostAsJsonAsync<CreateRoom>("Room/Create", create);
                postTask.Wait();
                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("RoomDetails");
                }
            }

            ModelState.AddModelError(string.Empty, "Server Error. Please check Connection string.");

            return View(create);
        }
        public ActionResult EditRoom()
        {
            return View();
        }
        [HttpPost]
        public ActionResult EditRoom(EditRoom edit)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);

                //HTTP POST
                var putTask = client.PutAsJsonAsync<EditRoom>("Room/Update", edit);
                putTask.Wait();

                var result = putTask.Result;
                if (result.IsSuccessStatusCode)
                {

                    return RedirectToAction("RoomDetails");
                }
            }
            return View(edit);
        }

    }
}
