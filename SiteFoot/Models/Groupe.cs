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
        
        
        
    }
}