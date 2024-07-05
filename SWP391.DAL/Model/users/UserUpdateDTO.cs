namespace SWP391.DAL.Model.users
{
    public class UserUpdateDTO
    {
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string FullName { get; set; }
        public int? RoleId { get; set; } // 1: Admin, 2: Staff, 3: User
        public string? Image { get; set; }
    }
}
