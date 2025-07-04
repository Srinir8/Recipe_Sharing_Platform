using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using MediatR;
using RecipesharingService.Contracts;
using RecipesharingService.MongoDB;
using RecipesharingService.Repositories;
using RecipesharingService.Services;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:4200",
                "https://recipe-sharing-platform-ui.onrender.com"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // Add Bearer token configuration for Firebase ID tokens
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' followed by a valid Firebase ID token (e.g., 'Bearer ey...')"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });

    // Add support for file uploads (IFormFile)
    options.OperationFilter<SwaggerFileOperationFilter>();
});

// Register Firebase Admin SDK
var firebaseCredentialsBase64 = Environment.GetEnvironmentVariable("FIREBASE_CREDENTIALS_BASE64");
var firebaseCredentialsPath = builder.Configuration.GetValue<string>("Firebase:CredentialsPath");

if (!string.IsNullOrEmpty(firebaseCredentialsPath) && File.Exists(firebaseCredentialsPath))
{
    // Use the local JSON file for development
    FirebaseApp.Create(new AppOptions
    {
        Credential = GoogleCredential.FromFile(firebaseCredentialsPath)
    });
}
else if (!string.IsNullOrEmpty(firebaseCredentialsBase64))
{
    // Decode the Base64 string to get the JSON for production
    var firebaseCredentialsJson = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(firebaseCredentialsBase64));
    FirebaseApp.Create(new AppOptions
    {
        Credential = GoogleCredential.FromJson(firebaseCredentialsJson)
    });
}
else
{
    throw new Exception("Firebase credentials are not set.");
}

// Register MediatR
builder.Services.AddMediatR(typeof(Program).Assembly);

// Register Repository and Service
builder.Services.AddSingleton<MongoDbContext>();
builder.Services.AddSingleton<RecipeRepository>();
builder.Services.AddSingleton<LikeRepository>();
builder.Services.AddSingleton<UserRepository>();
builder.Services.AddSingleton<CommentRepository>();
builder.Services.AddScoped<IRecipeService, RecipeService>();
builder.Services.AddSingleton<CommentService>();
builder.Services.AddSingleton<LikeService>();
builder.Services.AddSingleton<UserService>();

var app = builder.Build();

// Use CORS policy
app.UseCors("AllowFrontend");

if (app.Environment.IsDevelopment())
{
    // Load Google OAuth2 credentials from JSON file
    var googleCredentialsPath = "client_secret_483376807658-o18uof4c2ckoma0deo3u0gnln4himtea.apps.googleusercontent.com.json";
    var googleCredentials = JsonSerializer.Deserialize<GoogleOAuthCredentials>(File.ReadAllText(googleCredentialsPath));
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.OAuthClientId(googleCredentials.web.client_id); // Use client_id from JSON
        options.OAuthClientSecret(googleCredentials.web.client_secret); // Use client_secret from JSON
        options.OAuthUsePkce(); // Enables PKCE for secure token exchange
        options.OAuthScopes("openid", "email", "profile");
    });
}

app.UseHttpsRedirection();
app.UseMiddleware<FirebaseAuthenticationMiddleware>(); // Middleware for Firebase token validation
app.UseAuthorization();

app.MapControllers();

app.Run();

// Class to deserialize Google OAuth2 credentials
public class GoogleOAuthCredentials
{
    public Web web { get; set; }

    public class Web
    {
        public string client_id { get; set; }
        public string client_secret { get; set; }
        public string auth_uri { get; set; }
        public string token_uri { get; set; }
        public string[] redirect_uris { get; set; }
        public string[] javascript_origins { get; set; }
    }
}

// Custom Swagger Operation Filter for File Uploads
public class SwaggerFileOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var fileParameters = context.MethodInfo
            .GetParameters()
            .Where(p => p.ParameterType == typeof(IFormFile) ||
                        (p.ParameterType.IsGenericType &&
                         p.ParameterType.GetGenericTypeDefinition() == typeof(IEnumerable<>) &&
                         p.ParameterType.GetGenericArguments()[0] == typeof(IFormFile)));

        foreach (var parameter in fileParameters)
        {
            operation.RequestBody = new OpenApiRequestBody
            {
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    ["multipart/form-data"] = new OpenApiMediaType
                    {
                        Schema = new OpenApiSchema
                        {
                            Type = "object",
                            Properties =
                            {
                                [parameter.Name] = new OpenApiSchema
                                {
                                    Type = "string",
                                    Format = "binary"
                                }
                            }
                        }
                    }
                }
            };
        }
    }
}