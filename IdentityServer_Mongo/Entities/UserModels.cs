using System.Collections.Generic;

namespace IdentityServer_Mongo.Entities
{
    public class UserModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public List<string> UserRoles { get; set; }
    }

    public class AddRoleModel
    {
        public string Email { get; set; }
        public string Role { get; set; }
    }

    public class LoginModel 
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}