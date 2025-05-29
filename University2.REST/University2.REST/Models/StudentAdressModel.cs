using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University2.REST.Models;

public class StudentAddressModel
{
    public int Id { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public Guid StudentId { get; set; }
    public object Student { get; internal set; }
}

public class StudentAddressCreateModel
{
    public string Address { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
}

public class StudentAddressUpdateModel
{
    public int Id { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
}
