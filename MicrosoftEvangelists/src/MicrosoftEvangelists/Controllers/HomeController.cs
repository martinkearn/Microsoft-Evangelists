using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using MicrosoftEvangelists.Models;
using Newtonsoft.Json;
using MicrosoftEvangelists.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MicrosoftEvangelists.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            //get data
            var data = System.IO.File.ReadAllText(@".\Data\data.json");
            var profiles = JsonConvert.DeserializeObject<List<Profile>>(data);

            //mine data for drop-down values and sort them
            var avaliableCities = profiles.SelectMany(p => p.cities).Distinct().ToList();
            var avaliableRegions = profiles.SelectMany(p => p.regions).Distinct().ToList();
            var avaliableTags = profiles.SelectMany(p => p.tags).Distinct().ToList();
            var avaliableCountries = profiles.Select(p => p.country).Distinct().ToList();
            avaliableCities.Sort();
            avaliableRegions.Sort();
            avaliableTags.Sort();
            avaliableCountries.Sort();

            //create select lists for view
            var avaliableCitiesSelect = new SelectList(avaliableCities);
            var avaliableRegionsSelect = new SelectList(avaliableRegions);
            var avaliableTagsSelect = new SelectList(avaliableTags);
            var avaliableCountriesSelect = new SelectList(avaliableCountries);

            //construct view model
            var vm = new HomeIndexViewModel() {
                Profiles = profiles,
                AvaliableCities = avaliableCitiesSelect,
                AvaliableRegions = avaliableRegionsSelect,
                AvaliableTags = avaliableTagsSelect,
                AvaliableCountries = avaliableCountriesSelect
            };

            //render view
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
