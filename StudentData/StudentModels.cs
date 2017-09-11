using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentData
{
    public class StudentModels
    {
        public int StudentModelsID { get; set; }
        public string LastName { get; set; }
        public string FirstMidName { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public virtual ICollection<EnrollmentModels> EnrollmentModelses { get; set; }
    }

    public class CourseModels
    {
        public int CourseModelsID { get; set; }
        public string Title { get; set; }
        public int Credits { get; set; }
        public virtual ICollection<EnrollmentModels> EnrollmentModelses { get; set; }
    }

    public class EnrollmentModels
    {
        public int EnrollmentModelsID { get; set; }
        public int CourseModelsID { get; set; }
        public int StudentModelsID { get; set; }
        public decimal? Grade { get; set; }
        public virtual CourseModels Course { get; set; }
        public virtual StudentModels Student { get; set; }
    }

    public partial class Students
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
}
