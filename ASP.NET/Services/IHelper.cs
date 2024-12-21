using ASP.NET.Models;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;

namespace ASP.NET.Services
{
    public class IHelper
    {
     
        public static string GenerateToken(string Username)
        {
            var claims = new List<Claim> { new(ClaimTypes.Name, Username) };
            var jwt = new JwtSecurityToken(
                issuer: "AuthServer",
                audience: "AuthClient",
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(30)),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(
                        "aASDoOPJdsfshdfhdfsDFidASDdOv112IJDjf)@#$@#%(JIO22222211444vvnvnsDSFcpSDFokFSDdfJDdSsAdCfOISDsDgjgjgjdHvbJfObIFJhgShghjghfgddDxXOcIFJbSDloOIpJF"u8.ToArray()
                    ),
                    SecurityAlgorithms.HmacSha256
                )
            );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        public static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var exceptionDetails = exception.Message.Split('*');
            Response response;

            if (exception.Message.Contains('*'))
            {
                response = new Response { Status = exceptionDetails[0], Message = exceptionDetails[1] };
                context.Response.StatusCode = int.Parse(exceptionDetails[0]);
            }
            else
            {
                response = new Response { Status = "Error", Message = exception.Message };
                context.Response.StatusCode = 500;
            }
                
            context.Response.ContentType = "application/json";
            return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }
        
    }
   
}
