using SiteFoot.Façades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiteFoot.Controllers
{
    public class AnnonceController : Controller
    {
        //
        // GET: /Annonce/

        public ActionResult Annonce()
        {
            return View(@"~/Views/Home/Annonces.cshtml");
        }
        public JsonResult GetAnnonces(int offset)
        {
            try
            {
                DataTable annonces = AnnonceManager.GetAnnoncesScroll(offset);
                string html = "";

                foreach (DataRow row in annonces.Rows)
                {
                    html += "<li><div class='card-panel'><center><h2>" + row["titre"].ToString() + "</h2></center><br><section>" + row["texte"].ToString() + "</section></div></li>";
                }
                return Json(new { ok = true, html = html });
            }
            catch (Exception e)
            {
                return Json(new { ok = false, error = e.Message });
            }
        }
    }
}
