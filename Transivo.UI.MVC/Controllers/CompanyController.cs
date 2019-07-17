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
    public class CompanyController : Controller
    {
        ICompanyService _companyService;
        IUserService _userService;
        IMessageService _messageService;

        public CompanyController(ICompanyService companyService, IUserService userService, IMessageService messageService)
        {
            _companyService = companyService;
            _userService = userService;
            _messageService = messageService;
        }

        public ActionResult Company()
        {
            TempData["Companies"] = _companyService.GetAll();
            return View();
        }

        public ActionResult CompanyDetail()
        {
            int idm = 0;
            try
            {
                bool result = int.TryParse(this.RouteData.Values["id"].ToString(), out idm);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Companies");
            }

            if (idm == 0)
            {
                return RedirectToAction("Companies");
            }
            else
            {
                Company comp = _companyService.Get(idm);
                if (comp != null)
                {
                    CompanyViewModel cvm = new CompanyViewModel();
                    cvm.About = comp.About;
                    cvm.CompanyName = comp.CompanyName;
                    cvm.Freight = comp.Freight;
                    cvm.LogoPath = comp.LogoPath;
                    cvm.Phone = comp.Phone;
                    cvm.ID = comp.ID;
                    cvm.TaxNumber = comp.TaxNumber;

                    List<Company> companies = _companyService.GetAll();
                    List<Company> Get4Companies = new List<Company>();
                    int counter = 0;
                    for (int i = 0; i < companies.Count; i++)
                    {
                        if (comp.ID != companies[i].ID)
                        {
                            if (counter < 3)
                            {
                                Get4Companies.Add(companies[i]);
                                counter++;
                            }
                        }
                    }
                    ViewBag.Companies = Get4Companies;
                    return View(cvm);
                }
                else
                {
                    return View();
                }
            }
        }

        [HttpPost]
        public ActionResult CompanyDetail(string name, string surname, string email, string message)
        {
            bool resultEnd = false;
            User currentUser = new User();
            bool result;
            try
            {
                int idm = 0;
                result = int.TryParse(RouteData.Values["id"].ToString(), out idm);
                if (result && idm != 0)
                {
                    currentUser.FirstName = name;
                    currentUser.LastName = surname;
                    currentUser.EMail = email;
                    currentUser.UserRoleID = 2;
                    currentUser.IsActive = false;
                    bool result1 = false;
                    try
                    {
                        result1 = _userService.Add(currentUser);
                    }
                    catch (Exception ex)
                    {

                        throw;
                    }
                    finally
                    {
                        User user = new User();
                        user = _userService.GetUserByEmail(currentUser.EMail);
                        Message msg = new Message();
                        msg.IsActive = true;
                        msg.Detail = message;
                        msg.CreatedDate = DateTime.Now;
                        msg.CompanyID = idm;
                        msg.UserID = user.ID;
                        resultEnd = _messageService.Add(msg);
                    }

                }
            }
            catch (Exception ex)
            {
                RedirectToAction("Companies");
            }

            if (resultEnd)
            {
                ViewBag.Message = "Mesajınız başarı ile gönderildi.";
            }

            return RedirectToAction("CompanyDetail");
        }
    }
}