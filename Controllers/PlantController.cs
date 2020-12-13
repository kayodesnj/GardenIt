using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Http;

using GardenIt.Models;
using GardenIt.Models.ViewModels;
using GardenIt.Models.Engine;

namespace GardenIt.Controllers
{
    [Authorize]
    public class PlantController : Controller
    {
        private readonly Garden _garden;
        private UserManager<IdentityUser> _userManager;
        private readonly IWebHostEnvironment _hostEnv;
        private readonly static string DEFAULT_IMAGE_NAME = "default.png"; 

        public PlantController(Garden garden, UserManager<IdentityUser> userManager, IWebHostEnvironment hostEnv)
        {
            _garden = garden;
            _userManager = userManager;
            _hostEnv = hostEnv;
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
                string imageName = SaveAndGenerateImageName(plantViewModel.ImageFile);
                var newPlant = new Plant() {
                    Id = Guid.NewGuid(),
                    UserId = UserId(),
                    Name = plantViewModel.Name,
                    Type = plantViewModel.Type,
                    DateAdded = DateTime.Now,
                    DaysBetweenWatering = plantViewModel.DaysBetweenWatering,
                    Notes = plantViewModel.Notes,
                    ImageName = imageName,
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
                Notes = existingPlant.Notes,
                ImageName = existingPlant.ImageName,
            };

            ViewBag.IsEditing = true;
            return View("Form", plantViewModel);
        }

        [HttpPost]
        public IActionResult Edit(PlantViewModel updatedPlant) {
            if (ModelState.IsValid) {
                string imageName = SaveAndGenerateImageName(updatedPlant.ImageFile);

                var existingPlant = _garden.GetPlant(updatedPlant.Id.Value, UserId());
                var plant = new Plant() {
                    Id = existingPlant.Id,
                    Name = updatedPlant.Name,
                    Type = updatedPlant.Type,
                    DaysBetweenWatering = updatedPlant.DaysBetweenWatering,
                    DateAdded = existingPlant.DateAdded,
                    Waterings = existingPlant.Waterings,
                    UserId = UserId(),
                    Notes = updatedPlant.Notes,
                    ImageName = imageName == DEFAULT_IMAGE_NAME ? existingPlant.ImageName : imageName
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

        private string SaveAndGenerateImageName(IFormFile image) {
            string wwwRootPath = _hostEnv.WebRootPath;

            if (image != null) {
                string filename = Path.GetFileNameWithoutExtension(image.FileName);
                string dateString = DateTime.Now.ToString("yymmssfff");
                string ext = Path.GetExtension(image.FileName);
                string imageName = filename + dateString + ext;
                string path = Path.Combine(wwwRootPath + "/image/", imageName);
                var fileStream = new FileStream(path, FileMode.Create);
                image.CopyTo(fileStream);
                return imageName;
            } else {
                return DEFAULT_IMAGE_NAME;
            }
        }
    }
}
