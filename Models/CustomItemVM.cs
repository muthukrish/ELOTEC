using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELOTEC.Models
{
    public class CustomItemVM
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public byte IsActive { get; set; }
        public byte iscustom { get; set; }
        public byte RegStatus { get; set; }
        public int? RegistrationId { get; set; }
    }
}
