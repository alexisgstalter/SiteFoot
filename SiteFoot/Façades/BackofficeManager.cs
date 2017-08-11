using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Diagnostics;
using SiteFoot.Models;


namespace SiteFoot.Façades
{
    public class BackofficeManager
    {


        public static DataTable GetAllEquipe()
        {
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteFoot"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            SqlDataAdapter source = new SqlDataAdapter("SELECT * from Equipe order by nom_equipe", myConnection);
            DataTable data = new DataTable();
            source.Fill(data);
            myConnection.Close();
            return data;
        }
        public static String GetAllEntraineursEquipe(int id_equipe)
        {
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteFoot"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            SqlDataAdapter source = new SqlDataAdapter("SELECT * from Utilisateurs where id in (select id_entraineur from EntraineurParEquipe where id_equipe=@id_equipe)", myConnection);
            source.SelectCommand.Parameters.Add("@id_equipe", SqlDbType.Int).Value = id_equipe;
            DataTable data = new DataTable();
            source.Fill(data);
            myConnection.Close();
            String res = "";
            foreach (DataRow row in data.Rows)
            {
                res += row["nom"].ToString() + " " + row["prenom"].ToString() + ",";
            }
            if (res.Contains(","))
            {
            res = res.Remove(res.LastIndexOf(','), 1);
            }
            return res;
        }

        public static void RemoveEquipe(String id)
        {
            String requete = "DELETE FROM Equipe WHERE id=@id";
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteFoot"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            SqlCommand command = new SqlCommand(requete, myConnection);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
            myConnection.Close();
        }


        public static void SaveEquipe(String nom_equipe, String liste_categorie, String ecusson, int[] entraineur)
        {
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteFoot"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            SqlCommand cmd = new SqlCommand("INSERT INTO Equipe (nom_equipe,categorie,ecusson) output INSERTED.ID values(@nom_equipe, @liste_categorie, @ecusson)", myConnection);
            cmd.Parameters.Add("@nom_equipe", SqlDbType.VarChar).Value = nom_equipe;
            cmd.Parameters.Add("@liste_categorie", SqlDbType.VarChar).Value = liste_categorie;
            cmd.Parameters.Add("@ecusson", SqlDbType.VarChar).Value = ecusson;
            int id_equipe = (int)cmd.ExecuteScalar();
            foreach (int id_entraineur in entraineur)
            {
                cmd = new SqlCommand("insert into EntraineurParEquipe values (@id_equipe, @id_entraineur)", myConnection);
                cmd.Parameters.Add("@id_equipe", SqlDbType.Int).Value = id_equipe;
                cmd.Parameters.Add("@id_entraineur", SqlDbType.Int).Value = id_entraineur;
                cmd.ExecuteNonQuery();
            }
            myConnection.Close();
        }


        public static DataTable GetEventEquipeById(int id)
        {
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteFoot"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            SqlDataAdapter source = new SqlDataAdapter("SELECT * from Equipe where id=@id", myConnection);
            source.SelectCommand.Parameters.Add("@id", SqlDbType.Int).Value = id;
            DataTable data = new DataTable();
            source.Fill(data);
            myConnection.Close();
            return data;
        }


        public static void UpdateEquipe(int[] entraineur_update, String ecusson_update, int id_equipe_clef)
        {

            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteFoot"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            SqlCommand cmd = new SqlCommand("UPDATE Equipe set ecusson=@ecusson_update where id=@id_equipe_clef", myConnection);
            cmd.Parameters.Add("@ecusson_update", SqlDbType.VarChar).Value = ecusson_update;
            cmd.Parameters.Add("@id_equipe_clef", SqlDbType.Int).Value = id_equipe_clef;
            cmd.ExecuteNonQuery();
            
            cmd = new SqlCommand("DELETE FROM EntraineurParEquipe WHERE id_equipe=@id_equipe", myConnection);
            cmd.Parameters.Add("@id_equipe", SqlDbType.Int).Value = id_equipe_clef;
            cmd.ExecuteNonQuery();
            
            foreach (int id_entraineur in entraineur_update)
            {
                cmd = new SqlCommand("insert into EntraineurParEquipe values (@id_equipe, @id_entraineur)", myConnection);
                cmd.Parameters.Add("@id_equipe", SqlDbType.Int).Value = id_equipe_clef;
                cmd.Parameters.Add("@id_entraineur", SqlDbType.Int).Value = id_entraineur;
                cmd.ExecuteNonQuery();
            }
            
            myConnection.Close();
        }
        

        public static DataTable GetInfoEntraineur(int id)
        {
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteFoot"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            SqlDataAdapter source = new SqlDataAdapter("SELECT * from Utilisateurs where id=@id order by nom,prenom", myConnection);
            source.SelectCommand.Parameters.Add("@id", SqlDbType.Int).Value = id;
            DataTable data = new DataTable();
            source.Fill(data);
            myConnection.Close();
            return data;
        }

