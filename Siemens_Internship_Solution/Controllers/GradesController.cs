using Microsoft.AspNetCore.Mvc;
using Siemens.Internship2026.GradeBook.Services;
namespace Siemens.Internship2026.GradeBook.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GradesController : ControllerBase {
    private readonly IGradeService _gradeService;
    private readonly ILogger<GradesController> _logger;
    public GradesController(IGradeService gradeService, ILogger<GradesController> logger) { // Grades controller constructor
        _gradeService = gradeService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll() { // Gets all active grades along with computed statistics
        _logger.LogInformation("GET api/grades called at {Time}", DateTime.UtcNow);
        var (grades, info) = await _gradeService.GetAllWithStatsAsync();
        _logger.LogInformation("Returning {TotalCount} grades, average value: {AverageValue}",
            info.TotalCount, info.AverageValue);
        return Ok(new {
            Data = grades,
            Info = info
        });
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id) { // Takes a specific grade by ID and handles not-found scenarios
        _logger.LogInformation("GET api/grades/{Id} called at {Time}", id, DateTime.UtcNow);
        if (id <= 0) {
            _logger.LogWarning("Invalid id: {Id}", id);
            return BadRequest("Id must be a positive integer.");
        }
        var grade = await _gradeService.GetByIdAsync(id);
        if (grade == null) {
            _logger.LogWarning("Grade {Id} not found", id);
            return NotFound($"Grade with Id {id} was not found.");
        }
        return Ok(grade);
    }
    [HttpGet("passing/{n}")]
    public async Task<IActionResult> GetPassingGrades(int n) { // Filters and returns the first N active passing grades
        _logger.LogInformation("GET api/grades/passing/{N} called at {Time}", n, DateTime.UtcNow);
        if (n <= 0) {
            return BadRequest("N must be a positive integer. Please provide a valid number.");
        }
        var passingGrades = await _gradeService.GetPassingGradesAsync(n);
        return Ok(passingGrades);
    }
}