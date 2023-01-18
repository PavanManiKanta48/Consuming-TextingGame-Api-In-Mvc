using Microsoft.AspNetCore.Mvc;

namespace ConsumingWebApi.Controllers
{
    public class Base : Controller
    {
        public const string SessionKey = "userId";
        [ApiExplorerSettings(IgnoreApi = true)]
        public string? loginID(string sessionkey)
        {
            var test = HttpContext.Session.GetString(sessionkey);
            return test;
        }
    }
}
