using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University.Infrastructure.Models;

public class StudentAddressModel
{
    public Guid Id { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string Country { get; set; }

    public Guid StudentId { get; set; }
    public StudentModel Student { get; set; }
}