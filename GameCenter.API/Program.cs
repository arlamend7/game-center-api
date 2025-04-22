
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using GameCenter.Domain;
using GameCenter.Domain.Models.Games.Entities;
using GameCenter.Domain.Models.Games.Entities.Fileds;
using GameCenter.Domain.Models.Items.Entities;
using GameCenter.Domain.Responses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

const string JwtSecret = "2bfa15feba1b91f5f104342af1ebb4246241713c7843ad39579146862c887eed";

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; // IGNORA RECURSIVIDADE
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    options.JsonSerializerOptions.Converters.Add(new PolymorphicJsonConverter<Item>(
    knownTypes: new[] { typeof(ServerItem), typeof(Game) }
));
    options.JsonSerializerOptions.Converters.Add(new PolymorphicJsonConverter<GameOption>(
  knownTypes: new[] { typeof(NumericConfig), typeof(MultiChoiceConfig), typeof(SingleChoiceConfig), typeof(TextConfig) }
));
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
public class PolymorphicJsonConverter<TBase> : JsonConverter<TBase>
{
    private readonly Dictionary<string, Type> _typeMap;
    private readonly string _typeDiscriminator;

    public PolymorphicJsonConverter(string typeDiscriminator = "Type", params Type[] knownTypes)
    {
        _typeDiscriminator = typeDiscriminator;
        _typeMap = knownTypes.ToDictionary(
            t => t.Name,
            t => t
        );
    }

    public override TBase Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using JsonDocument document = JsonDocument.ParseValue(ref reader);
        if (!document.RootElement.TryGetProperty(_typeDiscriminator, out var typeElement))
            throw new JsonException($"Missing discriminator '{_typeDiscriminator}'");

        var typeName = typeElement.GetString();
        if (!_typeMap.TryGetValue(typeName, out var targetType))
            throw new JsonException($"Unknown type discriminator '{typeName}'");

        string rawJson = document.RootElement.GetRawText();
        return (TBase)JsonSerializer.Deserialize(rawJson, targetType, options)!;
    }

    public override void Write(Utf8JsonWriter writer, TBase value, JsonSerializerOptions options)
    {
        var type = value!.GetType();
        using var jsonDoc = JsonDocument.Parse(JsonSerializer.Serialize(value, type, options));

        writer.WriteStartObject();
        writer.WriteString(_typeDiscriminator, type.Name);

        foreach (var prop in jsonDoc.RootElement.EnumerateObject())
        {
            prop.WriteTo(writer);
        }

        writer.WriteEndObject();
    }
}
