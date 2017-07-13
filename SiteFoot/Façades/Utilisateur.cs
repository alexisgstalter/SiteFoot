using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Security;
using System.Web.Mvc;
using System.Web.Routing;
using System.Diagnostics;
using SiteFoot.Models;

namespace SiteFoot.Façades
{
    public class Utilisateur
    {

        public static bool Connect(User u)
        {
            Debug.WriteLine("cxc");
            User user = Utilisateur.GetByName(u);
            if (user != null)
            {
                String passwordToHash = u.Password;
                String login = u.Login;
                String saltKey = user.Salt;

                String passwordToCheck = Hash.GetHashSHA256("#" + passwordToHash + saltKey);
                //Vérifications dans la BDD
                String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteFoot"].ToString(); //Récupération de la chaîne de connexion
                SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
                myConnection.Open(); //On ouvre la connexion
                SqlCommand command = new SqlCommand("SELECT * from Utilisateurs where login=@login AND password=@password", myConnection);
                command.Parameters.Add("@login", SqlDbType.VarChar).Value = login;
                command.Parameters.Add("@password", SqlDbType.VarChar).Value = passwordToCheck;
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    reader.Close();
                    myConnection.Close();
                    return true;
                }
                else
                {
                    reader.Close();
                    myConnection.Close();
                    return false;
                }

            }
            else
            {
                return false;
            }
        }

        //Cette fonction permettra de savoir si l'utilisateur est connecté, que ses informations d'authentification sont valides
        public static bool IsAccessGranted(HttpContextBase HttpContext)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                //L'utilisateur est bien connecté
                User u = new User();
                
