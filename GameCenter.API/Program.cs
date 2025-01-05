
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.

    app.UseSwagger();
    app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors(x =>
{
    x.SetIsOriginAllowed((origin) => true)
    .AllowAnyMethod()
    .AllowAnyHeader();
});
app.MapControllers();
app.MapHub<RoomHub>("/room");

app.Run();
