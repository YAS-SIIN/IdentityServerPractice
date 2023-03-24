using IdentityServer4.Models;

using IdentityServerPractice.Infrustructure;

using Microsoft.AspNetCore.Connections;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllersWithViews();

string connectionString = builder.Configuration["ApplicationOptions:ConnectionString"];
string assemblyName = typeof(Program).Assembly.GetName().Name;

builder.Services.AddIdentityServer()
    .AddConfigurationStore(opt =>
    {
        opt.ConfigureDbContext = bldr => bldr.UseSqlServer(builder.Configuration["ApplicationOptions:ConnectionString"], opt => opt.MigrationsAssembly(assemblyName));
    })
    .AddOperationalStore(opt =>
    {
        opt.ConfigureDbContext = bldr => bldr.UseSqlServer(builder.Configuration["ApplicationOptions:ConnectionString"], opt => opt.MigrationsAssembly(assemblyName));
    })
        //.AddInMemoryApiResources(IdentityData.GetApiResources())
        //.AddInMemoryIdentityResources(IdentityData.GetIdentityResources())                                                  
        //.AddInMemoryClients(IdentityData.GetClients()).AddTestUsers(IdentityData.GetTestUsers())
        .AddDeveloperSigningCredential();
builder.Services.AddMvc();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
    //app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseIdentityServer();
app.Run();


