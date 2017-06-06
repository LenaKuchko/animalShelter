using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace AnimalShelter
{
  public class Species
  {
    private int _speciesId;
    private string _type;

    public Species(string speciesType, int speciesId = 0)
    {
      _type = speciesType;
      _speciesId = speciesId;
    }

    public string GetType()
    {
      return _type;
    }

    public int GetSpeciesId()
    {
      return _speciesId;
    }

    public override bool Equals(System.Object otherSpecies)
    {
      if (!(otherSpecies is Species))
      {
        return false;
      }
      else
      {
        Species newSpecies = (Species) otherSpecies;
        bool idEquality = (this.GetSpeciesId() == newSpecies.GetSpeciesId());
        bool typeEquality = (this.GetType() == newSpecies.GetType());

        return (idEquality && typeEquality);
      }
    }

    public static Species FindSpecies(int searchId)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM species WHERE id = @searchId;", conn);
      SqlParameter categoryIdParameter = new SqlParameter();
     categoryIdParameter.ParameterName = "@searchId";
     categoryIdParameter.Value = searchId.ToString();
     cmd.Parameters.Add(categoryIdParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      int speciesId = 0;
      string speciesType = "";
      while(rdr.Read())
      {
        speciesId = rdr.GetInt32(0);
        speciesType = rdr.GetString(1);
      }
      Species newSpecies = new Species(speciesType, speciesId);
      if (rdr != null)
      {
         rdr.Close();
       }
       if (conn != null)
       {
           conn.Close();
       }
       return newSpecies;
    }

    public static List<Species> GetAll()
    {
      List<Species> allSpecies = new List<Species>();

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM species;", conn);
      SqlDataReader  rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int speciesId = rdr.GetInt32(0);
        string speciesType = rdr.GetString(1);
        Species newSpecies = new Species(speciesType, speciesId);
        allSpecies.Add(newSpecies);
      }
      if (rdr != null)
      {
         rdr.Close();
       }
       if (conn != null)
       {
           conn.Close();
       }

       return allSpecies;
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM species;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO species (type) OUTPUT INSERTED.id VALUES (@Type);", conn);

      SqlParameter typeParameter = new SqlParameter();
      typeParameter.ParameterName = "@Type";
      typeParameter.Value = this.GetType();

      cmd.Parameters.Add(typeParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._speciesId = rdr.GetInt32(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
    }
  }
}
