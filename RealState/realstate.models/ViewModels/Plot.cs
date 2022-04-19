using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace realstate.models.ViewModels
{
   public class Plot
    {
        [Key]
        public int PlotId { get; set; }
        
        [Required]
        public double PlotSize { get; set; } //TODO Multiple units
        [Required]
        public string CompleteAddress { get; set; }
        [Required]
        public string Block { get; set; }
        [Required]
        public double Price { get; set; }
        public string UpdateOn { get; set; }
        public string Type { get; set; }

        public string Description { get; set; }
        [Required]
        public int SocietyId { get; set; }
        [ForeignKey(nameof(SocietyId))]
        public Society SocietyTBL { get; set; }

        [Required]
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public ApplicationUser ApplicationUserTBL { get; set; }
    }
}
