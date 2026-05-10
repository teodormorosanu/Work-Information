using Siemens.Internship2026.GradeBook.Models;
namespace Siemens.Internship2026.GradeBook.Services;
public record GradeInfo(int TotalCount, decimal AverageValue, DateTime RetrievedAt); // Moved from grade controller

public interface IGradeService {
    Task<Grade?> GetByIdAsync(int id); // Gets a grade by ID
    Task<(IEnumerable<Grade> Grades, GradeInfo Info)> GetAllWithStatsAsync(); // Gets all grades and their information
    Task<IEnumerable<Grade>> GetPassingGradesAsync(int n); // Gets a specific number of active passing grades
}