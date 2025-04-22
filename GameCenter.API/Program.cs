using System.Text;
using System.Text.Json.Serialization;
using GameCenter.API.Converters;
using GameCenter.Domain;
using GameCenter.Domain.Enums;
using GameCenter.Domain.Models.Items.Entities;
using GameCenter.Domain.Models.Items.Games.Entities;
using GameCenter.Domain.Models.Items.Games.Entities.Fileds;
using GameCenter.Utilities.Injectors.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;


var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; // IGNORA RECURSIVIDADE
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    options.JsonSerializerOptions.Converters.Add(
        PolymorphicJsonConverterFactory
            .For<Item>()
            .WithDiscriminator(x => x.Type)
            .Add<Game>(ItemTypeEnum.Game)
            .Add<ServerItem>(ItemTypeEnum.Article)
            .Build()
    );
    options.JsonSerializerOptions.Converters.Add(
        PolymorphicJsonConverterFactory
            .For<GameOption>()
            .WithDiscriminator(x => x.Type)
            .Add<NumericConfig>(FieldTypeEnum.Numeric)
            .Add<MultiChoiceConfig>(FieldTypeEnum.MultiChoice)
            .Add<SingleChoiceConfig>(FieldTypeEnum.SingleChoice)
            .Add<TextConfig>(FieldTypeEnum.Text)
            .Build()
    );
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.InjectDomain(builder.Configuration);

var injectorSetting = new InjectorSetting();
builder.Configuration.Bind(injectorSetting);
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
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(injectorSetting.Token.Key))
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