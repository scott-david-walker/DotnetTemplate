using Api;
using Api.Controllers.Framework;
using Core;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();
builder.Services.ConfigureMediatrBehaviours();
builder.Services.ConfigureAuth(builder.Configuration);
builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseNpgsql("Host=localhost;Database=database;Username=postgres;Password=password;Include Error Detail = true"));

builder.Services.AddIdentityApiEndpoints<User>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddScoped<ICurrentUser, CurrentUser>();

builder.Services.AddCors( options =>
{
    options.AddPolicy("all",
        policy =>
        {
            policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("all");
}

app.UseExceptionHandler(new ExceptionHandlerOptions
{
    ExceptionHandlingPath = new($"/{ExceptionHandlerController.Route}"),
    AllowStatusCode404Response = true
});

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapIdentityApi<User>();

app.Run();