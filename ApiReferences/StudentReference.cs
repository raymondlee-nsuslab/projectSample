using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace ApiReferences
{
    public class StudentReference
    {
        public List<string> GetTitles()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var resp = client.GetAsync("students/gettitles").Result;
                if (!resp.IsSuccessStatusCode)
                {
                    var respErr = new HttpResponseMessage(HttpStatusCode.NotFound)
                    {
                        Content = new StringContent(string.Format(Convert.ToString(resp.StatusCode))),
                        ReasonPhrase = "Student Titles is not found"
                    };
                    throw new HttpResponseException(respErr);
                }
                var titles = resp.Content.ReadAsAsync<List<string>>().Result;
                return titles;
            }    
        }

        public GetRequestStudents GetStudents(Students students)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var resp = client.PostAsJsonAsync("students/getschools",students).Result;
                if (!resp.IsSuccessStatusCode)
                {
                    var respErr = new HttpResponseMessage(HttpStatusCode.NotFound)
                    {
                        Content = new StringContent(string.Format(Convert.ToString(resp.StatusCode))),
                        ReasonPhrase = "Student Infomation is not found"
                    };
                    throw new HttpResponseException(respErr);
                }
                var getStudents = resp.Content.ReadAsAsync<GetRequestStudents>().Result;
                return getStudents;
            }
        }

        public Student GetStudent(int enrollId)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var resp = client.GetAsync("students/getstudent?enrollId=" + enrollId).Result;
                if (!resp.IsSuccessStatusCode)
                {
                    var respErr = new HttpResponseMessage(HttpStatusCode.NotFound)
                    {
                        Content = new StringContent(string.Format(Convert.ToString(resp.StatusCode))),
                        ReasonPhrase = "Student ID Not Found"
                    };
                    throw new HttpResponseException(respErr);
                }
                var getStudent = resp.Content.ReadAsAsync<Student>().Result;
                return getStudent;
            }
        }

        public bool SetStudent(Student student)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var resp = client.PutAsJsonAsync("students/updatestudent", student).Result;
                if (!resp.IsSuccessStatusCode)
                {
                    var respErr = new HttpResponseMessage(HttpStatusCode.NotFound)
                    {
                        Content = new StringContent(string.Format(Convert.ToString(resp.StatusCode))),
                        ReasonPhrase = "Student ID Not Found"
                    };
                    throw new HttpResponseException(respErr);
                }
                var status = resp.Content.ReadAsAsync<bool>().Result;
                return status;
            }
        }

        public bool DeleteStudent(Student student)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var resp = client.DeleteAsync("students/deletestudent?enrollmentModelsId=" + student.EnrollmentModelsID).Result;
                if (!resp.IsSuccessStatusCode)
                {
                    var respErr = new HttpResponseMessage(HttpStatusCode.NotFound)
                    {
                        Content = new StringContent(string.Format(Convert.ToString(resp.StatusCode))),
                        ReasonPhrase = "Student ID Not Found"
                    };
                    throw new HttpResponseException(respErr);
                }
                var status = resp.Content.ReadAsAsync<bool>().Result;
                return status;
            }
        }
    }
}
