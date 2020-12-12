using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using GardenIt.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

using GardenIt.Models.ViewModels;
using GardenIt.Models.Engine;

namespace GardenIt.Controllers
{
    [Authorize]
    public class PlantController : Controller
    {
        private readonly Garden _garden;
        private UserManager<IdentityUser> _userManager;

        public PlantController(Garden garden, UserManager<IdentityUser> userManager)
        {
            _garden = garden;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var result = _garden.GetAllPlants(UserId());
            return View(result);
        }

        public IActionResult Details(Guid id)
        {
            var result = _garden.GetPlant(id, UserId());
            return View(result);
        }

        public IActionResult Create() {
            ViewBag.IsEditing = false;
            return View("Form");  
        }

        [HttpPost]
        public IActionResult Create(PlantViewModel plantViewModel) {
            if (ModelState.IsValid) {
                var newPlant = new Plant() {
                    Id = Guid.NewGuid(),
                    UserId = UserId(),
                    Name = plantViewModel.Name,
                    Type = plantViewModel.Type,
                    DateAdded = DateTime.Now,
                    DaysBetweenWatering = plantViewModel.DaysBetweenWatering,
                    Notes = plantViewModel.Notes,
                    Waterings = new List<Watering>()
                };
                _garden.CreatePlant(newPlant);
                return RedirectToAction("Index");
            }

            return View("Form");

        }

        public IActionResult Edit(Guid id) {
            var existingPlant = _garden.GetPlant(id, UserId());

            var plantViewModel = new PlantViewModel() {
                Id = id,
                Name = existingPlant.Name,
                Type = existingPlant.Type,
                DaysBetweenWatering = existingPlant.DaysBetweenWatering,
                Notes = existingPlant.Notes
            };

            ViewBag.IsEditing = true;
            return View("Form", plantViewModel);
        }

        [HttpPost]
        public IActionResult Edit(PlantViewModel updatedPlant) {
            if (ModelState.IsValid) {
                var existingPlant = _garden.GetPlant(updatedPlant.Id.Value, UserId());
                var plant = new Plant() {
                    Id = existingPlant.Id,
                    Name = updatedPlant.Name,
                    Type = updatedPlant.Type,
                    DaysBetweenWatering = updatedPlant.DaysBetweenWatering,
                    DateAdded = existingPlant.DateAdded,
                    Waterings = existingPlant.Waterings,
                    UserId = UserId(),
                    Notes = updatedPlant.Notes
                };
                _garden.UpdatePlant(plant);
                return RedirectToAction("Details", new { id = existingPlant.Id});
            } else {
                ViewBag.IsEditing = true;
                return View("Form", updatedPlant);
            }
        }

        [HttpPost]
        public IActionResult Delete(Guid id) {
            _garden.DeletePlant(id, UserId());
            return RedirectToAction("Index");
        }


        public IActionResult Water(Guid id) {
            _garden.WaterPlant(id, UserId());
            return RedirectToAction("Details", new { id });
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private Guid UserId() {
            string stringUserId = _userManager.GetUserId(User);
            return Guid.Parse(stringUserId);
        }
    }
}
