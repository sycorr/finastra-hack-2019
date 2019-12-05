﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Finastra.Hackathon.Finastra;
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

        public ActionResult proposeLoanAdjustment()
        {
            SimulationConfiguration.ProposedAmoritizationTable = new FusionAPIClient().GetAmortizationAprTable();
            SimulationConfiguration.LenderProposedAction = true;
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            if (String.IsNullOrWhiteSpace(username))
                return RedirectToAction("Login", "Authorization");

            var identity =
                StaticData.Lenders.FirstOrDefault(x => x.Username.Equals(username, StringComparison.OrdinalIgnoreCase));

            if (identity == null)
                identity = StaticData.BusinessOwner;

            HttpContext.Session.SetString("Id", identity.Id);
            HttpContext.Session.SetString("Role", identity.IsLender ? "Lender" : "Customer");
            HttpContext.Session.SetString("Name", identity.Name);
            HttpContext.Session.SetString("Username", identity.Username);

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
