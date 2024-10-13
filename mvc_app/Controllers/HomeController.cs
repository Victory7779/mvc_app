using Microsoft.AspNetCore.Mvc;

namespace mvc_app.Controllers
{
    public class HomeController : Controller
    {
        public ViewResult Index()
        {
            //ViewData["Name"] = "Alex";
            //ViewBag.Age = 27;
            return View();
        }
    }
}
