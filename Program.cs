using DinkToPdf.Contracts;
using DinkToPdf;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using momken_backend.Data;
using momken_backend.Dtos;
using momken_backend.Hubs;
using momken_backend.Providers;
using momken_backend.Services;
using Npgsql;
using System.Security.Claims;
using System.Text;
using momken_backend.Data.Zahran;
using momken_backend.Services.Zahran;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


#region services Region

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllersWithViews();
builder.Services.AddSwaggerGen(c => c.AddSignalRSwaggerGen());
var jwtOptionPartner = builder.Configuration.GetSection("JwtPartner").Get<JwtOptionPartner>();
var jwtOptionclient = builder.Configuration.GetSection("Jwtclient").Get<JwtOptionclient>();
builder.Services.AddSingleton<JwtOptionPartner>(jwtOptionPartner);
builder.Services.AddSingleton<JwtOptionclient>(jwtOptionclient);
builder.Services.AddSingleton<IHashPasswordService, HashPasswordService>();
builder.Services.AddSingleton<IJwtServicePartner, JwtServicePartner>();
builder.Services.AddSingleton<IJwtServiceclient, JwtServiceClient>();
builder.Services.AddSingleton<IUploadFileService, UploadFileService>();
builder.Services.AddSingleton<IMyfatoorahService, MyfatoorahService>();
builder.Services.AddSingleton<ITokenService, TokenService>();

builder.Services.AddSingleton<ITempImgService, TempImgService>();
builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
builder.Services.AddScoped<PdfService>();

#region MyRegion
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtOptionPartner.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtOptionPartner.Audience,
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptionPartner.SingningKey))
        };
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                if (!string.IsNullOrEmpty(accessToken) && context.HttpContext.Request.Path.StartsWithSegments("/hub"))
                {
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            },
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine("Authentication failed: " + context.Exception.Message);
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("client", options =>
    {
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtOptionPartner.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtOptionPartner.Audience,
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptionPartner.SingningKey))
        };
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                if (!string.IsNullOrEmpty(accessToken) && context.HttpContext.Request.Path.StartsWithSegments("/hub"))
                {
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            },
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine("Authentication failed: " + context.Exception.Message);
                return Task.CompletedTask;
            }
        };
    });


#endregion

builder.Services.AddSignalR();
builder.Services.AddSingleton<IUserIdProvider, ProductUserIdProvider>();
builder.Services.AddCors();


var ConnectionString = builder.Configuration.GetConnectionString("WebApiDatabase");
var dataSourceBuilder = new NpgsqlDataSourceBuilder(ConnectionString);
// Enable dynamic JSON serializatio
dataSourceBuilder.EnableDynamicJson();
var dataSource = dataSourceBuilder.Build();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(dataSource);
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    // Add a security definition for Bearer token
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter 'Bearer' [space] and then your token in the text input below.\n\nExample: \"Bearer 12345abcdef\"",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });



    // Add security requirement for the swagger documentation
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 104857600; // 100 MB
});

#endregion


var app = builder.Build();

#region Seed Data By Migration


using var scope = app.Services.CreateScope();

var services = scope.ServiceProvider;
var _dbcontext = services.GetRequiredService<AppDbContext>();
var LoggerFactory = services.GetRequiredService<ILoggerFactory>();

if (app.Environment.IsDevelopment())
{
    try
    {
        await _dbcontext.Database.MigrateAsync();
        await AppDbContextSeed.SeedAsync(_dbcontext);
    }

    catch (Exception Ex)
    {
        Console.WriteLine(Ex);
        var logger = LoggerFactory.CreateLogger<Program>();
        logger.LogError(Ex, "error here");
    }
}

#endregion




// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();

//}


#region file upload comment it for error in code

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "Public")),
    RequestPath = "/public"
});
#endregion

app.UseRouting();
app.UseCors();
//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapHub<PartnerHub>("/hub/v1/partner_hub");
app.MapControllers();

app.Run();
