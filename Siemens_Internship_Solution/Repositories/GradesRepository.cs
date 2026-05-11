using System.Text.Json;
using Siemens.Internship2026.GradeBook.Interfaces;
using Siemens.Internship2026.GradeBook.Models;

namespace Siemens.Internship2026.GradeBook.Repositories;

public class GradesRepository : IGradeReader {
    private readonly HttpClient _httpClient;
    private const string DataUrl = "https://gist.githubusercontent.com/ArdeleanTudor/8ea407832cd9794960e0e6bbd1319f6e/raw/145b121103dd1cee3737a681c487f7295ac82e6b/gistfile1.txt";
    public GradesRepository(HttpClient httpClient) { // Grades repository constructor
        _httpClient = httpClient;
    }
    public async Task<Grade?> GetByIdAsync(int id) { // Takes all grades from API and finds the active one matching the ID
        var grades = await GetAllAsync();
        return grades.FirstOrDefault(i => i.Id == id);
    }
    public async Task<IEnumerable<Grade>> GetAllAsync() { // Takes data from the external API and returns the active grades
        var response = await _httpClient.GetAsync(DataUrl);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var options = new JsonSerializerOptions {
            PropertyNameCaseInsensitive = true
        };
        var wrapper = JsonSerializer.Deserialize<ApiResponseWrapper>(content, options);
        var grades = wrapper?.Items ?? new List<Grade>();
        return grades.Where(i => i.IsActive);
    }
    private class ApiResponseWrapper { // Wrapper class
        public List<Grade>? Items { get; set; }
    }
}