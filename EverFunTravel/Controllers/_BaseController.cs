using EverFunTravel.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;
using TravelRepository;
using TravelRepository.Table;
using Microsoft.Extensions.Logging;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace EverFunTravel.Controllers
{
    [Authorize]
    public abstract class _BaseController<T> : Controller where T : _BaseController<T>
    {
        private ILogger<T>? _logger;
        protected ILogger<T> Logger
        {
            get
            {
                _logger ??= HttpContext.RequestServices.GetService<ILogger<T>>();
                if (_logger == null)
                    throw new Exception("Logger is not found.");
                return _logger;
            }
        }


        private IDatabaseHelper? _dbHelper;
        protected IDatabaseHelper DbHelper
        {
            get
            {
                _dbHelper ??= HttpContext.RequestServices.GetService<IDatabaseHelper>();
                if (_dbHelper == null)
                    throw new Exception("DbHelper is not found.");
                return _dbHelper;
            }
        }

        private static IMemoryCache? _memoryCache;
        protected IMemoryCache MemoryCache
        {
            get
            {
                _memoryCache ??= HttpContext.RequestServices.GetService<IMemoryCache>();
                if (_memoryCache == null)
                    throw new Exception("MemoryCache is not found.");
                return _memoryCache;
            }
        }

        public static Customer_UserMstr LoginUser { get; internal set; }
        public bool isExpired { get ; set; } = false;
        public static string Id { get; set; }


        public Customer_UserMstr GetUser(string uid)
        {
            var cacheKey = $"UserMstr:{uid}";
            var cacheUser = MemoryCache.Get<Customer_UserMstr>(cacheKey);
            if (cacheUser == null)
            {
                cacheUser = DbHelper.GetUserMstrById(uid);
                MemoryCache.Set(cacheKey, cacheUser, TimeSpan.FromMinutes(10));
            }
            return cacheUser;
        }
        public async Task SetUser(string uid)
        {
            HttpContext.Request.Cookies.TryGetValue("UserId", out string userId);
            if (string.IsNullOrEmpty(userId))
            {
                HttpContext.Response.Cookies.Append("UserId", uid
                , new CookieOptions() { MaxAge = TimeSpan.FromHours(3) });
                isExpired = false;
            }
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (LoginUser != null)
            {
                ViewData["LoginUserName"] = LoginUser.CustomerName;
            }
            HttpContext.Request.Cookies.TryGetValue("UserId", out string userId);
            if (string.IsNullOrEmpty(userId) && isExpired)
            {
                isExpired = true;
                context.Result = Redirect("/Account/Login");
            }
            else
            {
                LoginUser = GetUser(userId);
                Id = userId;
            }
        }
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        protected ContentResult RefreshParent(string alert = "")
        {
            var script = string.Format("<script>{0}parent.location.reload(1)</script>", !string.IsNullOrEmpty(alert) ? "alert('" + alert + "');" : "");
            return Content(script);
        }
        protected void AddErrorMessage(string err)
        {
            ModelState.AddModelError("GeneralError", err);
        }
        protected void AddImportErrorMessage(string err)
        {
            ModelState.AddModelError("GeneralImportError", err);
        }
        protected JsonResult JsonResult(bool success, string message, object content = null, string redirecturl = null)
        {
            AopResult<object> json = new AopResult<object>() { Success = success, Message = message, Content = content, Redirecturl = redirecturl };
            return Json(json);
        }


    }
}
