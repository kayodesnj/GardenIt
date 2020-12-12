using System;
using System.Collections.Generic;
using GardenIt.Models.Engine;

namespace GardenIt.Models.Storage
{
    public interface IStorePlants
    {
         void InsertPlant(Plant newPlant);

         void UpdatePlant(Plant updatedPlant);

         Plant GetPlant(Guid id, Guid userId);

         List<Plant> GetAllPlants(Guid userId);
         
         void DeletePlant(Guid id, Guid userId);
         void Water(Guid id, Guid userId);
    }
}