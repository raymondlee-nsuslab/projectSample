using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace ApiReferences
{
    public class StudentData
    {
        public List<string> GetTitles()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = client.GetAsync("students/gettitles").Result;
                if (!response.IsSuccessStatusCode)
                {
                    var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.NotFound)
                    {
                        Content = new StringContent(string.Format(Convert.ToString(response.StatusCode))),
                        ReasonPhrase = "Student Titles is not found"
                    };
                    throw new HttpResponseException(httpResponseMessage);
                }
                var titles = response.Content.ReadAsAsync<List<string>>().Result;
                return titles;
            }    
        }

        public StudentsResponse GetStudents(StudentDataRequest studentDataRequest)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = client.PostAsJsonAsync("students/getstudents", studentDataRequest).Result;
                if (!response.IsSuccessStatusCode)
                {
                    var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.NotFound)
                    {
                        Content = new StringContent(string.Format(Convert.ToString(response.StatusCode))),
                        ReasonPhrase = "Student Infomation is not found"
                    };
                    throw new HttpResponseException(httpResponseMessage);
                }
                var students = response.Content.ReadAsAsync<StudentsResponse>().Result;
                return students;
            }
        }

        public Student GetStudent(int enrollmentId)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = client.GetAsync("students/getstudent?enrollmentId=" + enrollmentId).Result;
                if (!response.IsSuccessStatusCode)
                {
                    var responseError = new HttpResponseMessage(HttpStatusCode.NotFound)
                    {
                        Content = new StringContent(string.Format(Convert.ToString(response.StatusCode))),
                        ReasonPhrase = "Student ID Not Found"
                    };
                    throw new HttpResponseException(responseError);
                }
                var student = response.Content.ReadAsAsync<Student>().Result;
                return student;
            }
        }

        public bool SetStudent(Student student)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = client.PutAsJsonAsync("students/updatestudent", student).Result;
                if (!response.IsSuccessStatusCode)
                {
                    var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.NotFound)
                    {
                        Content = new StringContent(string.Format(Convert.ToString(response.StatusCode))),
                        ReasonPhrase = "Student ID Not Found"
                    };
                    throw new HttpResponseException(httpResponseMessage);
                }
                var status = response.Content.ReadAsAsync<bool>().Result;
                return status;
            }
        }

        public bool DeleteStudent(Student student)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = client.DeleteAsync("students/deletestudent?enrollmentId=" + student.EnrollmentId).Result;
                if (!response.IsSuccessStatusCode)
                {
                    var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.NotFound)
                    {
                        Content = new StringContent(string.Format(Convert.ToString(response.StatusCode))),
                        ReasonPhrase = "Student ID Not Found"
                    };
                    throw new HttpResponseException(httpResponseMessage);
                }
                var status = response.Content.ReadAsAsync<bool>().Result;
                return status;
            }
        }
    }
}
