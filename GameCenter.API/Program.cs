
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using GameCenter.Domain;
using System.Text.Json.Serialization;
using GameCenter.Domain.Models.Items.Entities;
using System.Text.Json;
using GameCenter.Domain.Models.Games.Entities;

const string JwtSecret = "2bfa15feba1b91f5f104342af1ebb4246241713c7843ad39579146862c887eed";

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; // IGNORA RECURSIVIDADE
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    options.JsonSerializerOptions.Converters.Add(new ItemConverter());
    options.JsonSerializerOptions.IncludeFields = true;
    options.JsonSerializerOptions.UnknownTypeHandling = JsonUnknownTypeHandling.JsonElement;
}); ;
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthorization(x =>
{
    x.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
            .RequireAuthenticatedUser()
            .Build();
    x.AddPolicy(JwtBearerDefaults.AuthenticationScheme, new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
        .RequireAuthenticatedUser()
        .Build());
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSecret))
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];
                            var path = context.HttpContext.Request.Path;
                            // If token is not found in query string, check the Authorization header
                            if (string.IsNullOrEmpty(accessToken) && context.Request.Headers.ContainsKey("Authorization"))
                            {
                                var authorizationHeader = context.Request.Headers["Authorization"].ToString();

                                // Check if it starts with "Bearer " and remove it
                                if (authorizationHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                                {
                                    accessToken = authorizationHeader["Bearer ".Length..]; // Remove the "Bearer " prefix
                                }
                            }

                            // Assign the token for further processing
                            context.Token = accessToken;

                            return Task.CompletedTask;
                        }
                    };
                });
builder.Services.AddSignalR();
builder.Services.InjectDomain();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();



app.UseCors(x =>
{
    x.SetIsOriginAllowed((origin) => true)
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials();
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<RoomHub>("/room");

app.Run();


static string GenerateToken(string userId, int expireMinutes = 60)
{
    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSecret));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var claims = new[]
    {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

    var token = new JwtSecurityToken(
        claims: claims,
        expires: DateTime.UtcNow.AddMinutes(expireMinutes),
        signingCredentials: creds);

    return new JwtSecurityTokenHandler().WriteToken(token);
}

class ItemConverter : JsonConverter<Item>
{
    public override Item Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using JsonDocument doc = JsonDocument.ParseValue(ref reader);
        if (!doc.RootElement.TryGetProperty("Type", out var typeProp))
            throw new JsonException("Missing Type discriminator");

        string type = typeProp.GetString();

        return type switch
        {
            "Weapon" => doc.RootElement.Deserialize<Game>(options),
            "Armor" => doc.RootElement.Deserialize<ServerItem>(options),
            _ => throw new JsonException($"Unknown type: {type}")
        };
    }

    public override void Write(Utf8JsonWriter writer, Item value, JsonSerializerOptions options)
    {
        var type = value.GetType().Name;
        using var jsonDoc = JsonDocument.Parse(JsonSerializer.Serialize(value, value.GetType(), options));
        writer.WriteStartObject();
        writer.WriteString("Type", type); // Discriminator

        foreach (var prop in jsonDoc.RootElement.EnumerateObject())
        {
            prop.WriteTo(writer);
        }

        writer.WriteEndObject();
    }
}
