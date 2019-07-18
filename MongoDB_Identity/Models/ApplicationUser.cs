using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;

namespace MongoDB_Identity.Models
{
    [CollectionName("Users")]
    public class ApplicationUser : MongoIdentityUser
    {
    }

    [CollectionName("Roles")]
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
