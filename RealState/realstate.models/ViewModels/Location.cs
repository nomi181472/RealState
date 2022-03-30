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
        public string City { get; set; }
        
        public string District { get; set; }
        public string Province { get; set; }
        public string Country { get; set; }
    }
}
