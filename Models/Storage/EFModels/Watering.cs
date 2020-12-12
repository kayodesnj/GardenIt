using System;
using System.Collections.Generic;

namespace GardenIt.Models.Storage.EFModels
{
    public class Watering
    {
        public Guid WateringId { get; set; }
        public Guid PlantId { get; set; }
        public Plant Plant { get; set; }
        public DateTime WateringDate { get; set; }
    }
}