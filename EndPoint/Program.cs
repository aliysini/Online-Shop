using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OnlineShop.Application.AutoMapperProfiles;
using OnlineShop.Application.Interfaces;
using OnlineShop.Application.Services;
using OnlineShop.Domain.Entity;
using OnlineShop.Infrastructure.Repositories.User;
using OnlineShop.Persistance.Context.OnlineShopDbContext;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<IWriteRepository<User>,UserWriteRepository>();
builder.Services.AddScoped<IReadRepository<User>, UserReadRepository>();
builder.Services.AddAutoMapper(typeof(UserMappingProfile));
/*builder.Services.AddDbContext<CommandDbContext>(options =>
    options.UseSqlServer("YourConnectionString"));*/

builder.Services.AddDbContext<OnlineShopDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer") ?? throw new InvalidOperationException("Connection string 'Default' not found.")));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
