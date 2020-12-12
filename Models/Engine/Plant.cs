using System;
using System.Collections.Generic;
using System.Linq;

namespace GardenIt.Models.Engine
{
    public class Plant
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public DateTime DateAdded { get; set; }
        public int DaysBetweenWatering { get; set; }
        public string Notes { get; set; }
        public List<Watering> Waterings { get; set; }

        public DateTime NextWateringDate { 
            get {
                if (Waterings.Count > 0) {
                    var lastWatering = Waterings.OrderByDescending(x => x.WateringDate).First();
                    return lastWatering.WateringDate.AddDays(DaysBetweenWatering);
                }
                else {
                    return DateAdded.AddDays(DaysBetweenWatering);
                }
            } 
        }

        public void Water() {
            var watering = new Watering() {
                Id = Guid.NewGuid(),
                WateringDate = DateTime.Now,
                PlantId = Id,
            };
            Waterings.Add(watering);
        }
    }
}