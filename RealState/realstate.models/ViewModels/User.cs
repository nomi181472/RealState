using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace realstate.models.ViewModels
{
    public class User
    {
        [Required]
        public string Email { get; set; }
        [Key]
        public int UserGUID { get; set; }
    }
}
