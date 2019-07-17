using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transivo.BLL.Abstract;
using Transivo.DAL.Abstract;
using Transivo.Model.Models;

namespace Transivo.BLL.Concrete
{
    public class ShippingService : IShippingService
    {
        IShippingDAL _shippingDAL;

        public ShippingService(IShippingDAL shippingDAL)
        {
            _shippingDAL = shippingDAL;
        }

        public bool Add(Shipping model)
        {
            return _shippingDAL.Add(model)>0;
        }

        public bool Delete(Shipping model)
        {
            return _shippingDAL.Delete(model) > 0;
        }

        public bool Delete(int modelID)
        {
            Shipping shipping = _shippingDAL.Get(a => a.ID == modelID);
            return _shippingDAL.Delete(shipping) > 0;
        }

        public Shipping Get(int modelID)
        {
            return _shippingDAL.Get(a => a.ID == modelID);
        }

        public List<Shipping> GetAll()
        {
            return _shippingDAL.GetAll().ToList();
        }

        public bool Update(Shipping model)
        {
            return _shippingDAL.Update(model) > 0;
        }
    }
}