                String authString = HttpContext.User.Identity.Name;   //Contient le nom d'utilisateur et le mot de passe
                u.Login = authString.Split('#')[0];
                u.Password = authString.Split('#')[1];
                if (Utilisateur.Connect(u))
                {

                    //Si l'utilisateur n'es pas présent dans la session, on refait la session (ses informations d'authentifications sont valides)
                    User connected_user = Utilisateur.GetByName(u);
                    HttpContext.Session["CurrentUser"] = connected_user;    //L'utilisateur est connecté donc on raffraichit sa session
                    HttpContext.Session["CurrentGroupe"] = GroupeManager.GetById(connected_user.Id);
                    //HttpContext.Session["CurrentActivity"] = Utilisateur.GetActivityFromReflex(connected_user); //Si la session est terminée, on recharge les informations de la base de données
                    //HttpContext.Session["CurrentDepot"] = Utilisateur.GetDepotFromReflex(connected_user);   //Si la session est terminée, on recharge les informations de la base de données
                    return true;
                }
                else
                {
                    HttpContext.Session["CurrentUser"] = null;    //On vide la session
                    HttpContext.Session["CurrentGroupe"] = null;
                    FormsAuthentication.SignOut();  //Le cookie n'es plus valide ou a été modifié donc l'utilisateur est décnnecté

                }
            }
            HttpContext.Session["CurrentUser"] = null;    //On vide la session
            HttpContext.Session["CurrentGroupe"] = null;
            FormsAuthentication.SignOut();  //Le cookie n'es plus valide ou a été modifié donc l'utilisateur est décnnecté
            return false;
        }

        public static void Create(User u)
        {

            //Dans un premier temps on récupère le mot de passe, on y ajoute la clé de salage et on le hash
            String saltkey = Hash.GetNewSaltKey(); //On récupère une clé de salage
            String pwd = Utilisateur.GetNewPassword(u.Password, saltkey); //On créé le mot de passe
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteFoot"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            String query = "INSERT INTO Utilisateurs (login,password,email,salt, telephone,nom, prenom) output INSERTED.ID VALUES (@login,@password,@email,@salt,@telephone,@nom,@prenom)";
            SqlCommand command = new SqlCommand(query, myConnection);
            command.Parameters.AddWithValue("@login", u.Login);
            command.Parameters.AddWithValue("@password", pwd);
            command.Parameters.AddWithValue("@email", u.Email);
            command.Parameters.AddWithValue("@salt", saltkey);
            command.Parameters.AddWithValue("@telephone", u.Telephone);
            command.Parameters.AddWithValue("@nom", u.Nom);
            command.Parameters.AddWithValue("@prenom", u.Prenom);
            int id_user = (int)command.ExecuteScalar();

            foreach (int g in u.Groupe)
            {
                query = "INSERT INTO GroupesUtilisateur VALUES (@id_groupe, @id_user)";

                command = new SqlCommand(query, myConnection);
                command.Parameters.AddWithValue("@id_groupe", g);
                command.Parameters.AddWithValue("@id_user", id_user);
                command.ExecuteNonQuery();
            }
            myConnection.Close();

            User clearpassword = Utilisateur.GetByName(u);
            clearpassword.Password = u.Password;    //On donne à cet utilisateur un mot de passe en clair
            AddNewClearPassword(clearpassword); //On isert le nouveau mot de passe en clair dans la bdd associée


        }

        private static void AddNewClearPassword(User u)
        {
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteFoot"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            String query = "INSERT INTO Passwords (id_user,clear_password) VALUES (@id,@password)";
            SqlCommand command = new SqlCommand(query, myConnection);
            command.Parameters.AddWithValue("@id", u.Id);
            command.Parameters.AddWithValue("@password", u.Password);            
            command.ExecuteNonQuery();
            myConnection.Close();
        }

        public static DataTable getAll() //Renvoi un utilisateur avec son nom de groupe
        {

            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteFoot"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            SqlDataAdapter source = new SqlDataAdapter("SELECT * FROM Utilisateurs ORDER BY login", myConnection); //Permet de remplacer l'id du groupe par son nom
            DataTable table = new DataTable(); //On créé une table pour avoir une structure de nos données
            source.Fill(table);
            myConnection.Close();
            return table;


        }

        public static User GetById(int id)
        {

            User user = new User();

            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteFoot"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            SqlCommand command = new SqlCommand("SELECT * from Utilisateurs where  id=@id", myConnection);
            command.Parameters.AddWithValue("@id", id);
            SqlDataReader reader = command.ExecuteReader();


            if (reader.Read())
            {

                user.Id = (int)reader[0];
                user.Login = (string)reader[1];
                user.Password = (string)reader[2];
                user.Email = (string)reader[3];

                user.Salt = (string)reader[4];
                reader.Close();
                SqlCommand command2 = new SqlCommand("select id from Groupe where id in (select id from GroupesUtilisateur where id_utilisateur=@id)", myConnection);
                command2.Parameters.AddWithValue("@id", id);
                SqlDataReader reader_groupe = command2.ExecuteReader();
                while (reader_groupe.Read())
                {
                    user.Groupe.Add((int)reader[0]);
                }
                reader_groupe.Close();
                myConnection.Close();
                return user;
            }
            else
            {
                reader.Close();
                myConnection.Close();
                return null;
            }

        }

        public static String GetClearPassword(User u)
        {
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteFoot"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            String query = "SELECT clear_password FROM Passwords WHERE id_user=@id";
            SqlCommand command = new SqlCommand(query, myConnection);
            command.Parameters.AddWithValue("@id", u.Id);
            return command.ExecuteScalar().ToString();
        }



        public static void Delete(int userId)
        {

            //Dans un premier temps on récupère le mot de passe, on y ajoute la clé de salage et on le hash

            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteFoot"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            String query = "DELETE FROM Utilisateurs WHERE id=@id";
            SqlCommand command = new SqlCommand(query, myConnection);
            command.Parameters.AddWithValue("@id", userId);
            command.ExecuteNonQuery();
            myConnection.Close();
        }

        public static void Update(User u)
        {

            Debug.WriteLine("id :" + u.Id);
            String saltkey = Utilisateur.GetSaltKeyById(u); //On récupère la clé de salage de l'utilisateur
            String pwd = Utilisateur.GetNewPassword(u.Password, saltkey); //On créé le mot de passe crypté
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteFoot"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            String query = "UPDATE Utilisateurs SET login=@login,password=@password,email=@email, nom=@nom, prenom=@prenom, telephone=@telephone WHERE id=@id";
            SqlCommand command = new SqlCommand(query, myConnection);
            command.Parameters.AddWithValue("@login", u.Login);
            command.Parameters.AddWithValue("@password", pwd);
            command.Parameters.AddWithValue("@email", u.Email);
            command.Parameters.AddWithValue("@id", u.Id);
            command.Parameters.AddWithValue("@nom", u.Nom);
            command.Parameters.AddWithValue("@prenom", u.Prenom);
            command.Parameters.AddWithValue("@telephone", u.Telephone);

            command.ExecuteNonQuery();

            command = new SqlCommand("delete from GroupesUtilisateur where id_utilisateur = @id", myConnection);
            command.Parameters.AddWithValue("@id", u.Id);
            command.ExecuteNonQuery();
            foreach (int g in u.Groupe)
            {
                query = "INSERT INTO GroupesUtilisateur VALUES (@id_groupe, @id_user)";

                command = new SqlCommand(query, myConnection);

                command.Parameters.AddWithValue("@id_groupe", g);
                command.Parameters.AddWithValue("@id_user", u.Id);
                command.ExecuteNonQuery();
            }
            myConnection.Close();
            UpdateClearPassword(u);

        }

        public static void UpdateClearPassword(User u)
        {
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteFoot"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            String query = "UPDATE Passwords SET clear_password=@password WHERE id_user=@id";
            SqlCommand command = new SqlCommand(query, myConnection);            
            command.Parameters.AddWithValue("@password", u.Password);
            command.Parameters.AddWithValue("@id", u.Id);            
            command.ExecuteNonQuery();
            myConnection.Close();
        }

        

        public static String GetNewPassword(String password, String saltkey)
        {
            String pwd = Hash.GetHashSHA256("#" + password + saltkey); //On créé le mot de passe
            return pwd;
        }

        public static String GetSaltKeyById(User u)
        {
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteFoot"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            SqlCommand command = new SqlCommand("SELECT salt from Utilisateurs where id=@id", myConnection);
            command.Parameters.AddWithValue("@id", u.Id);
            SqlDataReader reader = command.ExecuteReader();
            String key = "";
            if (reader.Read())
            {
                key = reader.GetString(0);

            }
            reader.Close();
            myConnection.Close();
            return key;
        }

        public static User GetByName(User u)
        {
            User user = new User();

            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteFoot"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            SqlCommand command = new SqlCommand("SELECT * from Utilisateurs where  login=@login", myConnection);
            command.Parameters.AddWithValue("@login", u.Login);
            SqlDataReader reader = command.ExecuteReader();


            if (reader.Read())
            {

                user.Id = (int)reader[0];
                user.Login = (string)reader[1];
                user.Password = (string)reader[2];
                user.Email = (string)reader[3];

                user.Salt = (string)reader[4];
                reader.Close();
                SqlCommand command2 = new SqlCommand("select id from Groupe where id in (select id_groupe from GroupesUtilisateur where id_utilisateur=@id)", myConnection);
                command2.Parameters.AddWithValue("@id", u.Id);
                SqlDataReader reader_groupe = command2.ExecuteReader();
                user.Groupe = new List<int>();
                if (reader_groupe.Read())
                {
                    user.Groupe.Add(reader_groupe.GetInt32(0));
                }
                reader_groupe.Close();
                myConnection.Close();
                return user;
            }
            else
            {
                reader.Close();
                myConnection.Close();
                return null;
            }
        }

        public static bool Exists(User u) //On vérifie sur les 10 premiers caractères pour ne pas avoir de conflits avec reflex
        {

            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteFoot"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            SqlCommand command = new SqlCommand("SELECT * from Utilisateurs where login=@login", myConnection);
            command.Parameters.AddWithValue("@login", u.Login);

            SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                reader.Close();
                myConnection.Close();
                return true;
            }
            else
            {
                reader.Close();
                myConnection.Close();
                return false;
            }

        }

        public static String[] GetLoginAutoComplete(User u)
        {
            List<String> res = new List<String>();
            String connectionString = ConfigurationManager.ConnectionStrings["SQLProd"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            SqlCommand command = new SqlCommand("SELECT utcprf FROM reflex.hlutilp where utcprf LIKE '%' + @login +'%'", myConnection);
            command.Parameters.AddWithValue("@login", u.Login);
            SqlDataReader reader = command.ExecuteReader();
            myConnection.Close();
            while (reader.Read())
            {

                res.Add(reader[0].ToString());
            }

            return res.ToArray();
        }



        public static bool ExistsForUpdate(User u)
        {
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteFoot"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            SqlCommand command = new SqlCommand("SELECT * from Utilisateurs where login=@login AND id<>@id", myConnection);
            command.Parameters.AddWithValue("@login", u.Login);
            command.Parameters.AddWithValue("@id", u.Id);
            SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                reader.Close();
                myConnection.Close();
                return true;
            }
            else
            {
                reader.Close();
                myConnection.Close();
                return false;
            }
        }
    }
}