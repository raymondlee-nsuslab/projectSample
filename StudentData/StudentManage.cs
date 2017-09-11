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
        public List<string> GetTitleList()
        {
            var schoolContext = new SchoolContext();
            var titles = from course in schoolContext.CourseModelses
                         select course.Title;
            return titles.ToList();
        }

        public StudentListReturn GetSchoolList(SchoolListRequest request)
        {
            using (SchoolContext schoolContext = new SchoolContext())
            {
                var start = request.Start;
                var length = request.Length;
                var sortColumn = request.Columns[request.Order[0].Column].Name;
                var sortColumnDir = request.Order[0].Dir;
                var searchId = request.Columns[1].Search.Value;
                var searchTitle = request.Columns[6].Search.Value;

                var schoolList = schoolContext.EnrollmentModelses.GroupJoin(schoolContext.CourseModelses,
                        e => e.CourseModelsID, c => c.CourseModelsID, (e, c) => new { enroll = e, course = c })
                    .SelectMany(course => course.course.DefaultIfEmpty(), (e, c) => new { enroll = e.enroll, coruse = c })
                    .GroupJoin(schoolContext.StudentModels, e => e.enroll.StudentModelsID, s => s.StudentModelsID, (e, s) => new { enroll = e, student = s })
                    .SelectMany(student => student.student.DefaultIfEmpty(), (e, s) => new { enroll = e.enroll, student = s });

                if (!string.IsNullOrEmpty(searchId))
                {
                    var findId = Convert.ToInt32(searchId);
                    schoolList = schoolList.Where(e => e.student.StudentModelsID == findId);

                }
                if (!string.IsNullOrEmpty(searchTitle))
                {
                    schoolList = schoolList.Where(s => s.enroll.coruse.Title.Contains(searchTitle));
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
                var studentListReturn = new StudentListReturn();
                studentListReturn.TotalRecord = schoolList.Count();

                var pageSize = length;
                var skip = start;

                var infoList = schoolList.Select(
                    list => new Students()
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
                studentListReturn.SchoolLists = infoList;

                return studentListReturn;
            }
        }

        public Students GetStudent(int enrollId)
        {
            using (SchoolContext schoolContext = new SchoolContext())
            {
                var studentInfo = schoolContext.EnrollmentModelses.Join(schoolContext.StudentModels, e => e.StudentModelsID,
                        s => s.StudentModelsID, (e, s) => new { enroll = e, student = s }).Join(
                        schoolContext.CourseModelses,
                        e => e.enroll.CourseModelsID,
                        c => c.CourseModelsID, (e, c) => new { enroll = e, course = c })
                    .Where(e => e.enroll.enroll.EnrollmentModelsID == enrollId).Select(
                        student => new Students()
                        {
                            StudentId = student.enroll.enroll.StudentModelsID,
                            CourseId = student.course.CourseModelsID,
                            EnrollmentId = student.enroll.enroll.EnrollmentModelsID,
                            LastName = student.enroll.student.LastName,
                            FirstMidName = student.enroll.student.FirstMidName,
                            Title = student.course.Title
                        }).FirstOrDefault();

                return studentInfo;
            }
        }


        public bool SetStudent(Students requestStudent)
        {
            if (string.IsNullOrEmpty(requestStudent.Title) == true || string.IsNullOrEmpty(requestStudent.FirstMidName) || string.IsNullOrEmpty(requestStudent.LastName))
            {
                var response = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent(string.Format("Please fill in all fields")),
                    ReasonPhrase = "Please fill in all fields"
                };
                throw new HttpResponseException(response);
            }
            using (SchoolContext schoolContext = new SchoolContext())
            {
                var courseId = schoolContext.CourseModelses.FirstOrDefault(c => c.Title.Contains(requestStudent.Title));
                if (courseId == null)
                {
                    return false;
                }

                if (!(requestStudent.StudentId <= 0))
                {
                    //edit
                    var student = schoolContext.StudentModels
                        .FirstOrDefault(s => s.StudentModelsID == requestStudent.StudentId);

                    if (student == null)
                    {
                        var response = new HttpResponseMessage(HttpStatusCode.NotFound)
                        {
                            Content = new StringContent(string.Format("Student ID Not Found")),
                            ReasonPhrase = "Student ID Not Found"
                        };
                        throw new HttpResponseException(response);
                    }

                    student.FirstMidName = requestStudent.FirstMidName;
                    student.LastName = requestStudent.LastName;
                    var enroll =
                        schoolContext.EnrollmentModelses.FirstOrDefault(
                            e => e.EnrollmentModelsID == requestStudent.EnrollmentId);
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

                    var studentModels = new StudentModels();
                    studentModels.StudentModelsID = addId;
                    studentModels.FirstMidName = requestStudent.FirstMidName;
                    studentModels.LastName = requestStudent.LastName;
                    studentModels.EnrollmentDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    schoolContext.StudentModels.Add(studentModels);

                    var enroll = new EnrollmentModels();
                    enroll.StudentModelsID = addId;
                    enroll.CourseModelsID = courseId.CourseModelsID;
                    schoolContext.EnrollmentModelses.Add(enroll);
                }
                schoolContext.SaveChanges();
                return true;
            }
        }

        public bool DeleteStudent(int enrollmentModelsId)
        {
            using (SchoolContext schoolContext = new SchoolContext())
            {
                var enrollment = schoolContext.EnrollmentModelses.FirstOrDefault(
                    e => e.EnrollmentModelsID == enrollmentModelsId);
                if (enrollment != null)
                {
                    var studentModel =
                        schoolContext.StudentModels.FirstOrDefault(s => s.StudentModelsID == enrollment.StudentModelsID);
                    schoolContext.EnrollmentModelses.Remove(enrollment);
                    if (studentModel == null)
                    {
                        return false;
                    }
                    schoolContext.StudentModels.Remove(studentModel);
                    schoolContext.SaveChanges();
                    return true;
                }
                return false;
            }
        }
    }
}
