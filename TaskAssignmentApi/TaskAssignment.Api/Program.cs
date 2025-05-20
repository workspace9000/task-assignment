using TaskAssignment.Api.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.RegisterTaskAssignmentServices();

var app = builder.Build();

app.UseExceptionHandler("/api/error");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseCors(CorsConfig.AppCorsPolicy);
app.UseHttpsRedirection();
app.MapControllers();

app.ExecuteDbMigrations();

await app.RunAsync();
