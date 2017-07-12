using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SiteFoot.Façades
{
    public class CalendrierManager
    {
        public static DataTable GetEventsBuvette(String date_debut, String date_fin)
        {
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteFoot"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            SqlDataAdapter source = new SqlDataAdapter("select a.id, b.prenom + ' ' + b.nom as 'title', a.start, a.fin as 'end', a.allDay from Buvette a, Responsablebuvette b where a.id_responsable = b.id and start > @date_debut and fin < @date_fin", myConnection);
            source.SelectCommand.Parameters.Add("@date_debut", SqlDbType.DateTime).Value = DateTime.Parse(date_debut);
            source.SelectCommand.Parameters.Add("@date_fin", SqlDbType.DateTime).Value = DateTime.Parse(date_fin);
            DataTable data = new DataTable();
            source.Fill(data);
            myConnection.Close();
            return data;
        }
        public static DataTable GetEventBuvetteById(int id)
        {
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteFoot"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            SqlDataAdapter source = new SqlDataAdapter("select * from Buvette where id=@id", myConnection);
            source.SelectCommand.Parameters.Add("@id", SqlDbType.Int).Value = id;
            DataTable data = new DataTable();
            source.Fill(data);
            myConnection.Close();
            return data;
        }
        public static void UpdateEventBuvette(int id, String titre, int responsable, DateTime date_debut, DateTime date_fin)
        {
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteFoot"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            SqlCommand cmd = new SqlCommand("update Buvette set title=@titre, start=@date_debut, fin=@date_fin, id_responsable=@responsable where id=@id", myConnection);
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
            cmd.Parameters.Add("@date_debut", SqlDbType.DateTime).Value = date_debut;
            cmd.Parameters.Add("@date_fin", SqlDbType.DateTime).Value = date_fin;
            cmd.Parameters.Add("@titre", SqlDbType.VarChar).Value = titre;
            cmd.Parameters.Add("@responsable", SqlDbType.VarChar).Value = responsable;
            cmd.ExecuteNonQuery();
            myConnection.Close();
        }
        public static void SaveEventBuvette(String titre, int responsable, DateTime date_debut, DateTime date_fin)
        {
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteFoot"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            SqlCommand cmd = new SqlCommand("insert into Buvette (title,start,fin,allDay,id_responsable) values(@titre, @date_debut, @date_fin, 0, @responsable)", myConnection);
            cmd.Parameters.Add("@date_debut", SqlDbType.DateTime).Value = date_debut;
            cmd.Parameters.Add("@date_fin", SqlDbType.DateTime).Value = date_fin;
            cmd.Parameters.Add("@titre", SqlDbType.VarChar).Value = titre;
            cmd.Parameters.Add("@responsable", SqlDbType.Int).Value = responsable;
            cmd.ExecuteNonQuery();
            myConnection.Close();
        }

        public static void DeleteEventBuvette(int id)
        {
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteFoot"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            SqlCommand cmd = new SqlCommand("Delete from Buvette where id=@id", myConnection);
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;

            cmd.ExecuteNonQuery();
            myConnection.Close();
        }

        public static DataTable GetResponsablesBuvette()
        {
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteFoot"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            SqlDataAdapter source = new SqlDataAdapter("select * from ResponsableBuvette", myConnection);
            DataTable data = new DataTable();
            source.Fill(data);
            myConnection.Close();
            return data;
        }

        public static DataTable LoadEquipeByIDEntraineur(int id)
        {
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteFoot"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            SqlDataAdapter source = new SqlDataAdapter("select * from Equipe where id in (select id_equipe from EntrainementParEquipe where id_equipe=@id)", myConnection);
            source.SelectCommand.Parameters.Add("@id", SqlDbType.Int).Value = id;
            DataTable data = new DataTable();
            source.Fill(data);
            myConnection.Close();
            return data;
        }

        public static DataTable GetEntrainements(int id_entraineur, int id_equipe, String nom_membre, int id_terrain, DateTime date_debut, DateTime date_fin)
        {
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteFoot"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            string query = "select a.id, b.nom_equipe + ' - ' + b.categorie as 'title', a.start, a.fin as 'end' from Entrainement a, Equipe b where a.id_equipe=b.id ";
            if (id_entraineur != 0)
            {
                query += "and b.id in (select id_equipe from EntraineurParEquipe where id_entraineur=@id_entraineur) ";
            }
            if (id_equipe != 0)
            {
                query += "and a.id_equipe = @id_equipe ";
            }
            if(nom_membre != "" && nom_membre != null)
            {
                query += "and b.id in (select id_equipe from MembreEquipe where prenom + ' ' + nom =@id_membre) ";
            }
            if (id_terrain != 0)
            {
                query += "and a.id_terrain = @id_terrain ";
            }
            query += "and start > @date_debut and fin < @date_fin ";
            SqlDataAdapter source = new SqlDataAdapter(query, myConnection);
            source.SelectCommand.Parameters.Add("@id_entraineur", SqlDbType.Int).Value = id_entraineur;
            source.SelectCommand.Parameters.Add("@id_equipe", SqlDbType.Int).Value = id_equipe;
            source.SelectCommand.Parameters.Add("@id_membre", SqlDbType.VarChar).Value = nom_membre;
            source.SelectCommand.Parameters.Add("@id_terrain", SqlDbType.Int).Value = id_terrain;
            source.SelectCommand.Parameters.Add("@date_debut", SqlDbType.DateTime).Value = date_debut;
            source.SelectCommand.Parameters.Add("@date_fin", SqlDbType.DateTime).Value = date_fin;
            DataTable data = new DataTable();
            source.Fill(data);
            myConnection.Close();
            return data;
        }


        public static DataTable GetAllTerrain()
        {
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteFoot"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            SqlDataAdapter source = new SqlDataAdapter("select * from Terrain", myConnection);
            DataTable data = new DataTable();
            source.Fill(data);
            myConnection.Close();
            return data;
        }

        public static DataTable GetAllEntraineurs()
        {
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteFoot"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            SqlDataAdapter source = new SqlDataAdapter("select * from Utilisateurs where id in (select id_utilisateur from GroupesUtilisateur where id_groupe in (select id from Groupe where nom ='ENTRAINEUR'))", myConnection);
            DataTable data = new DataTable();
            source.Fill(data);
            myConnection.Close();
            return data;
        }

        public static DataTable GetAllEquipes()
        {
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteFoot"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            SqlDataAdapter source = new SqlDataAdapter("select * from Equipe", myConnection);
            DataTable data = new DataTable();
            source.Fill(data);
            myConnection.Close();
            return data;
        }


        public static String[] GetAutoComplete(String partial_name)
        {
            List<String> res = new List<String>();
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteFoot"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            SqlCommand command = new SqlCommand("SELECT prenom + ' ' + nom FROM MembreEquipe where prenom + ' ' + nom LIKE '%' + @nom + '%'", myConnection);
            command.Parameters.Add("@nom", SqlDbType.VarChar).Value = partial_name;
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                res.Add(reader[0].ToString());
            }
            return res.ToArray();
        }

        public static DataTable GetEntrainementByID(int id)
        {

            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteFoot"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            string query = "select * from Entrainement where id=@id";

            SqlDataAdapter source = new SqlDataAdapter(query, myConnection);
            source.SelectCommand.Parameters.Add("@id", SqlDbType.Int).Value = id;
            DataTable data = new DataTable();
            source.Fill(data);
            myConnection.Close();
            return data;
        
        }

        public static bool EstEntraineurEquipe(int id_entraineur, int id_equipe)
        {
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteFoot"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            SqlDataAdapter source = new SqlDataAdapter("select id_entraineur from EntraineurParEquipe where id_equipe in(select id from Equipe where id=@id_equipe)", myConnection);
            source.SelectCommand.Parameters.Add("@id_equipe", SqlDbType.Int).Value = id_equipe;
            DataTable data = new DataTable();
            source.Fill(data);
            myConnection.Close();
            if (data.Rows.Count > 0)
            {
                if (int.Parse(data.Rows[0][0].ToString()) == id_entraineur)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static void SaveEntrainement(String title, DateTime start, DateTime end, int id_terrain, int id_equipe)
        {
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteFoot"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            SqlCommand cmd = new SqlCommand("insert into Entrainement values (@title, @start, @end, @id_terrain, @id_equipe)", myConnection);
            cmd.Parameters.Add("@title", SqlDbType.VarChar).Value = title;
            cmd.Parameters.Add("@start", SqlDbType.DateTime).Value = start;
            cmd.Parameters.Add("@end", SqlDbType.DateTime).Value = end;
            cmd.Parameters.Add("@id_terrain", SqlDbType.Int).Value = id_terrain;
            cmd.Parameters.Add("@id_equipe", SqlDbType.Int).Value = id_equipe;
            cmd.ExecuteNonQuery();
            myConnection.Close();
        }
        public static void UpdateEntrainement(int id,String title, DateTime start, DateTime end, int id_terrain, int id_equipe)
        {
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteFoot"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            SqlCommand cmd = new SqlCommand("update Entrainement set title=@title, start=@start, fin=@end, id_terrain=@id_terrain, id_equipe=@id_equipe where id=@id", myConnection);
            cmd.Parameters.Add("@title", SqlDbType.VarChar).Value = title;
            cmd.Parameters.Add("@start", SqlDbType.DateTime).Value = start;
            cmd.Parameters.Add("@end", SqlDbType.DateTime).Value = end;
            cmd.Parameters.Add("@id_terrain", SqlDbType.Int).Value = id_terrain;
            cmd.Parameters.Add("@id_equipe", SqlDbType.Int).Value = id_equipe;
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
            cmd.ExecuteNonQuery();
            myConnection.Close();
        }
        public static void DeleteEntrainement(int id)
        {
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteFoot"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            SqlCommand cmd = new SqlCommand("delete from Entrainement where id=@id", myConnection);
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
            cmd.ExecuteNonQuery();
            myConnection.Close();
        }
    }
}