using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Muse.Controllers
{
    public class MediaBrowserController : Controller
    {
        //
        // GET: /Browser/
        public ActionResult Index()
        {
            return View();
        }
	}
}