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
                                var nomfichier = Path.GetFileName(file.FileName).ToLower();
                                var chemin = Path.Combine((@"\\sodium-01\c$\inetpub\Fichiers Foot"), nomfichier);
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

         

        public ActionResult SaveEcussonEquipe()
        {
            string nom_equipe = Request.Form["nom_equipe"];
            string liste_categorie = Request.Form["liste_categorie"];

            List<String> chemins = new List<string>();
            List<String> nom_fichier = new List<string>();

            foreach (String upload in Request.Files)
            {
                var nomfichier = Path.GetFileName(Request.Files[upload].FileName);
                var chemin = Path.Combine(@"\\hexane-01\d$\SiteFoot\Fichiers SiteFoot", nomfichier);

                int i = 1;
                while (System.IO.File.Exists(chemin))
                {
                    string extension = Path.GetExtension(Request.Files[upload].FileName);
                    nomfichier = Path.GetFileName(Request.Files[upload].FileName).Replace(extension, "");
                    nomfichier = nomfichier + "(" + i + ")" + extension;
                    chemin = Path.Combine(@"\\hexane-01\d$\SiteFoot\Fichiers SiteFoot", nomfichier);
                    i++;
                }
                chemins.Add(chemin);
                nom_fichier.Add(nomfichier);

                Request.Files[upload].SaveAs(chemin);
            }

            string paths = "";
            foreach (string chemin in chemins)
            {
                paths += chemin + ";";
            }
            paths = paths.Remove(paths.LastIndexOf(";"));

            string name_fichier = "";
            foreach (string name in nom_fichier)
            {
                name_fichier += name + ";";
            }
            name_fichier = name_fichier.Remove(name_fichier.LastIndexOf(";"));

            return RedirectToAction("SaisieEquipe", "Upload");
        }



        public ActionResult SavePieceJointeAnnonce()
        {
            int id_annonce = int.Parse(Request.Form["id_annonce"]);
            foreach (String upload in Request.Files)
            {
                var nomfichier = Path.GetFileName(Request.Files[upload].FileName);
                var chemin = Path.Combine(@"\\hexane-01\d$\SiteFoot\Fichiers SiteFoot", nomfichier);

                int i = 1;
                while (System.IO.File.Exists(chemin))
                {
                    string extension = Path.GetExtension(Request.Files[upload].FileName);
                    nomfichier = Path.GetFileName(Request.Files[upload].FileName).Replace(extension, "");
                    nomfichier = nomfichier + "(" + i + ")" + extension;
                    chemin = Path.Combine(@"\\hexane-01\d$\SiteFoot\Fichiers SiteFoot", nomfichier);
                    i++;
                }
                Request.Files[upload].SaveAs(chemin);
                AnnonceManager.SavePieceJointeAnnonce(id_annonce, nomfichier);
            }

            return Json(new { ok = true });
        }

        public ActionResult SaveEcussonAdversaire()
        {
            if (Request.Files.Count == 0)
            {
                /*string numero_accord = Request.Form["num_accord"];
                GestionRetourManager.DeletePieceJointeExpertise(numero_accord);*/
            }
            else
            {
                int id = int.Parse(Request.Form["id"]);
                List<String> chemins = new List<string>();
                foreach (String upload in Request.Files)
                {
                    var nomfichier = Path.GetFileName(Request.Files[upload].FileName);
                    var chemin = Path.Combine(@"\\hexane-01\d$\SiteFoot\Fichiers SiteFoot", nomfichier);
                    int i = 1;
                    while (System.IO.File.Exists(chemin))
                    {
                        Debug.WriteLine("le fichier existe déjà");
                        string extension = Path.GetExtension(Request.Files[upload].FileName);
                        nomfichier = Path.GetFileName(Request.Files[upload].FileName).Replace(extension, "");
                        nomfichier = nomfichier + "(" + i + ")" + extension;
                        Debug.WriteLine(nomfichier);
                        chemin = Path.Combine(@"\\hexane-01\d$\SiteFoot\Fichiers SiteFoot", nomfichier);
                        i++;
                    }
                    chemins.Add(chemin);
                    ResultatManager.SavePieceJointe(id, nomfichier);
                    Request.Files[upload].SaveAs(chemin);
                }
            }

            return RedirectToAction("Matchs", "Resultats");
        }

    }

}