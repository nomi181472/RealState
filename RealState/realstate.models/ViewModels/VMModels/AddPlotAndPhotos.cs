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
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public string SocietyName { get; set; }
        [Required]
        public double PlotSize { get; set; } //TODO Multiple units
        [Required]
        public string CompleteAddress { get; set; }
        [Required]
        public string Block { get; set; }
        [Required]
        public double Price { get; set; }
        public string[] ImagePaths { get; set; }
        public List<string> PhotosUrl { get; set; }

        public string Description { get; set; }
        [Required]
        public string Type { get; set; }
        public string   SelectedUnit { get; set; }
        public IEnumerable<SelectListItem> Units { get; set; }

        public IEnumerable<SelectListItem> allSocieties { get; set; }
        public IEnumerable<SelectListItem> PostType { get; set; }
        public IEnumerable<SelectListItem> BlockTypes { get; set; }
    }
    public class PlotCompleteDetails
    {
        public List<Photo> photos { get; set; }
        public List<string> Role { get; set; }


    }
}
