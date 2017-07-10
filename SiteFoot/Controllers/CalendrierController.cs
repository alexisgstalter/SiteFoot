using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SiteFoot.Façades;
using SiteFoot.Models;
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
        public JsonResult AutocompleteLogin(String username)
        {
            try
            {
                User u = new User();
                u.Login = Server.HtmlEncode(username);
                return Json(new { ok = true, content = Utilisateur.GetLoginAutoComplete(u) });
            }
            catch (Exception)
            {

                return Json(new { ok = false });
            }
        }
        public String GetEventsBuvette(string start, string end)
        {
            try
            {
                DataTable dt = CalendrierManager.GetEventsBuvette(start,end);
                DataTable dtCloned = dt.Clone();
                dtCloned.Columns[0].DataType = typeof(String);
                dtCloned.Columns[4].DataType = typeof(Boolean);
                foreach (DataRow row in dt.Rows)
                {
                    dtCloned.ImportRow(row);
                }
                
                string result = JsonConvert.SerializeObject(dtCloned);


                return result;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return "";
            }
        }

        public JsonResult GetEventBuvette(int id)
        {
            try
            {
                DataTable ev = CalendrierManager.GetEventBuvetteById(id);
                if (ev.Rows.Count > 0)
                {
                    return Json(new { ok = true, hasResult = true, id = ev.Rows[0]["id"].ToString(), titre = ev.Rows[0]["title"].ToString(), responsable = ev.Rows[0]["id_responsable"].ToString(), heure_debut = DateTime.Parse(ev.Rows[0]["start"].ToString()).ToString("dd/MM/yyyy HH:mm:ss"), heure_fin = DateTime.Parse(ev.Rows[0]["fin"].ToString()).ToString("dd/MM/yyyy HH:mm:ss") });
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

        public JsonResult UpdateEventBuvette(int id, String debut, String fin, String titre, int responsable)
        {
            try
            {
                DateTime date_debut;
                if(!DateTime.TryParse(debut, out date_debut) || !DateTime.TryParse(fin, out date_debut))
                {
                    throw new Exception("Le format de date saisi est incorrect");
                }
                date_debut = DateTime.Parse(debut);
                DateTime date_fin = DateTime.Parse(fin);
                CalendrierManager.UpdateEventBuvette(id, titre, responsable, date_debut, date_fin);
                return Json(new { ok = true });
                
            }
            catch (Exception e)
            {
                return Json(new { ok = false, error = e.Message });
            }
        }
        public JsonResult SaveEventBuvette(String debut, String fin, String titre, int responsable)
        {
            try
            {
                DateTime date_debut;
                if (!DateTime.TryParse(debut, out date_debut) || !DateTime.TryParse(fin, out date_debut))
                {
                    throw new Exception("Le format de date saisi est incorrect");
                }
                date_debut = DateTime.Parse(debut);
                DateTime date_fin = DateTime.Parse(fin);
                CalendrierManager.SaveEventBuvette(titre, responsable, date_debut, date_fin);
                return Json(new { ok = true });

            }
            catch (Exception e)
            {
                return Json(new { ok = false, error = e.Message });
            }
        }
        public JsonResult DeleteEventBuvette(int id)
        {
            try
            {
                CalendrierManager.DeleteEventBuvette(id);
                return Json(new { ok = true });

            }
            catch (Exception e)
            {
                return Json(new { ok = false, error = e.Message });
            }
        }

        public JsonResult GetResponsablesBuvette()
        {
            try
            {
                DataTable responsables = CalendrierManager.GetResponsablesBuvette();
                string html = "";
                foreach (DataRow row in responsables.Rows)
                {
                    html += "<option value ='"+ row["id"].ToString() +"'>"+ row["prenom"].ToString() + " " + row["nom"].ToString() +"</option>";
                }
                return Json(new { ok = true, html = html });
            }
            catch (Exception e)
            {
                return Json(new { ok = false, error = e.Message });
            }
        }

        public ActionResult LoadListTerrains()
        {
            try
            {
                string html = "";
                DataTable terrains = CalendrierManager.GetAllTerrain();
                foreach (DataRow t in terrains.Rows)
                {
                    html += "<option value=" + t["id"].ToString() + ">" + t["type"].ToString() + "</option>";
                }
                return Content(html);
            }
            catch (Exception)
            {
                return Content(null);
            }
        }
        public ActionResult LoadListEntraineurs()
        {
            try
            {
                string html = "";
                DataTable entraineurs = CalendrierManager.GetAllEntraineurs();
                foreach (DataRow t in entraineurs.Rows)
                {
                    html += "<option value=" + t["id"].ToString() + ">" + t["prenom"].ToString() + " " + t["nom"].ToString() + "</option>";
                }
                return Content(html);
            }
            catch (Exception)
            {
                return Content(null);
            }
        }
        public ActionResult LoadListEquipes()
        {
            try
            {
                string html = "";
                DataTable equipes = CalendrierManager.GetAllEquipes();
                foreach (DataRow t in equipes.Rows)
                {
                    html += "<option value=" + t["id"].ToString() + ">" + t["nom_equipe"].ToString() + "</option>";
                }
                return Content(html);
            }
            catch (Exception)
            {
                return Content(null);
            }
        }
        public ActionResult LoadListEquipesByIDEntraineur(int id)
        {
            try
            {
                string html = "";
                DataTable equipes = CalendrierManager.LoadEquipeByIDEntraineur(id);
                foreach (DataRow t in equipes.Rows)
                {
                    html += "<option value=" + t["id"].ToString() + ">" + t["nom_equipe"].ToString() + "</option>";
                }
                return Content(html);
            }
            catch (Exception)
            {
                return Content(null);
            }
        }

        public String GetEventsEntrainement(int id_entraineur, int id_equipe, String id_membre, int id_terrain, DateTime start, DateTime end)
        {
            try
            {
                DataTable entrainements = CalendrierManager.GetEntrainements(id_entraineur, id_equipe, id_membre, id_terrain, start, end);
                
                //entrainements.Columns[0].DataType = typeof(String);
                string result = JsonConvert.SerializeObject(entrainements);


                return result;
            }
            catch (Exception e)
            {
                return "";
            }
        }

        public String AutocompleteMembre(String partial_name)
        {
            try
            {
                String[] content = CalendrierManager.GetAutoComplete(Server.HtmlEncode(partial_name));
                string result = JsonConvert.SerializeObject(content);
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public JsonResult GetEventEntrainement(int id)
        {
            try
            {
                DataTable ev = CalendrierManager.GetEntrainementByID(id);
                if (ev.Rows.Count > 0)
                {
                    return Json(new { ok = true, hasResult = true, id = ev.Rows[0]["id"].ToString(), titre = ev.Rows[0]["title"].ToString(), equipe = ev.Rows[0]["id_equipe"].ToString(), terrain = ev.Rows[0]["id_terrain"].ToString(), heure_debut = DateTime.Parse(ev.Rows[0]["start"].ToString()).ToString("dd/MM/yyyy HH:mm:ss"), heure_fin = DateTime.Parse(ev.Rows[0]["fin"].ToString()).ToString("dd/MM/yyyy HH:mm:ss") });
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
        public JsonResult CheckEntraineurEquipe(int id_equipe)
        {
            try
            {
                User u = (User)Session["CurrentUser"];
                List<Groupe> grps = GroupeManager.GetById(u.Id);
                foreach (Groupe g in grps)
	            {
		            if(g.Droit_entrainement_autre)
                    {
                        return Json(true);
                    }
	            }
                bool check = CalendrierManager.EstEntraineurEquipe(u.Id, id_equipe);
                return Json(check);
                
            }
            catch (Exception e)
            {
                return Json(false);
            }
        }
    }

}