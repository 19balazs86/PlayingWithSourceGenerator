using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SampleApp;

[Table("university_students")]
public sealed class StudentTable
{
    [Key]
    [Column("ID")]
    public Guid Id { get; set; }

    [Column("First_name")]
    public string FirstName { get; set; } = string.Empty;

    [Column("Last_name")]
    public string LastName { get; set; } = string.Empty;

    [Column("Email_address")]
    public string EmailAddress { get; set; } = string.Empty;

    [Column("DateOfBirth")]
    public DateOnly BirthDate { get; set; }
}