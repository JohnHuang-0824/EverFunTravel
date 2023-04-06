using EverFunTravel.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelRepository;
using TravelRepository.Table;

namespace EverFunTravel.Controllers
{
    [AllowAnonymous]
    public class AccountController : _BaseController<AccountController>
    {
        [HttpGet]
        public IActionResult Login()
        {
            AccountLoginModel model = new AccountLoginModel();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Login(AccountLoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await DbHelper.GetUserAsync(model.Account, model.Password);
            if (user != null)
            {
                LoginUser = GetUser(user.CustomerId);
                await SetUser(LoginUser.CustomerId);
                HttpContext.Response.Cookies.Append("UserId", LoginUser.CustomerId
                , new CookieOptions() { MaxAge = TimeSpan.FromHours(3) });
                isExpired = false;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                AddErrorMessage("無效的帳號或密碼!");
                return View(model);
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Create()
        {
            AccountCreateModel model = new AccountCreateModel();
            return View(model);
        }
        [HttpPost]
        public IActionResult Create(AccountCreateModel model)
        {
            ModelState.Remove("CustomerId");
            ModelState.Remove("CustomerEName");
            ModelState.Remove("CustomerSex");
            ModelState.Remove("CustomerBirth");
            ModelState.Remove("CustomerAddress");
            ModelState.Remove("CustomerHeadImg");

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            string err = "";
            model.Add(DbHelper, out err);
            if (!string.IsNullOrEmpty(err))
            {
                AddErrorMessage(err);
                return View(model);
            }
            return View(model);
        }
    }
}
