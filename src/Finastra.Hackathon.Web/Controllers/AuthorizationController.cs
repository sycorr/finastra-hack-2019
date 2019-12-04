using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Finastra.Hackathon.Web.Controllers
{
    [AllowAnonymous]
    public class AuthorizationController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var bankers = new Dictionary<string, string>();
            bankers.Add("asmith", "Arday Smith");
            bankers.Add("vpi", "Venkat Pi");
            bankers.Add("nsanders", "Nicky Sanders");
            bankers.Add("dog", "Hello, this is banker dog.");

            var images = new Dictionary<string, string>();
            images.Add("asmith", "2b64c178-52ec-4c64-a776-a863c2223702");
            images.Add("vpi", "4dd87e63-ea30-4155-942a-59192cc5daff");
            images.Add("nsanders", "12");
            images.Add("dog", "359cb3fb-be1a-4e06-ab05-68992df1917f");

            HttpContext.Session.SetString("Id", "4e2c50da-ca50-4bbe-8163-902c1859f92a");
            HttpContext.Session.SetString("Role", "Banker");
            HttpContext.Session.SetString("Name", "Ryan Rogerson");

            if (bankers.ContainsKey(username))
            {
                HttpContext.Session.SetString("Id", images[username]);
                HttpContext.Session.SetString("Role", "Customer");
                HttpContext.Session.SetString("Name", bankers[username]);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username)
            };

            var userIdentity = new ClaimsIdentity(claims, "login");

            ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
            await HttpContext.SignInAsync(principal);

            return Redirect("/");
        }

        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }
    }
}
