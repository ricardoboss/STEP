using Microsoft.EntityFrameworkCore;
using StepLang.Libraries.API.DB;
using StepLang.Libraries.API.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<LibraryApiContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("LibraryAPI") ??
                           throw new("Connection string 'LibraryAPI' is not set");

    options.UseNpgsql(connectionString);
});

builder.Services.AddControllers(options => options.Filters.Add<NullResultToNotFoundFilter>());

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

app.UseAuthorization();

app.MapControllers();

app.Run();