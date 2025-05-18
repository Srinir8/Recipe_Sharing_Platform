using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace RecipesharingService.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("google")]
        public IActionResult GoogleLogin()
        {
            var clientId = _configuration["Google:ClientId"];
            var redirectUri = _configuration["Google:RedirectUri"];
            var scope = "openid email profile";
            var state = Guid.NewGuid().ToString();

            var authorizationUrl = $"https://accounts.google.com/o/oauth2/v2/auth?" +
                                   $"response_type=code&client_id={Uri.EscapeDataString(clientId)}" +
                                   $"&redirect_uri={Uri.EscapeDataString(redirectUri)}" +
                                   $"&scope={Uri.EscapeDataString(scope)}&state={state}";

            return Redirect(authorizationUrl);
        }

        [HttpGet("google/callback")]
        public async Task<IActionResult> GoogleCallback([FromQuery] string code, [FromQuery] string state)
        {
            var clientId = _configuration["Google:ClientId"];
            var clientSecret = _configuration["Google:ClientSecret"];
            var redirectUri = _configuration["Google:RedirectUri"];

            using var httpClient = new HttpClient();

            var tokenRequestParams = new Dictionary<string, string>
            {
                {"code", code },
                {"client_id", clientId },
                {"client_secret", clientSecret },
                {"redirect_uri", redirectUri },
                {"grant_type", "authorization_code" }
            };

            var tokenRequestContent = new FormUrlEncodedContent(tokenRequestParams);
            var tokenResponse = await httpClient.PostAsync("https://oauth2.googleapis.com/token", tokenRequestContent);
            if (!tokenResponse.IsSuccessStatusCode)
            {
                return Unauthorized("Failed to obtain access token from Google.");
            }

            var tokenResponseContent = await tokenResponse.Content.ReadAsStringAsync();
            var tokenData = JsonSerializer.Deserialize<JsonElement>(tokenResponseContent);
            var googleAccessToken = tokenData.GetProperty("access_token").GetString();

            // Exchange Google OAuth token for a Firebase ID token.
            var firebaseIdToken = await ExchangeGoogleOAuthTokenForFirebaseIdToken(googleAccessToken);

            // Redirect back to the Angular front end with the token.
            var frontendUrl = _configuration["Frontend:RedirectUri"] ?? "http://localhost:4200/login";
            return Redirect($"{frontendUrl}?token={firebaseIdToken}");
        }

        private async Task<string> ExchangeGoogleOAuthTokenForFirebaseIdToken(string googleOAuthToken)
        {
            using var httpClient = new HttpClient();

            // Validate Google token info (optional step).
            var tokenInfoResponse = await httpClient.GetAsync($"https://oauth2.googleapis.com/tokeninfo?access_token={googleOAuthToken}");
            if (!tokenInfoResponse.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to validate Google OAuth token: {tokenInfoResponse.StatusCode}");
            }
            var tokenInfoContent = await tokenInfoResponse.Content.ReadAsStringAsync();
            var tokenInfoData = JsonSerializer.Deserialize<JsonElement>(tokenInfoContent);
            if (!tokenInfoData.TryGetProperty("email", out _))
            {
                throw new Exception("Email not found in Google OAuth token info response.");
            }

            // Compose the request to exchange Google token for a Firebase ID token.
            var requestBody = new
            {
                postBody = $"access_token={googleOAuthToken}&providerId=google.com",
                requestUri = "http://localhost",
                returnIdpCredential = true,
                returnSecureToken = true
            };
            var jsonRequest = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            // Retrieve the Firebase API key directly from configuration.
            var firebaseApiKey = _configuration["Firebase:ApiKey"];
            var firebaseExchangeUrl = $"https://identitytoolkit.googleapis.com/v1/accounts:signInWithIdp?key={firebaseApiKey}";

            var response = await httpClient.PostAsync(firebaseExchangeUrl, content);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to exchange Google OAuth token: {response.StatusCode}");
            }
            var responseContent = await response.Content.ReadAsStringAsync();
            var responseData = JsonSerializer.Deserialize<JsonElement>(responseContent);
            if (responseData.TryGetProperty("idToken", out var idToken))
            {
                return idToken.GetString();
            }
            throw new Exception("Firebase ID token not found in the response.");
        }
    }
}