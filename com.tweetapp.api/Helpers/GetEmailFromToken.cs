﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace com.tweetapp.api.Helpers
{
    public class GetEmailFromToken
    {
        public static string GetEmail(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);
            var email = jwt.Claims.First(claim => claim.Type == ClaimTypes.Email).Value;
            return email;
        }
    }
}
