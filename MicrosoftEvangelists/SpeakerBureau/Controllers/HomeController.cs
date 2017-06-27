﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Newtonsoft.Json;
using SpeakerBureau.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using SpeakerBureau.ViewModels;

namespace SpeakerBureau.Controllers
{
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index(string SelectedCountry = "ALL", string SelectedRegion = "ALL", string SelectedCity = "ALL", string SelectedTag = "ALL")
        {
            //get data from live file or session
            if (string.IsNullOrEmpty(ReadSessionData("data")))
            {
                using (var httpClient = new HttpClient())
                {
                    var dataUrl = "http://microsoftevangelists.azurewebsites.net/data.json";
                    httpClient.BaseAddress = new Uri(dataUrl);
                    var responseMessage = await httpClient.GetAsync(dataUrl);
                    var responseMessageString = await responseMessage.Content.ReadAsStringAsync();
                    SetSessionData("data", responseMessageString);
                }
            }
            var data = ReadSessionData("data");

            //get profiles
            var allProfiles = JsonConvert.DeserializeObject<List<Profile>>(data);

            //get filtered profiles and randomise order
            var rnd = new Random();
            var filteredProfiles = allProfiles
                .Where(p => p.country == SelectedCountry || SelectedCountry == "ALL" || p.country.Contains("ALL"))
                .Where(p => p.regions.Contains(SelectedRegion) || SelectedRegion == "ALL" || p.regions.Contains("ALL"))
                .Where(p => p.cities.Contains(SelectedCity) || SelectedCity == "ALL" || p.cities.Contains("ALL"))
                .Where(p => p.tags.Contains(SelectedTag) || SelectedTag == "ALL" || p.tags.Contains("ALL"))
                .OrderBy(p => rnd.Next())
                .ToList();

            //mine data for drop-down values and sort them
            var avaliableCities = allProfiles.SelectMany(p => p.cities).Distinct().ToList();
            var avaliableRegions = allProfiles.SelectMany(p => p.regions).Distinct().ToList();
            var avaliableTags = allProfiles.SelectMany(p => p.tags).Distinct().ToList();
            var avaliableCountries = allProfiles.Select(p => p.country).Distinct().ToList();
            avaliableCities.Sort();
            avaliableCities.Remove("ALL");
            avaliableRegions.Sort();
            avaliableRegions.Remove("ALL");
            avaliableTags.Sort();
            avaliableTags.Remove("ALL");
            avaliableCountries.Sort();
            avaliableCountries.Remove("ALL");

            //create select lists for view
            var avaliableCitiesSelect = new SelectList(avaliableCities);
            var avaliableRegionsSelect = new SelectList(avaliableRegions);
            var avaliableTagsSelect = new SelectList(avaliableTags);
            var avaliableCountriesSelect = new SelectList(avaliableCountries);

            //construct view model
            var vm = new HomeIndexViewModel()
            {
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

        public IActionResult Refresh()
        {
            SetSessionData("data", string.Empty);
            return RedirectToAction("Index");
        }

        private string ReadSessionData(string key)
        {
            byte[] bytes;
            HttpContext.Session.TryGetValue(key, out bytes);
            if (bytes == null)
            {
                return string.Empty;
            }
            else
            {
                char[] chars = new char[bytes.Length / sizeof(char)];
                Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
                return new string(chars);
            }

        }

        private void SetSessionData(string key, string value)
        {
            byte[] valueAsBytes = new byte[value.Length * sizeof(char)];
            System.Buffer.BlockCopy(value.ToCharArray(), 0, valueAsBytes, 0, valueAsBytes.Length);
            HttpContext.Session.Set(key, valueAsBytes);
        }

    }
}
