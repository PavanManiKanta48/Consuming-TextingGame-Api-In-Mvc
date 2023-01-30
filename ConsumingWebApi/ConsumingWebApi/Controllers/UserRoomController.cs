using ConsumingWebApi.Models;
using ConsumingWebApi.Models.UserRoom;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace ConsumingWebApi.Controllers
{
    public class UserRoomController : Controller
    {
        string Baseurl = "https://localhost:44388/api/";
        private BaseResponse response;
        // GET: HomeController/Details/5
        
        public ActionResult GetUserRoom()
        {
            return View();
        }
        [HttpGet]
        public async Task<ActionResult> GetUserRoom(int id)
        {
            List<GetUserRoom> EmpInfo = new List<GetUserRoom>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync("UserRoom/GetUsersRoom?roomId=" + id);

                if (Res.IsSuccessStatusCode)
                {
                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;
                    EmpInfo = JsonConvert.DeserializeObject<List<GetUserRoom>>(EmpResponse)!;
                }
                return View(EmpInfo);
            }
        }

        // GET: HomeController/Create
        public ActionResult AddUsersToRoom()
        {
            return View();
        }

        // POST: HomeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddUsersToRoom(CreateUserRoom create)
        {
            try
            {

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);
                    //HTTP POST
                    var postTask = client.PostAsJsonAsync<CreateUserRoom>("UserRoom/AddUserToRoom", create);
                    postTask.Wait();
                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index", "Message");
                    }
                }

                ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

                return View(create);
            }
            catch
            {
                return View();
            }
        }

        // GET: HomeController/Delete/5
        public ActionResult DeleteUsersFromRoom(int RoomId, int UserId)
        {
            //RoomId = 2;
            HttpContext.Session.SetString(Constants.RoomId, RoomId.ToString());
            HttpContext.Session.SetString(Constants.UserId, UserId.ToString());
            return View();
        }

        //POST: UserRoomController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteUsersFromRoom(DeleteUserRoom delete)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var roomid = Convert.ToInt32(HttpContext.Session.GetString(Constants.RoomId));
                    var userid = Convert.ToInt32(HttpContext.Session.GetString(Constants.UserId));

                    delete.UserId = new int[] { userid };
                    delete.RoomId = roomid;
                    client.BaseAddress = new Uri("https://localhost:44388/api/UserRoom/Delete");
                    HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Delete, client.BaseAddress);
                    req.Content = new StringContent(JsonConvert.SerializeObject(delete), Encoding.UTF8, "application/json");
                    var postJob = client.SendAsync(req);
                    postJob.Wait();
                    //var response =await client.SendAsync(request);

                    //var deleteTask = client.DeleteAsync("");
                    //deleteTask.Wait();

                    var result = postJob.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction("GetUserRoom");
                    }
                }

                return RedirectToAction("GetUserRoom");
            }
            catch
            {
                return View();
            }
        }
    }
}
