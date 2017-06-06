using System;
using System.Collections.Generic;
using Nancy;

namespace AnimalShelter
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get["/"] = _ => {
        List<Species> allSpecies = Species.GetAll();
        return View["index.cshtml", allSpecies];
      };
      Post["/"] = _ => {
        Species newSpecies = new Species(Request.Form["species"]);
        newSpecies.Save();
        List<Species> allSpecies = Species.GetAll();
        return View["index.cshtml", allSpecies];
      };
      Get["/species/{id}"] = parameter => {
        Dictionary<string, object> model = new Dictionary<string, object>();
        Species searchSpecies = Species.FindSpecies(parameter.id);
        List<Animal> allAnimalsByType = Animal.GetAllByType(parameter.id);
        model.Add("species", searchSpecies);
        model.Add("animals", allAnimalsByType);
        return View["view_animals.cshtml", model];
      };
      Post["/species/{id}"] = parameter => {
        Animal newAnimal = new Animal(Request.Form["animal-date"], Request.Form["animal-gender"], Request.Form["animal-name"], Request.Form["species-id"]);
        newAnimal.Save();
        Dictionary<string, object> model = new Dictionary<string, object>();
        Species searchSpecies = Species.FindSpecies(parameter.id);
        List<Animal> allAnimalsByType = Animal.GetAllByType(parameter.id);
        model.Add("species", searchSpecies);
        model.Add("animals", allAnimalsByType);
        return View["view_animals.cshtml", model];

      };


      Post["/delete"] = _ => {
        Species.DeleteAll();
        Animal.DeleteAll();
        List<Species> allSpecies = Species.GetAll();
        return View["index.cshtml", allSpecies];
      };
    }
  }
}
