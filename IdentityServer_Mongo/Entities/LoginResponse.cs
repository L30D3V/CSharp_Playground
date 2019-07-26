namespace IdentityServer_Mongo.Entities 
{
    public class LoginResponse 
    {
        public bool Authenticated { get; set; }
        public string Created { get; set; }
        public string Expiration { get; set; }
        public string AccessToken { get; set; }

        public LoginResponse() {
            Authenticated = false;
            Created = "";
            Expiration = "";
            AccessToken = "";
        }
    }
}