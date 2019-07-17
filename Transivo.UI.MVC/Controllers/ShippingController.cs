using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Transivo.BLL.Abstract;
using Transivo.UI.MVC.Models;

namespace Transivo.UI.MVC.Controllers
{
    public class ShippingController : Controller
    {
        ICompanyService _companyService;

        public ShippingController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        public ActionResult Shipping()
        {
            ViewBag.Companies = _companyService.GetAll();
            return View();
        }

        [HttpPost]
        public ActionResult Shipping(ShippingViewModel shippingViewModel)
        {
            ViewBag.Companies = _companyService.GetAll();
            return View();
        }
    }
}