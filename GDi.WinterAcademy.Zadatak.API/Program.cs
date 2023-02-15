using GDi.WinterAcademy.Zadatak.API.Models.SignalR;
using GDi.WinterAcademy.Zadatak.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddSignalR();

builder.Services.AddDbContext<WinterAcademyZadatakDbContext>(
    configure =>
        configure.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), options =>
        {
            options.MigrationsAssembly(typeof(WinterAcademyZadatakDbContext).Assembly.FullName);
        }
));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(builder =>
{
    builder
       .WithOrigins("http://localhost:4200", "https://localhost:4200")
       //.AllowAnyOrigin()
       .SetIsOriginAllowedToAllowWildcardSubdomains()
       .AllowAnyHeader()
       .AllowCredentials()
       .WithMethods("GET", "PUT", "POST", "DELETE", "OPTIONS")
       .SetPreflightMaxAge(TimeSpan.FromSeconds(3600));
}
);

app.UseAuthorization();

app.MapControllers();
app.MapHub<AppHub>("/appHub");

app.Run();
