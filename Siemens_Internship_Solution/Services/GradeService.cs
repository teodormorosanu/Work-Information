using Siemens.Internship2026.GradeBook.Interfaces;
using Siemens.Internship2026.GradeBook.Models;

namespace Siemens.Internship2026.GradeBook.Services;
public class GradeService : IGradeService {
    private readonly IGradeReader _reader;
    public GradeService(IGradeReader reader) { // Grade service constructor
        _reader = reader;
    }
    public async Task<Grade?> GetByIdAsync(int id) { // Says which is the specific grade by ID to the repository
        return await _reader.GetByIdAsync(id);
    }
    public async Task<(IEnumerable<Grade> Grades, GradeInfo Info)> GetAllWithStatsAsync() { // Takes all grades and calculates business info (count and average)
        var grades = await _reader.GetAllAsync();
        var gradeList = grades.ToList();
        var stats = new GradeInfo(
            TotalCount: gradeList.Count,
            AverageValue: gradeList.Any() ? gradeList.Average(g => g.Value) : 0,
            RetrievedAt: DateTime.UtcNow
        );
        return (gradeList, stats);
    }
    public async Task<IEnumerable<Grade>> GetPassingGradesAsync(int n) { // Filters the repository data to get the top N active passing grades (>= 5)
        var allGrades = await _reader.GetAllAsync();
        return allGrades
            .Where(g => g.IsActive && g.Value >= 5)
            .Take(n);
    }
}