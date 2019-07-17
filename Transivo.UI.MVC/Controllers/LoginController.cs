using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Transivo.BLL.Abstract;
using Transivo.Model.Models;
using Transivo.UI.MVC.Models;

namespace Transivo.UI.MVC.Controllers
{
    public class LoginController : Controller
    {
        IUserService _userService;
        IAdminService _adminService;

        public LoginController(IUserService userService, IAdminService adminService)
        {
            _userService = userService;
            _adminService = adminService;
        }

        public ActionResult UserLogin()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult USerLogin(LoginViewModel loginModel)
        {
            if (ModelState.IsValid)
            {
                User user = _userService.GetUserByUserNameAndPassword(loginModel.UserName, loginModel.Password);
                if (user != null && !user.IsActive)
                {
                    ViewBag.hata = "Üyeliğinizi aktifleştirmediniz!";
                    return View();
                }
                else if (user != null && user.IsActive)
                {
                    Session["user"] = user;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.hata = "Böyle Bir Kullanıcı Bulunamadı";
                    return View();
                }
            }
            else
            {
                return View();
            }
        }

        public ActionResult AdminLogin()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdminLogin(LoginViewModel loginModel)
        {
            if (ModelState.IsValid)
            {
                Admin admin = _adminService.GetAdminByUserNameAndPassword(loginModel.UserName, loginModel.Password);
                if (admin != null)
                {
                    Session["admin"] = admin;
                    return RedirectToAction("AdminHome", "Admin");
                }
                else
                {
                    ViewBag.hata = "Böyle Bir Kullanıcı Bulunamadı";
                    return View();
                }
            }
            else
            {
                return View();
            }
        }
    }
}