using Microsoft.AspNetCore.Mvc;

namespace Members.Controllers
{
    public class AdminController:Controller
    {
        public ActionResult ChildOne()
        {
            return View();
        }

        public ActionResult ChildTwo()
        {
            return View();
        }
    }
}
