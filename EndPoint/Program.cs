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
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMediatR(Assembly.GetAssembly(typeof(CreateUserCommand)));
builder.Services.AddMediatR(Assembly.GetAssembly(typeof(DeleteUserCommand)));
builder.Services.AddMediatR(Assembly.GetAssembly(typeof(GetAllUser)));
builder.Services.AddMediatR(Assembly.GetAssembly(typeof(GetUserById)));
builder.Services.AddMediatR(Assembly.GetAssembly(typeof(UpdateUserCommand)));
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IUserRepository, UserRopository>();
builder.Services.AddAutoMapper(typeof(OnlineShop.Application.AutoMapperProfiles.UserMappingProfile));
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
