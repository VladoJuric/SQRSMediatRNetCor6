using Application;
using Application.Common.Hubs;
using Infrastructure;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using WebUI.Filters;
using WebUI.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, lc) => lc
    .Enrich.FromLogContext()
    .MinimumLevel.Warning()
    .WriteTo.Debug()
    .WriteTo.MSSqlServer(
                            connectionString: builder.Configuration.GetSection("Serilog:ConnectionStrings:LogDatabase").Value,
                            sinkOptions: new MSSqlServerSinkOptions
                            {
                                TableName = builder.Configuration.GetSection("Serilog:TableName").Value,
                                SchemaName = "dbo",
                                AutoCreateSqlTable = true,
                            }
                        )
    );

// Add Redis
builder.Services.AddStackExchangeRedisCache(options =>
 {
     options.Configuration = "localhost:6000";
 });

// Add SignaR
builder.Services.AddSignalR();

// Add services to the container.
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddControllersWithViews(opt =>
{
    opt.Filters.Add(new ApiExceptionFilter());
});

builder.Services.AddSwaggerGen(config =>
{
    config.CustomOperationIds(e => $"{(e?.ActionDescriptor as ControllerActionDescriptor)?.ActionName}");
    config.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "WebUI Api", Version = "v1" });
    config.SchemaFilter<ModifyEnumSchemaFilter>();
});

builder.Services.AddCors(option => option.AddPolicy(
    "AllowAll",
    corsBuilder =>
    {
        corsBuilder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    }));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors("AllowAll");

app.UseSwagger()
    .UseSwaggerUI(config =>
    {
        config.RoutePrefix = "swagger";
    });

app.MapHub<NotificationHub>("/notify");

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller}/{action=Index}/{id?}");
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

// apply migrations on build
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

app.MapFallbackToFile("index.html"); ;

app.Run();
