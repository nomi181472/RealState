using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace realstate.models.ViewModels.VMModels
{
   public class AddPlotAndPhotos
    {
        
        public int PlotId { get; set; }
        [Required]
        public int SocietyId { get; set; }
        [Required]
        public double PlotSize { get; set; } //TODO Multiple units
        [Required]
        public string CompleteAddress { get; set; }
        [Required]
        public string Block { get; set; }
        [Required]
        public double Price { get; set; }
        public string[] ImagePaths { get; set; }

        public string Description { get; set; }

    
        public IEnumerable<SelectListItem> allSocieties { get; set; }
    }
}
