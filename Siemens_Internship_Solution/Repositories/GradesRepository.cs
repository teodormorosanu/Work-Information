using System.Text.Json;
using Siemens.Internship2026.GradeBook.Interfaces;
using Siemens.Internship2026.GradeBook.Models;

namespace Siemens.Internship2026.GradeBook.Repositories;

public class GradesRepository : IGradeReader {
    private readonly HttpClient _httpClient;
    private const string DataUrl = "https://gist.githubusercontent.com/ArdeleanTudor/8ea407832cd9794960e0e6bbd1319f6e/raw/145b121103dd1";
    public GradesRepository(HttpClient httpClient) { // Grades repository constructor
        _httpClient = httpClient;
    }
    public async Task<Grade?> GetByIdAsync(int id) { // Takes all grades from API and finds the active one matching the ID
        var items = await GetAllAsync();
        return items.FirstOrDefault(i => i.Id == id);
    }
    public async Task<IEnumerable<Grade>> GetAllAsync() { // Takes data from the external API and returns the active grades
        var response = await _httpClient.GetAsync(DataUrl);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var options = new JsonSerializerOptions {
            PropertyNameCaseInsensitive = true
        };
        var items = JsonSerializer.Deserialize<List<Grade>>(content, options) ?? new List<Grade>();
        return items.Where(i => i.IsActive);
    }
}