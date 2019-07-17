using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Transivo.BLL.Abstract;
using Transivo.Model.Models;
using Transivo.UI.MVC.Helpers;
using Transivo.UI.MVC.Models;

namespace Transivo.UI.MVC.Controllers
{
    public class RegisterController : Controller
    {
        IUserService _userService;
        IAdminService _adminService;
        ICompanyService _companyService;
        IAdminRoleService _adminRoleService;
        IUserRoleService _userRoleService;
        ICountryService _countryService;
        ICityService _cityService;
        IDistrictService _districtService;
        IAddressService _addressService;

        public RegisterController(IUserService userService, IAdminService adminService, ICountryService countryService, ICompanyService companyService, IAdminRoleService adminRoleService, IUserRoleService userRoleService, ICityService cityService, IDistrictService districtService, IAddressService addressService)
        {
            _userService = userService;
            _adminService = adminService;
            _countryService = countryService;
            _companyService = companyService;
            _adminRoleService = adminRoleService;
            _userRoleService = userRoleService;
            _cityService = cityService;
            _districtService = districtService;
            _addressService = addressService;
        }
        public ActionResult UserRegister()
        {
            ViewBag.Countries = _countryService.GetAll();
            return View();
        }

        [HttpPost]
        public JsonResult GetCitiesByCountry(int id)
        {
            Country currentCountry = _countryService.Get(id);
            var cities = _cityService.GetCitiesByCountry(currentCountry.ID);
            return Json(new SelectList(cities.ToArray(), "ID", "Name"), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetDistrictByCity(int id)
        {
            City currentCity = _cityService.Get(id);
            var districts = _districtService.GetDistrictsByCity(currentCity.ID);
            return Json(new SelectList(districts.ToArray(), "ID", "Name"), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserRegister(RegisterUserViewModel model)
        {
            ViewBag.Countries = _countryService.GetAll();
            try
            {
                if (model.Password != model.RePassword)
                {
                    ViewBag.Message = "Şifreler uyuşmuyor";
                    return View("UserRegister");
                }
                else
                {
                    Address address = new Address();
                    address.Name = model.AddressName;
                    address.AddresssDetail = model.AddressDetail;
                    address.District = _districtService.Get(model.DistrictID);
                    address.District.City = _cityService.Get(model.CityID);
                    address.District.City.Country = _countryService.Get(model.CountryID);
                    bool isAddressAdd = _addressService.Add(address);
                    if (!isAddressAdd)
                    {
                        ViewBag.Message = "Adres eklerken hata meydana geldi!";
                    }

                    User user = new User();
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.UserRole = _userRoleService.GetUserRoleByName("Standart");
                    user.BirthDate = model.BirthDate.Date;
                    user.EMail = model.EMail;
                    user.Password = model.Password;
                    user.UserName = model.UserName;
                    user.Phone = model.Phone;
                    user.AddressID = address.ID;
                    user.IsActive = false;
                    user.ActivationCode = Guid.NewGuid();
                    bool result = _userService.Add(user);
                    if (result)
                    {
                        result = MailHelper.SendMail(model.EMail, user.ActivationCode);
                        ViewBag.Message = result ? "Aktivasyon maili gönderilmiştir. Mailinizi kontrol ediniz." : "Aktivasyon maili gönderilemedi!!";
                    }
                    else
                    {
                        ViewBag.Message = "Kullanıcı eklerken hata meydana geldi!";
                    }
                    return RedirectToAction("UserLogin", "Login");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View("UserRegister");
            }
        }

        public ActionResult Activate(Guid code)
        {
            User user = _userService.GetUserByCode(code);
            if (user != null)
            {
                _userService.ActivateUser(user);
                ViewBag.Result = "Üyeliğiniz aktifleştirilmiştir";
                return RedirectToAction("UserLogin", "Login");
            }
            else
            {
                ViewBag.Result = "Böyle bir kullanıcı bulunamadı!";
                return RedirectToAction("USerRegister");
            }
        }

        public ActionResult AdminRegister()
        {
            ViewBag.Countries = _countryService.GetAll();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdminRegister(RegisterAdminAndCompanyViewModel model)
        {
            ViewBag.Countries = _countryService.GetAll();
            if (ModelState.IsValid)
            {
                try
                {
                    if (model.Password != model.RePassword)
                    {
                        ViewBag.Message = "Şifreler uyuşmuyor";
                        return View("AdminRegister");
                    }
                    else
                    {
                        Company company = new Company();
                        company.About = model.About;
                        company.CompanyName = model.CompanyName;
                        company.Freight = model.Freight;
                        company.LogoPath = model.LogoPath;
                        company.TaxNumber = model.TaxNumber;
                        company.Phone = model.Phone;
                        bool resultCompany = _companyService.Add(company);
                        if (!resultCompany)
                        {
                            ViewBag.Message = "Şirket eklemede hata meydana geldi!";
                        }

                        Admin admin = new Admin();
                        admin.AdminRole = _adminRoleService.GetAdminRoleByName("Company Admin");
                        admin.EMail = model.EMail;
                        admin.Password = model.Password;
                        admin.Username = model.Username;
                        admin.Company = company;
                        bool resultAdmin = _adminService.Add(admin);
                        if (!resultAdmin)
                        {
                            ViewBag.Message = "Şirket eklemede hata meydana geldi!";
                        }

                        return RedirectToAction("AdminLogin","Login");
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Message = ex.Message;
                    return View("AdminRegister");
                }
            }
            else
            {
                return View();
            }
        }
    }
}