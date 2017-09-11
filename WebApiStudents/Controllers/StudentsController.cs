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
            var titles = studentManage.GetTitleList();
            return Json(titles, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetSchools(SchoolListRequest request)
        {
            var studentManage = new StudentManage();
            var schools = studentManage.GetSchoolList(request);
            return Json(new
            {
                draw = request.Draw,
                recordsFiltered = schools.TotalRecord,
                recordTotal = schools.TotalRecord,
                StudentsModelses = schools.SchoolLists
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetStudent(int enrollId)
        {
            var studentManage = new StudentManage();
            return Json(studentManage.GetStudent(enrollId), JsonRequestBehavior.AllowGet);
        }

        [HttpPut]
        public ActionResult UpdateStudent(Students student)
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
