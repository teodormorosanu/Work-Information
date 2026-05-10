using Siemens.Internship2026.GradeBook.Models;
namespace Siemens.Internship2026.GradeBook.Interfaces;

public interface IGradeReader {
    Task<Grade?> GetByIdAsync(int id); // Gets a single grade by its ID
    Task<IEnumerable<Grade>> GetAllAsync(); // Gets all available active grades
}
