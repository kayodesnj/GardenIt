using System;
using System.Collections.Generic;
using GardenIt.Models.Storage;

namespace GardenIt.Models.Engine
{
    public class Garden
    {
        private readonly IStorePlants _plantStorage;
        public Garden(IStorePlants plantStorage) {
            _plantStorage = plantStorage;
        }

        public void CreatePlant(Plant newPlant) {
            _plantStorage.InsertPlant(newPlant);
        }

        public List<Plant> GetAllPlants(Guid userId) {
            return _plantStorage.GetAllPlants(userId);
        }

        public Plant GetPlant(Guid id, Guid userId) {
            return _plantStorage.GetPlant(id, userId);
        }

        public void UpdatePlant(Plant plantToUpdate) {
            _plantStorage.UpdatePlant(plantToUpdate);
        }

        public void DeletePlant(Guid plantId, Guid userId) {
            _plantStorage.DeletePlant(plantId, userId);
        }

        public void WaterPlant(Guid plantId, Guid userId) {
            var plantToWater = GetPlant(plantId, userId);
            plantToWater.Water();
            _plantStorage.Water(plantId, userId);
        }

    }
}