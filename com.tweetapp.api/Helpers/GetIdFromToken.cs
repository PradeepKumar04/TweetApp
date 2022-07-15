using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace com.tweetapp.api.Helpers
{
    public class GetIdFromToken
    {
        public static int GetId(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);
            var userId = jwt.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
            return Int32.Parse(userId);
        }
    }
}
