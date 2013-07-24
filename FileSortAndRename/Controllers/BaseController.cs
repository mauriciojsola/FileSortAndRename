using System.Web.Mvc;
using FileSortAndRename.Helpers;

namespace FileSortAndRename.Controllers
{
    public class BaseController : Controller
    {
        private readonly Flash _flash;

        protected Flash Flash
        {
            get { return _flash; }
        }

        protected BaseController()
        {
            _flash = new Flash(TempData);
        }
    }
}
