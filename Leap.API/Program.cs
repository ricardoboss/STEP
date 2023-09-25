using Leap.API.DB;
using Leap.API.DB.Entities;
using Leap.API.Extensions;
using Leap.API.Filters;
using Leap.API.Interfaces;
using Leap.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((context, services, configuration) => configuration
        .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
        .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .WriteTo.Console());

    builder.Services.AddControllers(options => options.Filters.Add<NullResultToNotFoundFilter>());

    // Add services to the container.
    builder.Services.AddDbContext<LeapApiDbContext>(options => LeapApiDbContextFactory.ConfigureDbContext(options, builder.Configuration));

    builder.Services.AddSingleton<IPasswordHasher<Author>, PasswordHasher<Author>>();
    builder.Services.AddSingleton<ILibraryStorage, FilesystemLibraryStorage>();
    builder.Services.AddScoped<ITokenGenerator, TokenGenerator>();

    builder.Services
        .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration.GetJwtIssuer(),
                ValidAudience = builder.Configuration.GetJwtAudience(),
                IssuerSigningKey = builder.Configuration.GetJwtSecretKey(),
            };
        });

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        var scheme = new OpenApiSecurityScheme
        {
            BearerFormat = "JWT",
            Name = "JWT Authentication",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = JwtBearerDefaults.AuthenticationScheme,
            Reference = new()
            {
                Id = JwtBearerDefaults.AuthenticationScheme,
                Type = ReferenceType.SecurityScheme,
            },
        };

        options.AddSecurityDefinition(scheme.Reference.Id, scheme);

        options.AddSecurityRequirement(new()
        {
            { scheme, Array.Empty<string>() },
        });
    });

    var app = builder.Build();

    app.UseSerilogRequestLogging();

    await using (var scope = app.Services.CreateAsyncScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<LeapApiDbContext>();

        await context.InitializeAsync();
    }

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
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}