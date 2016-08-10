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
        public IActionResult Index(string SelectedCountry = "ALL", string SelectedRegion = "ALL", string SelectedCity = "ALL", string SelectedTag = "ALL")
        {
            //get data
            var data = System.IO.File.ReadAllText(@".\Data\data.json");
            var allProfiles = JsonConvert.DeserializeObject<List<Profile>>(data);

            //get filtered profiles
            var filteredProfiles = allProfiles
                .Where(p => p.country == SelectedCountry || SelectedCountry == "ALL")
                .Where(p => p.regions.Contains(SelectedRegion) || SelectedRegion == "ALL")
                .Where(p => p.cities.Contains(SelectedCity) || SelectedCity == "ALL")
                .Where(p => p.tags.Contains(SelectedTag) || SelectedTag == "ALL")
                .ToList();

            //mine data for drop-down values and sort them
            var avaliableCities = allProfiles.SelectMany(p => p.cities).Distinct().ToList();
            var avaliableRegions = allProfiles.SelectMany(p => p.regions).Distinct().ToList();
            var avaliableTags = allProfiles.SelectMany(p => p.tags).Distinct().ToList();
            var avaliableCountries = allProfiles.Select(p => p.country).Distinct().ToList();
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
                Profiles = filteredProfiles,
                AvaliableCities = avaliableCitiesSelect,
                AvaliableRegions = avaliableRegionsSelect,
                AvaliableTags = avaliableTagsSelect,
                AvaliableCountries = avaliableCountriesSelect,
                SelectedCountry = SelectedCountry,
                SelectedRegion = SelectedRegion,
                SelectedCity = SelectedCity,
                SelectedTag = SelectedTag
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
