﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

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

        
        public static void SaveEquipe(String nom_equipe, String liste_categorie, String entraineur, String ecusson)
        {
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteFoot"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            SqlCommand cmd = new SqlCommand("INSERT INTO Equipe (nom_equipe,categorie,ecusson,id_entraineur) values(@nom_equipe, @liste_categorie, @ecusson, @entraineur)", myConnection);
            cmd.Parameters.Add("@nom_equipe", SqlDbType.VarChar).Value = nom_equipe;
            cmd.Parameters.Add("@liste_categorie", SqlDbType.VarChar).Value = liste_categorie;
            cmd.Parameters.Add("@entraineur", SqlDbType.VarChar).Value = entraineur;
            cmd.Parameters.Add("@ecusson", SqlDbType.VarChar).Value = ecusson;
            cmd.ExecuteNonQuery();
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


        public static void UpdateEquipe(String nom_equipe_update, String categorie_update, String entraineur_update, String ecusson_update)
        {
            String connectionString = ConfigurationManager.ConnectionStrings["SQLSiteFoot"].ToString(); //Récupération de la chaîne de connexion
            SqlConnection myConnection = new SqlConnection(connectionString); //Nouvelle connexion à la base de donnée
            myConnection.Open(); //On ouvre la connexion
            SqlCommand cmd = new SqlCommand("UPDATE Equipe set id_entraineur=@entraineur_update, ecusson=@ecusson_update where nom_equipe=@nom_equipe_update and categorie=@categorie_update", myConnection);
            cmd.Parameters.Add("@nom_equipe_update", SqlDbType.VarChar).Value = nom_equipe_update;
            cmd.Parameters.Add("@categorie_update", SqlDbType.VarChar).Value = categorie_update;
            cmd.Parameters.Add("@entraineur_update", SqlDbType.VarChar).Value = entraineur_update;
            cmd.Parameters.Add("@ecusson_update", SqlDbType.VarChar).Value = ecusson_update;
            cmd.ExecuteNonQuery();
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
    }
}