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
    public class ContactController : Controller
    {
        IUserService _userService;
        IUserRoleService _userRoleService;
        IMessageService _messageService;
        ICompanyService _companyService;

        public ContactController(IUserService userService, IUserRoleService userRoleService, IMessageService messageService, ICompanyService companyService)
        {
            _userService = userService;
            _userRoleService = userRoleService;
            _messageService = messageService;
            _companyService = companyService;
        }

        public ActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Contact(GuestUserVM guestUser)
        {
            if (ModelState.IsValid)
            {
                User user;
                bool result = false;
                try
                {
                    user = SendMessageToCompany(guestUser);

                    result = _userService.Add(user);
                    guestUser.GuestUserID = user.ID;

                    Company currentCompany = _companyService.GetCompanyByName("Transivo");

                    Message currentMessage = new Message();
                    currentMessage.Detail = guestUser.Message;
                    currentMessage.User = _userService.Get(guestUser.GuestUserID);
                    currentMessage.Company = currentCompany;
                    bool resultMessage = _messageService.Add(currentMessage);

                    if (result && resultMessage)
                    {
                        ViewBag.result = "Mesajınız Başarılı Bir Şekilde İletildi Size En Kısa Zamanda Dönüş Yapacağız";
                        ViewBag.isSend = "Gönderildi";
                    }
                    else
                    {
                        ViewBag.result = "Bir Hata Oluştu. İlgili Alanların Doğruluğunu Kontrol Ediniz.";
                    }
                }
                catch (Exception)
                {
                    ViewBag.result = "Bir Hata Oluştu";
                }
                return View();
            }
            else
            {
                return View();
            }
        }
        
        public User SendMessageToCompany(GuestUserVM guestUser)
        {
            User user = new User();
            user.FirstName = guestUser.FirstName;
            user.LastName = guestUser.LastName;
            user.EMail = guestUser.Email;
            user.UserRole = _userRoleService.GetUserRoleByName("Guest");
            return user;
        }
    }
}