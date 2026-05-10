using Siemens.Internship2026.GradeBook.Interfaces;
using Siemens.Internship2026.GradeBook.Repositories;
using Siemens.Internship2026.GradeBook.Services;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient<IGradeReader, GradesRepository>(); // Registers the repository
builder.Services.AddScoped<IGradeService, GradeService>(); // Registers the business logic service with a Scoped lifetime
builder.Services.AddControllers();
var app = builder.Build();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();