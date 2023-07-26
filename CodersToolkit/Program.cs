using Lamar.Microsoft.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Email;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add services to the container.
builder.Host
    .UseSerilog((context, services, loggerConfiguration) =>
    {
        var configuration = context.Configuration;
        loggerConfiguration.ReadFrom
            .Configuration(configuration)/*
            .WriteTo.Elasticsearch(new Serilog.Sinks.Elasticsearch.ElasticsearchSinkOptions(new Uri(configuration["Serilog:ElasticSearchUrl"]))
            {
                AutoRegisterTemplate = true,
                FormatStackTraceAsArray = true,
            })*/
            .WriteTo.Email(new EmailConnectionInfo()
            {
                EmailSubject = configuration["Serilog:EmailSubject"],
                MailServer = configuration["Serilog:MailServer"],
                ToEmail = configuration["Serilog:ToEmail"],
                FromEmail = "noreply@versionverve.com",
                EnableSsl = false,
                ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => true,
            },
            restrictedToMinimumLevel: LogEventLevel.Error,
            batchPostingLimit: 1);
    })
    .UseLamar((context, registry) =>
    {
        registry.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        registry.AddEndpointsApiExplorer();
        //registry.AddSwaggerGen();
        registry.Scan(scanner =>
        {
            scanner.AssembliesAndExecutablesFromApplicationBaseDirectory();
            scanner.WithDefaultConventions();
        });
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
