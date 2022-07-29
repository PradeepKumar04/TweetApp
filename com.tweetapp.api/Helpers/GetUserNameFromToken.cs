using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace com.tweetapp.api.Helpers
{
    public class GetUserNameFromToken
    {
        public static string GetNameFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);
            var userName = jwt.Claims.First(claim => claim.Type == ClaimTypes.Name).Value;
            return userName;
        }
    }
}
