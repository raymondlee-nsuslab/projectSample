using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiReferences
{
    public class Student
    {
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public int EnrollmentId { get; set; }
        public string LastName { get; set; }
        public string FirstMidName { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public string Title { get; set; }
        public int Credits { get; set; }
        public decimal? Grade { get; set; }
    }

    public class StudentsResponse
    {
        public string Draw { get; set; }
        public int RecordsFiltered { get; set; }
        public int RecordTotal { get; set; }
        public List<Student> Student { get; set; }
    }
}
