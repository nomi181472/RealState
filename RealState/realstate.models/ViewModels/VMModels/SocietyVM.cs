using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace realstate.models.ViewModels.VMModels
{
    public class SocietyVM
    {
        public Society society { get; set; }
        public IEnumerable<SelectListItem> allLocations { get; set; }
        public SocietyVM()
        {
            society = new Society();
            allLocations = new List<SelectListItem>();
        }
    }
}
