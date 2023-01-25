﻿using TodoApp.Api.Auth;
using TodoApp.Api.Db;
using Microsoft.EntityFrameworkCore;
using TodoApp.Api.Repositories;


var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddTransient<IAuthService, AuthService>();
AuthConfigurator.Configure(builder);

builder.Services.AddDbContext<AppDbContext>(
    c => c.UseSqlServer(builder.Configuration["AppDbContextConnection"]
    ));

builder.Services.AddTransient<ISendEmailRequestRepository, SendEmailRequestRepository>();
builder.Services.AddTransient<ITodoRepository, TodoRepository>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
