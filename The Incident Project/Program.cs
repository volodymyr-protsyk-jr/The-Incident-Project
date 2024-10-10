using Microsoft.EntityFrameworkCore;
using The_Incident_Project.DATA;
using The_Incident_Project.Models;
using The_Incident_Project.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IIncidentService, IncidentService>();

builder.Services.AddControllers();


builder.Services.AddDbContext<IncidentContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
           .EnableSensitiveDataLogging()
);


builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();
var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<IncidentContext>();

    try
    {
        context.Database.Migrate();

        SampleData.Initialize(context);
    }
    catch (Exception ex)
    {
        Console.WriteLine("Ann error hac occured while seeding", ex.Message);
    }
}


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = string.Empty; // This makes Swagger UI available at the app's root URL (e.g., https://localhost:{port}/)
    });
}

app.MapControllers();
app.Run();
