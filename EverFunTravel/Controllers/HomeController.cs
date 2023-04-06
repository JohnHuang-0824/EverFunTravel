using EverFunTravel.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace EverFunTravel.Controllers
{
    [AllowAnonymous]
    public class HomeController : _BaseController<HomeController>
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var model = await AccountManageModel.GetCustomerInfo(DbHelper, LoginUser.CustomerId) ?? new AccountManageModel();
            model.CustomerHeadImg = String.IsNullOrEmpty(model?.CustomerHeadImg) ? "/people.png"
                : model?.CustomerHeadImg;
            return View(model);
        }
        //[HttpPost]
        //public async Task<IActionResult> _UpdStudPhoto(RecruitStudEditModel model)
        //{
        //    if (model.UploadStudPhoto != null && model.UploadStudPhoto.Length > 0)
        //    {
        //        if (model.SaveStudPhoto(DbHelper, _uploadService, LoginUser?.Uid).Result)
        //        {
        //            return JsonResult(true, "大頭照上傳成功", null);
        //        }
        //        else
        //        {
        //            return JsonResult(false, "大頭照上傳失敗", null);
        //        }
        //    }
        //    else
        //    {
        //        return JsonResult(true, "請選擇相片", null);
        //    }


        //}
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
