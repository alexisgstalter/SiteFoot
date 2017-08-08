using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiteFoot.Models
{
    public class Groupe
    {
        public Groupe()
        {

        }

        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private String nom;

        public String Nom
        {
            get { return nom; }
            set { nom = value; }
        }

        private String code_groupe;

        public String Code_Groupe
        {
            get { return code_groupe; }
            set { code_groupe = value; }
        }

        private bool droit_gerer_buvette;

        public bool Droit_gerer_buvette
        {
            get { return droit_gerer_buvette; }
            set { droit_gerer_buvette = value; }
        }
        private bool droit_gerer_entrainement;

        public bool Droit_gerer_entrainement
        {
            get { return droit_gerer_entrainement; }
            set { droit_gerer_entrainement = value; }
        }
        private bool droit_entrainement_autre; //droit de modifier le créneau des autres

        public bool Droit_entrainement_autre
        {
            get { return droit_entrainement_autre; }
            set { droit_entrainement_autre = value; }
        }
        private bool droit_gerer_formateur;

        public bool Droit_gerer_formateur
        {
            get { return droit_gerer_formateur; }
            set { droit_gerer_formateur = value; }
        }
        private bool droit_formateur_autre; //droit de modifier la formation des autres

        public bool Droit_formateur_autre
        {
            get { return droit_formateur_autre; }
            set { droit_formateur_autre = value; }
        }
        private bool droit_poster_annonce;

        public bool Droit_poster_annonce
        {
            get { return droit_poster_annonce; }
            set { droit_poster_annonce = value; }
        }

        private bool droit_gerer_match;

        public bool Droit_gerer_match
        {
            get { return droit_gerer_match; }
            set { droit_gerer_match = value; }
        }
    }
}