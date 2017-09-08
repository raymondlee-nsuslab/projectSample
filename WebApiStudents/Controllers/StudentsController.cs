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

        [HttpGet]
        public ActionResult GetSchools(SchoolListRequest request)
        {
            var studentManage = new StudentManage();
            var schools = studentManage.GetSchoolList(request);
            return Json(new
            {
                draw = request.Draw,
                recordsFiltered = schools.TotalRecord,
                recordTotal = schools.TotalRecord,
                data = schools.SchoolLists
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetStudent(int enrollId)
        {
            var studentManage = new StudentManage();

            return Json(studentManage.GetStudent(enrollId), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddStudent(Student student)
        {
            var studentManage = new StudentManage();

            return Json(studentManage.AddStudent(student), JsonRequestBehavior.AllowGet);
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
        public ActionResult DeleteStudent(Student student)
        {
            var status = false;
            var studentManage = new StudentManage();
            status = studentManage.DeleteStudent(student);
            return Json(status);
        }
    }

}
