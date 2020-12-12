using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using GardenIt.Models.Engine;

namespace GardenIt.Models.Storage
{
    public class PlantStorageEF : IStorePlants
    {
        private readonly ApplicationDbContext _context;

        public PlantStorageEF(ApplicationDbContext context) {
            _context = context;
        }


        public void InsertPlant(Plant newPlant) {
            var plantDb = ConvertToDb(newPlant);
            _context.Plants.Add(plantDb);
            _context.SaveChanges();
        }

        public void UpdatePlant(Plant updatedPlant) {
            var plantDb = ConvertToDb(updatedPlant);
            _context.Plants.Update(plantDb);
            _context.SaveChanges();
        }

        public void Water(Guid id, Guid userId) {
            var plantDb = _context.Plants
                .Include(x => x.Waterings)
                .First(x => x.PlantId == id && x.UserId == userId);
            
            plantDb.Waterings.Add(new EFModels.Watering(){
                PlantId = id,
                WateringDate = DateTime.Now
            });
            
            _context.SaveChanges();
        }

        public Plant GetPlant(Guid id, Guid userId) {
            var plantDb = _context.Plants
                .AsNoTracking()
                .Include(x => x.Waterings)
                .First(x => x.PlantId == id && x.UserId == userId);

            return ConvertFromDb(plantDb);
        }

        public List<Plant> GetAllPlants(Guid userId) {
            return _context.Plants   
                .AsNoTracking()
                .Include(x => x.Waterings)
                .Where(x => x.UserId == userId && x.IsDeleted == false)
                .Select(x => ConvertFromDb(x))
                .ToList();
        }
         
        public void DeletePlant(Guid id, Guid userId) {
            var plantDb = _context.Plants
                .AsNoTracking()
                .First(x => x.PlantId == id && x.UserId == userId);
            plantDb.IsDeleted = true;
            _context.Plants.Update(plantDb);
            _context.SaveChanges();
        }

        private static EFModels.Plant ConvertToDb(Plant plant) {
            return new EFModels.Plant() {
                PlantId = plant.Id,
                Name = plant.Name,
                Type = plant.Type,
                DateAdded = plant.DateAdded,
                DaysBetweenWatering = plant.DaysBetweenWatering,
                UserId = plant.UserId,
                Notes = plant.Notes,
                IsDeleted = false,
                Waterings = plant.Waterings
                    .Select(x => ConvertToDb(x))
                    .ToList(),
            };

        }

        private static Plant ConvertFromDb(EFModels.Plant plantDb) {
            return new Plant() {
                Id = plantDb.PlantId,
                Name = plantDb.Name,
                Type = plantDb.Type,
                DateAdded = plantDb.DateAdded,
                DaysBetweenWatering = plantDb.DaysBetweenWatering,
                UserId = plantDb.UserId,
                Notes = plantDb.Notes,
                Waterings = plantDb.Waterings
                    .Select(x => ConvertFromDb(x))
                    .ToList(),
            };
        }

        private static EFModels.Watering ConvertToDb(Watering watering) {
            return new EFModels.Watering() {
                WateringId = watering.Id,
                WateringDate = watering.WateringDate,
                PlantId = watering.PlantId,
            };
        }

        private static Watering ConvertFromDb(EFModels.Watering wateringDb) {
            return new Watering() {
                Id = wateringDb.WateringId,
                WateringDate = wateringDb.WateringDate,
                PlantId = wateringDb.PlantId,
            };
        }
    }
}