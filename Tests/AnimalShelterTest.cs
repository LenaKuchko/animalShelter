using Xunit;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace AnimalShelter
{
  public class AnimalShelterTest : IDisposable
  {
    public AnimalShelterTest()
    {
       DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=test_animal_shelter;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_DatabaseEmptyAtFirst()
    {
      //Arrange
      int result = Species.GetAll().Count;
      int animalResult = Animal.GetAll().Count;

      //Assert
      Assert.Equal(0, animalResult + result);
    }

    [Fact]
    public void Test_Species_ReturnValuesEqualEachother()
    {
      //Arrange, Act
      Species speciesOne = new Species("Chinchilla");
      Species speciesTwo = new Species("Chinchilla");

      //Assert
      Assert.Equal(speciesOne, speciesTwo);
    }

    [Fact]
    public void Test_Animal_ReturnValuesEqualEachother()
    {
      //Arrange, Act
      Animal animalOne = new Animal("06/06/2017", "Female", "Bella", 1);
      Animal animalTwo = new Animal("06/06/2017", "Female", "Bella", 1);

      //Assert
      Assert.Equal(animalOne, animalTwo);
    }


    [Fact]
     public void Test_Species_Save_SaveToDataBase()
     {
       //Arrange
       Species testSpecies = new Species("Chinchilla");


       //Act
       testSpecies.Save();
       List<Species> result = Species.GetAll();
       List<Species> testList = new List<Species>{testSpecies};

       //Assert
       Assert.Equal(testList, result);
     }

     [Fact]
      public void Test_Animal_Save_SaveToDataBase()
      {
        //Arrange
        Animal testAnimal = new Animal("06/06/2017", "Female", "Bella", 1);


        //Act
        testAnimal.Save();
        List<Animal> result = Animal.GetAll();
        List<Animal> testList = new List<Animal>{testAnimal};

        //Assert
        Assert.Equal(testList, result);
      }



    public void Dispose()
    {
      Species.DeleteAll();
      Animal.DeleteAll();
    }

  }
}
