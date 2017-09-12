using System.Web.Mvc;
using StudentData;

namespace WebApiStudents.Controllers
{
    public class StudentsController : Controller
    {
        [HttpGet]
        public ActionResult GetTitles()
        {
            var studentManage = new StudentManage();
            var titles = studentManage.GetTitles();
            return Json(titles, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetStudents(StudentDataRequest request)
        {
            var studentManage = new StudentManage();
            var students = studentManage.GetStudents(request);
            return Json(new
            {
                draw = request.Draw,
                recordsFiltered = students.TotalRecord,
                recordTotal = students.TotalRecord,
                Student = students.Students
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetStudent(int enrollmentId)
        {
            var studentManage = new StudentManage();
            return Json(studentManage.GetStudent(enrollmentId), JsonRequestBehavior.AllowGet);
        }

        [HttpPut]
        public ActionResult UpdateStudent(Student student)
        {
            var status = false;
            if (ModelState.IsValid)
            {
                var studentManage = new StudentManage();
                status = studentManage.SetStudent(student);
            }
            return Json(status);
        }

        [HttpDelete]
        public ActionResult DeleteStudent(int enrollmentId)
        {
            var status = false;
            var studentManage = new StudentManage();
            status = studentManage.DeleteStudent(enrollmentId);
            return Json(status, JsonRequestBehavior.AllowGet);
        }
    }

}
