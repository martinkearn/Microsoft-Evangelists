using MicrosoftEvangelists.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicrosoftEvangelists.ViewModels
{
    public class HomeIndexViewModel
    {
        public List<Profile> Profiles { get; set; }

        public List<string> AvaliableRegions { get; set; }

        public List<string> AvaliableCities { get; set; }

        public List<string> AvaliableTags { get; set; }
    }
}
