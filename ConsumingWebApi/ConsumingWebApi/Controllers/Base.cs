using Microsoft.AspNetCore.Mvc;

namespace ConsumingWebApi.Controllers
{
    public class Base : Controller
    {
        public const string SessionKey = "userId";
        public const string SessionId = "id";
        [ApiExplorerSettings(IgnoreApi = true)]
        public string? loginID(string sessionkey)
        {
            var test = HttpContext.Session.GetString(sessionkey);
            return test;
        }
        [ApiExplorerSettings(IgnoreApi = true)]
        public int? getid(string sessionid)
        {
            var test = HttpContext.Session.GetInt32(sessionid);
            return test;
        }
    }
}
