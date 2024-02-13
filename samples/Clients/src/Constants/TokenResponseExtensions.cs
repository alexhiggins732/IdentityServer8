using IdentityModel;
using IdentityModel.Client;
using System.Text;
using System.Text.Json;

namespace Clients
{
    public static class TokenResponseExtensions
    {
        public static void Show(this TokenResponse response)
        {
            if (!response.IsError)
            {
                "Token response:".ConsoleGreen();
                Console.WriteLine(response.Json);

                if (response.AccessToken.Contains("."))
                {
                    "\nAccess Token (decoded):".ConsoleGreen();

                    var parts = response.AccessToken.Split('.');
                    var header = parts[0];
                    var claims = parts[1];

                    var headerJson = Encoding.UTF8.GetString(Base64Url.Decode(header));
                    var claimsJson = Encoding.UTF8.GetString(Base64Url.Decode(claims));
                    Console.WriteLine(JsonSerializer.Deserialize<JsonElement>(headerJson));
                    Console.WriteLine(JsonSerializer.Deserialize<JsonElement>(claimsJson));
                }
            }
            else
            {
                if (response.ErrorType == ResponseErrorType.Http)
                {
                    "HTTP error: ".ConsoleGreen();
                    Console.WriteLine(response.Error);
                    "HTTP status code: ".ConsoleGreen();
                    Console.WriteLine(response.HttpStatusCode);
                }
                else
                {
                    "Protocol error response:".ConsoleGreen();
                    Console.WriteLine(response.Raw);
                }
            }
        }
    }
}
