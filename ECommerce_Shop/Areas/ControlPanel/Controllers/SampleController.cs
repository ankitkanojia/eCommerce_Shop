using System.Web.Mvc;

namespace ECommerce_Shop.Areas.ControlPanel.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SampleController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Form()
        {
            return View();
        }
    }
}