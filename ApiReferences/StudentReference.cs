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
                    var responseError = new HttpResponseMessage(HttpStatusCode.NotFound)
                    {
                        Content = new StringContent(string.Format(Convert.ToString(response.StatusCode))),
                        ReasonPhrase = "Student Titles is not found"
                    };
                    throw new HttpResponseException(responseError);
                }
                var titles = response.Content.ReadAsAsync<List<string>>().Result;
                return titles;
            }    
        }

        public GetStudentsRequest GetStudents(Students students)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = client.PostAsJsonAsync("students/getschools",students).Result;
                if (!response.IsSuccessStatusCode)
                {
                    var responseError = new HttpResponseMessage(HttpStatusCode.NotFound)
                    {
                        Content = new StringContent(string.Format(Convert.ToString(response.StatusCode))),
                        ReasonPhrase = "Student Infomation is not found"
                    };
                    throw new HttpResponseException(responseError);
                }
                var getStudents = response.Content.ReadAsAsync<GetStudentsRequest>().Result;
                return getStudents;
            }
        }

        public Student GetStudent(int enrollId)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = client.GetAsync("students/getstudent?enrollId=" + enrollId).Result;
                if (!response.IsSuccessStatusCode)
                {
                    var responseError = new HttpResponseMessage(HttpStatusCode.NotFound)
                    {
                        Content = new StringContent(string.Format(Convert.ToString(response.StatusCode))),
                        ReasonPhrase = "Student ID Not Found"
                    };
                    throw new HttpResponseException(responseError);
                }
                var getStudent = response.Content.ReadAsAsync<Student>().Result;
                return getStudent;
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
                    var responseError = new HttpResponseMessage(HttpStatusCode.NotFound)
                    {
                        Content = new StringContent(string.Format(Convert.ToString(response.StatusCode))),
                        ReasonPhrase = "Student ID Not Found"
                    };
                    throw new HttpResponseException(responseError);
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
                    var responseError = new HttpResponseMessage(HttpStatusCode.NotFound)
                    {
                        Content = new StringContent(string.Format(Convert.ToString(response.StatusCode))),
                        ReasonPhrase = "Student ID Not Found"
                    };
                    throw new HttpResponseException(responseError);
                }
                var status = response.Content.ReadAsAsync<bool>().Result;
                return status;
            }
        }
    }
}
