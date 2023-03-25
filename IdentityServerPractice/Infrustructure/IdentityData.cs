
using IdentityModel;

using IdentityServer4;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.Models;
using IdentityServer4.Test;

using Microsoft.VisualBasic;

using Newtonsoft.Json;

namespace IdentityServerPractice.Infrustructure
{
    public class IdentityData
    {
        public static List<IdentityServer4.EntityFramework.Entities.ApiResource> GetApiResources()
        {
            return new List<IdentityServer4.EntityFramework.Entities.ApiResource> { new IdentityServer4.EntityFramework.Entities.ApiResource { Name = "myApi", DisplayName = "My Api DisplayName" } };
        }

        public static List<IdentityServer4.EntityFramework.Entities.Client> GetClients()
        {
            return new List<IdentityServer4.EntityFramework.Entities.Client> {
                new IdentityServer4.EntityFramework.Entities.Client {
                    ClientId = "PostmanClient",
                    ClientSecrets = new List<ClientSecret> { new ClientSecret { Value = "secret".Sha256() } },
                    AllowedGrantTypes =new List<ClientGrantType> { new ClientGrantType { GrantType = IdentityServer4.Models.GrantType.ResourceOwnerPassword },  new ClientGrantType { GrantType = IdentityServer4.Models.GrantType.ClientCredentials } },
                    AllowedScopes= new List<ClientScope> { new ClientScope { Scope= "MyApi" }}
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

        public static List<IdentityServer4.EntityFramework.Entities.IdentityResource> GetIdentityResources()
        {
            return new List<IdentityServer4.EntityFramework.Entities.IdentityResource> {
                new IdentityServer4.EntityFramework.Entities.IdentityResource
                {
                     Name = IdentityServerConstants.StandardScopes.OpenId,
                    DisplayName = "Your user identifier",
                    Required = true,
                    UserClaims =new List<IdentityResourceClaim>{ new IdentityResourceClaim { Type= JwtClaimTypes.Subject }  },
                },
                new IdentityServer4.EntityFramework.Entities.IdentityResource
                {
                    Name = IdentityServerConstants.StandardScopes.Profile,
                    DisplayName = "User profile",
                    Description = "Your user profile information (first name, last name, etc.)",
                    Emphasize = true,
                    //UserClaims = IdentityServer4.IdentityServerConstants.ScopeToClaimsMapping[IdentityServerConstants.StandardScopes.Profile].ToList(),
                }
            };
        }
    }
}
