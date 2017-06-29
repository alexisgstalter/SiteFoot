using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiteFoot.Controllers
{
    public class CalendrierController : Controller
    {
        // GET: Calendrier
        public ActionResult Buvette()
        {
            return View();
        }
        public ActionResult Entrainement()
        {
            return View();
        }
        public ActionResult FormationEducateur()
        {
            return View();
        }
    }
}