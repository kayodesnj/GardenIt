using System;
using System.Collections.Generic;

namespace GardenIt.Models.Storage.EFModels
{
    public class Plant
    {
        public Guid PlantId { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public DateTime DateAdded { get; set; }
        public int DaysBetweenWatering { get; set; }
        public string Notes { get; set; }
        public List<Watering> Waterings { get; set; }
        public bool IsDeleted { get; set; }
        public string ImageName { get; set; }
    }
}