using Aspose.Slides;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.DirectoryServices.Protocols;
using System.Net;
using System.Runtime.InteropServices;
using WebApi1;

var builder = WebApplication.CreateBuilder(args);

var di = new LdapDirectoryIdentifier(server: "dc.contoso.com", 389);
var connection = new LdapConnection(di, new NetworkCredential("administrator", "123qweAa", "contoso.com"), AuthType.Kerberos);
connection.Bind();
Console.WriteLine("Hello, World!");

builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
    .AddNegotiate(options =>
    {
        
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            options.EnableLdap("contoso.com");
        }
    });


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
