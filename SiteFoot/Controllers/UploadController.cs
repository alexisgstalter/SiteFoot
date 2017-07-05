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
using System.IO;

namespace SiteFoot.Controllers
{
    [CustomAuthorize(GroupeAllow="ADMIN")]
    public class UploadController : Controller
    {
        public ActionResult CreateEquipe()
        {
            return View("~/Views/Backoffice/CreateEquipe.cshtml");
        }


        // GET: Upload
        public ActionResult Upload(HttpPostedFileBase file)
        {
            //On récupère le fichier
            foreach (string upload in Request.Files)
            {
                //Test si on à bien sélectionné un fichier
                if (!(Request.Files[upload] != null && Request.Files[upload].ContentLength > 0)) continue;

                //On vérifie si l'état du modèle est valide 
                if (ModelState.IsValid)
                {
                    //On teste si fichier est sélectionné
                    if (file == null)
                    {
                        ViewBag.Message = "Sélectionner votre fichier à intégrer";
                    }
                    //On teste si le fichier n'est pas vide
                    else if (file.ContentLength > 0)
                    {
                        int MaxContentLength = 1024 * 1024 * 1; //On fixe la taille du fichier à uploader à 1 MB
                        string[] tabextension = new string[] {".jpg" }; //type d'extension pris en compte

                        //On teste l'extention si respecté
                        if (!tabextension.Contains(file.FileName.Substring(file.FileName.LastIndexOf('.'))))
                        {
                            ViewBag.Message = "Type du fichier incorrect";
                        }
                        //On teste la taille du fichier
                        else if (file.ContentLength > MaxContentLength)
                        {
                            ViewBag.Message = "Taille du fichier trop importante";
                        }
                        else
                        {
                            try
                            {
                                //On transfert le fichier vers le chemin spécifié
                                var nomfichier = Path.GetFileName(file.FileName);
                                var chemin = Path.Combine((@"\\neon-01\c$\Fichiers Clients\Ecusson"), nomfichier);
                                file.SaveAs(chemin);
                                //On efface toutes les erreurs du Modèle
                                ModelState.Clear();

                                //Message Upload Ok
                                ViewBag.Message = "Upload du fichier avec succès";
                            }
                            catch (Exception ex)
                            {
                                ViewBag.Message = "Erreur: " + ex.Message.ToString();
                            }
                        }
                    }
                }
            }
            //Effectue une redirection vers l'action spécifiée à l'aide du nom d'action et du nom de contrôleur
            return RedirectToAction("CreateEquipe");
        }



    }

}