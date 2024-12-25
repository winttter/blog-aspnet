using ASP.NET.Models;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;

namespace ASP.NET.Helper
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

        private static readonly HttpClient _httpClient = new HttpClient();

        public static async Task<bool> IsImageUrlValidAsync(string imageUrl)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Head, imageUrl);
                var response = await _httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    return false;
                }

                if (response.Content.Headers.ContentType != null)
                {
                    var contentType = response.Content.Headers.ContentType.MediaType;
                    return contentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase);
                }

                return false;
            }
            catch (HttpRequestException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }

}
