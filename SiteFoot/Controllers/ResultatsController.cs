using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiteFoot.Controllers
{
    public class ResultatsController : Controller
    {
        //
        // GET: /Match/

        public ActionResult Championnat()
        {
            return View();
        }
        public ActionResult Match()
        {
            return View();
        }

    }
}
