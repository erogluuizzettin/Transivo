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
    public class AdminController : Controller
    {
        IDriverService _driverService;
        IVehicleService _vehicleService;
        IMessageService _messageService;

        public AdminController(IDriverService driverService,IVehicleService vehicleService,IMessageService messageService)
        {
            _driverService = driverService;
            _vehicleService = vehicleService;
            _messageService = messageService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DriverRegister1()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DriverRegister1(DriverRegisterViewModel newDriver)
        {
            Driver driver;
            bool result = false;           
            try
            {
                driver = new Driver();
                driver.FirstName = newDriver.FirstName;
                driver.Lastname = newDriver.LastName;
                driver.TC = newDriver.TC;
                driver.BirthDate = newDriver.BirthDate;
                driver.BirthPlace = newDriver.BirthPlace;
                driver.Phone = newDriver.Phone;
                driver.EMail = newDriver.EMail;
                Admin admin = Session["admin"] as Admin;
                driver.CompanyID = admin.CompanyID;
                result = _driverService.Add(driver);
            }
            catch (Exception)
            {
                ViewBag.result = "Bir Hata Oluştu";
            }  

            if (result)
            {
                ViewBag.result = "Sürücü Kaydı Başarılı Bir Şekilde Oluşturuldu";
            }
            else
            {
                ViewBag.result = "Kayıt Oluşturulurken Bir Hata Oluştu";
            }
            return View();
        }
        
        public ActionResult VehicleRegister1()
        {
            Admin admin = Session["admin"] as Admin;
            List<Driver> drivers = _driverService.GetDriversByCompanyID(admin.CompanyID);            
            ViewBag.drivers = drivers;
            return View();
        }

        public ActionResult ListDrivers()
        {
            Admin admin = Session["admin"] as Admin;
            List<Driver> drivers = _driverService.GetDriversByCompanyID(admin.CompanyID);
            return View(drivers);
        }

        public ActionResult EditDriver(int id)
        {
            Driver driver= _driverService.Get(id);
            return View(driver);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult VehicleRegister1(VehicleRegisterViewModel newVehicleModel,int id)
        {
            bool result = false;
            try
            {
                Vehicle newVehicle= RegisterVehicle(newVehicleModel,id);
                result=_vehicleService.Add(newVehicle);
        }
            catch (Exception)
            {
                ViewBag.result = "Bir Hata Oluştu";
            }

            if (result)
            {
                ViewBag.result = "Araç Kaydı Başarılı Bir Şekilde Oluşturuldu";
            }
            else
            {
                ViewBag.result = "Kayıt Oluşturulurken Bir Hata Oluştu";
            }
            
            return View();
        }
        
        public ActionResult AdminHome()
        {
            Admin admin = Session["admin"] as Admin;
            ViewBag.messages = _messageService.GetByID(admin.Company.ID);
            List<Driver> drivers = _driverService.GetDriversByCompanyID(admin.CompanyID);
            ViewBag.allDrivers = drivers.Count;
            return View();
        }

        public Vehicle RegisterVehicle(VehicleRegisterViewModel newVehicle,int id)
        {
            Vehicle vehicle = new Vehicle();
            vehicle.Plate = newVehicle.Plate;
            vehicle.RegistrationFirstDate = newVehicle.RegistrationFirstDate;
            vehicle.RegistrationDate = newVehicle.RegistrationDate;
            vehicle.RegistrationNo = newVehicle.RegistrationNo;
            vehicle.Brand = newVehicle.Brand;
            vehicle.Type = newVehicle.Type;
            vehicle.CommercialName = newVehicle.CommercialName;
            vehicle.ModelYear = newVehicle.ModelYear;
            vehicle.CarClass = newVehicle.CarClass;
            vehicle.Genus = newVehicle.Genus;
            vehicle.Color = newVehicle.Color;
            vehicle.MotorNo = newVehicle.MotorNo;
            vehicle.ChasisNo = newVehicle.ChasisNo;
            vehicle.WeightNet = newVehicle.WeightNet;
            vehicle.WeightWeigher = newVehicle.WeightWeigher;
            vehicle.SeatsOfNumber = newVehicle.SeatsOfNumber;
            vehicle.CylinderVolume = newVehicle.CylinderVolume;
            vehicle.FuelType = newVehicle.FuelType;
            vehicle.PurposeOf = newVehicle.PurposeOf;
            vehicle.MaxLoadWeight = newVehicle.MaxLoadWeight;
            vehicle.MotorPower = newVehicle.MotorPower;
            vehicle.PowerToWeightRatio = newVehicle.PowerToWeightRatio;
            vehicle.NoterySalesDate = newVehicle.NoterySalesDate;
            vehicle.NotarySalesNo = newVehicle.NotarySalesNo;
            vehicle.NotaryName = newVehicle.NotaryName;
            vehicle.DocumentSerialNo = newVehicle.DocumentSerialNo;
            Admin admin=Session["admin"] as Admin;
            vehicle.CompanyID = admin.CompanyID;
            vehicle.Driver = _driverService.Get(id);
            return vehicle;
        }

        public ActionResult LogOut()
        {
            Session["admin"] = null;
            ViewBag.drivers = null;
            Session.Abandon();
            return View();
        }
    }
}