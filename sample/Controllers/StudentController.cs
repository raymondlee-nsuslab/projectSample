using System.Web.Mvc;
using ApiReferences;

namespace sample.Controllers
{
    public class StudentController : Controller
    {

        // GET: Student
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetTitles()
        {
            var studentData = new StudentData();
            return Json(new { Data = studentData.GetTitles() },
                JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetStudents(Students request)
        {
            var studentData = new StudentData();
            var students = studentData.GetStudents(request);

            return Json(new
            {
                draw = request.Draw,
                recordsFiltered = students.RecordTotal,
                recordTotal = students.RecordTotal,
                data = students.StudentsModelses
            }, JsonRequestBehavior.AllowGet
            );
        }

        [HttpGet]
        public ActionResult Save(int enrollId)
        {
            var studentData = new StudentData();
            return View(studentData.GetStudent(enrollId));
        }

        [HttpPost]
        public ActionResult Save(Student student)
        {
            var status = false;
            if (ModelState.IsValid)
            {
                var studentData = new StudentData();
                status = studentData.SetStudent(student);
            }
            return Json(status);
        }


        [HttpGet]
        public ActionResult Delete(int enrollId)
        {
            var studentData = new StudentData();
            return View(studentData.GetStudent(enrollId));
        }

        [HttpPost]
        public ActionResult Delete(Student student)
        {
            var status = false;
            var studentData = new StudentData();
            status = studentData.DeleteStudent(student);
            return Json(status);
        }
    }





}