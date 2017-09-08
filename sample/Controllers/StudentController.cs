using System;
using System.Web.Mvc;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using StudentData;

namespace sample.Controllers
{
    public class StudentController : Controller
    {

        // GET: Student
        public ActionResult Index()
        {
            return View();
        }

        [System.Web.Mvc.HttpGet]
        public ActionResult GetTitles()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:9090/");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage resp = client.GetAsync("Studnet/gettitles").Result;
            if (!resp.IsSuccessStatusCode)
            {
                var respErr = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent(string.Format("Student Titles is not found")),
                    ReasonPhrase = "Student Titles is not found"
                };
                throw new HttpResponseException(respErr);
            }
            return Json(new { resp.Content.ReadAsStringAsync().Result },
                JsonRequestBehavior.AllowGet);
            /*var studentManage = new StudentManage();
            return Json(new {Data = studentManage.GetTitleList()}, JsonRequestBehavior.AllowGet);*/
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult LoadSchoolList(SchoolListRequest request)
        {
            

            var studentManage = new StudentManage();
            var schoolList = studentManage.GetSchoolList(request);

            return Json(new
            {
                draw = request.Draw,
                recordsFiltered = schoolList.TotalRecord,
                recordTotal = schoolList.TotalRecord,
                data = schoolList.SchoolLists
            }, JsonRequestBehavior.AllowGet
            );

        }

        [System.Web.Mvc.HttpGet]
        public ActionResult Save(int enrollId)
        {
            var studentManage = new StudentManage();
            var student = studentManage.GetStudent(enrollId);
            return View(student);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult Save(Student student)
        {
            var status = false;
            if (ModelState.IsValid)
            {
                var studentManage = new StudentManage();
                status = studentManage.SetStudent(student);
            }
            return Json(status);
        }


        [System.Web.Mvc.HttpGet]
        public ActionResult Delete(int enrollId)
        {
            var studentManage = new StudentManage();
            return View(studentManage.GetStudent(enrollId));
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult Delete(Student student)
        {
            var status = false;
            var studentManage = new StudentManage();
            status = studentManage.DeleteStudent(student);
            return Json(status);
        }
    }

   



}