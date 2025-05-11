using FirebaseAdmin.Auth;
using System.Security.Claims;

public class FirebaseAuthenticationMiddleware
{
    private readonly RequestDelegate _next;

    public FirebaseAuthenticationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var authHeader = context.Request.Headers["Authorization"].ToString();
        if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
        {
            var idToken = authHeader.Substring("Bearer ".Length).Trim();

            try
            {
                // Verify the Firebase ID token
                var decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(idToken);

                // Extract user information from the token
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, decodedToken.Uid), // Firebase UID
                    new Claim(ClaimTypes.Email, decodedToken.Claims.TryGetValue("email", out var email) ? email.ToString() : string.Empty),
                    new Claim("name", decodedToken.Claims.TryGetValue("name", out var name) ? name.ToString() : string.Empty),
                    new Claim("picture", decodedToken.Claims.TryGetValue("picture", out var picture) ? picture.ToString() : string.Empty)
                };

                var identity = new ClaimsIdentity(claims, "Firebase");
                context.User = new ClaimsPrincipal(identity);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync($"Invalid Firebase ID Token: {ex.Message}");
                return;
            }
        }

        await _next(context);
    }
}
