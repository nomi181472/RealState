using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace realstate.models.ViewModels
{
    public class Society
    {
        [Key]
        public int SocietyId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int LocationId { get; set; }
        [ForeignKey(nameof(LocationId))]
        public Location LocationTBL { get; set; }
        
        public string Map { get; set; }
        
    }
}
