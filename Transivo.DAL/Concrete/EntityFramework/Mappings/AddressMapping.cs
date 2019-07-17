﻿using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transivo.Model.Models;

namespace Transivo.DAL.Concrete.EntityFramework.Mappings
{
    public class AddressMapping:EntityTypeConfiguration<Address>
    {
        public AddressMapping()
        {
            Property(a => a.Name).HasMaxLength(50);
            Property(a => a.AddresssDetail).HasMaxLength(255);
        }
    }
}
