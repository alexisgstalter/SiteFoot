using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SiteFoot.Façades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiteFoot.Controllers
{
    [CustomAuthorize(GroupeAllow="ADMIN")]
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
        public String GetEventsBuvette(string start, string end)
        {
            try
            {
                DataTable dt = CalendrierManager.GetEventsBuvette(start,end);
                dt.Columns[3].ColumnName = "end";
                DataTable dtCloned = dt.Clone();
                dtCloned.Columns[0].DataType = typeof(String);
                dtCloned.Columns[4].DataType = typeof(Boolean);
                foreach (DataRow row in dt.Rows)
                {
                    dtCloned.ImportRow(row);
                }
                
                string result = JsonConvert.SerializeObject(dtCloned);

                result = result.Replace("\\","");

                return result;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return "";
            }
        }

        public JsonResult GetEvent(int id)
        {
            try
            {
                DataTable ev = CalendrierManager.GetEventBuvetteById(id);
                if (ev.Rows.Count > 0)
                {
                    return Json(new { ok = true, hasResult = true, id = ev.Rows[0]["title"].ToString(), titre = ev.Rows[0]["title"].ToString(), responsable = ev.Rows[0]["utilisateur"].ToString(), heure_debut = DateTime.Parse(ev.Rows[0]["start"].ToString()).ToString("dd/MM/yyyy HH:mm:ss"), heure_fin = DateTime.Parse(ev.Rows[0]["fin"].ToString()).ToString("dd/MM/yyyy HH:mm:ss") });
                }
                else
                {
                    return Json(new { ok = true, hasResult = false });
                }
            }
            catch (Exception e)
            {
                return Json(new { ok = false, error = e.Message });
            }
        }



        public String GetEventsEntrainement(string start, string end)
        {
            try
            {
                DataTable dt = CalendrierManager.GetEventsEntrainement(start, end);
                dt.Columns[3].ColumnName = "end";
                DataTable dtCloned = dt.Clone();
                dtCloned.Columns[0].DataType = typeof(String);
                dtCloned.Columns[4].DataType = typeof(Boolean);
                foreach (DataRow row in dt.Rows)
                {
                    dtCloned.ImportRow(row);
                }

                string result = JsonConvert.SerializeObject(dtCloned);

                result = result.Replace("\\", "");

                return result;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return "";
            }
        }

        public JsonResult GetEntrainement(int id)
        {
            try
            {
                DataTable ev = CalendrierManager.GetEvententrainementById(id);
                if (ev.Rows.Count > 0)
                {
                    return Json(new { ok = true, hasResult = true, id = ev.Rows[0]["equipe"].ToString(), terrain = ev.Rows[0]["terrain"].ToString(), heure_debut = DateTime.Parse(ev.Rows[0]["start"].ToString()).ToString("dd/MM/yyyy HH:mm:ss"), heure_fin = DateTime.Parse(ev.Rows[0]["fin"].ToString()).ToString("dd/MM/yyyy HH:mm:ss") });
                }
                else
                {
                    return Json(new { ok = true, hasResult = false });
                }
            }
            catch (Exception e)
            {
                return Json(new { ok = false, error = e.Message });
            }
        }






    }

}