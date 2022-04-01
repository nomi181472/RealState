using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace realstate.models.ViewModels
{
 public class Location
    {
        [Key]
        public int LocationId { get; set; }
        [Required]
        public string City { get; set; }
        
        [Required]
        public string Province { get; set; }
        [Required]
        public string Country { get; set; }
    }
}
