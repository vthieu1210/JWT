using Newtonsoft.Json;
using Register.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using Jose;

namespace Register.Controllers
{
    public class GenerateTokenController : ApiController
    {
        [HttpGet]
        public String GenerateToken()
        {
            ConsumerJWT consumerJwt = new ConsumerJWT();
            var headers = Request.Headers;
            try
            {
                string[] authorization = headers.GetValues("Authorization").First().Split(' ');
                string encodedString = authorization[1];
                byte[] data = Convert.FromBase64String(encodedString);
                string decodedString = Encoding.UTF8.GetString(data);
                consumerJwt.key = decodedString.Split(':')[0];

                string secret = consumerJwt.getSecret();
                if (secret == null)
                    return "Consumer not found! (Invalid username or wrong syntax)";

                var secretKey = Encoding.UTF8.GetBytes(secret);

                TimeSpan t = DateTime.UtcNow.AddMinutes(consumerJwt.exp) - new DateTime(1970, 1, 1);
                int secondsSinceEpoch = (int)t.TotalSeconds;

                var payload = new JwtPayload
                {
                    { "iss", consumerJwt.key},
                    { "exp", secondsSinceEpoch},
                };

                string tokenString = Jose.JWT.Encode(payload, secretKey, JwsAlgorithm.HS256);

                return tokenString;
            }
            catch (Exception e)
            {
                return e.GetType().Name + ": " + e.Message;
            }
        }
    }
}
