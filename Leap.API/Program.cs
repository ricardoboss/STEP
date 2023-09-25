using Leap.API.DB;
using Leap.API.DB.Entities;
using Leap.API.Extensions;
using Leap.API.Filters;
using Leap.API.Interfaces;
using Leap.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options => options.Filters.Add<NullResultToNotFoundFilter>());

// Add services to the container.
builder.Services.AddDbContext<LibraryApiContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("LibraryAPI") ??
                           throw new("Connection string 'LibraryAPI' is not set");

    options.UseNpgsql(connectionString);
});

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
builder.Services.AddSwaggerGen();

var app = builder.Build();

await using (var scope = app.Services.CreateAsyncScope())
{
    var context = scope.ServiceProvider.GetRequiredService<LibraryApiContext>();

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