using FluentValidation.AspNetCore;
using FluentValidation;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OnlineShop.Application.AutoMapperProfiles;
using OnlineShop.Application.Features.User.Commands;
using OnlineShop.Domain.Entity;
using OnlineShop.Persistance.Context.OnlineShopDbContext;
using MediatR;
using System.Reflection;
using OnlineShop.Persistance.Repositories;
using OnlineShop.Domain.Contracts;
using OnlineShop.Application.Features.User.Queries;
using AutoMapper;
using OnlineShop.Application.Features.Product.Commands;
using OnlineShop.Application.Features.Product.Queries;
using OnlineShop.Application.Features.Category.Commands;
using OnlineShop.Application.Features.Category.Queries;
using StackExchange.Redis;
using Microsoft.Extensions.Configuration;
using OnlineShop.Application.Features.Baskets.Queries;
using OnlineShop.Application.Features.Baskets.Commands;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddMediatR(Assembly.GetAssembly(typeof(CreateUserCommand))); builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
#region MediatR

#endregion /MediatR

#region Repository
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IUserRepository, UserRopository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IBasketRepository, BasketRepository>();
#endregion /Repository

var redisConnection = builder.Configuration.GetValue<string>("Redis:RedisConnection");

// اتصال به Redis با استفاده از IDistributedCache
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = redisConnection;
    options.InstanceName = "OnlineShop_";
});
#region Mapper
builder.Services.AddAutoMapper(typeof(OnlineShop.Application.AutoMapperProfiles.UserMappingProfile));
builder.Services.AddAutoMapper(typeof(OnlineShop.Application.AutoMapperProfiles.ProductMappingProfile));
builder.Services.AddAutoMapper(typeof(OnlineShop.Application.AutoMapperProfiles.CategoryMappingProfile)); ;
builder.Services.AddAutoMapper(typeof(OnlineShop.Application.AutoMapperProfiles.ShoppingCartProfile)); ;
#endregion /Mapper
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddFluentValidationAutoValidation();
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
