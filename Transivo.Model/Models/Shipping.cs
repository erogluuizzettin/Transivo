using Transivo.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transivo.Model.Models
{
    public class Shipping : BaseEntity
    {
        public DateTime ShippedDate { get; set; }
        public DateTime RequiredDate { get; set; }
        public decimal Km { get; set; }
        public string Comment { get; set; }

        //Foreign Key
        public int UserID { get; set; }
        public int CompanyID { get; set; }
        public int StartAddressID { get; set; }
        public int ArrivalAddressID { get; set; }
        public int PayTypeID { get; set; }
        public int ShipCategoryID { get; set; }

        //Nav Props
        public User User { get; set; }
        public Company Company { get; set; }
        public Address StartAddress { get; set; }
        public Address ArrivalAddress { get; set; }
        public PayType PayType { get; set; }
        public ShipCategory ShipCategory { get; set; }
    }
}
