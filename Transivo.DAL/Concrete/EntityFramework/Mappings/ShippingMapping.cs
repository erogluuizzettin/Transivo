using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transivo.Model.Models;

namespace Transivo.DAL.Concrete.EntityFramework.Mappings
{
    public class ShippingMapping : EntityTypeConfiguration<Shipping>
    {
        public ShippingMapping()
        {
            HasRequired(a => a.PayType)
                .WithMany(a => a.Shippings)
                .HasForeignKey(a => a.PayTypeID);
        }
    }
}
