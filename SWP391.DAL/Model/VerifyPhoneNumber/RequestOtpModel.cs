using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391.DAL.Model.VerifyPhoneNumber
{
    public class RequestOtpModel
    {
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
    }
}
