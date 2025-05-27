using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University.Infrastructure.Models;

public class StudentModel
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }

    public ICollection<EnrollmentModel> Enrollments { get; set; }

    public StudentAddressModel Address { get; set; }
}