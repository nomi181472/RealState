using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace realstate.models.ViewModels
{
   public class Photo
    {
        [Key]
        public int PhotoId { get; set; }
        public int PublicURL { get; set; }
        public bool IsActive { get; set; }


    }
}
