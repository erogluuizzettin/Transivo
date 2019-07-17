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
    public class HomeController : Controller
    {
        ICountryService _countryService;

        public HomeController(ICountryService countryService)
        {
            _countryService = countryService;
        }

        public ActionResult Index()
        {
            ViewBag.Countries = _countryService.GetAll();
            ViewBag.CountriesArrive = _countryService.GetAll();
            return View();
        }

        [HttpPost]
        public ActionResult Index(IndexViewModel indexViewModel)
        {
            ViewBag.Countries = _countryService.GetAll();
            ViewBag.CountriesArrive = _countryService.GetAll();
            return View();
        }        
        
        public ActionResult SSS()
        {
            return View();
        }      
    }
}
