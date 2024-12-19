using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
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
        }
   
}
