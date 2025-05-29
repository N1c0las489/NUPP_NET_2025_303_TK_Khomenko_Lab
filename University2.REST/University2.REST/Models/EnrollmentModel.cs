using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University2.REST.Models;

public class EnrollmentModel
{
    public int Id { get; set; }
    public Grade? Grade { get; set; }
    public int CourseId { get; set; }
    public Guid StudentId { get; set; }
}

public enum Grade
{
    A, B, C, D, F
}

public class EnrollmentCreateModel
{
    public Guid StudentId { get; set; }
    public int CourseId { get; set; }
    public Grade? Grade { get; set; }
}

public class EnrollmentUpdateModel
{
    public Guid StudentId { get; set; }
    public int CourseId { get; set; }
    public Grade? Grade { get; set; }
}