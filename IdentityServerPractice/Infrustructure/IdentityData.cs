using IdentityServer4.Models;
using IdentityServer4.Test;

namespace IdentityServerPractice.Infrustructure
{
    public class IdentityData
    {
        public static List<ApiResource> GetApiResources()
        {
            return new List<ApiResource> { new ApiResource("myApi", "My Api DisplayName") };
        }

        public static List<Client> GetClients()
        {
            return new List<Client> {
                new Client {
                    ClientId = "PostmanClient",
                    ClientSecrets = new[] { new Secret("secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    AllowedScopes= new[] { "MyApi" }
                }
            };
        }

        public static List<TestUser> GetTestUsers()
        {
            return new List<TestUser> { 
                new TestUser { 
                    SubjectId = "1",
                    Username= "Aro",
                    Password="password"
                } 
            };
        }
    }
}
