using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using LimsBffWeb.Utils;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "Authorized",
                      policy  =>
                      {
                          policy.WithOrigins("http://0.0.0.0:5204", "http://127.0.0.1:5204")
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                      });
});

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase; // Default
    // To use PascalCase
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
});

//authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false, // Set to true if you want to validate the Issuer
        ValidateAudience = false, // Set to true if you want to validate the Audience
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("L8jR5vH1aX3kZp9QoT2yW6e4UvYmNpA7T9fKdXrPoWyQvLXs")) // Use your actual secret key
    };
    options.Events = new JwtBearerEvents
    {
        OnChallenge = context =>
        {
            // Customize the unauthorized response (401)
            context.HandleResponse();
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";
            return context.Response.WriteAsync(@"{ ""message"": ""Veuillez vous connecter"",  ""statusCode"" : 401 }");
        },
        OnForbidden = context =>
        {
            // Customize the forbidden response (403)
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            context.Response.ContentType = "application/json";
            return context.Response.WriteAsync(@"{ ""message"": ""Vous n'avez pas la permission"", ""statusCode"" : 403 }");
        }
    };
});

builder.Services.AddAuthorization(options =>
{
    // Policy for Admin
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));

    // Policy for ClieTesteurnt
    options.AddPolicy("TesteurPolicy", policy => policy.RequireRole("Testeur"));

    // Policy that allows both Admin and Testeur
    options.AddPolicy("AdminOrTesteurPolicy", policy => policy.RequireRole("Admin", "Testeur"));
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient(); // Register HttpClient here

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("Authorized");

// app.UseMiddleware<TokenValidationMiddleware>();
app.UseAuthentication(); // Add this before UseAuthorization
app.UseAuthorization();


app.UseHttpsRedirection();

app.MapControllers();

app.Run();
