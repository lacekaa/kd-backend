using Microsoft.OpenApi.Models;
using kd_backend.Services;

var builder = WebApplication.CreateBuilder(args);

// CORS-Service hinzufügen
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

// DataProcessingService registrieren
builder.Services.AddScoped<DataProcessingService>();

// Dienste hinzufügen
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API Dokumentation",
        Version = "v1",
        Description = "Tolle Doku und so"
    });
});

var app = builder.Build();

// Fehlerbehandlung und Swagger je nach Umgebung
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Dokumentation V1");
    });
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// CORS Middleware einfügen
app.UseCors("AllowAngularApp");

app.UseRouting();
app.UseAuthorization();

// Stelle sicher, dass du die statischen Assets wie gewohnt einbindest
app.MapStaticAssets();
app.MapGet("/health", () => Results.Ok("OK"));
app.MapControllerRoute(
        name: "default",
        pattern: "")
    .WithStaticAssets();

app.Run();