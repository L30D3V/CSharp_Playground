using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MdeIdentity.Models
{
    public class UserModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }

    public class AddRoleModel
    {
        public string Email { get; set; }
        public string Role { get; set; }
    }
}
