﻿using Microsoft.AspNetCore.Mvc.Rendering;
using SpeakerBureau.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpeakerBureau.ViewModels
{
    public class HomeIndexViewModel
    {
        public List<Profile> Profiles { get; set; }

        public SelectList AvaliableRegions { get; set; }

        public SelectList AvaliableCities { get; set; }

        public SelectList AvaliableTags { get; set; }

        public SelectList AvaliableCountries { get; set; }
        public string SelectedCountry { get; set; }

        public string SelectedRegion { get; set; }
        public string SelectedCity { get; set; }
        public string SelectedTag { get; set; }
    }
}
