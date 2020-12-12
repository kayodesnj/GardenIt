using System;

namespace GardenIt.Models.Engine
{
    public class Watering
    {
        public Guid Id { get; set; }
        public Guid PlantId { get; set; }
        public DateTime WateringDate { get; set; }
    }
}