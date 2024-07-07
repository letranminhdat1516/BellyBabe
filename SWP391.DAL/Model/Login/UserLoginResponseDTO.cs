using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391.DAL.Model.Login
{
    public class UserLoginResponseDTO
    {
        public string Token { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string FullName { get; set; }
        public int? RoleId { get; set; }
        public string Image { get; set; }
        public bool IsFirstLogin { get; set; }
    }
}
