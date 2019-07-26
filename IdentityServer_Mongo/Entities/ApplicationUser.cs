using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;

namespace IdentityServer_Mongo.Entities
{
    [CollectionName("Users")]
    public class ApplicationUser : MongoIdentityUser { }
}