using System;
using System.Collections.Generic;

namespace Data.DataModels;

public partial class Student
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public int? CourseId { get; set; }

    public int? Age { get; set; }

    public string Email { get; set; } = null!;

    public string Gender { get; set; } = null!;

    public string Course { get; set; } = null!;

    public string Grade { get; set; } = null!;

    public DateOnly Dob { get; set; }

    public virtual Course? CourseNavigation { get; set; }
}
