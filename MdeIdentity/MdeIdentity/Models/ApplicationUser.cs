using AspNetCore.Identity.MongoDbCore.Models;

namespace MdeIdentity.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : MongoIdentityUser
    {
    }

    public class ApplicationRole : MongoIdentityRole
    {
        public ApplicationRole() : base()
        {
        }

        public ApplicationRole(string roleName) : base(roleName)
        {
        }
    }
}
