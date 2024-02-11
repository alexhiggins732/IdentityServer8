using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace MvcImplicit.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Secure()
        {
            return View();
        }

        public IActionResult Logout()
        {
            return new SignOutResult(new string[] { "oidc", "Cookies" });
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
