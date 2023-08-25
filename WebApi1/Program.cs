using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.DirectoryServices.Protocols;
using System.Net;
using System.Runtime.InteropServices;
using WebApi1;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
    .AddNegotiate(options =>
    {
        
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            options.EnableLdap(settings =>
            {
                settings.Domain = "contoso.com";
                var ldapConnection = new LdapConnection(
                    new LdapDirectoryIdentifier("dc.contoso.com"),
                    new NetworkCredential("user@CONTOSO.COM", "123qweAa"), AuthType.Basic);
                ldapConnection.SessionOptions.ReferralChasing = ReferralChasingOptions.None;
                settings.LdapConnection = ldapConnection;
            });
        }
    });

builder.WebHost.UseKestrel().UseIISIntegration();



builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = options.DefaultPolicy;
});


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.

    app.UseSwagger();
    app.UseSwaggerUI();


app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
