using ConsumingWebApi.Models;
using ConsumingWebApi.Models.User;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http.Headers;

namespace ConsumingWebApi.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        string Baseurl = "https://localhost:44388/api/";
        private BaseResponse token;
        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }

        public ActionResult index()
        {
            //ViewBag.Message = "You have clicked on Export to pdf";
            return View();
        }
        public ActionResult Details()
        {            
            return View();
        }
        public async Task<ActionResult> UserDetails()
        {
            List<GetUser> EmpInfo = new List<GetUser>();
            using (var client = new HttpClient())
            {
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
                client.BaseAddress = new Uri(Baseurl + "User/UserRegister");
                //HTTP POST
                var postTask = client.PostAsJsonAsync<RegisterUser>("UserRegister", register);
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
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Baseurl);
               // LoginRequest log = new LoginRequest();
                var postJob = client.PostAsJsonAsync<LoginRequest>("User/LogIn", user);
                postJob.Wait();
                var postResult = postJob.Result;
                var resultMessage = postResult.Content.ReadAsStringAsync().Result;
                token = JsonConvert.DeserializeObject<BaseResponse>(resultMessage)!;
                //TempData["Token"] = token;
                if (postResult.IsSuccessStatusCode)
                {
                    if (token.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        return RedirectToAction("Details");
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
        public ActionResult ForgetPwd()
        {
            return View();

        }
        [HttpPost]
        public ActionResult ForgetPwd(RegisterUser forgetPwd)
        {
            using (var client = new HttpClient())
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