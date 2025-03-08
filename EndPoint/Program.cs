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
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
#region MediatR
builder.Services.AddMediatR(Assembly.GetAssembly(typeof(CreateUserCommand)));
builder.Services.AddMediatR(Assembly.GetAssembly(typeof(DeleteUserCommand)));
builder.Services.AddMediatR(Assembly.GetAssembly(typeof(GetAllUser)));
builder.Services.AddMediatR(Assembly.GetAssembly(typeof(GetUserById)));
builder.Services.AddMediatR(Assembly.GetAssembly(typeof(UpdateUserCommand)));
builder.Services.AddMediatR(Assembly.GetAssembly(typeof(CreateProductCommand)));
builder.Services.AddMediatR(Assembly.GetAssembly(typeof(CreateCategoryCommand)));
builder.Services.AddMediatR(Assembly.GetAssembly(typeof(UpdateProductCommand)));
builder.Services.AddMediatR(Assembly.GetAssembly(typeof(DeleteProductCommand)));
builder.Services.AddMediatR(Assembly.GetAssembly(typeof(GetProductByNameQuery)));
builder.Services.AddMediatR(Assembly.GetAssembly(typeof(GetAllProductQuery)));
builder.Services.AddMediatR(Assembly.GetAssembly(typeof(UpdateCategoryCommand)));
builder.Services.AddMediatR(Assembly.GetAssembly(typeof(DeleteCategoryCommand)));
builder.Services.AddMediatR(Assembly.GetAssembly(typeof(GetCategoryQuery)));
builder.Services.AddMediatR(Assembly.GetAssembly(typeof(GetAllCategoryQuerirs)));

#endregion /MediatR

#region Repository
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IUserRepository, UserRopository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
#endregion /Repository

#region Mapper
builder.Services.AddAutoMapper(typeof(OnlineShop.Application.AutoMapperProfiles.UserMappingProfile));
builder.Services.AddAutoMapper(typeof(OnlineShop.Application.AutoMapperProfiles.ProductMappingProfile));
builder.Services.AddAutoMapper(typeof(OnlineShop.Application.AutoMapperProfiles.CategoryMappingProfile));
#endregion /Mapper

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
    ConnectionMultiplexer.Connect(builder.Configuration["Redis:ConnectionString"]));
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
