using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University2.REST.Models;

public class CourseModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int Credits { get; set; }
}

public class CourseCreateModel
{
    public string Title { get; set; }
    public int Credits { get; set; }
}

public class CourseUpdateModel
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public Guid Credits { get; set; }

}