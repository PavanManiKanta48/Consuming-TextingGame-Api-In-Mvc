using ConsumingWebApi.Models;
using ConsumingWebApi.Models.User;
using Domain;
using Domain.UserModel;
using Intuit.Ipp.Core.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http.Headers;

namespace ConsumingWebApi.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly HttpClient client;
        string Baseurl = "https://localhost:44388/api/";
        public new const string SessionKey = "Token";
        private BaseResponse token;
        public new const string SessionId = "id";
        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
            client = new HttpClient();
        }

        public ActionResult index()
        {
            //ViewBag.Message = "You have clicked on Export to pdf";
            return View();
        }
        public async Task<ActionResult> Details()
        {
            //string? token = HttpContext.Session.GetString(SessionKey);
            int? id = HttpContext.Session.GetInt32(SessionId);
            UserDetailsModel users = null!;
            //var responseTask = client.GetAsync("https://localhost:44388/api/User/UserDetails");
            client.BaseAddress = new Uri(Baseurl);
            client.DefaultRequestHeaders.Clear();
            //Define request data format
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //Sending request to find web api REST service resource GetAllUsers using HttpClient
            HttpResponseMessage responseTask =await  client.GetAsync("User/UserDetails?id="+id);
           // responseTask.Wait();
           // var result = responseTask.Result;
            if (responseTask.IsSuccessStatusCode)
            {
                //var readJob = result.Content.ReadFromJsonAsync<UserDetailsModel>();
                //readJob.Wait();
                //users = readJob.Result!;
                var EmpResponse = responseTask.Content.ReadAsStringAsync().Result;
                users = JsonConvert.DeserializeObject<UserDetailsModel>(EmpResponse)!;
            }
            else
            {
                ModelState.AddModelError(string.Empty, "server error");
            }

            return View(users);
        }
        public async Task<ActionResult> UserDetails()
        {
            List<GetUser> EmpInfo = new List<GetUser>();
            string? token = HttpContext.Session.GetString(SessionKey);
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer", parameter: token);
            //Passing service base url
            client.BaseAddress = new Uri(Baseurl);
            client.DefaultRequestHeaders.Clear();
            //Define request data format
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //Sending request to find web api REST service resource GetAllUsers using HttpClient
            HttpResponseMessage Res = await client.GetAsync("User");
            //Checking the response is successful or not which is sent using HttpClient
            if (Res.IsSuccessStatusCode)
            {
                //Storing the response details recieved from web api
                var EmpResponse = Res.Content.ReadAsStringAsync().Result;
                //Deserializing the response recieved from web api and storing into the Employee list
                EmpInfo = JsonConvert.DeserializeObject<List<GetUser>>(EmpResponse)!;
            }
            //returning the employee list to view
            return View(EmpInfo);

        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ActionName("Login")]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginRequest user)
        {
            try
            {
                client.BaseAddress = new Uri(Baseurl);
                var postJob = client.PostAsJsonAsync<LoginRequest>("User/LogIn", user);
                postJob.Wait();
                var postResult = postJob.Result;
                var resultMessage = postResult.Content.ReadAsStringAsync().Result;
                var response = JsonConvert.DeserializeObject<LoginUserResponseModel>(resultMessage)!;
                token = JsonConvert.DeserializeObject<BaseResponse>(resultMessage)!;
                //TempData["Token"] = token;
                HttpContext.Session.SetString(SessionKey, token.Token!);
                HttpContext.Session.SetInt32(SessionId, Convert.ToInt32(token.id));
                //HttpContext.Session.SetString(Constants.UserId, response.userId.ToString());
                if (postResult.IsSuccessStatusCode)
                {
                    if (token.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        return RedirectToAction("Details");//"Index","Room"
                                                           // return RedirectToAction("Details");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, token.Token!);
                        return View(user);
                    }
                }
                ModelState.AddModelError(string.Empty, "server Error");
                return View(user);
            }
            catch
            {
                return View();
            }
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(RegisterUser register)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                //HTTP POST
                var postTask = client.PostAsJsonAsync<RegisterUser>("User/UserRegister", register);
                postTask.Wait();
                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("index");
                }
            }

            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

            return View(register);
        }

        public ActionResult ForgetPwd()
        {
            return View();

        }
        [HttpPost]
        public ActionResult ForgetPwd(RegisterUser forgetPwd)
        {

            client.BaseAddress = new Uri(Baseurl + "User/ForgetPassword");

            //HTTP POST
            var putTask = client.PutAsJsonAsync<RegisterUser>("ForgetPassword", forgetPwd);
            putTask.Wait();

            var result = putTask.Result;
            if (result.IsSuccessStatusCode)
            {

                return RedirectToAction("Login");
            }
            return View(forgetPwd);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}