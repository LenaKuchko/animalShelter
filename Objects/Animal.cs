using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace AnimalShelter
{
  public class Animal
  {
    private int _animalId;
    private string _date;
    private string _gender;
    private string _name;
    private int _speciesId;

    public Animal(string date, string gender, string name, int speciesId, int animalId = 0)
    {
      _date = date;
      _gender = gender;
      _name = name;
      _speciesId = speciesId;
      _animalId = animalId;
    }

    public string GetDate()
    {
      return _date;
    }

    public string GetGender()
    {
      return _gender;
    }

    public string GetName()
    {
      return _name;
    }
    public int GetAnimalId()
    {
      return _animalId;
    }
    public int GetSpeciesId()
    {
      return _speciesId;
    }
    public override bool Equals(System.Object otherAnimal)
    {
      if (!(otherAnimal is Animal))
      {
        return false;
      }
      else
      {
        Animal newAnimal = (Animal) otherAnimal;
        bool idEquality = (this.GetAnimalId() == newAnimal.GetAnimalId());
        bool nameEquality = (this.GetName() == newAnimal.GetName());
        bool genderEquality = (this.GetGender() == newAnimal.GetGender());
        bool dateEquality = (this.GetDate() == newAnimal.GetDate());
        bool speciesIdEquality = (this.GetSpeciesId() == newAnimal.GetSpeciesId());

        Console.WriteLine(idEquality);
        Console.WriteLine(nameEquality);
        Console.WriteLine(genderEquality);
        Console.WriteLine(dateEquality);
        Console.WriteLine(speciesIdEquality);
        Console.WriteLine(this.GetDate());
        Console.WriteLine(newAnimal.GetDate());


        return (idEquality && nameEquality && genderEquality && dateEquality && speciesIdEquality);
      }
    }

    public static List<Animal> GetAll()
    {
      List<Animal> allAnimals = new List<Animal>();

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM animals;", conn);
      SqlDataReader  rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int animalId = rdr.GetInt32(0);
        string animalName = rdr.GetString(1);
        string animalGender = rdr.GetString(2);
        string animalDate =rdr.GetString(3);
        int speciesId = rdr.GetInt32(4);

        Animal newAnimal = new Animal(animalDate, animalGender, animalName, speciesId, animalId);
        allAnimals.Add(newAnimal);
      }
      if (rdr != null)
      {
         rdr.Close();
       }
       if (conn != null)
       {
           conn.Close();
       }

       return allAnimals;
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM animals;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO animals (name, gender, date, species_id) OUTPUT INSERTED.id VALUES (@Name, @Gender, @Date, @Species_id);", conn);

      SqlParameter nameParameter = new SqlParameter();
      nameParameter.ParameterName = "@Name";
      nameParameter.Value = this.GetName();

      SqlParameter genderParameter = new SqlParameter();
      genderParameter.ParameterName = "@Gender";
      genderParameter.Value = this.GetGender();

      SqlParameter dateParameter = new SqlParameter();
      dateParameter.ParameterName = "@Date";
      dateParameter.Value = this.GetDate();

      SqlParameter species_idParameter = new SqlParameter();
      species_idParameter.ParameterName = "@Species_id";
      species_idParameter.Value = this.GetSpeciesId();

      cmd.Parameters.Add(nameParameter);
      cmd.Parameters.Add(genderParameter);
      cmd.Parameters.Add(dateParameter);
      cmd.Parameters.Add(species_idParameter);

      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._animalId = rdr.GetInt32(0);
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

    public static List<Animal> GetAllByType(int typeId)
    {
      List<Animal> allAnimals = new List<Animal>();

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM animals WHERE species_id = @Id;", conn);
      SqlParameter animalIdParameter = new SqlParameter();
      animalIdParameter.ParameterName = "@Id";
      animalIdParameter.Value = typeId.ToString();
      cmd.Parameters.Add(animalIdParameter);
      SqlDataReader  rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int animalId = rdr.GetInt32(0);
        string animalName = rdr.GetString(1);
        string animalGender = rdr.GetString(2);
        string animalDate =rdr.GetString(3);
        int speciesId = rdr.GetInt32(4);

        Animal newAnimal = new Animal(animalDate, animalGender, animalName, speciesId, animalId);
        allAnimals.Add(newAnimal);
      }
      if (rdr != null)
      {
         rdr.Close();
       }
       if (conn != null)
       {
           conn.Close();
       }

       return allAnimals;
    }

  }
}
