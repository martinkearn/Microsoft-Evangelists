using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using MicrosoftEvangelists.Models;
using Newtonsoft.Json;
using MicrosoftEvangelists.ViewModels;

namespace MicrosoftEvangelists.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var data = System.IO.File.ReadAllText(@".\Data\data.json");

            var profiles = JsonConvert.DeserializeObject<List<Profile>>(data);

            var avaliableCities = profiles.SelectMany(p => p.cities).Distinct().ToList();
            var avaliableRegions = profiles.SelectMany(p => p.regions).Distinct().ToList();
            var avaliableTags = profiles.SelectMany(p => p.tags).Distinct().ToList();

            avaliableCities.Sort();
            avaliableRegions.Sort();
            avaliableTags.Sort();

            var vm = new HomeIndexViewModel() {
                Profiles = profiles,
                AvaliableCities = avaliableCities,
                AvaliableRegions = avaliableRegions,
                AvaliableTags = avaliableTags
            };

            return View(vm);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
