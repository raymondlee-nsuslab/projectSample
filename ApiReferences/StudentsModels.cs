using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiReferences
{
    public class StudentsModels
    {
        public int StudentModelsID { get; set; }
        public int CourseModelsID { get; set; }
        public int EnrollmentModelsID { get; set; }
        public string LastName { get; set; }
        public string FirstMidName { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public string Title { get; set; }
        public int Credits { get; set; }
        public decimal? Grade { get; set; }
    }

    public class GetRequestStudents
    {
        public string Draw { get; set; }
        public int RecordsFiltered { get; set; }
        public int RecordTotal { get; set; }
        public List<StudentsModels> StudentsModelses { get; set; }
    }

    public class Student
    {
        public int StudentModelsID { get; set; }
        public int CourseModelsID { get; set; }
        public int EnrollmentModelsID { get; set; }
        public string LastName { get; set; }
        public string FirstMidName { get; set; }
        public string Title { get; set; }
    }
}
