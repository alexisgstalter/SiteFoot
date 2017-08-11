using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiteFoot.Models
{
    public class User
    {
        private int id;
        private String login;
        private String password;
        private String email;
        private List<int> groupe;
        private String salt;


        public User()
        {

        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        public String Login
        {
            get { return login; }
            set { login = value; }
        }


        public String Password
        {
            get { return password; }
            set { password = value; }
        }
        public String Email
        {
            get { return email; }
            set { email = value; }
        }
        public List<int> Groupe
        {
            get { return groupe; }
            set { groupe = value; }
        }
        public String Salt
        {
            get { return salt; }
            set { salt = value; }
        }

        private string nom;

        public string Nom
        {
          get { return nom; }
          set { nom = value; }
        }

        private string prenom;

        public string Prenom
        {
            get { return prenom; }
            set { prenom = value; }
        }
        private string telephone;

        public string Telephone
        {
            get { return telephone; }
            set { telephone = value; }
        }
        private string adresse;

        public string Adresse
        {
            get { return adresse; }
            set { adresse = value; }
        }
    }
}