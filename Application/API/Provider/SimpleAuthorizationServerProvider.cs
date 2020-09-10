using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http.Cors;
using DAL;

namespace API.Provider
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated(); //   
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

          //  System.Guid customerRole = new System.Guid("5812d0c0-264f-4a9b-96bb-42dfc70538e6");

            UnitOfWork db = new UnitOfWork();

            //var user = db.UserRepository.Get(current => current.RoleId == customerRole &&
            //        current.CellNum == context.UserName && current.Password == context.Password &&
            //        current.IsActive == true).FirstOrDefault();
            var user = db.UserRepository.Get(current => current.CellNum == context.UserName && 
                                             current.Password == context.Password &&
                                             current.IsActive).FirstOrDefault();
            if (user != null)
            {

                //identity.AddClaim(new Claim("Age", "16"));

                var props = new AuthenticationProperties(new Dictionary<string, string>
                            {
                                {
                                    "usercellnumber", context.UserName
                                }
                               ,{
                                     "role", "admin"
                                }
                             });

                var ticket = new AuthenticationTicket(identity, props);
                context.Validated(ticket);

                //user.Token = ticket.ToString();
                //db.UserRepository.Update(user);
            }
            else
            {
                context.SetError("invalid_grant", "Provided username and password is incorrect");
                context.Rejected();
            }
        }
        //else
        //{
        //    context.SetError("invalid_grant", "Provided username and password is incorrect");
        //    context.Rejected();
        //}
        //return;
        //    }
        //}
    }
}