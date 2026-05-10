namespace Siemens.Internship2026.GradeBook.Models;

public class Grade {
    public int Id { get; set; }
    public decimal Value { get; set; }
    public bool IsActive { get; set; } = true;
}