        public static DataTable GetlisteEntraineur()
        {
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteFoot"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            SqlDataAdapter source = new SqlDataAdapter("SELECT * from Utilisateurs order by nom,prenom", myConnection);
            DataTable data = new DataTable();
            source.Fill(data);
            myConnection.Close();
            return data;
        }


        public static String GetEcussonEquipeFoot(String nom_equipe)
        {
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteFoot"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            SqlCommand cmd = new SqlCommand("select top 1 ecusson from Equipe where nom_equipe=@nom_equipe", myConnection);
            cmd.Parameters.Add("@nom_equipe", SqlDbType.VarChar).Value = nom_equipe;
            string res = cmd.ExecuteScalar().ToString();
            myConnection.Close();
            return res;
        }


      


        public static void SaveEcussonEquipeFoot(string fullpaths, String nom_equipe, String liste_categorie)
        {
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteFoot"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            SqlCommand cmd = new SqlCommand("INSERT INTO Equipe (nom_equipe,categorie,ecusson) values(@nom_equipe, @liste_categorie, @ecusson)", myConnection);
            cmd.Parameters.Add("@nom_equipe", SqlDbType.VarChar).Value = nom_equipe;
            cmd.Parameters.Add("@ecusson", SqlDbType.VarChar).Value = fullpaths;
            cmd.Parameters.Add("@liste_categorie", SqlDbType.VarChar).Value = liste_categorie;
            cmd.ExecuteNonQuery();
            myConnection.Close();
        }

        public static void SaveJoueur(int id_equipe, String prenom, String nom, String adresse, String telephone, String email, String login, String password)
        {
            String saltkey = Hash.GetNewSaltKey(); //On récupère une clé de salage
            String pwd = Utilisateur.GetNewPassword(password, saltkey); //On créé le mot de passe
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteFoot"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            SqlCommand cmd = new SqlCommand("INSERT INTO Utilisateurs (login, password, salt,nom, prenom, adresse, telephone, email, id_equipe) values(@login, @password, @salt,@nom, @prenom, @adresse, @telephone, @email, @id_equipe)", myConnection);
            cmd.Parameters.Add("@id_equipe", SqlDbType.Int).Value = id_equipe;
            cmd.Parameters.Add("@prenom", SqlDbType.VarChar).Value = prenom;
            cmd.Parameters.Add("@nom", SqlDbType.VarChar).Value = nom;
            cmd.Parameters.Add("@adresse", SqlDbType.VarChar).Value = adresse;
            cmd.Parameters.Add("@telephone", SqlDbType.VarChar).Value = telephone;
            cmd.Parameters.Add("@email", SqlDbType.VarChar).Value = email;
            cmd.Parameters.Add("@password", SqlDbType.VarChar).Value = pwd;
            cmd.Parameters.Add("@login", SqlDbType.VarChar).Value = login;
            cmd.Parameters.Add("@salt", SqlDbType.VarChar).Value = saltkey;
            cmd.ExecuteNonQuery();
            myConnection.Close();
            User u = new User();
            u.Login = login;
            u = Utilisateur.GetByName(u);
            u.Password = password;
            Utilisateur.AddNewClearPassword(u);
        }
        public static void UpdateJoueur(int id, int id_equipe, String prenom, String nom, String adresse, String telephone, String email, String login, String password)
        {
            User u = new User();
            u.Id = id;
            u.Password = password;
            String saltkey = Utilisateur.GetSaltKeyById(u); //On récupère la clé de salage de l'utilisateur
            String pwd = Utilisateur.GetNewPassword(password, saltkey); //On créé le mot de passe crypté
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteFoot"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            SqlCommand cmd = new SqlCommand("update Utilisateurs set  nom=@nom, prenom=@prenom, adresse=@adresse, telephone=@telephone, email=@email, id_equipe=@id_equipe, login=@login, password=@password where id=@id", myConnection);
            cmd.Parameters.Add("@id_equipe", SqlDbType.Int).Value = id_equipe;
            cmd.Parameters.Add("@prenom", SqlDbType.VarChar).Value = prenom;
            cmd.Parameters.Add("@nom", SqlDbType.VarChar).Value = nom;
            cmd.Parameters.Add("@adresse", SqlDbType.VarChar).Value = adresse;
            cmd.Parameters.Add("@telephone", SqlDbType.VarChar).Value = telephone;
            cmd.Parameters.Add("@email", SqlDbType.VarChar).Value = email;
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
            cmd.Parameters.Add("@password", SqlDbType.VarChar).Value = pwd;
            cmd.Parameters.Add("@login", SqlDbType.VarChar).Value = login;
            cmd.ExecuteNonQuery();
            myConnection.Close();
            Utilisateur.UpdateClearPassword(u);
        }
        public static void DeleteJoueur(int id)
        {
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteFoot"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            SqlCommand cmd = new SqlCommand("delete from Utilisateurs where id=@id", myConnection);

            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
            cmd.ExecuteNonQuery();
            myConnection.Close();
        }
    }
}