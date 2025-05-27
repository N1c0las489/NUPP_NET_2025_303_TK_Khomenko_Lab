using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University.Infrastructure.Models;

public class CourseModel
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public Guid Credits { get; set; }

    public ICollection<EnrollmentModel> Enrollments { get; set; }
}