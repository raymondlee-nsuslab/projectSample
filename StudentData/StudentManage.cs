using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace StudentData
{
    public class StudentManage
    {
        public List<string> GetTitles()
        {
            var schoolContext = new SchoolContext();
            var titles = from course in schoolContext.CourseModelses
                         select course.Title;
            return titles.ToList();
        }

        public StudentsResponse GetStudents(StudentDataRequest studentDataRequest)
        {
            using (SchoolContext schoolContext = new SchoolContext())
            {
                var start = studentDataRequest.Start;
                var length = studentDataRequest.Length;
                var sortColumn = studentDataRequest.Columns[studentDataRequest.Order[0].Column].Name;
                var sortColumnDir = studentDataRequest.Order[0].Dir;
                var searchId = studentDataRequest.Columns[1].Search.Value;
                var searchTitle = studentDataRequest.Columns[6].Search.Value;

                var studentQuery = schoolContext.EnrollmentModelses.GroupJoin(schoolContext.CourseModelses,
                        e => e.CourseModelsID, c => c.CourseModelsID, (e, c) => new { enroll = e, course = c })
                    .SelectMany(course => course.course.DefaultIfEmpty(), (e, c) => new { enroll = e.enroll, coruse = c })
                    .GroupJoin(schoolContext.StudentModels, e => e.enroll.StudentModelsID, s => s.StudentModelsID, (e, s) => new { enroll = e, student = s })
                    .SelectMany(student => student.student.DefaultIfEmpty(), (e, s) => new { enroll = e.enroll, student = s });

                if (!string.IsNullOrEmpty(searchId))
                {
                    var findId = Convert.ToInt32(searchId);
                    studentQuery = studentQuery.Where(e => e.student.StudentModelsID == findId);

                }
                if (!string.IsNullOrEmpty(searchTitle))
                {
                    studentQuery = studentQuery.Where(s => s.enroll.coruse.Title.Contains(searchTitle));
                }

                if (string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir))
                {
                    var response = new HttpResponseMessage(HttpStatusCode.NotFound)
                    {
                        Content = new StringContent(string.Format("SortColumn is Null")),
                        ReasonPhrase = "SortColumn is Null"
                    };
                    throw new HttpResponseException(response);
                }
                var studentsResponse = new StudentsResponse();
                studentsResponse.TotalRecord = studentQuery.Count();

                var pageSize = length;
                var skip = start;

                var students = studentQuery.Select(
                    list => new Student()
                    {
                        EnrollmentId = list.enroll.enroll.EnrollmentModelsID,
                        StudentId = list.student.StudentModelsID,
                        CourseId = list.enroll.coruse.CourseModelsID,
                        Grade = list.enroll.enroll.Grade,
                        Title = list.enroll.coruse.Title,
                        Credits = list.enroll.coruse.Credits,
                        LastName = list.student.LastName,
                        FirstMidName = list.student.FirstMidName,
                        EnrollmentDate = list.student.EnrollmentDate
                    }).OrderBy(sortColumn + " " + sortColumnDir).Skip(skip).Take(pageSize).ToList();
                studentsResponse.Students = students;

                return studentsResponse;
            }
        }

        public Student GetStudent(int enrollmentId)
        {
            using (SchoolContext schoolContext = new SchoolContext())
            {
                var student = schoolContext.EnrollmentModelses.Join(schoolContext.StudentModels, e => e.StudentModelsID,
                        s => s.StudentModelsID, (e, s) => new { enroll = e, student = s }).Join(
                        schoolContext.CourseModelses,
                        e => e.enroll.CourseModelsID,
                        c => c.CourseModelsID, (e, c) => new { enroll = e, course = c })
                    .Where(e => e.enroll.enroll.EnrollmentModelsID == enrollmentId).Select(
                        s => new Student()
                        {
                            StudentId = s.enroll.enroll.StudentModelsID,
                            CourseId = s.course.CourseModelsID,
                            EnrollmentId = s.enroll.enroll.EnrollmentModelsID,
                            LastName = s.enroll.student.LastName,
                            FirstMidName = s.enroll.student.FirstMidName,
                            Title = s.course.Title
                        }).FirstOrDefault();

                return student;
            }
        }


        public bool SetStudent(Student studentUpdate)
        {
            if (string.IsNullOrEmpty(studentUpdate.Title) == true || string.IsNullOrEmpty(studentUpdate.FirstMidName) || string.IsNullOrEmpty(studentUpdate.LastName))
            {
                var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent(string.Format("Please fill in all fields")),
                    ReasonPhrase = "Please fill in all fields"
                };
                throw new HttpResponseException(httpResponseMessage);
            }
            using (SchoolContext schoolContext = new SchoolContext())
            {
                var courseId = schoolContext.CourseModelses.FirstOrDefault(c => c.Title.Contains(studentUpdate.Title));
                if (courseId == null)
                {
                    return false;
                }

                if (!(studentUpdate.StudentId <= 0))
                {
                    //edit
                    var student = schoolContext.StudentModels
                        .FirstOrDefault(s => s.StudentModelsID == studentUpdate.StudentId);

                    if (student == null)
                    {
                        var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.NotFound)
                        {
                            Content = new StringContent(string.Format("Student ID Not Found")),
                            ReasonPhrase = "Student ID Not Found"
                        };
                        throw new HttpResponseException(httpResponseMessage);
                    }

                    student.FirstMidName = studentUpdate.FirstMidName;
                    student.LastName = studentUpdate.LastName;
                    var enroll =
                        schoolContext.EnrollmentModelses.FirstOrDefault(
                            e => e.EnrollmentModelsID == studentUpdate.EnrollmentId);
                    if (enroll == null)
                    {
                        return false;
                    }
                    enroll.CourseModelsID = courseId.CourseModelsID;
                }
                else
                {
                    //save
                    var maxid = schoolContext.StudentModels.Max(s => s.StudentModelsID);
                    var addId = maxid + 1;

                    var student = new StudentModels();
                    student.StudentModelsID = addId;
                    student.FirstMidName = studentUpdate.FirstMidName;
                    student.LastName = studentUpdate.LastName;
                    student.EnrollmentDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    schoolContext.StudentModels.Add(student);

                    var enrollment = new EnrollmentModels();
                    enrollment.StudentModelsID = addId;
                    enrollment.CourseModelsID = courseId.CourseModelsID;
                    schoolContext.EnrollmentModelses.Add(enrollment);
                }
                schoolContext.SaveChanges();
                return true;
            }
        }

        public bool DeleteStudent(int enrollmentId)
        {
            using (SchoolContext schoolContext = new SchoolContext())
            {
                var enrollment = schoolContext.EnrollmentModelses.FirstOrDefault(
                    e => e.EnrollmentModelsID == enrollmentId);
                if (enrollment != null)
                {
                    var student =
                        schoolContext.StudentModels.FirstOrDefault(s => s.StudentModelsID == enrollment.StudentModelsID);
                    schoolContext.EnrollmentModelses.Remove(enrollment);
                    if (student == null)
                    {
                        return false;
                    }
                    schoolContext.StudentModels.Remove(student);
                    schoolContext.SaveChanges();
                    return true;
                }
                return false;
            }
        }
    }
}
