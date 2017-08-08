using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using SiteFoot.Models;
using System.Diagnostics;

namespace SiteFoot.Façades
{
    public class ResultatManager
    {
        public static String[] GetAutoComplete(String partial_name)
        {
            List<String> res = new List<String>();
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteFoot"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            SqlCommand command = new SqlCommand("SELECT nom + ' - ' + categorie as 'nom' FROM Adversaires where nom LIKE '%' + @nom + '%'", myConnection);
            command.Parameters.Add("@nom", SqlDbType.VarChar).Value = partial_name;
            SqlDataReader reader = command.ExecuteReader();
            
            while (reader.Read())
            {
                res.Add(reader[0].ToString());
            }
            myConnection.Close();
            return res.ToArray();
        }
        public static DataTable GetMatchs(int id_equipe, String adversaire, DateTime date_debut, DateTime date_fin)
        {
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteFoot"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            String query = "select a.*, b.nom_equipe, b.ecusson as 'ecusson_equipe', b.categorie as 'categorie_equipe', c.nom, c.ecusson as 'ecusson_adversaire', c.categorie as 'categorie_adversaire' from Match a, Equipe b, Adversaires c where a.id_equipe=b.id and a.id_adversaire=c.id and a.date between @date_debut and @date_fin ";
            if (id_equipe != 0)
            {
                query += " and a.id_equipe = @id_equipe";
            }
            if (adversaire != "")
            {
                query += " and c.nom + ' - ' + c.categorie=@id_adversaire";
            }
            SqlDataAdapter source = new SqlDataAdapter(query, myConnection);
            source.SelectCommand.Parameters.Add("@id_equipe", SqlDbType.Int).Value = id_equipe;
            source.SelectCommand.Parameters.Add("@id_adversaire", SqlDbType.VarChar).Value = adversaire;
            source.SelectCommand.Parameters.Add("@date_debut", SqlDbType.DateTime).Value = date_debut;
            source.SelectCommand.Parameters.Add("@date_fin", SqlDbType.DateTime).Value = date_fin;

            DataTable data = new DataTable();
            source.Fill(data);
            myConnection.Close();
            return data;
        }

        public static int GetAdversaireByNameAndCategory(String name)
        {
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteFoot"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            String query = "select * from Adversaires where nom + ' - ' + categorie =@nom";
            
            SqlDataAdapter source = new SqlDataAdapter(query, myConnection);

            source.SelectCommand.Parameters.Add("@nom", SqlDbType.VarChar).Value = name;


            DataTable data = new DataTable();
            source.Fill(data);
            myConnection.Close();
            if (data.Rows.Count > 0)
            {
                return int.Parse(data.Rows[0]["id"].ToString());
            }
            else
            {
                return -1;
            }
        }

        public static void SaveMatch(int equipe, int adversaire, String score, DateTime date)
        {
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteFoot"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            SqlCommand source = new SqlCommand("insert into Match (score, id_equipe, id_adversaire, date) values (@score, @id_equipe, @id_adversaire, @date)", myConnection);
            source.Parameters.Add("@score", SqlDbType.VarChar).Value = score;
            source.Parameters.Add("@id_equipe", SqlDbType.Int).Value = equipe;
            source.Parameters.Add("@id_adversaire", SqlDbType.Int).Value = adversaire;
            source.Parameters.Add("@date", SqlDbType.DateTime).Value = date;

            source.ExecuteNonQuery();
            myConnection.Close();
        }

        public static void UpdateMatch(int id,int equipe, int adversaire, String score, DateTime date)
        {
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteFoot"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            SqlCommand source = new SqlCommand("Update Match set score=@score, id_equipe=@id_equipe, id_adversaire=@id_adversaire, date=@date where id=@id", myConnection);
            source.Parameters.Add("@score", SqlDbType.VarChar).Value = score;
            source.Parameters.Add("@id_equipe", SqlDbType.Int).Value = equipe;
            source.Parameters.Add("@id_adversaire", SqlDbType.Int).Value = adversaire;
            source.Parameters.Add("@date", SqlDbType.DateTime).Value = date;
            source.Parameters.Add("@id", SqlDbType.Int).Value = id;

            source.ExecuteNonQuery();
            myConnection.Close();
        }
        public static void DeleteMatch(int id)
        {
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteFoot"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            SqlCommand source = new SqlCommand("delete from Match where id=@id", myConnection);
            source.Parameters.Add("@id", SqlDbType.Int).Value = id;

            source.ExecuteNonQuery();
            myConnection.Close();
        }
        public static int SaveAdversaire(String nom, String categorie)
        {
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteFoot"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            SqlCommand source = new SqlCommand("insert into Adversaires  (nom, categorie) output INSERTED.ID values (@nom, @categorie)", myConnection);
            source.Parameters.Add("@nom", SqlDbType.VarChar).Value = nom;
            source.Parameters.Add("@categorie", SqlDbType.VarChar).Value = categorie;
            int id = (int)source.ExecuteScalar();
            myConnection.Close();
            return id;
        }
        public static void DeleteAdversaire(String nom, String categorie)
        {
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteFoot"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            SqlCommand source = new SqlCommand("delete from Adversaires where nom + ' - ' + categorie = @nom + ' - ' + @categorie", myConnection);
            source.Parameters.Add("@nom", SqlDbType.VarChar).Value = nom;
            source.Parameters.Add("@categorie", SqlDbType.VarChar).Value = categorie;
            source.ExecuteNonQuery();
            myConnection.Close();
        }
        public static void SavePieceJointe(int id, String ecusson)
        {
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteFoot"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            SqlCommand source = new SqlCommand("Update Adversaires set ecusson=@ecusson where id=@id", myConnection);
            source.Parameters.Add("@ecusson", SqlDbType.VarChar).Value = ecusson;

            source.Parameters.Add("@id", SqlDbType.Int).Value = id;

            source.ExecuteNonQuery();
            myConnection.Close();
        }
    }
}