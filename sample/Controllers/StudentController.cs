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
            var studentReference = new StudentReference();
            return Json(new { Data = studentReference.GetTitles() },
                JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetStudents(Students request)
        {
            var studentReference = new StudentReference();
            var students = studentReference.GetStudents(request);

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
            var studentReference = new StudentReference();
            return View(studentReference.GetStudent(enrollId));
        }

        [HttpPost]
        public ActionResult Save(Student student)
        {
            var status = false;
            if (ModelState.IsValid)
            {
                var studentReference = new StudentReference();
                status = studentReference.SetStudent(student);
            }
            return Json(status);
        }


        [HttpGet]
        public ActionResult Delete(int enrollId)
        {
            var studentReference = new StudentReference();
            return View(studentReference.GetStudent(enrollId));
        }

        [HttpPost]
        public ActionResult Delete(Student student)
        {
            var status = false;
            var studentReference = new StudentReference();
            status = studentReference.DeleteStudent(student);
            return Json(status);
        }
    }





}