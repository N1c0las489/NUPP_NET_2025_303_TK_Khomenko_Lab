using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University.Infrastructure.Models;

public class EnrollmentModel
{
    public Guid Id { get; set; }
    public Grade? Grade { get; set; }

    public Guid CourseId { get; set; }
    public CourseModel Course { get; set; }

    public Guid StudentId { get; set; }
    public StudentModel Student { get; set; }
}

public enum Grade
{
    A, B, C, D, F
}