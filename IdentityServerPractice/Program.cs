using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;

using IdentityServerPractice.IdentityEntity;
using IdentityServerPractice.Infrustructure;

using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

string connectionString = builder.Configuration["ApplicationOptions:ConnectionString"];
string assemblyName = typeof(Program).Assembly.GetName().Name;

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AspNetIdentityDbContext>(opt => opt.UseSqlServer(connectionString));

builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AspNetIdentityDbContext>();

   //IdentityServer4.EntityFramework.DbContexts.ConfigurationDbContext
builder.Services.AddIdentityServer()
    .AddAspNetIdentity<IdentityUser>()
    .AddConfigurationStore(opt =>
    {
        opt.ConfigureDbContext = bldr => bldr.UseSqlServer(connectionString, opt => opt.MigrationsAssembly(assemblyName));
    })
    .AddOperationalStore(opt =>
    {
        opt.ConfigureDbContext = bldr => bldr.UseSqlServer(connectionString, opt => opt.MigrationsAssembly(assemblyName));
    })
        //.AddInMemoryApiResources(IdentityData.GetApiResources())
        //.AddInMemoryIdentityResources(IdentityData.GetIdentityResources())                                                  
        //.AddInMemoryClients(IdentityData.GetClients()).AddTestUsers(IdentityData.GetTestUsers())
        .AddDeveloperSigningCredential();
builder.Services.AddMvc();

var app = builder.Build();

InitalIdentityData(app);

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


void InitalIdentityData(IApplicationBuilder app)
{
    using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
    {
    ConfigurationDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
    if (!dbContext.Clients.Any())
    {
        foreach (var client in IdentityData.GetClients())
        {
            dbContext.Clients.Add(client);
        }
        dbContext.SaveChanges();
    }

    if (!dbContext.IdentityResources.Any())
    {
        foreach (var resource in IdentityData.GetIdentityResources())
        {
            dbContext.IdentityResources.Add(resource);
        }
        dbContext.SaveChanges();
    }

    if (!dbContext.ApiResources.Any())
    {
        foreach (var resource in IdentityData.GetApiResources())
        {
            dbContext.ApiResources.Add(resource);
        }
        dbContext.SaveChanges();
    }
}
    
  
}