using Transivo.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transivo.Model.Models
{
    public class Message : BaseEntity
    {
       
        public string Detail { get; set; }

        //Keys
        public int CompanyID { get; set; }
        public int UserID { get; set; }

        //Nav Props
        public virtual Company Company { get; set; }
        public virtual User User { get; set; }

    }
}
