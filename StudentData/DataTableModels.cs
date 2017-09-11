using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentData
{
    public class StudentRequest
    {
        public StudentSearch Search { get; set; }
        public string Name { get; set; }
    }
    public class StudentSearch
    {
        public string Value { get; set; }
    }

    public class StudentOrder
    {
        public int Column { get; set; }
        public string Dir { get; set; }
    }

    public class StudentDataRequest
    {
        public string Draw { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
        public List<StudentRequest> Columns { get; set; }
        public List<StudentOrder> Order { get; set; }
    }
}